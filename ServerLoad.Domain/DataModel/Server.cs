using System;
using System.Collections.Generic;
namespace ServerLoad.Domain.DataModel
{
	public class Server
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string OperatingSystem { get; set; }

		public List<Checkin> Checkins { get; set; }
	}
}
