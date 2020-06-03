using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VotingUI.Models.Account;

namespace VotingUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ModelUserView model)
        {
            foreach (var modelState in ViewData.ModelState)
            {
                if (modelState.Key == "RoleId" || modelState.Key == "ConfirmPassword" || modelState.Key == "Name")
                {
                    modelState.Value.ValidationState = ModelValidationState.Valid;
                }

            }

            if (ModelState.IsValid)
            {
                // Logic login here

                return RedirectToAction("Index", "Home");
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
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["RegisterSuccess"] = "Register Success. Please login !!";

                    return RedirectToAction(nameof(Login));
                }                
            }
            catch (Exception ex)
            {
                ViewData["GlobalErrorMessage"] = ex.Message;
                return View(model);
            }

            return View(model);
        }
    }
}