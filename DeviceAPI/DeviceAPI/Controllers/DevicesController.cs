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
        public async Task<ActionResult<IEnumerable<Device>>> GetDevice(string brand)
        {
            // if brand is not passed then return all devices
            if (string.IsNullOrEmpty(brand))
            {
                return await _context.Device.ToListAsync();
            }

            // find the group of devices of a brand
            var devices = await _context.Device.Where(d => d.Brand == brand).ToListAsync();
            if (devices == null)
            {
                return NotFound();
            }

            return devices;
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
