using LightTrackerServerless.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTrackerServerless.Repository
{
    public interface IDeviceRepository
    {
        public Task<List<Device>> PlayerDevicesList(string userId);

        public Task DeleteDevice(string userId, string deviceUniqueIdentifier);

        public Task AddDevice(Device device);

        public Task UpdateDevice(Device device);
    }
}
