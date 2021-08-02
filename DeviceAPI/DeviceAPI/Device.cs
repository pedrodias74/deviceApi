using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceAPI
{
    /// <summary>
    /// Device model data
    /// </summary>
    public class Device
    {
        public Device()
        {

        }

        public Device(DeviceData deviceData, string user = null)
        {
            Id = deviceData.Id;
            Name = deviceData.Name;
            Brand = deviceData.Brand;
            CreatedOn = DateTime.Now;
            CreatedBy = user;
        }

        /// <summary>
        /// device id
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

        /// <summary>
        /// device creation date
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// user which has created the device
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// updated date if any
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// user which has updated the device
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}
