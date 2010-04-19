using System.Text.RegularExpressions;

namespace AxeFrog.Net.Query
{
	public interface IXQuerySelector
	{
		void Execute(XQueryResultsContext context);
		bool IsTransposeSelector { get; }
	}
}