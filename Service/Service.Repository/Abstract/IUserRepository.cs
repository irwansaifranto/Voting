using Service.Models.Entities;
using Service.Models.Models;
using Service.Models.Models.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.Abstract
{
    public interface IUserRepository
    {
        Task<BaseResponse> GetUserByUsernameOrEmail(string key);
        Task<BaseResponse> InsertUser(ModelUserView model);
    }
}
