using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VotingUI.Helper;
using VotingUI.Models.Account;
using VotingUI.Services;

namespace VotingUI.Controllers
{
    public class AccountController : BaseController
    {
        readonly IConfiguration _configuration;
        readonly VotingService _votingService;

        public AccountController(IConfiguration configuration, VotingService votingService)
        {
            _configuration = configuration;
            _votingService = votingService;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ModelLoginView model)
        {
            if (ModelState.IsValid)
            {
                var response = await _votingService.GetAsync(_configuration.GetValue<string>("VotingEndPoint:GetUserByUsernameOrEmail") + "/?key=" + model.UserName);

                if (response.Data == null)
                {
                    ModelState.AddModelError("UserName", "Username or email not registered.");
                }
                else
                {
                    ModelLoginView userModel = response.Data != null ? JsonConvert.DeserializeObject<ModelLoginView>(Convert.ToString(response.Data)) : null;

                    if (EncryptionHelper.Decrypt(userModel.Password) != model.Password)
                    {
                        ModelState.AddModelError("Password", "Username and Password not match.");
                    }
                    else
                    {
                        SetSession(userModel.UserId, userModel.RoleId, userModel.UserName);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(model);
        }

        public IActionResult Register()
        {
            ModelUserView model = new ModelUserView();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ModelUserView model)
        {
            var isUsernameExist = await _votingService.GetAsync(_configuration.GetValue<string>("VotingEndPoint:GetUserByUsernameOrEmail") + "/?key=" + model.UserName);
            var isEmailExist = await _votingService.GetAsync(_configuration.GetValue<string>("VotingEndPoint:GetUserByUsernameOrEmail") + "/?key=" + model.Email);

            if (isUsernameExist.Data != null)
                ModelState.AddModelError("Username", "Username already taken");

            if (isEmailExist.Data != null)
                ModelState.AddModelError("Email", "Your Email already registered");

            try
            {
                if (ModelState.IsValid)
                {
                    model.Password = EncryptionHelper.Encrypt(model.Password);
                    model.RoleId = (int)_votingService.GetAsync(_configuration.GetValue<string>("VotingEndPoint:GetRoleId") + "/?key=" + "Voter").Result.Data;

                    var response = await _votingService.PostAsync(_configuration.GetValue<string>("VotingEndPoint:InsertUser"), model);

                    if (response.Code == (int)HttpStatusCode.OK)
                    {
                        TempData["RegisterSuccess"] = "Register Success. Please login !!";

                        return RedirectToAction(nameof(Login));
                    } else
                    {
                        ViewData["GlobalErrorMessage"] = response.Message;

                        return View(model);
                    }                   
                }                
            }
            catch (Exception ex)
            {
                ViewData["GlobalErrorMessage"] = ex.Message;
                return View(model);
            }

            return View(model);
        }

        private void SetSession(int userId, int roleId, string userName)
        {
            UserId = userId.ToString();
            UserName = userName;
            switch (roleId)
            {
                case 1:
                    UserLevel = "Admin";
                    break;
                case 2:
                    UserLevel = "Voter";
                    break;
                default:
                    break;
            }
        }
    }
}