using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;
using ShopTarge24.Models;
using ShopTarge24.Models.Accounts;

namespace ShopTarge24.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser>  _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailServices _emailServices;

        public AccountsController
            (
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IEmailServices emailServices

            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = vm.Email,
                    Name = vm.Name,
                    Email = vm.Email,
                    City = vm.City,
                };

                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = token }, Request.Scheme);

                    EmailTokenDto newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Please registrate your account by: <a href=\"{confirmationLink}\">clicking here</a>";
                    newsignup.Subject = "CRUD registration";
                    newsignup.To = user.Email;

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administrations");
                    }

                    _emailServices.SendEmailToken(newsignup, token);
                    List<string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration Sucess",
                        "ActedOn", $"{vm.Email}",
                        "CreatedAccountData", $"{vm.Email}\n{vm.City}\n[password hidden]\n[password hidden]"
                        ];
                    ViewBag.ErrorDatas = errordatas;
                    ViewBag.ErrorTitle = "You have successfully registered";
                    ViewBag.ErrorMessage = "Before you can log in, please confirm email from the link" +
                        "\nwe have emailed to your email address.";
                    return View("ConfirmationEmailMessage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id of {userId} is not valid";
                return View("NotFound");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            List<string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration Sucess",
                        "ActedOn", $"{user.Email}",
                        "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]\n[password hidden]"
                        ];
            if (result.Succeeded)
            {
                errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration Sucess",
                        "ActedOn", $"{user.Email}",
                        "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]\n[password hidden]"
                        ];
                ViewBag.ErrorDatas = errordatas;
                return View();
            }

            ViewBag.ErrorDatas = errordatas;
            ViewBag.ErrorTitle = "Email cannot be confirmed";
            ViewBag.ErrorMessage = $"The users email, with userid of {userId}, cannot be confirmed.";
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            return View();
        }
    }
}
