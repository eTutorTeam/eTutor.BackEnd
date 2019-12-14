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
    
    [Produces("application/json")]
    [Route("api/users/devices")]
    [Authorize]
    public class UserDevicesController : EtutorBaseController
    {
        private DevicesManager _devicesManager;
        private IMapper _mapper;

        public UserDevicesController(DevicesManager devicesManager, IMapper mapper)
        {
            _devicesManager = devicesManager;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IOperationResult<string>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> UploadFcmToken([FromBody] DeviceTokenRequest request)
        {
            int userId = GetUserId();
            
            var device = _mapper.Map<Device>(request);
            device.UserId = userId;
            var result = await _devicesManager.StoreDeviceInfoForUser(device);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}