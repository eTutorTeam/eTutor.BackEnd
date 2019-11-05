using System;
using System.Threading.Tasks;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Persistence.Seeders;
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
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

                if (!result.Succeeded)
                {
                    return BadRequest("User not found");
                }

                User user = await _userManager.FindByNameAsync(request.Email);
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new
                {
                    user = user,
                    roles = roles
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }


        [HttpGet("test")]
        public async Task<IActionResult> CreateDemoAccounts()
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}