using LightTrackerServerless.Database.Models;
using LightTrackerServerless.Model;

namespace LightTrackerServerless.Service
{
    public interface IDeviceService
    {
        public Task<IEnumerable<Device>> AddDevice(DeviceDto dtoDevice);

        public Task<IEnumerable<Device>> UpdateDevice(DeviceDto dtoDevice);

        public Task DeleteDevice(string userId, string deviceUniqueIdentifier);
    }
}
