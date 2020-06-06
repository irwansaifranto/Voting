using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VotingUI.Models.Category;
using VotingUI.Models.Response;
using VotingUI.Models.Voting;
using VotingUI.Services;

namespace VotingUI.Controllers
{
    public class VotingController : BaseController
    {
        readonly IConfiguration _configuration;
        readonly VotingService _votingService;
        
        public VotingController(IConfiguration configuration, VotingService votingService)
        {
            _configuration = configuration;
            _votingService = votingService;
        }

        [Route("Voting")]
        [Route("Voting/ManageVote")]
        public async Task<IActionResult> Index()
        {           
            var response = await _votingService.GetAsync(_configuration.GetValue<string>("VotingEndPoint:GetCategories"));

            ModelVotingView model = new ModelVotingView();

            model.CategoryOptions =  model.ConstructBookOptions(response.Data != null ? JsonConvert.DeserializeObject<List<ModelCategoryView>>(Convert.ToString(response.Data)) : new List<ModelCategoryView>());
            model.CreatedDate = DateTime.Now;
            model.DueDate = DateTime.Now.AddDays(1);

            return View(model);
        }

        [HttpGet]
        public async Task<string> GetVotings()
        {
            var response = await _votingService.GetAsync(_configuration.GetValue<string>("VotingEndPoint:GetVotings"));

            return JsonConvert.SerializeObject(response.Data);
        }

        [HttpPost]
        public async Task<BaseResponse> InsertVoting(ModelVotingView model)
        {
            return await _votingService.PostAsync(_configuration.GetValue<string>("VotingEndPoint:InsertVoting"), model);
        }

        [HttpPost]
        public async Task<BaseResponse> UpdateVoting(ModelVotingView model)
        {
            return await _votingService.PostAsync(_configuration.GetValue<string>("VotingEndPoint:UpdateVoting"), model);
        }

        [HttpPost]
        public async Task<BaseResponse> DeleteVoting([FromQuery] int votingProcessId)
        {
            ModelVotingView model = null;

            return await _votingService.PostAsync(_configuration.GetValue<string>("VotingEndPoint:DeleteVoting") + "?votingProcessId=" + votingProcessId, model);
        }
    }
}