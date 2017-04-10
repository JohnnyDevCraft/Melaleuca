using System;
using System.Collections.Generic;
using ServerLoad.Core.Abstraction.Model;

namespace ServerLoad.Core.Extension.Domain
{
	public static class ResultExtenders
	{
		public static ResultVm<T> FromException<T>(this ResultVm<T> rvm, Exception ex)
		{
			rvm.Result = Constants.FAILED_RESULT;
			rvm.Errors = new List<string>();
			rvm.Errors.Add(GetErrorString(ex, 0));

			return rvm;
		}

		public static ResultVm<T> FromErrorString<T>(this ResultVm<T> rvm, string error)
		{
			rvm.Result = Constants.FAILED_RESULT;
			rvm.Errors = new List<string>();
			rvm.Errors.Add(error);

			return rvm;
		}

		public static ResultVm<T> FromEmptySuccess<T>(this ResultVm<T> rvm)
		{
			rvm.Result = Constants.SUCCESS_RESULT;

			return rvm;
		}
		public static ResultVm<T> FromEmptyFailure<T>(this ResultVm<T> rvm)
		{
			rvm.Result = Constants.FAILED_RESULT;

			return rvm;
		}

		public static ResultVm<T> FromSuccessObject<T>(this ResultVm<T> rvm, T obj)
		{
			rvm.Result = Constants.SUCCESS_RESULT;
			rvm.Data = obj;

			return rvm;
		}

		public static ResultVm<List<T>> FromSuccessList<T>(this ResultVm<List<T>> rvm, List<T> objs)
		{
			rvm.Result = Constants.SUCCESS_RESULT;
			rvm.Data = objs;

			return rvm;
		}

		public static ResultVm<int> FromSuccessInt(this ResultVm<int> rvm, int val)
		{
			rvm.Result = Constants.SUCCESS_RESULT;
			rvm.Data = val;

			return rvm;
		}


		private static string GetErrorString(Exception ex, int level)
		{
			string result = level == 0 ? $"-{ex.Message}" : $"\n{GetTabs(level)}-{ex.Message}";

			if (ex.InnerException != null)
			{
				result = $"{result}{GetErrorString(ex.InnerException, level + 1)}";
			}

			return result;
		}

		private static string GetTabs(int level)
		{
			string str = "";

			for (var i = 0; i < level; i++)
			{
				str = str + "\t";
			}

			return str;
		}
	}
}
