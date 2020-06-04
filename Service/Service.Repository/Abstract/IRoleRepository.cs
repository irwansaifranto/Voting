using Service.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.Abstract
{
    public interface IRoleRepository
    {
        Task<BaseResponse> GetRoleId(string key);
    }
}
