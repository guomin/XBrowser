using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XAudioElement : XBrowserElement
	{
		public XAudioElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Audio, null)
		{
		}
	}
}