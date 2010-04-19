﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;

namespace AxeFrog.Net.Query.Selectors
{
	public class ElementSelector : IXQuerySelector
	{
		private readonly string _name;

		public ElementSelector(string name)
		{
			_name = name.ToLower();
		}

		public bool IsTransposeSelector { get { return false; } }

		public void Execute(XQueryResultsContext context)
		{
			context.ResultSetInternal = context.ResultSetInternal
				.Where(x => string.Compare(x.Name.LocalName, _name, true) == 0);
		}

		internal static readonly Regex RxSelector = new Regex("^[A-Za-z]+");
	}

	public class ElementSelectorCreator : XQuerySelectorCreator
	{
		public override Regex MatchNext { get { return ElementSelector.RxSelector; } }

		public override IXQuerySelector Create(XQueryParserContext context, Match match)
		{
			return new ElementSelector(match.Value);
		}
	}
}
