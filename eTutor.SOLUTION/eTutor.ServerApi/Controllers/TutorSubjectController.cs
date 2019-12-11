using AutoMapper;
using eTutor.Core.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/tutor-subject")]
    [Authorize]
    [Produces("application/json")]
    public class TutorSubjectController : EtutorBaseController
    {
        private readonly TutorsManager _tutorsManager;
        private readonly SubjectsManager _subjectsManager;
        private readonly IMapper _mapper;

        public TutorSubjectController(TutorsManager tutorsManager, 
            SubjectsManager subjectsManager, IMapper mapper)
        {
            _tutorsManager = tutorsManager;
            _subjectsManager = subjectsManager;
            _mapper = mapper;
        }
        
        
        
    }
}