using System;
using System.Threading.Tasks;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
      
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountsController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            return Ok();
        }


        [HttpGet("test")]
        public async Task<IActionResult> CreateDemoAccounts()
        {
            try
            {
                var user = new User
                {
                    Email = "admin@admin.com",
                    Gender = Gender.Male,
                    IsEmailValidated = true,
                    IsTemporaryPassword = false,
                    Name = "Admin",
                    LastName = "Admin",
                    UserName = "admin@admin.com",
                };
                
                IdentityResult res = await _userManager.CreateAsync(user, "123456");

                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}