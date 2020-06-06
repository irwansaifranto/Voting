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
                    response.Message = "Delete Bank Success.";
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
