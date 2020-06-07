using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VotingUI.Models.Category;
using VotingUI.Models.Response;
using VotingUI.Services;

namespace VotingUI.Controllers
{
    public class CategoryController : BaseController
    {
        readonly IConfiguration _configuration;
        readonly VotingService _votingService;

        public CategoryController(IConfiguration configuration, VotingService votingService)
        {
            _configuration = configuration;
            _votingService = votingService;
        }

        [Route("Category")]
        [Route("Category/ManageVote")]
        public IActionResult Index()
        {
            ModelCategoryView model = new ModelCategoryView();

            return View(model);
        }

        [HttpGet]
        public async Task<string> GetCategories()
        {
            var response = await _votingService.GetAsync(_configuration.GetValue<string>("VotingEndPoint:GetCategories"));

            return JsonConvert.SerializeObject(response.Data);
        }

        [HttpPost]
        public async Task<BaseResponse> InsertCategory(ModelCategoryView model)
        {
            return await _votingService.PostAsync(_configuration.GetValue<string>("VotingEndPoint:InsertCategory"), model);
        }

        [HttpPost]
        public async Task<BaseResponse> UpdateCategory(ModelCategoryView model)
        {
            return await _votingService.PostAsync(_configuration.GetValue<string>("VotingEndPoint:UpdateCategory"), model);
        }

        [HttpPost]
        public async Task<BaseResponse> DeleteCategory([FromQuery] int id)
        {
            ModelCategoryView model = null;
            return await _votingService.PostAsync(_configuration.GetValue<string>("VotingEndPoint:DeleteCategory") + "?id=" + id, model);
        }
    }
}