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
    }
}
