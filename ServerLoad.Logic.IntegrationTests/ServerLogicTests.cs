using System;
using Xunit;
using ServerLoad.Core.Abstraction.Val;
using ServerLoad.Core.Abstraction.Logic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ServerLoad.Core;



namespace ServerLoad.Logic.IntegrationTests
{
	public class ServerLogicTests
	{
		IServerLogic _log;

		public ServerLogicTests()
		{
			_log = new LogicFactory().GetServerLogic();
		}


		[Fact]
		public void MassTest5DayCheckin()
		{

			DateTime start = DateTime.Now.AddDays(-5);
			DateTime end = DateTime.Now;

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
							_log.Checkin(new Domain.ViewModel.CheckinActionVm()
							{
								ServerName = "My-Server",
								SampleTime = end.AddDays(day).AddHours(hour).AddMinutes(m).AddSeconds(s * 10),
								CpuUtilization = new Random(s + m + h * 12).NextDouble(),
								RamUtilization = new Random(s + m + h * 12).NextDouble()

							});

						}
					}

				}

			}

			Assert.Equal(Data.ServerRepo._servers[0].Checkins.Count, 43200);

			var result = _log.Loads("My-Server");

			Assert.NotNull(result);
			Assert.Equal(result.Result, Constants.SUCCESS_RESULT);
			Assert.NotEmpty(result.Data.LastHourAvg);
			Assert.NotEmpty(result.Data.LastDayAvg);

		}
	}
}
