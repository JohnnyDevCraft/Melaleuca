using System;
using System.Collections.Generic;
namespace ServerLoad.Domain.ViewModel
{
	public class ServerWithAvgLoadsVm
	{
		public string ServerName { get; set; }
		public List<AverageLoadVm> LastHourAvg { get; set; }
		public List<AverageLoadVm> LastDayAvg { get; set; }
	}
}
