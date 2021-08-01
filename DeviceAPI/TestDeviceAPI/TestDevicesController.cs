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

            var result = await controller.GetDevice(null);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(List<Device>), result.Value.GetType());
            List<Device> devices = (List<Device>)result.Value;
            Assert.AreEqual(5, devices.Count);
        }

        [Test]
        public async Task TestGetDeviceByBrand()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            var result1 = await controller.GetDevice("Brand1");
            Assert.IsNotNull(result1.Value);
            Assert.AreEqual(typeof(List<Device>), result1.Value.GetType());
            List<Device> devices1 = (List<Device>)result1.Value;
            Assert.AreEqual(3, devices1.Count);
            Assert.AreEqual("Brand1", devices1[0].Brand);

            var result2 = await controller.GetDevice("Brand2");
            Assert.IsNotNull(result2.Value);
            Assert.AreEqual(typeof(List<Device>), result2.Value.GetType());
            List<Device> devices2 = (List<Device>)result2.Value;
            Assert.AreEqual(2, devices2.Count);
            Assert.AreEqual("Brand2", devices2[0].Brand);

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

            var result = await controller.GetDevice(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(Device), result.Value.GetType());
            Device device = result.Value;
            Assert.AreEqual(1, device.Id);
            Assert.AreEqual("Device1", device.Name);
            Assert.AreEqual("Brand1", device.Brand);

            var result1 = await controller.GetDevice(6);
            Assert.IsNull(result1.Value);
        }

        [Test]
        public async Task TestDeleteDevice()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            var result = await controller.DeleteDevice(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(Device), result.Value.GetType());
            Device device = result.Value;
            Assert.AreEqual(1, device.Id);
            Assert.AreEqual("Device1", device.Name);
            Assert.AreEqual("Brand1", device.Brand);

            var results = await controller.GetDevice(null);
            Assert.IsNotNull(results.Value);
            Assert.AreEqual(typeof(List<Device>), results.Value.GetType());
            List<Device> devices = (List<Device>)results.Value;
            Assert.AreEqual(4, devices.Count);

            var result1 = await controller.DeleteDevice(1);
            Assert.IsNull(result1.Value);
        }

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