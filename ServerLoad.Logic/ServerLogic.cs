using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerLoad.Core.Abstraction.Logic;
using ServerLoad.Domain.ViewModel;
using ServerLoad.Core.Abstraction.Val;
using ServerLoad.Core;
using ServerLoad.Domain.DataModel;
using ServerLoad.Core.Extension.Domain;
using ServerLoad.Core.Abstraction.Model;
using System.Linq;

namespace ServerLoad.Logic
{
	public class ServerLogic : IServerLogic
	{
		private IServerVal _val;

		public ServerLogic(IServerVal val)
		{
			_val = val;
		}

		public ResultVm<bool> Checkin(CheckinActionVm model)
		{
			var getServerResult = _val.GetServer(model.ServerName);

			if (getServerResult.Result == Constants.SUCCESS_RESULT && getServerResult.Data != null)
			{
				var result = _val.AddCheckin(model.ServerName, model.ToCheckin());
				return result;
			}

			if (_val.AddServer(new ServerOnlyVm() { Name = model.ServerName }).Result == Constants.SUCCESS_RESULT)
			{
				var result = _val.AddCheckin(model.ServerName, model.ToCheckin());
				return result;
			}

			return new ResultVm<bool>().FromErrorString(Constants.NON_EXISTENT_UNABLE_TO_CREATE);
		}

		public async Task<ResultVm<bool>> CheckinAsync(CheckinActionVm model)
		{
			return await Task.Run(() =>
			{
				var getServerResult = _val.GetServer(model.ServerName);

				if (getServerResult.Result == Constants.SUCCESS_RESULT && getServerResult.Data != null)
				{
					var result = _val.AddCheckin(model.ServerName, model.ToCheckin());
					return result;
				}

				if (_val.AddServer(new ServerOnlyVm() { Name = model.ServerName }).Result == Constants.SUCCESS_RESULT)
				{
					var result = _val.AddCheckin(model.ServerName, model.ToCheckin());
					return result;
				}

				return new ResultVm<bool>().FromErrorString(Constants.NON_EXISTENT_UNABLE_TO_CREATE);
			});
		}

		public ResultVm<ServerWithAvgLoadsVm> Loads(string serverName)
		{
			try
			{
				var now = DateTime.Now;

				var serverResult = _val.GetServerMetrics(serverName, now.AddHours(-1), now);

				var result = new ResultVm<ServerWithAvgLoadsVm>().FromSuccessObject(new ServerWithAvgLoadsVm()
				{
					ServerName = serverName,
					LastDayAvg = GetLast24Hours(now, serverResult.Data.Checkins),
					LastHourAvg = GetLast60Minutes(now, serverResult.Data.Checkins)
				});

				return result;
			}
			catch (Exception ex)
			{
				return new ResultVm<ServerWithAvgLoadsVm>().FromException(ex);
			}

		}

		public async Task<ResultVm<ServerWithAvgLoadsVm>> LoadsAsync(string serverName)
		{
			return await Task.Run(() =>
			{
				try
				{
					var now = DateTime.Now;

					var serverResult = _val.GetServerMetrics(serverName, now.AddHours(-1), now);

					return new ResultVm<ServerWithAvgLoadsVm>().FromSuccessObject(new ServerWithAvgLoadsVm()
					{
						ServerName = serverName,
						LastDayAvg = GetLast24Hours(now, serverResult.Data.Checkins),
						LastHourAvg = GetLast60Minutes(now, serverResult.Data.Checkins)
					});
				}
				catch (Exception ex)
				{
					return new ResultVm<ServerWithAvgLoadsVm>().FromException(ex);
				}
			});
		}

		public ResultVm<List<AverageLoadVm>> LoadsByHour(string serverName, DateTime start, DateTime end)
		{
			try
			{
				var serverResult = _val.GetServerMetrics(serverName, start, end);

				return new ResultVm<List<AverageLoadVm>>().FromSuccessObject(GetAvgByHour(start, end, serverResult.Data.Checkins));
			}
			catch (Exception ex)
			{
				return new ResultVm<List<AverageLoadVm>>().FromException(ex);
			}
		}

		public async Task<ResultVm<List<AverageLoadVm>>> LoadsByHourAsync(string serverName, DateTime start, DateTime end)
		{
			return await Task.Run(() =>
			{
				try
				{
					var serverResult = _val.GetServerMetrics(serverName, start, end);

					return new ResultVm<List<AverageLoadVm>>().FromSuccessObject(GetAvgByHour(start, end, serverResult.Data.Checkins));
				}
				catch (Exception ex)
				{
					return new ResultVm<List<AverageLoadVm>>().FromException(ex);
				}
			});

		}

		public ResultVm<List<AverageLoadVm>> LoadsByMinute(string serverName, DateTime start, DateTime end)
		{
			try
			{
				var serverResult = _val.GetServerMetrics(serverName, start, end);

				return new ResultVm<List<AverageLoadVm>>().FromSuccessObject(GetAvgByMinute(start, end, serverResult.Data.Checkins));
			}
			catch (Exception ex)
			{
				return new ResultVm<List<AverageLoadVm>>().FromException(ex);
			}
		}

