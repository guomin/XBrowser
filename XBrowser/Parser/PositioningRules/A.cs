using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxeFrog.Net.Parser.PositioningRules
{
	class A : BodyElementPositioningRule
	{
		static readonly string[] _allowedParentTags = new [] { "" };
		public override string TagName { get { return "a"; } }
	}
}
