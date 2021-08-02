using DeviceAPI;
using DeviceAPI.Controllers;
using DeviceAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestDeviceAPI
{
    public class TestDevicesController
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestGetDevice()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            // tests getting all devices
            var result = await controller.GetDevice(null);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            List<Device> devices = (List<Device>)result.Value;
            Assert.AreEqual(5, devices.Count);
        }

        [Test]
        public async Task TestGetDeviceWithPagination()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            // tests getting all devices
            var result = await controller.GetDevice(null, 1, 20);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            List<Device> devices = (List<Device>)result.Value;
            Assert.AreEqual(5, devices.Count);

            // tests getting 1st page
            result = await controller.GetDevice(null, 1, 5);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            devices = (List<Device>)result.Value;
            Assert.AreEqual(5, devices.Count);

            // tests getting 1st page
            result = await controller.GetDevice(null, 1, 2);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            devices = (List<Device>)result.Value;
            Assert.AreEqual(2, devices.Count);

            // tests getting 2nd page
            result = await controller.GetDevice(null, 2, 2);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            devices = (List<Device>)result.Value;
            Assert.AreEqual(2, devices.Count);

            // tests getting 3nd page
            result = await controller.GetDevice(null, 3, 2);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            devices = (List<Device>)result.Value;
            Assert.AreEqual(1, devices.Count);

            // tests getting 4th page
            result = await controller.GetDevice(null, 4, 2);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            devices = (List<Device>)result.Value;
            Assert.AreEqual(0, devices.Count);

            // tests getting 1st page with brand
            result = await controller.GetDevice("Brand1", 1, 2);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            devices = (List<Device>)result.Value;
            Assert.AreEqual(2, devices.Count);

            // tests bad page request
            result = await controller.GetDevice(null, null, 2);
            Assert.IsNull(result.Value);

            // tests bad page request
            result = await controller.GetDevice(null, 2, null);
            Assert.IsNull(result.Value);
        }

        [Test]
        public async Task TestGetDeviceByBrand()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            // tests getting devices with Brand1
            var result1 = await controller.GetDevice("Brand1");
            Assert.IsNotNull(result1.Value);
            Assert.AreEqual(typeof(List<Device>), result1.Value.GetType());
            List<Device> devices1 = (List<Device>)result1.Value;
            Assert.AreEqual(3, devices1.Count);
            Assert.AreEqual("Brand1", devices1[0].Brand);

            // tests getting devices with Brand2
            var result2 = await controller.GetDevice("Brand2");
            Assert.IsNotNull(result2.Value);
            Assert.AreEqual(typeof(List<Device>), result2.Value.GetType());
            List<Device> devices2 = (List<Device>)result2.Value;
            Assert.AreEqual(2, devices2.Count);
            Assert.AreEqual("Brand2", devices2[0].Brand);

            // tests getting devices with an unknown brand
            var result3 = await controller.GetDevice("Brand3");
            Assert.IsNotNull(result3.Value);
            Assert.AreEqual(typeof(List<Device>), result3.Value.GetType());
            List<Device> devices3 = (List<Device>)result3.Value;
            Assert.AreEqual(0, devices3.Count);
        }

        [Test]
        public async Task TestGetDeviceById()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            // tests gettind device by Id
            var result = await controller.GetDevice(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(Device), result.Value.GetType());
            Device device = result.Value;
            Assert.AreEqual(1, device.Id);
            Assert.AreEqual("Device1", device.Name);
            Assert.AreEqual("Brand1", device.Brand);

            // tests gettint device with an unknown id
            var result1 = await controller.GetDevice(6);
            Assert.IsNull(result1.Value);
        }

        [Test]
        public async Task TestDeleteDevice()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            // tests deleting a device
            var result = await controller.DeleteDevice(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(Device), result.Value.GetType());
            Device device = result.Value;
            Assert.AreEqual(1, device.Id);
            Assert.AreEqual("Device1", device.Name);
            Assert.AreEqual("Brand1", device.Brand);

            // checks if the device was deleted
            var results = await controller.GetDevice(null);
            Assert.IsNotNull(results.Value);
            Assert.AreEqual(typeof(List<Device>), results.Value.GetType());
            List<Device> devices = (List<Device>)results.Value;
            Assert.AreEqual(4, devices.Count);

            // tries to delete an unknown device
            var result1 = await controller.DeleteDevice(1);
            Assert.IsNull(result1.Value);
        }

        [Test]
        public async Task TestPostDevice()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            DeviceData device = new DeviceData { 
                Id = 6,
                Name = "Device6",
                Brand = "Brand3"
            };

            // tests creating a new device
            var result = await controller.PostDevice(device);
            Assert.IsNull(result.Value);

            // checks if the device was created
            var results = await controller.GetDevice(null);
            Assert.IsNotNull(results.Value);
            Assert.AreEqual(typeof(List<Device>), results.Value.GetType());
            List<Device> devices = (List<Device>)results.Value;
            Assert.AreEqual(6, devices.Count);

            Assert.AreEqual(6, devices[5].Id);
            Assert.AreEqual("Device6", devices[5].Name);
            Assert.AreEqual("Brand3", devices[5].Brand);
        }

        [Test]
        public async Task TestPutDevice()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            DeviceData device = new DeviceData
            {
                Id = 1,
                Name = "Device6",
                Brand = "Brand3"
            };

            // tests changing all the device properties
            await controller.PutDevice(1, device);

            Device device1;
            var result = await controller.GetDevice(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(Device), result.Value.GetType());
            device1 = result.Value;
            Assert.AreEqual(1, device1.Id);
            Assert.AreEqual("Device6", device1.Name);
            Assert.AreEqual("Brand3", device1.Brand);

            // tests changing only one property
            device.Name = "Device7";
            device.Brand = null;
            await controller.PutDevice(1, device);

            result = await controller.GetDevice(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(Device), result.Value.GetType());
            device1 = result.Value;
            Assert.AreEqual(1, device1.Id);
            Assert.AreEqual("Device7", device1.Name);
            Assert.AreEqual("Brand3", device1.Brand);

            // tests changing only one property
            device.Name = null;
            device.Brand = "Brand4";
            await controller.PutDevice(1, device);

            result = await controller.GetDevice(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(Device), result.Value.GetType());
            device1 = result.Value;
            Assert.AreEqual(1, device1.Id);
            Assert.AreEqual("Device7", device1.Name);
            Assert.AreEqual("Brand4", device1.Brand);

            await controller.PutDevice(7, device);

            device.Id = 8;
            await controller.PutDevice(8, device);
        }

        // created the test db context
        private DeviceAPIContext GetData()
        {
            var options = new DbContextOptionsBuilder<DeviceAPIContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new DeviceAPIContext(options);

            context.Device.Add(new DeviceAPI.Device { 
                Id = 1,
                Name = "Device1",
                Brand = "Brand1",
                CreatedOn = DateTime.Now
            });

            context.Device.Add(new DeviceAPI.Device
            {
                Id = 2,
                Name = "Device2",
                Brand = "Brand1",
                CreatedOn = DateTime.Now
            });

            context.Device.Add(new DeviceAPI.Device
            {
                Id = 3,
                Name = "Device3",
                Brand = "Brand1",
                CreatedOn = DateTime.Now
            });

            context.Device.Add(new DeviceAPI.Device
            {
                Id = 4,
                Name = "Device4",
                Brand = "Brand2",
                CreatedOn = DateTime.Now
            });

            context.Device.Add(new DeviceAPI.Device
            {
                Id = 5,
                Name = "Device5",
                Brand = "Brand2",
                CreatedOn = DateTime.Now
            });

            context.SaveChanges();

            return context;
        }
    }
}