using System;
using XBrowserProject.BrowserModel.Internal.HtmlDom;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser;
using System.IO;

namespace XBrowserProject.BrowserModel.Internal
{
	internal class Window : IHTMLWindow
	{
		private string windowName = "_blank";
		private Location windowLocation = new Location();
		private History windowHistory = new History();
		private Window topWindow;
		private Window openerWindow;
		private HtmlFrameElement frame;
		private Parser htmlParser = new Parser();
		//private JintEngine eng = new JintEngine();
		private delegate IHTMLWindow OpenDelegate(string url, string target, string features, string replace);
		private delegate void AlertDelegate(string message);
		private delegate bool ConfirmDelegate(string message);
		private delegate string PromptDelegate(string message, string defaultValue);
		private delegate void VoidDelegate();

		//public event EventHandler<DialogDisplayedEventArgs> DialogDisplayed;

		public Window()
		{
			htmlParser.Document.Window = this;
			//eng.SetParameter("window", this.window);
			//eng.SetParameter("self", this.self);
			//eng.SetParameter("document", this.document);
			//eng.SetParameter("name", this.name);
			//eng.SetParameter("location", this.location);
			//eng.SetParameter("history", this.history);
			//eng.SetParameter("top", this.top);
			//eng.SetParameter("opener", this.opener);
			//eng.SetParameter("frameElement", this.frameElement);
			//eng.SetParameter("frames", this.frames);
			//eng.SetFunction("open", new OpenDelegate(open));
			//eng.SetFunction("alert", new AlertDelegate(alert));
			//eng.SetFunction("confirm", new ConfirmDelegate(confirm));
			//eng.SetFunction("prompt", new PromptDelegate(prompt));
			//eng.SetFunction("close", new VoidDelegate(close));
			//eng.SetFunction("focus", new VoidDelegate(focus));
			//eng.SetFunction("blur", new VoidDelegate(blur));
			//eng.Run("window = this");
		}

		//public JintEngine ScriptEngine
		//{
		//    get { return eng; }
		//}

		public void LoadString(StringReader htmlSource)
		{
			LoadDocument(htmlSource);
		}

		public void LoadStream(StreamReader htmlSource)
		{
			LoadDocument(htmlSource);
		}

		public void LoadDocument(TextReader htmlSource)
		{
			htmlParser.ParseDocument(htmlSource);
			if(document.body.hasAttribute("onload"))
			{
				string onLoadScript = document.body.getAttribute("onload");
				//RunScript(onLoadScript);
			}
		}

		public object RunScript(string script)
		{
			throw new NotImplementedException();
			//object toReturn = null;
			//if(!string.IsNullOrEmpty(script))
			//{
			//    string scriptToRun = script;
			//    if(script.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
			//    {
			//        scriptToRun = script.Substring("javascript:".Length);
			//    }

			//    toReturn = ScriptEngine.Run(scriptToRun);
			//}

			//return toReturn;
		}

		#region IHTMLWindow Members

		public IHTMLWindow window
		{
			get { return this; }
		}

		public IHTMLWindow self
		{
			get { return this; }
		}

		public HtmlDocument document
		{
			get { return htmlParser.Document; }
		}

		public string name
		{
			get { return windowName; }
			set { windowName = value; }
		}

		public IHTMLLocation location
		{
			get { return windowLocation; }
		}

		public IHTMLHistory history
		{
			get { return windowHistory; }
		}

		public void close()
		{
			throw new NotImplementedException();
		}

		public void focus()
		{
			throw new NotImplementedException();
		}

		public void blur()
		{
			throw new NotImplementedException();
		}

		public IHTMLWindow frames
		{
			get { return this; }
		}

		public int length
		{
			get { throw new NotImplementedException(); }
		}

		public IHTMLWindow top
		{
			get
			{
				Window topWindowContext = this;
				if(topWindow != null)
				{
					topWindowContext = topWindow;
				}

				return topWindowContext;
			}
		}

		public IHTMLWindow opener
		{
			get
			{
				Window openerWindowContext = this;
				if(openerWindow != null)
				{
					openerWindowContext = openerWindow;
				}

				return openerWindowContext;
			}
		}

		public IHTMLWindow parent
		{
			get { throw new NotImplementedException(); }
		}

		public IHTMLElement frameElement
		{
			get { return frame; }
		}

		public IHTMLWindow open(string url, string target, string features, string replace)
		{
			throw new NotImplementedException();
		}

		public IHTMLWindow this[int index]
		{
			get { throw new NotImplementedException(); }
		}

		public IHTMLWindow this[string name]
		{
			get { throw new NotImplementedException(); }
		}

		public void alert(string message)
		{
			//JavaScriptAlertDialog alertDialog = new JavaScriptAlertDialog(message);
			//OnDialogDisplayed(new DialogDisplayedEventArgs(alertDialog));
		}

		public bool confirm(string message)
		{
			throw new NotImplementedException();
			//JavaScriptConfirmDialog confirmDialog = new JavaScriptConfirmDialog(message);
			//OnDialogDisplayed(new DialogDisplayedEventArgs(confirmDialog));
			//return confirmDialog.IsConfirmed;
		}

		public string prompt(string message, string defaultValue)
		{
			throw new NotImplementedException();
		}

		#endregion

		//protected void OnDialogDisplayed(DialogDisplayedEventArgs e)
		//{
		//    if(DialogDisplayed != null)
		//    {
		//        DialogDisplayed(this, e);
		//    }
		//}
	}
}
