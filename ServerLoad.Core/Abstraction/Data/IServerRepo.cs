using System;
using ServerLoad.Domain.DataModel;
using System.Collections.Generic;
namespace ServerLoad.Core.Abstraction.Data
{
	public interface IServerRepo
	{
		bool AddServer(Server server);
		bool UpdateServer(string serverName, string address);
		bool AddCheckin(string serverName, Checkin checkin);
		List<Server> GetServers();
		Server GetServer(string name);
		Server GetServerMetrics(string serverName, DateTime start, DateTime end);
		bool RemoveServer(string name);
		bool ClearLogs(string serverName);
	}
}
