using System;
using ServerLoad.Core.Abstraction.Val;
using ServerLoad.Core.Abstraction.Data;
using ServerLoad.Data;
namespace ServerLoad.Val
{
	public class ValFactory : IValFactory
	{
		IRepoFactory _factory;
		IServerVal _serverVal;

		public ValFactory()
		{
			_factory = new RepoFactory();
		}

		public IServerVal GetServerVal()
		{
			if (_serverVal == null)
			{
				_serverVal = new ServerVal(_factory.GetServerRepo());
			}

			return _serverVal;
		}
	}
}
