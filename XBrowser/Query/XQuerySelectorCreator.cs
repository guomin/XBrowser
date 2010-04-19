using System;
using System.Text.RegularExpressions;

namespace AxeFrog.Net.Query
{
	public abstract class XQuerySelectorCreator
	{
		public abstract Regex MatchNext { get; }
		public abstract IXQuerySelector Create(XQueryParserContext context, Match match);
	}
}