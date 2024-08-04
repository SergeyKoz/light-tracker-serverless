using LightTrackerServerless.Database.Models;
using LightTrackerServerless.Model;

namespace LightTrackerServerless.Service
{
    public interface IMapService
    {
        public Device DeviceDtoToDeviceEntity(DeviceDto deviceDto);

        public void ApplyDtoToDeviceEntity(DeviceDto deviceDto, Device device);
    }
}
