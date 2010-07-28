using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserAudioElement : XBrowserElement
	{
		public XBrowserAudioElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Audio, null)
		{
		}
	}
}