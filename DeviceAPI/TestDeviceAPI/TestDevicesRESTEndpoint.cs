using DeviceAPI;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace TestDeviceAPI
{
    public class TestDevicesRESTEndpoint
    {
        private HttpClient _client;
        //private const string URL = "https://localhost:44343/api/";
        private const string URL = "https://localhost:5001/api/";

        public TestDevicesRESTEndpoint()
        {

        }

        [OneTimeSetUp]
        public void Init()
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
                UseDefaultCredentials = true
            };

            // create client with windows authentication
            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri(URL)
            };

            // add an Accept header for JSON format.
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!InitializeData())
            {
                Assert.Fail();
            }
        }

        [OneTimeTearDown]
        public void Close()
        {
            // get all devices for deletion
            HttpResponseMessage response = _client.GetAsync("devices").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // parse the response body.
            List<Device> dataObjects = response.Content.ReadAsAsync<List<Device>>().Result;

            // deletes all devices
            foreach (var d in dataObjects)
            {
                DeleteDevice(d.Id);
            }

        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestGet()
        {
            // List data response.
            HttpResponseMessage response = _client.GetAsync("devices").Result;

            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // parse the response body.
            List<Device> dataObjects = response.Content.ReadAsAsync<List<Device>>().Result;
            foreach (var d in dataObjects)
            {
                TestContext.Out.WriteLine("{0} - {1} - {2}", d.Id, d.Name, d.Brand);
            }

            Assert.GreaterOrEqual(dataObjects.Count, 3);

            //string dataObjects = response.Content.ReadAsStringAsync().Result;
            //TestContext.Out.WriteLine(dataObjects);
            Assert.Pass();
        }

        [Test]
        public void TestGetWithPagination()
        {
            // gets a device page
            HttpResponseMessage response = _client.GetAsync("devices?page=1&rowsPerPage=2").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // parse the response body.
            List<Device> dataObjects = response.Content.ReadAsAsync<List<Device>>().Result;
            foreach (var d in dataObjects)
            {
                TestContext.Out.WriteLine("{0} - {1} - {2}", d.Id, d.Name, d.Brand);
            }

            Assert.AreEqual(2, dataObjects.Count);

            // gets a device page
            response = _client.GetAsync("devices?brand=Brand1&page=1&rowsPerPage=2").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // parse the response body.
            dataObjects = response.Content.ReadAsAsync<List<Device>>().Result;
            foreach (var d in dataObjects)
            {
                TestContext.Out.WriteLine("{0} - {1} - {2}", d.Id, d.Name, d.Brand);
                Assert.AreEqual("Brand1", d.Brand);
            }

            Assert.AreEqual(2, dataObjects.Count);

            // tests a wrong pagination request
            response = _client.GetAsync("devices?rowsPerPage=2").Result;
            if (response.IsSuccessStatusCode)
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            //string dataObjects = response.Content.ReadAsStringAsync().Result;
            //TestContext.Out.WriteLine(dataObjects);
            Assert.Pass();
        }

        [Test]
        public void TestGetById()
        {
            // get device
            HttpResponseMessage response = _client.GetAsync("devices/1").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            Device dev = response.Content.ReadAsAsync<Device>().Result;
            Assert.AreEqual(1, dev.Id);
            Assert.AreEqual("Device1", dev.Name);
            Assert.AreEqual("Brand1", dev.Brand);

            // get device
            response = _client.GetAsync("devices/1").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            string dataObjects = response.Content.ReadAsStringAsync().Result;
            TestContext.Out.WriteLine(dataObjects);

            JObject device = JObject.Parse(dataObjects);
            Assert.AreEqual(1, device["id"].Value<int>());
            Assert.AreEqual("Device1", device["name"].Value<string>());
            Assert.AreEqual("Brand1", device["brand"].Value<string>());

            response = _client.GetAsync("devices/20").Result;
            if (response.IsSuccessStatusCode)
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Pass();
        }

        [Test]
        public void TestGetByBrand()
        {
            // List data response.
            HttpResponseMessage response = _client.GetAsync("devices?brand=Brand1").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // Parse the response body.
            List<Device> dataObjects = response.Content.ReadAsAsync<List<Device>>().Result;

            foreach (var d in dataObjects)
            {
                TestContext.Out.WriteLine("{0}", d.Id);
                Assert.AreEqual("Brand1", d.Brand);
            }

            Assert.AreEqual(3, dataObjects.Count);

            //string dataObjects = response.Content.ReadAsStringAsync().Result;
            //TestContext.Out.WriteLine(dataObjects);
            Assert.Pass();
        }

        [Test]
        public void TestDelete()
        {
            // delete device 5
            HttpResponseMessage response = _client.DeleteAsync("devices/5").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // parse the response body.
            var device = response.Content.ReadAsAsync<Device>().Result;
            TestContext.Out.WriteLine("{0}", device.Id);

            Assert.AreEqual(5, device.Id);

            response = _client.DeleteAsync("devices/5").Result;
            if (response.IsSuccessStatusCode)
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            //string dataObjects = response.Content.ReadAsStringAsync().Result;
            //TestContext.Out.WriteLine(dataObjects);
            Assert.Pass();
        }

        [Test]
        public void TestPost()
        {
            StringContent stringContent = CreateDeviceForPost(6, "Device6", "Brand2");
            HttpResponseMessage response = _client.PostAsync("devices", stringContent).Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // parse the response body.
            var device = response.Content.ReadAsAsync<Device>().Result;
            TestContext.Out.WriteLine("{0}", device.Id);
            Assert.AreEqual(6, device.Id);
            Assert.AreEqual("Device6", device.Name);
            Assert.AreEqual("Brand2", device.Brand);

            Assert.Pass();
        }

        [Test]
        public void TestPut()
        {
            // get device
            HttpResponseMessage response = _client.GetAsync("devices/4").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            Device dev = response.Content.ReadAsAsync<Device>().Result;
            Assert.AreEqual(4, dev.Id);
            Assert.AreEqual("Device4", dev.Name);
            Assert.AreEqual("Brand2", dev.Brand);

            // test changing name
            response = _client.PutAsync("devices/4", new StringContent("{\"id\":4, \"name\":\"Device44\"}", UnicodeEncoding.UTF8, MediaTypeNames.Application.Json)).Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // get device
            response = _client.GetAsync("devices/4").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            dev = response.Content.ReadAsAsync<Device>().Result;
            Assert.AreEqual(4, dev.Id);
            Assert.AreEqual("Device44", dev.Name);
            Assert.AreEqual("Brand2", dev.Brand);
            Assert.IsNotNull(dev.UpdatedOn);

            // test changing brand
            response = _client.PutAsync("devices/4", new StringContent("{\"id\":4, \"brand\":\"Brand4\"}", UnicodeEncoding.UTF8, MediaTypeNames.Application.Json)).Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // get device
            response = _client.GetAsync("devices/4").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            dev = response.Content.ReadAsAsync<Device>().Result;
            Assert.AreEqual(4, dev.Id);
            Assert.AreEqual("Device44", dev.Name);
            Assert.AreEqual("Brand4", dev.Brand);

            // test changing brand and name
            response = _client.PutAsync("devices/4", new StringContent("{\"id\":4, \"name\":\"Device4\", \"brand\":\"Brand3\"}", UnicodeEncoding.UTF8, MediaTypeNames.Application.Json)).Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            // get device
            response = _client.GetAsync("devices/4").Result;
            if (!response.IsSuccessStatusCode)
            {
                TestContext.Out.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Assert.Fail();
                return;
            }

            dev = response.Content.ReadAsAsync<Device>().Result;
            Assert.AreEqual(4, dev.Id);
            Assert.AreEqual("Device4", dev.Name);
            Assert.AreEqual("Brand3", dev.Brand);
            Assert.Pass();
        }

        private bool InitializeData()
        {
            StringContent stringContent = CreateDeviceForPost(1, "Device1", "Brand1");
            HttpResponseMessage response = _client.PostAsync("devices", stringContent).Result;
            if (!response.IsSuccessStatusCode) return false;

            stringContent = CreateDeviceForPost(2, "Device2", "Brand1");
            response = _client.PostAsync("devices", stringContent).Result;
            if (!response.IsSuccessStatusCode) return false;

            stringContent = CreateDeviceForPost(3, "Device3", "Brand1");
            response = _client.PostAsync("devices", stringContent).Result;
            if (!response.IsSuccessStatusCode) return false;

            stringContent = CreateDeviceForPost(4, "Device4", "Brand2");
            response = _client.PostAsync("devices", stringContent).Result;
            if (!response.IsSuccessStatusCode) return false;

            stringContent = CreateDeviceForPost(5, "Device5", "Brand2");
            response = _client.PostAsync("devices", stringContent).Result;
            if (!response.IsSuccessStatusCode) return false;

            return true;
        }

        private void DeleteDevice(int id)
        {
            _client.DeleteAsync(string.Format("devices/{0}", id));
        }

        private StringContent CreateDeviceForPost(int id, string name, string brand)
        {
            Device device = new Device
            {
                Id = id,
                Brand = brand,
                Name = name,
                CreatedOn = DateTime.Now
            };

            string devJson = JsonSerializer.Serialize(device);
            return new StringContent(devJson, UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);
        }

    }

}

