using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightTrackerServerless.Database;
using LightTrackerServerless.Database.Models;
using LightTrackerServerless.Exceptions;
using LightTrackerServerless.Model;
using LightTrackerServerless.Repository;
using Microsoft.EntityFrameworkCore;

namespace LightTrackerServerless.Service
{
    public class DeviceService: IDeviceService
    {
        private readonly IMapService _mapService;

        private readonly IDeviceRepository _deviceRepository; 

        public DeviceService(IMapService mapService, IDeviceRepository deviceRepository)
        {
            _mapService = mapService;
            _deviceRepository = deviceRepository;
        }

        public async Task<IEnumerable<Device>> AddDevice(DeviceDto deviceDto)
        {
            var userDevices = await _deviceRepository.PlayerDevicesList(deviceDto.UserId);

            var device = userDevices?.FirstOrDefault(item => item.DeviceUniqueIdentifier == deviceDto.DeviceUniqueIdentifier);

            if (device != null)
            {
                _mapService.ApplyDtoToDeviceEntity(deviceDto, device);
                await _deviceRepository.UpdateDevice(device);
            }
            else
            {
                var newDevice = _mapService.DeviceDtoToDeviceEntity(deviceDto);
                await _deviceRepository.AddDevice(newDevice);
                userDevices.Add(newDevice);
            }

            return userDevices;
        }

        public async Task DeleteDevice(string userId, string deviceUniqueIdentifier)
        {
            await _deviceRepository.DeleteDevice(userId, deviceUniqueIdentifier);
        }

        public async Task<IEnumerable<Device>> UpdateDevice(DeviceDto deviceDto)
        {
            var isMemoryRepository = Environment.GetEnvironmentVariable("REPOSITORY_TYPE") == "memory";

            if (isMemoryRepository)
            {
                return await AddDevice(deviceDto);
            }

            var userDevices = await _deviceRepository.PlayerDevicesList(deviceDto.UserId);

            var device = userDevices?.FirstOrDefault(item => item.DeviceUniqueIdentifier == deviceDto.DeviceUniqueIdentifier);

            if (device != null) {
                _mapService.ApplyDtoToDeviceEntity(deviceDto, device);
                await _deviceRepository.UpdateDevice(device);
            }
            else
            {
                throw new DeviceNotFoundException();
            }

            return userDevices;
        }
    }
}
