using System;
using System.IO;
using System.Net;

namespace XBrowserProject.Internal
{
	public class Response
	{
		internal Response(HttpWebResponse response)
		{
			Status = response.StatusCode;
			var stream = response.GetResponseStream();
			if (stream != null)
			{
				if (response.ContentType == "text" || response.ContentType.StartsWith("text/"))
				{
					using(var reader = new StreamReader(stream))
					{
						ResponseText = reader.ReadToEnd();
						reader.Close();
					}
				}
				else
				{
					using (var ms = new MemoryStream())
					{
						var buf = new byte[5000];
						for (var len = stream.Read(buf, 0, buf.Length); len > 0; len = stream.Read(buf, 0, buf.Length))
							ms.Write(buf, 0, len);
						ResponseData = ms.ToArray();
					}
				}
			}
			ContentType = response.ContentType;
			Uri = response.ResponseUri;
		}

		internal Response(WebException ex)
		{
			Status = ((HttpWebResponse)ex.Response).StatusCode;
		}

		public HttpStatusCode Status { get; private set; }
		public string ResponseText { get; private set; }
		public byte[] ResponseData { get; set; }
		protected string ContentType { get; private set; }
		public Uri Uri { get; private set; }
	}
}
