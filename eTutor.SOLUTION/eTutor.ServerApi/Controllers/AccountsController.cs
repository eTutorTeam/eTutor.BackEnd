using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Persistence;
using eTutor.Persistence.Seeders;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ETutorContext _context;
        private readonly IConfiguration _configuration;

        public AccountsController(UserManager<User> userManager, SignInManager<User> signInManager,
            IConfiguration configuration, ETutorContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserTokenResponse), 200)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest("User not found");
            }

            User user = await _userManager.FindByNameAsync(request.Email);
            var token = await GenerateJwtToken(user);

            return Ok(token);
        }

        private async Task<UserTokenResponse> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var jwtConfigSection = _configuration.GetSection("JWT");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigSection["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtConfigSection["ExpireDays"]));

            var token = new JwtSecurityToken(
                jwtConfigSection["Issuer"],
                jwtConfigSection["Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds);

            string writtenToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserTokenResponse
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = writtenToken,
                Roles = _context.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Id).ToArray()
            };
        }
    }
}