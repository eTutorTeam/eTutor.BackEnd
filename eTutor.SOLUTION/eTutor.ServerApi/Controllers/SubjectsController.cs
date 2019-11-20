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
    [Route("api/subjects")]
    [ApiController]
    [Authorize]
    public class SubjectsController : EtutorBaseController
    {

        private readonly SubjectsManager _subjectsManager;
        private readonly IMapper _mapper;

        public SubjectsController(SubjectsManager subjectsManager, IMapper mapper)
        {
            _subjectsManager = subjectsManager;
            _mapper = mapper;
        }

        // GET: api/Topics
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<SubjectResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetAll()
        {
            var result =  await _subjectsManager.GetAllSubjects();

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var response = _mapper.Map<IEnumerable<SubjectResponse>>(result.Entity);

            return Ok(response);
        }

        /// <summary>
        /// Allows Guests
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-unrestricted")]
        [ProducesResponseType(typeof(IEnumerable<SubjectUnrestrictedResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPublic()
        {
            var result = await _subjectsManager.GetAllSubjects();

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var response = _mapper.Map<IEnumerable<SubjectUnrestrictedResponse>>(result.Entity);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SubjectResponseTutorDetail), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var result = await _subjectsManager.GetSubject(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var response = _mapper.Map<SubjectResponseTutorDetail>(result.Entity);

            return Ok(response);
        }

        /// <summary>
        /// Required Role: admin
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(SubjectResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectRequest request)
        {
            Subject model = _mapper.Map<Subject>(request);

            IOperationResult<Subject> operationResult = await _subjectsManager.CreateSubject(model);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var response = _mapper.Map<SubjectResponse>(operationResult.Entity);

            return Ok(response);
        }

        /// <summary>
        /// Required Role: admin
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(SubjectResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UpdateSubject([FromBody] SubjectRequest request)
        {
            Subject model = _mapper.Map<Subject>(request);

            IOperationResult<Subject> operationResult = await _subjectsManager.UpdateSubject(model);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var response = _mapper.Map<SubjectResponse>(operationResult.Entity);

            return Ok(response);
        }

        /// <summary>
        /// Required Role: admin
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(SubjectResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _subjectsManager.RemoveSubject(id);

            if (!res.Success)
            {
                return BadRequest(res.Message);
            }

            var response = _mapper.Map<SubjectResponse>(res.Entity);

            return Ok(response);
        }
    }
}
