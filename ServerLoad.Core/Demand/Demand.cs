using System;
using System.Collections.Generic;
using ServerLoad.Domain.ViewModel;
using ServerLoad.Core.Abstraction.Model;
namespace ServerLoad.Core.Demand
{
	public class Demand<T>
	{
		private Demand(T obj, string name)
		{
			Value = obj;
			ValidationErrors = new List<string>();
			Name = name;
		}

		public static Demand<T> That(T obj, string name)
		{
			return new Demand<T>(obj, name);
		}

		public T Value { get; set; }

		public string Name { get; set; }

		public List<string> ValidationErrors { get; set; }

		public bool HasErrors()
		{
			return ValidationErrors.Count > 0;
		}

		public void Result(IResult result)
		{
			if (HasErrors())
			{
				result.Errors.AddRange(ValidationErrors);
				result.Result = "Failure";
			}
		}
	}
}
