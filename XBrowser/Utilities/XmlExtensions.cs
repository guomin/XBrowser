using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XBrowserProject
{
	public static class XmlExtensions
	{
		public static bool HasAttributeCI(this XElement x, string attributeName)
		{
			return x.Attributes().Where(a => a.Name.LocalName.CaseInsensitiveCompare(attributeName)).FirstOrDefault() != null;
		}

		public static string GetAttributeCI(this XElement x, string attributeName)
		{
			var attr = x.Attributes().Where(a => a.Name.LocalName.CaseInsensitiveCompare(attributeName)).FirstOrDefault();
			return attr == null ? null : attr.Value;
		}
	}
}
