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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/parent")]
    [ApiController]
    public class ParentController : EtutorBaseController
    {
        private readonly ParentsManager _parentsManager;
        private readonly IMapper _mapper;
        
        public ParentController(ParentsManager parentsManager, IMapper mapper)
        {
            _parentsManager = parentsManager;
            _mapper = mapper;
        }
        
        [HttpGet("my-students")]
        [ProducesResponseType(typeof(IEnumerable<StudentUserViewModel>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetStudentsForParent()
        {
            int parentId = GetUserId();
            
            IOperationResult<IEnumerable<User>> operationResult = await _parentsManager.GetStudentsByParentId(parentId);
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
        public async Task<IActionResult> GetSimpleUserResponse(int userId)
        {
            IOperationResult<IEnumerable<User>> operationResult = await _usersManager.GetStudentsByParentId(userId);

            if (!operationResult.Success)
            {
                return NotFound(operationResult.Message);
            }

            IEnumerable<StudentUserViewModel> model = _mapper.Map<IEnumerable<StudentUserViewModel>>(operationResult.Entity);

            return Ok(model);
        }
        
        [HttpPost("toggle-student-state/{studentId}/student")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Error), 404)]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> ToggleStudentAccountState([FromRoute] int studentId)
        {
            int parentId = GetUserId();
            
            IOperationResult<bool> operationResult = await _parentsManager.ToggerStudentAccountActivation(studentId, parentId);
    
            if (!operationResult.Success)
            {
                return NotFound(operationResult.Message);
            }
            
            return Ok();
            var model = _mapper.Map<IEnumerable<User>>(operationResult.Entity);
            
            

            return Ok(model);
        }
    }
}