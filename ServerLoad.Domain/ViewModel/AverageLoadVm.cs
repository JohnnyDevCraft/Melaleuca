using System;
namespace ServerLoad.Domain.ViewModel
{
	public class AverageLoadVm
	{
		public int Increment { get; set; }
		public DateTime TimeIndex { get; set; }
		public double AvgCpuLoad { get; set; }
		public double AvgMemLoad { get; set; }
	}
}
