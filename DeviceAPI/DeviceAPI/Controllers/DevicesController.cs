using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeviceAPI;
using DeviceAPI.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace DeviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        //private readonly ILogger<DevicesController> _logger;
        private readonly DeviceAPIContext _context;

        public DevicesController(DeviceAPIContext context)
        {
            // TODO: add logger
            //_logger = logger;
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevice(string brand = null, int? page = null, int? rowsPerPage = null)
        {
            // check if the pagination parameters are consistent
            if((page is null && !(rowsPerPage is null)) || (rowsPerPage is null && !(page is null)))
            {
                return BadRequest();
            }
            else
            {
                // page and rows per page must be greated then 0
                if (!(page is null) && (page < 1 || rowsPerPage < 1))
                    return BadRequest();
            }

            List<Device> devices;

            if (string.IsNullOrEmpty(brand))
            {
                // if brand is not passed then return all devices
                devices = await _context.Device.ToListAsync();
            }
            else
            {
                // find the group of devices of a brand
                devices = await _context.Device.Where(d => d.Brand == brand).ToListAsync();
                if (devices == null)
                {
                    return NotFound();
                }
            }

            // if no pagination return all devices found
            if(page is null)
            {
                return devices.OrderBy(d => d.Id).ToList();
            }

            List<Device> paged = new List<Device>();

            List<Device> ordered = devices.OrderBy(d => d.Id).ToList();

            try
            {
                int count = devices.Count;

                int pages = count / (int) rowsPerPage ;
                pages++;
                int rest = count % (int)rowsPerPage;

                if ((int)page > pages)
                    return paged;

                if (pages == (int)page)
                    paged = ordered.GetRange((int)page - 1, rest);
                else
                    paged = ordered.GetRange((int)page - 1, (int) rowsPerPage);

                return paged;
            }
            catch(ArgumentException)
            {

            }

            return paged;
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _context.Device.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        // PUT: api/Devices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, DeviceData deviceData)
        {
            if (id != deviceData.Id)
            {
                return BadRequest();
            }

            var device = await _context.Device.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            // only update the given properties
            if(!(deviceData.Name is null))
                device.Name = deviceData.Name;

            if (!(deviceData.Brand is null))
                device.Brand = deviceData.Brand;

            device.UpdatedOn = DateTime.Now;
            device.UpdatedBy = User?.Identity?.Name;

            //_context.Entry(deviceData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Devices
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(DeviceData deviceData)
        {
            // generates the new device based on the properties passed
            Device device = new Device(deviceData, User?.Identity?.Name);
            _context.Device.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevice", new { id = device.Id }, device);
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Device>> DeleteDevice(int id)
        {
            var device = await _context.Device.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Device.Remove(device);
            await _context.SaveChangesAsync();

            return device;
        }

        private bool DeviceExists(int id)
        {
            return _context.Device.Any(e => e.Id == id);
        }
    }
}
