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
    public class RoleRepository : IRoleRepository
    {
        readonly VotingContext _context;
        public RoleRepository(VotingContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse> GetRoleId(string key)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var result = await(from role in _context.MasterRole
                                   where role.RoleName == key 
                                   select role.RoleId).FirstOrDefaultAsync();

                response.Status = HttpStatusCode.OK.ToString();
                response.Code = (int)HttpStatusCode.OK;

                if (result != 0)
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
