using Service.Models.Entities;
using Service.Models.Models;
using Service.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Service.Models.Models.View;
using System.Security.Cryptography;

namespace Service.Repository.Concrete
{
    public class VotingRepository : IVotingRepository
    {
        readonly VotingContext _context;
        public VotingRepository(VotingContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse> GetVotings()
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var result = await (from voting in _context.MasterVotingProcess
                                    select new
                                    {
                                        voting.VotingProcessId,
                                        voting.VotingProcessName,
                                        voting.Description,
                                        voting.CreatedDate,
                                        StringCreatedDate = voting.CreatedDate.ToString("dd/MM/yyyy"),
                                        voting.SupportersCount,
                                        Reviewers = voting.MappingVotingUsers.Count(),
                                        voting.DueDate,
                                        StringDueDate = voting.DueDate.ToString("dd/MM/yyyy"),
                                        voting.VotingCategory.VotingCategoryName,
                                        voting.VotingCategoryId                                      
                                    }).ToListAsync();

                response.Status = HttpStatusCode.OK.ToString();
                response.Code = (int)HttpStatusCode.OK;

                if (result != null)
                {
                    response.Message = "Retrieve data success";
                }
                else
                {
                    response.Message = "No result";
                }

                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError.ToString();
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.ToString();
            }

            return response;
        }

        public async Task<BaseResponse> InsertVoting(Models.Models.View.ModelVotingView model)
        {
            BaseResponse response = new BaseResponse();

            using (var dbcxtransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    MasterVotingProcess newVoting = new MasterVotingProcess
                    {
                        VotingProcessName = model.VotingProcessName,
                        Description = model.Description,
                        CreatedDate = model.CreatedDate,
                        DueDate = model.DueDate,
                        VotingCategoryId = model.VotingCategoryId,
                        SupportersCount = 0,
                        CreatedBy = "Admin",
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = "Admin",
                        IsDelete = false
                    };

                    await _context.MasterVotingProcess.AddAsync(newVoting);
                    await _context.SaveChangesAsync();

                    dbcxtransaction.Commit();

                    response = ResponseConstant.SAVE_SUCCESS;
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Status = HttpStatusCode.InternalServerError.ToString();

                    dbcxtransaction.Rollback();
                }
            }

            return response;
        }

        public async Task<BaseResponse> UpdateVoting(ModelVotingView model)
        {
            BaseResponse response = new BaseResponse();

            using (var dbcxtransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var voting = await _context.MasterVotingProcess.FindAsync(model.VotingProcessId);

                    voting.VotingProcessName = model.VotingProcessName;
                    voting.Description = model.Description;
                    voting.CreatedDate = model.CreatedDate;
                    voting.DueDate = model.DueDate;
                    voting.VotingCategoryId = model.VotingCategoryId;
                    voting.ModifiedDate = DateTime.Now;
                    voting.ModifiedBy = "Admin";

                    _context.Entry(voting).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    dbcxtransaction.Commit();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Status = HttpStatusCode.OK.ToString();
                    response.Message = "Update data success.";
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Status = HttpStatusCode.InternalServerError.ToString();

                    dbcxtransaction.Rollback();
                }
            }

            return response;
        }

        public async Task<BaseResponse> DeleteVoting(int votingProcessId)
        {
            BaseResponse response = new BaseResponse();

            using (var dbcxtransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var book = await _context.MasterVotingProcess.FindAsync(votingProcessId);
                    _context.MasterVotingProcess.Remove(book);
                    await _context.SaveChangesAsync();

                    dbcxtransaction.Commit();

                    response.Code = (int)HttpStatusCode.OK;
                    response.Status = HttpStatusCode.OK.ToString();
                    response.Message = "Delete Voting Success.";
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Status = HttpStatusCode.InternalServerError.ToString();

                    dbcxtransaction.Rollback();
                }
            }

            return response;
        }

        public async Task<BaseResponse> SubmitVote(SubmitVoteParameter param)
        {
            BaseResponse response = new BaseResponse();

            using (var dbcxtransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var checkDuplicateVote = await _context.MappingVotingUsers.FirstOrDefaultAsync(n => n.UserId == param.UserId && n.VotingProcessId == param.VotingProcessId);

                    if (checkDuplicateVote != null)
                    {
                        dbcxtransaction.Rollback();

                        response.Code = 6001;
                        response.Status = HttpStatusCode.BadRequest.ToString();
                        response.Message = "You cant vote one item two times.";
                    }
                    else
                    {
                        // Save new voter
                        MappingVotingUsers userVote = new MappingVotingUsers
                        {
                            UserId = param.UserId,
                            VotingProcessId = param.VotingProcessId
                        };

                        await _context.MappingVotingUsers.AddAsync(userVote);
                        await _context.SaveChangesAsync();

                        // Update voting process
                        var voting = await _context.MasterVotingProcess.FindAsync(param.VotingProcessId);

                        voting.SupportersCount = voting.SupportersCount + param.VotingValue;
                        voting.ModifiedDate = DateTime.Now;
                        voting.ModifiedBy = "Admin";

                        _context.Entry(voting).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        dbcxtransaction.Commit();

                        // Get new response after voting
                        var responseData = await (from vote in _context.MasterVotingProcess
                                                  where vote.VotingProcessId == param.VotingProcessId
                                                  select new
                                                  {
                                                      vote.VotingProcessId,
                                                      Reviewers = voting.MappingVotingUsers.Count(),
                                                      StarValue = vote.SupportersCount.Value / vote.MappingVotingUsers.Count()
                                                  }).FirstOrDefaultAsync();

                        response.Data = responseData;
                        response.Code = (int)HttpStatusCode.OK;
                        response.Status = HttpStatusCode.OK.ToString();
                        response.Message = "Thank you for your feedback.";
                    }                   
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Status = HttpStatusCode.InternalServerError.ToString();

                    dbcxtransaction.Rollback();
                }
            }

            return response;
        }
    }
}
