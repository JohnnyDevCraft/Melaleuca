using System;
using ServerLoad.Core.Abstraction.Data;
namespace ServerLoad.Data
{
	public class RepoFactory : IRepoFactory
	{
		public static IServerRepo _serverRepo { get; set; }

		public RepoFactory()
		{
			_serverRepo = ServerRepo.GetInstance();
		}

		public IServerRepo GetServerRepo()
		{
			return _serverRepo;
		}
	}
}
