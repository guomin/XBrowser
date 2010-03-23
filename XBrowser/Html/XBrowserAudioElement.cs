using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserAudioElement : XBrowserElement
	{
		public XBrowserAudioElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Audio, null)
		{
		}
	}
}