using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTrackerServerless.Model
{
    public class DeviceDto
    {
        public string UserId { get; set; }

        public string? DeviceName { get; set; }

        public required string DeviceUniqueIdentifier { get; set; }

        public float BatteryLevel { get; set; }

        public int BatteryStatus { get; set; }

        public int NetworkReachability { get; set; }

        public long ReceivedAt { get; set; }
    }
}
