using System;
namespace ServerLoad.Core.Abstraction.Data
{
	public interface IRepoFactory
	{
		IServerRepo GetServerRepo();
	}
}
