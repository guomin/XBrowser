using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;

namespace XBrowserProject
{
	public static class ObjectExtensions
	{
		public static bool EqualsAny(this object source, params object[] comparisons)
		{
			return comparisons.Any(o => Equals(source, o));
		}

		public static NameValueCollection ToNameValueCollection(this object o)
		{
			var nvc = new NameValueCollection();
			foreach(var p in o.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance))
				nvc.Add(p.Name, (p.GetValue(o, null) ?? "").ToString());
			return nvc;
		}

		public static PropertyInfo[] GetSettableProperties(this object o)
		{
			return o.GetType()
				.GetProperties(BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.GetSetMethod() != null)
				.ToArray();
		}

		public static string ToQueryString(this object o)
		{
			return o.ToNameValueCollection().ToQueryString();
		}

		public static string ToJson(this object obj)
		{
			return new JavaScriptSerializer().Serialize(obj);
		}

		public static T DuckTypeAs<T>(this object o)
		{
			var jss = new JavaScriptSerializer();
			return jss.Deserialize<T>(jss.Serialize(o));
		}

		/// <summary>
		/// Calculates the number of full months between two dates
		/// </summary>
		public static int MonthsFrom(this DateTime baseDate, DateTime date)
		{
			// Set date to "0001/01/01"
			DateTime systemStartDate = new DateTime();

			// Avoid a negative value for time difference
			TimeSpan timeDifference = date > baseDate ? date.Subtract(baseDate) : baseDate.Subtract(date);

			// Generate a date from the systemStartDate 
			DateTime generatedDate = systemStartDate.Add(timeDifference);

			// Substract 1 because the systemStartDate is "0001/01/01"
			int noOfYears = generatedDate.Year - 1;
			int noOfMonths = generatedDate.Month - 1;

			noOfMonths = noOfMonths + (noOfYears * 12);

			return noOfMonths;
		}
	}
}