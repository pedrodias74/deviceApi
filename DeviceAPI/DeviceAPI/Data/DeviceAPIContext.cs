using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DeviceAPI;

namespace DeviceAPI.Data
{
    public class DeviceAPIContext : DbContext
    {
        public DeviceAPIContext (DbContextOptions<DeviceAPIContext> options)
            : base(options)
        {
        }

        public DbSet<DeviceAPI.Device> Device { get; set; }
    }
}
