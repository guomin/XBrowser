using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;

namespace XBrowserProject.Query.Selectors
{
	public class IdSelector : IXQuerySelector
	{
		private readonly string _id;

		public IdSelector(string id)
		{
			_id = id.ToLower();
		}

		public bool IsTransposeSelector { get { return false; } }

		public void Execute(XQueryResultsContext context)
		{
			context.ResultSetInternal = context.ResultSetInternal
				.Where(x => x.GetAttributeCI("id") == _id);
		}

		internal static readonly Regex RxSelector = new Regex(@"^\#(?<id>[A-Za-z_][A-Za-z0-9_]+)");
	}

	public class IdSelectorCreator : XQuerySelectorCreator
	{
		public override Regex MatchNext { get { return IdSelector.RxSelector; } }

		public override IXQuerySelector Create(XQueryParserContext context, Match match)
		{
			return new IdSelector(match.Groups["id"].Value);
		}
	}
}
