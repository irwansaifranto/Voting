using Service.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository.Abstract
{
    public interface IVotingRepository
    {
        Task<BaseResponse> GetVotings();
    }
}
