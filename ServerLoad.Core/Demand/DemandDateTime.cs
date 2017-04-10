using System;
namespace ServerLoad.Core.Demand
{
	public static class DemandDateTime
	{
		public static Demand<DateTime> IsBefore(this Demand<DateTime> val, DateTime end)
		{
			if (val.Value > end)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.DATE_BEFORE, val.Name, end.ToString("G")));
			}
			return val;
		}

		public static Demand<DateTime> IsOnOrBefore(this Demand<DateTime> val, DateTime end)
		{
			if (val.Value >= end)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.DATE_ON_BEFORE, val.Name, end.ToString("G")));
			}
			return val;
		}

		public static Demand<DateTime> IsAfter(this Demand<DateTime> val, DateTime start)
		{
			if (val.Value < start)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.DATE_AFTER, val.Name, start.ToString("G")));
			}
			return val;
		}

		public static Demand<DateTime> IsOnOrAfter(this Demand<DateTime> val, DateTime start)
		{
			if (val.Value <= start)
			{
				val.ValidationErrors.Add(string.Format(ValidationMessages.DATE_ON_AFTER, val.Name, start.ToString("G")));
			}
			return val;
		}


	}
}
