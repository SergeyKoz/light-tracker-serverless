using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTrackerServerless.Database.Models
{
    //    {
    //    "device_name": "DESKTOP-A70LNK8",
    //    "device_unique_identifier": "61f847aab444ee67483118c395e231a2decc47b0",
    //    "battery_level": 1.0,
    //    "battery_status": 4,
    //    "network_reachability": 2
    //}

    [PrimaryKey(nameof(UserId), nameof(DeviceUniqueIdentifier))]
    public class Device
    {
        [Required]
        public required string UserId { get; set; }

        public string? DeviceName { get; set; }

        [Required]
        public required string DeviceUniqueIdentifier { get; set; }

        [Required]
        public float BatteryLevel { get; set; }

        [Required]
        public int BatteryStatus { get; set; }

        [Required]
        public int NetworkReachability { get; set; }

        public long CreatedAt { get; set; }

        public long UpdatedAt { get; set; }
    }
}
