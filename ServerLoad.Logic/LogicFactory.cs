using ServerLoad.Core.Abstraction.Logic;
using ServerLoad.Val;

namespace ServerLoad.Logic
{
	public class LogicFactory : ILogicFactory
	{
		IServerLogic _serverLogic;

		public LogicFactory()
		{
			_serverLogic = new ServerLogic(new ValFactory().GetServerVal());
		}

		public IServerLogic GetServerLogic()
		{
			return _serverLogic;
		}
	}
}
