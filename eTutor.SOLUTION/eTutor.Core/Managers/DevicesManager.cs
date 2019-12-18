using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Helpers;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using eTutor.Core.Validations;

namespace eTutor.Core.Managers
{
    public sealed class DevicesManager
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IUserRepository _userRepository;

        public DevicesManager(IDeviceRepository deviceRepository, IUserRepository userRepository)
        {
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
        }

        public async Task<IOperationResult<string>> StoreDeviceInfoForUser(Device device)
        {
            int userId = device.UserId;
            if (!await _userRepository.Exists(u => u.Id == userId))
            {
                return BasicOperationResult<string>.Fail("El usuario que intenta asociar a este dispositivo no existe");
            }

            var validator = new DevicesValidator();

            var validationResult = validator.Validate(device);
            if (!validationResult.IsValid)
            {
                return BasicOperationResult<string>.Fail(validationResult.JSONFormatErrors());
            }

            if (await _deviceRepository.Exists(d => d.FcmToken == device.FcmToken
                                                    && d.Platform == device.Platform
                                                    && d.UserId == userId))
            {
                return BasicOperationResult<string>.Ok("El Dispositivo ya ha sido guardado para este usuario");
            }

            var updateDevice =
                await _deviceRepository.Find(d => d.FcmToken == device.FcmToken && d.Platform == device.Platform);

            if (updateDevice != null)
            {
                updateDevice.UserId = device.UserId;
                _deviceRepository.Update(updateDevice);
                await _deviceRepository.Save();
                return BasicOperationResult<string>.Ok("El token del usuario ha sido actualizado");
            }

            _deviceRepository.Create(device);

            await _deviceRepository.Save();
            
            return BasicOperationResult<string>.Ok("Dispositivo guardado exitosamente");
        }
    }
}