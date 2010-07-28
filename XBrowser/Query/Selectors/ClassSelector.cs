﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;

namespace XBrowserProject.Query.Selectors
{
	public class ClassSelector : IXQuerySelector
	{
		private readonly string _class;

		public ClassSelector(string @class)
		{
			_class = @class.ToLower();
		}

		public bool IsTransposeSelector { get { return false; } }

		public void Execute(XQueryResultsContext context)
		{
			context.ResultSetInternal = context.ResultSetInternal
				.Where(x => x.GetAttributeCI("class") == _class);
		}

		internal static readonly Regex RxSelector = new Regex(@"^\.(?<class>[A-Za-z0-9_\-]+)");
	}

	public class ClassSelectorCreator : XQuerySelectorCreator
	{
		public override Regex MatchNext { get { return ClassSelector.RxSelector; } }

		public override IXQuerySelector Create(XQueryParserContext context, Match match)
		{
			return new ClassSelector(match.Groups["class"].Value);
		}
	}
}
