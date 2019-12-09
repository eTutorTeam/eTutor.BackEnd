using System;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Contracts;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public sealed class ChangePasswordController : EtutorBaseController
    {
        private readonly AccountsManager _accountsManager;
        private readonly UsersManager _usersManager;
        private readonly IMapper _mapper;

        public ChangePasswordController(AccountsManager accountsManager, 
            IMapper mapper, UsersManager usersManager)
        {
            _accountsManager = accountsManager;
            _mapper = mapper;
            _usersManager = usersManager;
        }
        
        [HttpGet("change-password-request/{changePasswordId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> ValidateChangePasswordRequest([FromRoute]Guid changePasswordId)
        {
            var result = await _accountsManager.CheckIfChangePasswordRequestIsValid(changePasswordId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
        
        
        [HttpPost("forget-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ChangePasswordResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _accountsManager.GenerateForgetPasswordRequest(request.Email);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var response = _mapper.Map<ChangePasswordResponse>(result.Entity);

            return Ok(response);
        }

        [HttpPut("forget-password/{changePasswordId}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> ChangePasswordUsingChangeRequestId([FromRoute] Guid changePasswordId, [FromBody] ForgetPasswordChangeRequest request)
        {
            IOperationResult<bool> result =
                await _accountsManager.ChangePasswordWithChangeRequestId(changePasswordId, request.Password,
                    request.ConfirmPassword);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();

        }
        
        [HttpPut("change-password")]
        [ProducesResponseType( 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changeRequest)
        {
            int userId = GetUserId();

            IOperationResult<bool> result =
                await _accountsManager.ChangePasswordForUser(userId, changeRequest.CurrentPassword,
                    changeRequest.NewPassword);
                
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
    }
}