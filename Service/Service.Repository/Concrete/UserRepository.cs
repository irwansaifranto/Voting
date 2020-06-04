using Service.Models.Entities;
using Service.Models.Models;
using Service.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Service.Models.Models.View;

namespace Service.Repository.Concrete
{
    public class UserRepository : IUserRepository
    {
        readonly VotingContext _context;
        public UserRepository(VotingContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse> GetUserByUsernameOrEmail(string key)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var result = await (from user in _context.MasterUser
                                    where user.Email == key || user.UserName == key
                                    select user).FirstOrDefaultAsync();

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

        public async Task<BaseResponse> InsertUser(ModelUserView model)
        {
            BaseResponse response = new BaseResponse();

            using (var dbcxtransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (_context.MasterUser.Where(n => n.Email == model.Email || n.UserName == model.UserName).Count() == 0)
                    {
                        MasterUser newUser = new MasterUser
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Password = model.Password,
                            RoleId = model.RoleId,
                            Gender = model.Gender,
                            Age = model.Age,
                            CreatedDate = DateTime.Now,
                            CreatedBy = model.UserName,
                            ModifiedDate = DateTime.Now,
                            ModifiedBy = model.UserName,
                            IsDelete = false
                        };

                        await _context.MasterUser.AddAsync(newUser);
                        await _context.SaveChangesAsync();

                        dbcxtransaction.Commit();

                        response = ResponseConstant.SAVE_SUCCESS;
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.BadRequest;
                        response.Status = HttpStatusCode.BadRequest.ToString();
                        response.Message = "Email already exist.";

                        dbcxtransaction.Rollback();
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
