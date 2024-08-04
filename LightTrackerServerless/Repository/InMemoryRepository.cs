using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightTrackerServerless.Database;
using LightTrackerServerless.Database.Models;
using LightTrackerServerless.Model;
using Microsoft.EntityFrameworkCore;

namespace LightTrackerServerless.Repository
{
    public class InMemoryRepository : IDeviceRepository
    {
        private readonly List<Device> _devices;

        public InMemoryRepository()
        {
            _devices = new List<Device>();
        }

        public Task<List<Device>> PlayerDevicesList(string userId)
        {
            var userDevices = _devices.Where(device => device.UserId == userId).ToList();

            return Task.FromResult(userDevices);
        }

        public Task AddDevice(Device device)
        {
            _devices.Add(device);

            return Task.FromResult(0);
        }

        public Task UpdateDevice(Device device)
        {
            _devices.Remove(device);

            return Task.FromResult(0);
        }

        public Task DeleteDevice(string userId, string deviceUniqueIdentifier)
        {
            _devices.RemoveAll(device => device.UserId == userId && device.DeviceUniqueIdentifier == deviceUniqueIdentifier);

            return Task.FromResult(0);
        }
    }
}
