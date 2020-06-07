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
        Task<BaseResponse> InsertVoting(Models.Models.View.ModelVotingView model);
        Task<BaseResponse> UpdateVoting(Models.Models.View.ModelVotingView model);
        Task<BaseResponse> DeleteVoting(int votingProcessId);
        Task<BaseResponse> SubmitVote(Models.Models.View.SubmitVoteParameter param);
    }
}
