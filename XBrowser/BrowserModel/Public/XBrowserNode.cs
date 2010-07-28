using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public abstract class XBrowserNode : IDisposable
	{
		public XNode XNode { get; private set; }
		public XBrowserDocument Document { get; protected set; }

		protected XBrowserNode(XNode node)
		{
			node.AddAnnotation(this);
			XNode = node;
		}

		public void Dispose()
		{
			XNode.RemoveAnnotations<XNode>();
		}
	}
}
