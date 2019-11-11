using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.Persistence;
using eTutor.Persistence.Seeders;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly UsersManager _usersManager;
        private readonly IConfiguration _configuration;

        public AccountsController(UsersManager usersManager)
        {
          
            _usersManager = usersManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserTokenResponse), 200)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            IOperationResult<User> result = await _usersManager.AuthenticateUser(request.Email, request.Password);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            User user = result.Entity;
            
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

            var roles = await _usersManager.GetRolesForUser(user.Id);
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
                Roles = user.UserRoles.Select(r => r.RoleId).ToArray()
            };
        }
    }
}