		public async Task<ResultVm<List<AverageLoadVm>>> LoadsByMinuteAsync(string serverName, DateTime start, DateTime end)
		{
			return await Task.Run(() =>
			{
				try
				{
					var serverResult = _val.GetServerMetrics(serverName, start, end);

					return new ResultVm<List<AverageLoadVm>>().FromSuccessObject(GetAvgByMinute(start, end, serverResult.Data.Checkins));
				}
				catch (Exception ex)
				{
					return new ResultVm<List<AverageLoadVm>>().FromException(ex);
				}
			});
		}

		public ResultVm<List<ServerOnlyVm>> Servers()
		{
			return _val.GetServers();
		}

		public async Task<ResultVm<List<ServerOnlyVm>>> ServersAsync()
		{
			return await Task.Run(() => { return _val.GetServers(); });
		}




		static List<AverageLoadVm> GetLast60Minutes(DateTime time, List<Checkin> checkins)
		{
			List<AverageLoadVm> list = new List<AverageLoadVm>();

			for (var i = -60; i < 0; i++)
			{
				var timeIndex = time.AddMinutes(i);

				var avg = new AverageLoadVm()
				{
					AvgCpuLoad = checkins.Where(c => c.SampleTime > timeIndex &&
									c.SampleTime < timeIndex.AddMinutes(1))
								 .Average(c => c.CpuUtilization),
					AvgMemLoad = checkins.Where(c => c.SampleTime > timeIndex &&
									c.SampleTime < timeIndex.AddMinutes(1))
								 .Average(c => c.RamUtilization),
					TimeIndex = timeIndex,
					Increment = 1
				};

				list.Add(avg);
			}

			return list;
		}

		static List<AverageLoadVm> GetLast24Hours(DateTime time, List<Checkin> checkins)
		{
			List<AverageLoadVm> list = new List<AverageLoadVm>();

			for (var i = -24; i < 0; i++)
			{
				var timeIndex = time.AddHours(i);

				var avg = new AverageLoadVm()
				{
					AvgCpuLoad = checkins.Where(c => c.SampleTime > timeIndex &&
								    c.SampleTime < timeIndex.AddHours(1))
								 .Average(c => c.CpuUtilization),
					AvgMemLoad = checkins.Where(c => c.SampleTime > timeIndex &&
									c.SampleTime < timeIndex.AddHours(1))
								 .Average(c => c.RamUtilization),
					TimeIndex = timeIndex,
					Increment = 1
				};

				list.Add(avg);
			}

			return list;
		}

		static List<AverageLoadVm> GetAvgByHour(DateTime start, DateTime end, List<Checkin> checkins)
		{
			var ts = end - start;
			var minutes = ts.Minutes % 60;
			var hours = ts.Hours;

			List<AverageLoadVm> list = new List<AverageLoadVm>();

			var first = new AverageLoadVm()
			{
				AvgCpuLoad = checkins.Where(c => c.SampleTime > start &&
								c.SampleTime < start.AddMinutes(minutes))
								 .Average(c => c.CpuUtilization),
				AvgMemLoad = checkins.Where(c => c.SampleTime > start &&
								c.SampleTime < start.AddMinutes(minutes))
								 .Average(c => c.RamUtilization),
				TimeIndex = start,
				Increment = minutes
			};

			list.Add(first);

			for (var i = hours * -1; i < 0; i++)
			{
				var timeIndex = end.AddHours(i);

				var avg = new AverageLoadVm()
				{
					AvgCpuLoad = checkins.Where(c => c.SampleTime > timeIndex &&
									c.SampleTime < timeIndex.AddHours(1))
								 .Average(c => c.CpuUtilization),
					AvgMemLoad = checkins.Where(c => c.SampleTime > timeIndex &&
									c.SampleTime < timeIndex.AddHours(1))
								 .Average(c => c.RamUtilization),
					TimeIndex = timeIndex,
					Increment = 60
				};

				list.Add(avg);
			}

			return list;
		}

		static List<AverageLoadVm> GetAvgByMinute(DateTime start, DateTime end, List<Checkin> checkins)
		{
			var ts = end - start;
			var minutes = ts.Minutes;

			List<AverageLoadVm> list = new List<AverageLoadVm>();


			for (var i = minutes * -1; i < 0; i++)
			{
				var timeIndex = end.AddMinutes(i);

				var avg = new AverageLoadVm()
				{
					AvgCpuLoad = checkins.Where(c => c.SampleTime > timeIndex &&
									c.SampleTime < timeIndex.AddMinutes(1))
								 .Average(c => c.CpuUtilization),
					AvgMemLoad = checkins.Where(c => c.SampleTime > timeIndex &&
									c.SampleTime < timeIndex.AddMinutes(1))
								 .Average(c => c.RamUtilization),
					TimeIndex = timeIndex,
					Increment = 1
				};

				list.Add(avg);
			}

			return list;
		}

	}
}
