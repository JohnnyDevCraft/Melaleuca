using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLoad.Core;
using ServerLoad.Core.Abstraction.Logic;
using ServerLoad.Core.Abstraction.Model;
using ServerLoad.Core.Abstraction.Val;
using ServerLoad.Core.Extension.Domain;
using ServerLoad.Domain.DataModel;
using ServerLoad.Domain.ViewModel;

namespace ServerLoad.Logic.Tests.Unit
{
    [TestClass]
    public class ServerLogicTests
    {
        private IServerLogic logic;

        [TestInitialize]
        public void setup()
        {
            logic = new ServerLogic(new FakeValidation());
        }

        [TestMethod]
        public void TestLoad()
        {
            var result = logic.Loads("my-server");

            Assert.IsNotNull(result);
            Assert.AreEqual(Constants.SUCCESS_RESULT, result.Result);
            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(5, result.Data.LastHourAvg.Count);
        }

        public class FakeValidation : IServerVal
        {
            #region Implementation of IServerVal

            public ResultVm<bool> AddServer(ServerOnlyVm server)
            {
                return null;
            }

            public ResultVm<bool> UpdateServer(string serverName, string address)
            {
                return null;
            }

            public ResultVm<bool> AddCheckin(string serverName, Checkin checkin)
            {
                return null;
            }

            public ResultVm<List<ServerOnlyVm>> GetServers()
            {
                return null;
            }

            public ResultVm<ServerOnlyVm> GetServer(string name)
            {
                return null;
            }

            public ResultVm<Server> GetServerMetrics(string serverName, DateTime start, DateTime end)
            {
                var server = new Server()
                {
                    Name = serverName,
                    Address = "127.0.0.1",
                    Checkins = new List<Checkin>(),
                    OperatingSystem = "Win95"
                };

                server.Checkins.Add(new Checkin()
                {
                    CpuUtilization = 0.67D,
                    RamUtilization = 0.05D,
                    SampleTime = DateTime.Now.AddMinutes(-5)
                });

                server.Checkins.Add(new Checkin()
                {
                    CpuUtilization = 0.67D,
                    RamUtilization = 0.05D,
                    SampleTime = DateTime.Now.AddMinutes(-4)
                });

                server.Checkins.Add(new Checkin()
                {
                    CpuUtilization = 0.67D,
                    RamUtilization = 0.05D,
                    SampleTime = DateTime.Now.AddMinutes(-3)
                });

                server.Checkins.Add(new Checkin()
                {
                    CpuUtilization = 0.67D,
                    RamUtilization = 0.05D,
                    SampleTime = DateTime.Now.AddMinutes(-2)
                });

                server.Checkins.Add(new Checkin()
                {
                    CpuUtilization = 0.67D,
                    RamUtilization = 0.05D,
                    SampleTime = DateTime.Now.AddMinutes(-1)
                });

                return new ResultVm<Server>().FromSuccessObject(server);
            }

            public ResultVm<bool> RemoveServer(string name)
            {
                return null;
            }

            public ResultVm<bool> ClearLogs(string serverName)
            {
                return null;
            }

            #endregion
        }
    }
}