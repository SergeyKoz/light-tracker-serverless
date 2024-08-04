using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightTrackerServerless.Database.Models;
using LightTrackerServerless.Model;

namespace LightTrackerServerless.Service
{
    public class MapService : IMapService
    {
        public Device DeviceDtoToDeviceEntity(DeviceDto deviceDto)
        {
            return new Device
            {
                UserId = deviceDto.UserId,
                DeviceName = deviceDto.DeviceName,
                DeviceUniqueIdentifier = deviceDto.DeviceUniqueIdentifier,
                BatteryLevel = deviceDto.BatteryLevel,
                BatteryStatus = deviceDto.BatteryStatus,
                NetworkReachability = deviceDto.NetworkReachability,
                CreatedAt = deviceDto.ReceivedAt,
                UpdatedAt = deviceDto.ReceivedAt,
            };
        }

        public void ApplyDtoToDeviceEntity(DeviceDto deviceDto, Device device)
        {
            device.DeviceUniqueIdentifier = deviceDto.DeviceUniqueIdentifier;
            device.BatteryLevel = deviceDto.BatteryLevel;
            device.BatteryStatus = deviceDto.BatteryStatus;
            device.NetworkReachability = deviceDto.NetworkReachability;
            device.UpdatedAt = deviceDto.ReceivedAt;
        }
    }
}
