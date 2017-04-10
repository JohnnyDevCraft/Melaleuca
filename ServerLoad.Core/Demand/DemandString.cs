using System;
namespace ServerLoad.Core.Demand
{
	public static class DemandString
	{
		public static Demand<string> HasNonEmptyValue(this Demand<string> val)
		{
			if (string.IsNullOrWhiteSpace(val.Value))
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.NULL_OR_EMPTY, val.Name));
			}
			return val;
		}

		public static Demand<string> HasMaxChars(this Demand<string> val, int max)
		{
			if (val.Value.Length > max)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MAX_CHARS, val.Name, max.ToString()));
			}
			return val;
		}

		public static Demand<string> HasMinChars(this Demand<string> val, int min)
		{
			if (val.Value.Length < min)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MIN_CHARS, val.Name, min.ToString()));
			}
			return val;
		}
	}
}
