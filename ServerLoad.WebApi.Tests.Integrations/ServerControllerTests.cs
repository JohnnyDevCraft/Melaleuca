using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLoad.WebApi.Controllers;
using ServerLoad.Logic;
using System;
using ServerLoad.Domain.DataModel;

namespace ServerLoad.WebApi.Tests.Integrations
{
    [TestClass]
    public class ServerControllerTests
    {
        [TestMethod]
        public async void TestDefaultLoadAsync()
        {
            var sc = new ServerController(new LogicFactory().GetServerLogic());

            var start = DateTime.Now.AddDays(-5);
            var end = DateTime.Now;

            for (var i = -5; i < 0; i++)
            {

                var day = i;
                for (var h = -24; h < 0; h++)
                {
                    var hour = h;
                    for (var m = -60; m < 0; m++)
                    {

                        for (var s = -6; s < 0; s++)
                        {
                            var result = await sc.Checkin("My-Server", new Checkin()
                            {
                                SampleTime = end.AddDays(day + 1).AddHours(hour + 1).AddMinutes(m + 1).AddSeconds((s + 1) * 10),
                                CpuUtilization = new Random(s + m + h * 12).NextDouble(),
                                RamUtilization = new Random(s + m + h * 12).NextDouble()
                            });

                            Assert.IsNotNull(result);
                        }
                    }

                }

            }



        }

    }
}