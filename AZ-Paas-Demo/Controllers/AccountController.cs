using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace AZ_Paas_Demo.Controllers
{
    //https://azure.microsoft.com/en-us/resources/samples/active-directory-dotnet-webapp-openidconnect-aspnetcore/
    public class AccountController : Controller
    {
        IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }
        public IActionResult SignOut()
        {
            string returnUrl = Url.Action(
                action: nameof(SignedOut),
                controller: "Account",
                values: null,
                protocol: Request.Scheme
                );
            return SignOut(new AuthenticationProperties { RedirectUri = returnUrl },
                               CookieAuthenticationDefaults.AuthenticationScheme,
                           OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult SignedOut()
        {
            return RedirectToAction(
                actionName: "Index",
                controllerName: "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                _accountService.RegisterNewStoreAndUser(model);
            }
            // If we got this far, redirect to successpage
            return RedirectToAction("success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}