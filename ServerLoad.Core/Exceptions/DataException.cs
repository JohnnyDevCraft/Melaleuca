using System;
namespace ServerLoad.Core.Exceptions
{
	public class DataException
	{
		public const string NULL_REFERENCE_CREATE = "Unable to add new {0}.  The value provided for {1} did not return any results.";
		public const string GENERAL_ERROR = "Unable to complete request.  Please contact your administrator.";
		public const string OBJECT_EXISTS = "{0} object already exists.  Please reference it using it's name.";
		public const string OBJECT_NON_EXISTANT = "{0} [{0}] does not exist in this system.";
	}
}
