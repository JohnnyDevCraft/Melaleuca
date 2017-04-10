using System;
using System.Collections.Generic;
namespace ServerLoad.Core.Abstraction.Model
{
	public class ResultVm<T> : IResult
	{
		public ResultVm()
		{
			Errors = new List<string>();
		}

		public string Result { get; set; }
		public List<string> Errors { get; set; }
		public T Data { get; set; }
	}
}
