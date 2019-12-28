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
    [Route("api/ratings")]
    [Produces("application/json")]
    [Authorize]
    public class RatingController : EtutorBaseController
    {
        private readonly RatingManager _ratingManager;
        private readonly IMapper _mapper;

        public RatingController(RatingManager ratingManager, IMapper mapper)
        {
            _ratingManager = ratingManager;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RatingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(401)]
        [Authorize(Roles = "student, tutor")]
        public async Task<IActionResult> CreateRating([FromBody] RatingRequest request)
        {
            int studentId = GetUserId();
            Rating model = _mapper.Map<Rating>(request);
            model.UserId = request.UserId;

            IOperationResult<Rating> operationResult = await _ratingManager.CreateUserRating(model);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var response = _mapper.Map<RatingResponse>(operationResult.Entity);

            return Ok(response);
        }

        [HttpGet("user-ratings")]
        [ProducesResponseType(typeof(MeetingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetUserRatings([FromRoute] int userId)
        {
            var result = await _ratingManager.GetUserRatings(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<MeetingResponse>>(result.Entity);
            
            return Ok(mapped);
        }

        [HttpGet("user-avg-rating/{userId}")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetUserAvgRatings([FromRoute] int userId)
        {
            var result = await _ratingManager.GetUserAvgRating(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
