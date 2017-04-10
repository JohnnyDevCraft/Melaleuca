using System;
using System.Collections.Generic;
using ServerLoad.Domain.DataModel;
using ServerLoad.Core.Abstraction.Data;
using System.Linq;
using ServerLoad.Core.Exceptions;
using ServerLoad.Core.Extension.Domain;
namespace ServerLoad.Data
{
	public class ServerRepo : IServerRepo
	{
		public static List<Server> _servers;

		static ServerRepo _instance;

		private ServerRepo()
		{
			_servers = new List<Server>();
		}

		public static ServerRepo GetInstance()
		{

			if (_instance == null)
			{
				_instance = new ServerRepo();
			}

			return _instance;
		}



		public bool AddCheckin(string serverName, Checkin checkin)
		{
			try
			{
				_servers.SingleOrDefault(s => s.Name == serverName).Checkins.Add(checkin);
				return true;
			}
			catch (NullReferenceException)
			{
				throw new ArgumentException(string.Format(DataException.NULL_REFERENCE_CREATE, "Checkin", "ServerName"));
			}
			catch (Exception)
			{
				throw new Exception(DataException.GENERAL_ERROR);
			}
		}

		public bool AddServer(Server server)
		{
			try
			{
				if (_servers.Any(s => s.Name == server.Name))
				{
					throw new ArgumentException(string.Format(DataException.OBJECT_EXISTS, "Server"));
				}
				if (server.Checkins == null) server.Checkins = new List<Checkin>();
				_servers.Add(server);
				return true;
			}
			catch
			{
				throw new Exception(DataException.GENERAL_ERROR);
			}
		}

		public bool ClearLogs(string serverName)
		{
			try
			{
				_servers.SingleOrDefault(s => s.Name == serverName).Checkins.Clear();
				return true;
			}
			catch (NullReferenceException)
			{
				throw new ArgumentException(string.Format(DataException.OBJECT_NON_EXISTANT, "Server", serverName));
			}
			catch
			{
				throw new Exception(DataException.GENERAL_ERROR);
			}
		}

		public Server GetServer(string name)
		{
			try
			{
				Server svr = _servers.SingleOrDefault(s => s.Name == name);
				if (svr == null)
				{
					throw new ArgumentException(string.Format(DataException.OBJECT_NON_EXISTANT, "Server", name));
				}
				return svr;
			}
			catch
			{
				throw new Exception(DataException.GENERAL_ERROR);
			}
		}

		public Server GetServerMetrics(string serverName, DateTime start, DateTime end)
		{
			try
			{
				var server = _servers.SingleOrDefault(s => s.Name == serverName).CopyNoCheckins();
				server.Checkins = _servers.SingleOrDefault(s => s.Name == serverName)
					.Checkins.Where(c => c.SampleTime >= start && c.SampleTime <= end).ToList();
				return server;
			}
			catch (NullReferenceException)
			{
				throw new ArgumentException(string.Format(DataException.OBJECT_NON_EXISTANT, "Server", serverName));
			}
			catch
			{
				throw new Exception(DataException.GENERAL_ERROR);
			}
		}

		public List<Server> GetServers()
		{
			try
			{
				return _servers;
			}
			catch
			{
				throw new Exception(DataException.GENERAL_ERROR);
			}
		}

		public bool RemoveServer(string name)
		{
			try
			{
				_servers.Remove(_servers.SingleOrDefault(s => s.Name == name));
				return true;
			}
			catch (NullReferenceException)
			{
				throw new ArgumentException(string.Format(DataException.OBJECT_NON_EXISTANT, "Server", name));
			}
			catch
			{
				throw new Exception(DataException.GENERAL_ERROR);
			}
		}

		public bool UpdateServer(string serverName, string address)
		{
			try
			{
				_servers.SingleOrDefault(s => s.Name == serverName).Address = address;
				return true;
			}
			catch (NullReferenceException)
			{
				throw new ArgumentException(string.Format(DataException.OBJECT_NON_EXISTANT, "Server", serverName));
			}
			catch
			{
				throw new Exception(DataException.GENERAL_ERROR);
			}
		}


	}
}
