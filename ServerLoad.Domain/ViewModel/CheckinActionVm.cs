using System;
namespace ServerLoad.Domain.ViewModel
{
	public class CheckinActionVm
	{
		public string ServerName { get; set; }
		public DateTime SampleTime { get; set; }
		public double CpuUtilization { get; set; }
		public double RamUtilization { get; set; }
	}

}
