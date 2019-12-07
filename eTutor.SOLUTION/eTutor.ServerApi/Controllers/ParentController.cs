using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Contracts;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly UsersManager _usersManager;
        private readonly IMapper _mapper;
        public ParentController(UsersManager usersManager, IMapper mapper)
        {
            _usersManager = usersManager;
            _mapper = mapper;
        }
        [HttpGet("my-students")]
        [ProducesResponseType(typeof(IEnumerable<UserAdminResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetSimpleUserResponse(int userId)
        {
            IOperationResult<IEnumerable<User>> operationResult = await _usersManager.GetStudentsByParentId(userId);

            if (!operationResult.Success)
            {
                return NotFound(operationResult.Message);
            }

            IEnumerable<UserAdminResponse> model = _mapper.Map<IEnumerable<UserAdminResponse>>(operationResult.Entity);

            return Ok(model);
        }
    }
}