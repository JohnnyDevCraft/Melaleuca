using System;
namespace ServerLoad.Core.Demand
{
	public static class ValidationMessages
	{
		public const string NULL_OR_EMPTY = "[{0}] must not be null or empty.";
		public const string NULL = "[{0}] must not be null.";
		public const string MIN_CHARS = "[{0}] must have a minimum of [{1}] characters.";
		public const string MAX_CHARS = "[{0}] must have a maximum of [[1}] characters.";
		public const string MIN_VALUE = "The value of [{0}] must be greater than [{1}].";
		public const string MAX_VALUE = "The value of [{0}] must be less than [{1}].";
		public const string MIN_VALUE_EQUAL = "The value of [{0}] must be greater than or equal to [{1}].";
		public const string MAX_VALUE_EQUAL = "The value of [{0}] must be less than or equal to [{1}].";
		public const string DATE_ON_BEFORE = "[{0}] must not be after [{1}].";
		public const string DATE_ON_AFTER = "[{0}] must not be before [{1}].";
		public const string DATE_BEFORE = "[{0}] must not be on or after [{1}].";
		public const string DATE_AFTER = "[{0}] must not be on or before [{1}].";


	}
}
