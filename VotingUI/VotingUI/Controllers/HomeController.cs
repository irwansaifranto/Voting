using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VotingUI.Models;
using VotingUI.Models.Response;
using VotingUI.Models.Voting;
using VotingUI.Services;

namespace VotingUI.Controllers
{
    public class HomeController : BaseController
    {
        readonly ILogger<HomeController> _logger;
        readonly IConfiguration _configuration;
        readonly VotingService _votingService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, VotingService votingService)
        {
            _logger = logger;
            _configuration = configuration;
            _votingService = votingService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _votingService.GetAsync(_configuration.GetValue<string>("VotingEndPoint:GetVotings"));

            List<ModelVotingView> model = response.Data != null ? JsonConvert.DeserializeObject<List<ModelVotingView>>(Convert.ToString(response.Data)) : new List<ModelVotingView>();

            foreach (var single in model)
                single.StarValue = single.Reviewers != 0 ? Math.Round(single.SupportersCount.Value / single.Reviewers, 1)  : 0;

            return View(model);
        }

        [HttpPost]
        public async Task<string> SubmitVote(SubmitVoteParameter model)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                model.UserId = Int32.Parse(UserId);
                model.VotingDate = DateTime.Now.Date;

                var data = UserId;
                response = await _votingService.PostAsync(_configuration.GetValue<string>("VotingEndPoint:SubmitVote"), model);
            }
            catch (Exception ex)
            {               
                if (ex.Message.Contains("Input string was not in a correct format"))
                {
                    response.Code = 6002;
                    response.Status = HttpStatusCode.BadRequest.ToString();
                    response.Message = "Please login before you submit your vote.";
                } else
                {
                    response.Message = ex.Message;
                }
            }
            
            return JsonConvert.SerializeObject(response);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
