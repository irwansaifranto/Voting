using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models.Models;
using Service.Models.Models.View;
using Service.Repository.Abstract;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        readonly IVotingRepository _votingRepository;
        public VotingController(IVotingRepository votingRepository)
        {
            _votingRepository = votingRepository;
        }

        [HttpGet("GetVotings")]
        public async Task<BaseResponse> GetVotings()
        {
            return await _votingRepository.GetVotings();
        }

        [HttpPost("InsertVoting")]
        public async Task<BaseResponse> InsertVoting(ModelVotingView model)
        {
            return await _votingRepository.InsertVoting(model);
        }

        [HttpPost("UpdateVoting")]
        public async Task<BaseResponse> UpdateVoting(ModelVotingView model)
        {
            return await _votingRepository.UpdateVoting(model);
        }

        [HttpPost("DeleteVoting")]
        public async Task<BaseResponse> DeleteVoting([FromQuery] int votingProcessId)
        {
            return await _votingRepository.DeleteVoting(votingProcessId);
        }

        [HttpPost("SubmitVote")]
        public async Task<BaseResponse> SubmitVote(SubmitVoteParameter param)
        {
            return await _votingRepository.SubmitVote(param);
        }
    }
}