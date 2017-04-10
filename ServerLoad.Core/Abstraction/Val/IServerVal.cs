using System;
using System.Collections.Generic;
using ServerLoad.Domain.DataModel;
using ServerLoad.Domain.ViewModel;
using R = ServerLoad.Core.Abstraction.Model;
namespace ServerLoad.Core.Abstraction.Val
{
	public interface IServerVal
	{
		R.ResultVm<bool> AddServer(ServerOnlyVm server);
		R.ResultVm<bool> UpdateServer(string serverName, string address);
		R.ResultVm<bool> AddCheckin(string serverName, Checkin checkin);
		R.ResultVm<List<ServerOnlyVm>> GetServers();
		R.ResultVm<ServerOnlyVm> GetServer(string name);
		R.ResultVm<Server> GetServerMetrics(string serverName, DateTime start, DateTime end);
		R.ResultVm<bool> RemoveServer(string name);
		R.ResultVm<bool> ClearLogs(string serverName);
	}
}
