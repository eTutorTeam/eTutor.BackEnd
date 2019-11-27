using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/users")]
    [Produces("application/json")]
    [Authorize]
    public sealed class UsersController : EtutorBaseController
    {

        private readonly UsersManager _usersManager;
        private readonly IMapper _mapper;

        public UsersController(UsersManager usersManager, IMapper mapper)
        {
            _usersManager = usersManager;
            _mapper = mapper;
        }

        [HttpGet("profile")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = GetUserId();

            IOperationResult<User> profile = await _usersManager.GetUserProfile(userId);

            if (!profile.Success)
            {
                return BadRequest(profile.Message);
            }

            return Ok(profile.Entity);

        }

        [HttpPut("profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateRequest request)
        {
            var userId = GetUserId();

            var user = _mapper.Map<User>(request);

            var result = await _usersManager.UpdateUserProfile(user, userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Entity);

        }
    }
}
