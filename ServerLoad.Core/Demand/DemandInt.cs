using System;
namespace ServerLoad.Core.Demand
{
	public static class DemandInt
	{
		public static Demand<int> IsGreaterThan(this Demand<int> val, int min)
		{
			if (val.Value < min)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MIN_VALUE, val.Name, min.ToString()));
			}
			return val;
		}

		public static Demand<int> IsLessThan(this Demand<int> val, int max)
		{
			if (val.Value > max)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MAX_VALUE, val.Name, max.ToString()));
			}
			return val;
		}
		public static Demand<int> IsGreaterThanOrEqualTo(this Demand<int> val, int min)
		{
			if (val.Value <= min)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MIN_VALUE, val.Name, min.ToString()));
			}
			return val;
		}

		public static Demand<int> IsLessThanOrEqualTo(this Demand<int> val, int max)
		{
			if (val.Value >= max)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MAX_VALUE_EQUAL, val.Name, max.ToString()));
			}
			return val;
		}
	}
}
