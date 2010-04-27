using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxeFrog.Net.Parser.PositioningRules
{
	class Abbr : IElementPositioningRule
	{
		public Abbr()
		{
			AllowedParentTags = new[] { "" };
		}

		public string TagName { get { return "abbr"; } }
		public DocumentArea Area { get { return DocumentArea.Body; } }

		public string[] AllowedParentTags { get; private set; }

		public string CreateParentTag
		{
			get { throw new NotImplementedException(); }
		}

		public bool? TextChildren
		{
			get { throw new NotImplementedException(); }
		}

		public bool AllowNesting
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public string[] NestingCheckBreakingTags
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}
	}
}
