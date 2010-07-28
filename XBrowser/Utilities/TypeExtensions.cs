using System;

namespace XBrowserProject
{
	public static class TypeExtensions
	{
		public static bool IsNullableValueType(this Type type)
		{
			return type.IsValueType && type.IsGenericType && Nullable.GetUnderlyingType(type) != null;
		}

		public static string GetMinimalAssemblyQualifiedName(this Type type)
		{
			string name = type.AssemblyQualifiedName;
			if(name.EndsWith("PublicKeyToken=null"))
				name = name.Substring(0, name.IndexOf(',', name.IndexOf(',') + 1));
			return name;
		}
	}
}