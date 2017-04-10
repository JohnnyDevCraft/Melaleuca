using System;

namespace ServerLoad.Domain.DataModel
{
	public class Checkin
	{
		public DateTime SampleTime { get; set; }
		public double CpuUtilization { get; set; }
		public double RamUtilization { get; set; }
	}
}
