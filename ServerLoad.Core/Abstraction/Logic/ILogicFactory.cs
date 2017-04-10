using System;
namespace ServerLoad.Core.Abstraction.Logic
{
	public interface ILogicFactory
	{
		IServerLogic GetServerLogic();
	}
}
