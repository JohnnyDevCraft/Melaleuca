using System;
using System.Collections.Generic;

namespace ServerLoad.Core.Abstraction.Model
{
	public interface IResult
	{
		string Result { get; set; }
		List<string> Errors { get; set; }
	}
}
