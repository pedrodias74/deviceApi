using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceAPI
{
    /// <summary>
    /// Defines the device changeable properties
    /// </summary>
    public class DeviceData
    {
        /// <summary>
        /// device id, use to validate Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// device name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// device brand 
        /// </summary>
        public string Brand { get; set; }
    }
}
