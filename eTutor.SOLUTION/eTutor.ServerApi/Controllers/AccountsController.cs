using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    [Route("api/accounts")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class AccountsController : EtutorBaseController
    {
        private readonly UsersManager _usersManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountsController(UsersManager usersManager, IMapper mapper, IConfiguration configuration)
        {
            _usersManager = usersManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserTokenResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
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

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserTokenResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest request)
        {
            User newUser = _mapper.Map<User>(request);
            string password = request.Password;
            var roles = request.Roles;

            IOperationResult<User> operationResult = await _usersManager.RegisterUser(newUser, password, roles);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var token = await GenerateJwtToken(operationResult.Entity);

            return Ok(token);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IOperationResult<string>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {

            IOperationResult<string> operationResult = await _usersManager.UserForgotPassword(request.Email);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            return Ok(BasicOperationResult<string>.Ok("Email sent with further instructions"));
        }

        [HttpPut("forgot-password")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetChangePassword([FromBody] ForgotChangePasswordRequest changeRequest)
        {
            IOperationResult<User> operationResult =
                await _usersManager.ChangePasswordUserForgot(changeRequest.UserId, changeRequest.NewPassword, changeRequest.ChangePasswordToken);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var user = operationResult.Entity;
            var response = _mapper.Map<UserResponse>(user);

            return Ok(user);
        }

        [HttpPut("change-password")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changeRequest)
        {
            IOperationResult<User> operationResult = await _usersManager.ChangePassword(changeRequest.UserId,
                changeRequest.NewPassword, changeRequest.CurrentPassword);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var user = operationResult.Entity;
            var response = _mapper.Map<UserResponse>(user);

            return Ok(user);
        }

        private async Task<UserTokenResponse> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            IEnumerable<Role> roles = await _usersManager.GetRolesForUser(user.Id);
            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)));

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