using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerLoad.Core.Abstraction.Model;
using ServerLoad.Domain.ViewModel;
namespace ServerLoad.Core.Abstraction.Logic
{
	public interface IServerLogic
	{
		ResultVm<List<AverageLoadVm>> LoadsByHour(string serverName, DateTime start, DateTime end);
		ResultVm<ServerWithAvgLoadsVm> Loads(string serverName);
		ResultVm<List<AverageLoadVm>> LoadsByMinute(string serverName, DateTime start, DateTime end);
		ResultVm<List<ServerOnlyVm>> Servers();
		ResultVm<bool> Checkin(CheckinActionVm model);

		Task<ResultVm<List<AverageLoadVm>>> LoadsByHourAsync(string serverName, DateTime start, DateTime end);
		Task<ResultVm<ServerWithAvgLoadsVm>> LoadsAsync(string serverName);
		Task<ResultVm<List<AverageLoadVm>>> LoadsByMinuteAsync(string serverName, DateTime start, DateTime end);
		Task<ResultVm<List<ServerOnlyVm>>> ServersAsync();
		Task<ResultVm<bool>> CheckinAsync(CheckinActionVm model);
	}
}
