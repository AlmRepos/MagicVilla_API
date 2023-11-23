using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthService userservice;

        public UserController(IAuthService userservice)
        {
            this.userservice = userservice;
        }



        public IActionResult Login()
        {
            LoginRequestDTO loginRequest = new();

            return View(loginRequest);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO requestDTO)
        {
            APIResponse response = await userservice.LoginAsync<APIResponse>(requestDTO);
            if(response != null && response.IsSuccess)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(response.Result.ToString());

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);


                HttpContext.Session.SetString(SD.SessionToken, model.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());

                return View(requestDTO);
            }
        }



        public IActionResult Register()
        {
            RegisterationRequestDTO request = new();

            return View(request);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO requestDTO)
        {
            APIResponse response = await userservice.RegisterAsync<APIResponse>(requestDTO);
            if(response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(requestDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");

            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {


            return View();
        }
    }
}
