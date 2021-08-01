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
            Assert.AreEqual(typeof(List<Device>), result1.Value.GetType());
            List<Device> devices1 = (List<Device>)result1.Value;
            Assert.AreEqual(3, devices1.Count);
            Assert.AreEqual("Brand1", devices1[0].Brand);

            var result2 = await controller.GetDevice("Brand2");
            Assert.AreEqual(typeof(List<Device>), result2.Value.GetType());
            List<Device> devices2 = (List<Device>)result2.Value;
            Assert.AreEqual(2, devices2.Count);
            Assert.AreEqual("Brand2", devices2[0].Brand);
        }

        [Test]
        public async Task TestGetDeviceById()
        {
            using var context = GetData();
            var controller = new DevicesController(context);

            var result = await controller.GetDevice(1);
            Assert.AreEqual(typeof(Device), result.Value.GetType());
            Device devices = result.Value;
            Assert.AreEqual(1, devices.Id);
            Assert.AreEqual("Device1", devices.Name);
            Assert.AreEqual("Brand1", devices.Brand);
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