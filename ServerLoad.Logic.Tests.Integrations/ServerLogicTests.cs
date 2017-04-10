using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLoad.Core;
using ServerLoad.Core.Abstraction.Logic;

namespace ServerLoad.Logic.Tests.Integrations
{
    [TestClass]
    public class ServerLogicTests
    {
        private IServerLogic logic;

        public ServerLogicTests()
        {
            logic = new LogicFactory().GetServerLogic();
        }

        [TestMethod]
        public void CheckinMass5DayTest()
        {
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
                            logic.Checkin(new Domain.ViewModel.CheckinActionVm()
                            {
                                ServerName = "My-Server",
                                SampleTime = end.AddDays(day+1).AddHours(hour+1).AddMinutes(m+1).AddSeconds((s+1) * 10),
                                CpuUtilization = new Random(s + m + h * 12).NextDouble(),
                                RamUtilization = new Random(s + m + h * 12).NextDouble()

                            });

                        }
                    }

                }

            }

            Assert.AreEqual(Data.ServerRepo._servers[0].Checkins.Count, 43200);

            var result = logic.Loads("My-Server");

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, Constants.SUCCESS_RESULT);
            Assert.AreNotEqual<int>(result.Data.LastHourAvg.Count, 0);
            Assert.AreNotEqual<int>(result.Data.LastDayAvg.Count, 0);

        }


    }
}