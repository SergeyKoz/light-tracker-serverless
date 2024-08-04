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
    public class DeviceRepository : IDeviceRepository
    {
        private readonly LightTrackerContext _lightTrackerContext;

        public DeviceRepository(LightTrackerContext lightTrackerContext)
        {
            _lightTrackerContext = lightTrackerContext;
        }

        public async Task<List<Device>> PlayerDevicesList(string userId)
        {
            return await _lightTrackerContext.Devices.Where(device => device.UserId == userId).ToListAsync();
        }

        public async Task AddDevice(Device device)
        {
            _lightTrackerContext.Add(device);
            await _lightTrackerContext.SaveChangesAsync();
        }

        public async Task UpdateDevice(Device device)
        {
            _lightTrackerContext.Update(device);
            await _lightTrackerContext.SaveChangesAsync();
        }

        public async Task DeleteDevice(string userId, string deviceUniqueIdentifier)
        {
            var device = await _lightTrackerContext
                .Devices
                .Where(device => device.UserId == userId && device.DeviceUniqueIdentifier == deviceUniqueIdentifier)
                .FirstAsync();

            if (device != null)
            {
                _lightTrackerContext.Remove(device);
                await _lightTrackerContext.SaveChangesAsync();
            }
        }
    }
}
