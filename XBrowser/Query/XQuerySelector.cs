using System.Text.RegularExpressions;

namespace XBrowserProject.Query
{
	public interface IXQuerySelector
	{
		void Execute(XQueryResultsContext context);
		bool IsTransposeSelector { get; }
	}
}