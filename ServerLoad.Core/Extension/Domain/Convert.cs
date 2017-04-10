using System;
using ServerLoad.Domain.DataModel;
using ServerLoad.Domain.ViewModel;
namespace ServerLoad.Core.Extension.Domain
{
	public static class Convert
	{
		public static ServerOnlyVm ToServerOnlyVm(this Server server)
		{
			return new ServerOnlyVm()
			{
				Name = server.Name,
				Address = server.Address,
				OperatingSystem = server.OperatingSystem
			};
		}

		public static Server ToDataModel(this ServerOnlyVm server)
		{
			return new Server()
			{
				Name = server.Name,
				Address = server.Address,
				OperatingSystem = server.OperatingSystem
			};
		}

		public static Server CopyNoCheckins(this Server svr)
		{
			Server server = new Server()
			{
				Name = svr.Name,
				Address = svr.Address,
				OperatingSystem = svr.OperatingSystem
			};

			return server;
		}

		public static Checkin ToCheckin(this CheckinActionVm vm)
		{
			return new Checkin()
			{
				CpuUtilization = vm.CpuUtilization,
				RamUtilization = vm.RamUtilization,
				SampleTime = vm.SampleTime
			};
		}
	}
}
