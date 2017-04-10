using System;
using System.Collections.Generic;
using ServerLoad.Core.Abstraction.Data;
using ServerLoad.Core.Abstraction.Val;
using ServerLoad.Domain.DataModel;
using ServerLoad.Domain.ViewModel;
using ServerLoad.Core.Demand;
using R = ServerLoad.Core.Abstraction.Model;
using ServerLoad.Core.Extension.Domain;
using System.Linq;
using ServerLoad.Core.Abstraction.Model;
using ServerLoad.Core;

namespace ServerLoad.Val
{
	public class ServerVal : IServerVal
	{
		IServerRepo _repo;

		public ServerVal(IServerRepo repo)
		{
			_repo = repo;
		}

		public R.ResultVm<bool> AddCheckin(string serverName, Checkin checkin)
		{
			var result = new R.ResultVm<bool>().FromEmptyFailure();

			Demand<string>.That(serverName, "serverName").HasNonEmptyValue().HasMaxChars(255).Result(result);
			Demand<double>.That(checkin.RamUtilization, "checkin.RamUtilization").IsGreaterThan(0.0D).Result(result);
			Demand<double>.That(checkin.CpuUtilization, "checkin.CpuUtilization").IsGreaterThan(0.0D).IsLessThanOrEqualTo(1.0D).Result(result);
			//Demand<DateTime>.That(checkin.SampleTime, "checkin.SampleTime").IsAfter(DateTime.Now.AddMinutes(-10.0D)).IsBefore(DateTime.Now.AddMinutes(10.00D)).Result(result);

			if (result.Errors.Count == 0)
			{
				try
				{
					result = new ResultVm<bool>().FromSuccessObject(_repo.AddCheckin(serverName, checkin));
				}
				catch (Exception ex)
				{
				    result = new ResultVm<bool>().FromException(ex);
				}
			}
			else
			{
				result.Data = false;
			}

			return result;

		}

		public R.ResultVm<bool> AddServer(ServerOnlyVm server)
		{
			var rv = new ResultVm<bool>().FromEmptyFailure();
			Demand<string>.That(server.Name, "server.Name").HasNonEmptyValue().Result(rv);

			if (rv.Errors.Count == 0)
			{
				try
				{
					return new ResultVm<bool>().FromSuccessObject(_repo.AddServer(server.ToDataModel()));
				}
				catch (Exception ex)
				{
					return new ResultVm<bool>().FromException(ex);
				}
			}
			else
			{
				return rv;
			}




		}

		public R.ResultVm<bool> ClearLogs(string serverName)
		{
			var result = new R.ResultVm<bool>().FromEmptyFailure();

			Demand<string>.That(serverName, "serverName").HasNonEmptyValue().Result(result);

			if (result.Errors.Count == 0)
			{
				try
				{
					return new ResultVm<bool>().FromSuccessObject( _repo.ClearLogs(serverName));
				}
				catch (Exception ex)
				{
				    return new ResultVm<bool>().FromException(ex);

				}
			}
			else
			{
				result.Data = false;
			}

			return result;
		}

		public R.ResultVm<ServerOnlyVm> GetServer(string name)
		{
			var result = new R.ResultVm<ServerOnlyVm>().FromEmptyFailure();

			Demand<string>.That(name, "name").HasNonEmptyValue().Result(result);

			if (result.Errors.Count == 0)
			{
				try
				{
					return new ResultVm<ServerOnlyVm>().FromSuccessObject(_repo.GetServer(name).ToServerOnlyVm());
				}
				catch (Exception ex)
				{
				    return new ResultVm<ServerOnlyVm>().FromException(ex);
				}
			}

			return result;
		}

		public R.ResultVm<Server> GetServerMetrics(string serverName, DateTime start, DateTime end)
		{
			var result = new R.ResultVm<Server>().FromEmptyFailure();

			Demand<string>.That(serverName, "serverName").HasNonEmptyValue().Result(result);
			Demand<DateTime>.That(start, "start").IsBefore(end).Result(result);

			if (result.Errors.Count == 0)
			{
				try
				{
					return new ResultVm<Server>().FromSuccessObject(_repo.GetServerMetrics(serverName, start, end));
				}
				catch (Exception ex)
				{
				    return new ResultVm<Server>().FromException(ex);
				}
			}

			return result;
		}

		public R.ResultVm<List<ServerOnlyVm>> GetServers()
		{
			var result = new R.ResultVm<List<ServerOnlyVm>>().FromEmptyFailure();

			if (result.Errors.Count == 0)
			{
				try
				{
					return new ResultVm<List<ServerOnlyVm>>().FromSuccessObject(_repo.GetServers().Select(s => s.ToServerOnlyVm()).ToList());
				}
				catch (Exception ex)
				{
				    return new ResultVm<List<ServerOnlyVm>>().FromException(ex);
				}
			}

			return result;
		}

		public R.ResultVm<bool> RemoveServer(string name)
		{
			var result = new R.ResultVm<bool>().FromEmptyFailure();

			Demand<string>.That(name, "name").HasNonEmptyValue().Result(result);

			if (result.Errors.Count == 0)
			{
				try
				{
					return new ResultVm<bool>().FromSuccessObject(_repo.RemoveServer(name));
				}
				catch (Exception ex)
				{
				    return new ResultVm<bool>().FromException(ex);
				}
			}

			return result;
		}

		public R.ResultVm<bool> UpdateServer(string serverName, string address)
		{
			var result = new R.ResultVm<bool>().FromEmptyFailure();

			Demand<string>.That(serverName, "serverName").HasNonEmptyValue().Result(result);

			if (result.Errors.Count == 0)
			{
				try
				{
					return new ResultVm<bool>().FromSuccessObject(_repo.UpdateServer(serverName, address));
				}
				catch (Exception ex)
				{
				    return new ResultVm<bool>().FromException(ex);
				}
			}

			return result;
		}
	}
}
