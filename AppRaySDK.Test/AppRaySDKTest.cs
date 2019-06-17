using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppRaySDK;
using System.Net.Http;

namespace AppRaySDK.Test
{
    [TestClass]
    public class AppRaySDKTest
    {
        [TestMethod]
        public void LoginTest()
        {
            AppRayClient client = new AppRayClient();
            client.Login();

            Assert.IsFalse(string.IsNullOrWhiteSpace(client.Token));
        }

        [TestMethod]
        public void PingTest()
        {
            AppRayClient client = new AppRayClient();
            double? responseTime = client.Ping();

            Assert.IsNotNull(responseTime);
        }

        [TestMethod]
        public void TestMethod1()
        {
            AppRayClient client = new AppRayClient();

            //var config = client.Job(Guid?.Parse("fd7dcfec-aa8d-11e6-b16f-0242ac110003"))
            //    .GetFileAccesses();

            var result = client.Job(Guid.Parse("a449d906-3ad5-11e7-839b-0242ac110003")).Get();

            //var result = Job.GetAllJobs();

            //var result = client.Job(Guid?.Parse("2548f350-362e-11e7-b648-0242ac110003")).Delete();

            //var result = Job.Submit(@"C:\Users\drdav\Desktop\Projektek\TC\ARP\arp\apks\normal.apk");
        }
    }
}
