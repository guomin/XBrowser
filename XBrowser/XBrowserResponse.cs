using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace AxeFrog.Net
{
	public class XBrowserResponse
	{
		internal XBrowserResponse(HttpWebResponse response)
		{
			Status = response.StatusCode;
			using(StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				ResponseText = reader.ReadToEnd();
				reader.Close();
			}
			ContentType = response.ContentType;
			Uri = response.ResponseUri;
		}

		internal XBrowserResponse(WebException ex)
		{
			Status = ((HttpWebResponse)ex.Response).StatusCode;
		}

		public HttpStatusCode Status { get; private set; }
		public string ResponseText { get; private set; }
		protected string ContentType { get; private set; }
		public Uri Uri { get; private set; }
	}
}
