using System;
namespace ServerLoad.Core.Demand
{
	public static class DemandDouble
	{
		public static Demand<double> IsGreaterThan(this Demand<double> val, double min)
		{
			if (val.Value < min)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MIN_VALUE, val.Name, min.ToString()));
			}
			return val;
		}

		public static Demand<double> IsLessThan(this Demand<double> val, double max)
		{
			if (val.Value > max)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MAX_VALUE, val.Name, max.ToString()));
			}
			return val;
		}

		public static Demand<double> IsGreaterThanOrEqualTo(this Demand<double> val, double min)
		{
			if (val.Value <= min)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MIN_VALUE_EQUAL, val.Name, min.ToString()));
			}
			return val;
		}

		public static Demand<double> IsLessThanOrEqualTo(this Demand<double> val, double max)
		{
			if (val.Value >= max)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.MAX_VALUE_EQUAL, val.Name, max.ToString()));
			}

			return val;

		}
	}
}
