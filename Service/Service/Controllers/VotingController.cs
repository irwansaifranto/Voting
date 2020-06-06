using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models.Models;
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
    }
}