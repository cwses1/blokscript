using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

using BlokScript.IO;
using BlokScript.Parsers;

namespace BlokScript.WebClients
{
	public class StoryblokManagementWebClient
	{
		public string GetString (string RequestPath)
		{
			//
			// CREATE THE REQUEST.
			//
			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(BaseUrl + RequestPath);
			Request.Method = "GET";
			Request.Headers["Authorization"] = Token;
			Request.ContentType = "application/json";

			//
			// SEND THE REQUEST AND READ THE RESPONSE.
			//
			HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

			using (MemoryStream TargetStream = new MemoryStream())
			{
				using (Stream ResponseStream = Response.GetResponseStream())
				{
					StreamCopier.Copy(ResponseStream, TargetStream);
					ResponseStream.Close();
				}

				return Encoding.UTF8.GetString(TargetStream.ToArray());
				/*
				string ResponseString = Encoding.UTF8.GetString(TargetStream.ToArray());

				using (StringReader ResponseStringReader = new StringReader(ResponseString))
				{
					return (VonageSendSmsResponseModel)JsonSerializer.CreateDefault().Deserialize(ResponseStringReader, typeof(VonageSendSmsResponseModel));
				}
				*/
			}
		}

		public bool GetOK (string RequestPath)
		{
			/*
			 * https://api-us.storyblok.com/v1/spaces/1019179
			 * 
			curl "https://mapi.storyblok.com/v1/spaces/606/" \
			  -X GET \
			  -H "Authorization: YOUR_OAUTH_TOKEN" \
			  -H "Content-Type: application/json"
			*/

			//
			// CREATE THE REQUEST.
			//
			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(BaseUrl + RequestPath);
			Request.Method = "GET";
			Request.Headers["Authorization"] = Token;
			Request.ContentType = "application/json";

			//
			// SEND THE REQUEST AND READ THE RESPONSE.
			//
			try
			{
				HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
				return Response.StatusCode == HttpStatusCode.OK;
			}
			catch (WebException)
			{
				return false;
			}
		}

		public string PostJson (string RequestPath, string RequestBody)
		{
			//
			// CREATE THE REQUEST BODY.
			//
			byte[] RequestBodyBytes = Encoding.UTF8.GetBytes(RequestBody);

			//
			// CREATE THE REQUEST.
			//
			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(BaseUrl + RequestPath);
			Request.Method = "POST";
			Request.Headers["Authorization"] = Token;
			Request.ContentType = "application/json";
			Request.ContentLength = RequestBodyBytes.Length;

			//
			// SEND THE REQUEST.
			//
			using (MemoryStream RequestBodyStream = new MemoryStream(RequestBodyBytes))
			{
				using (Stream RequestStream = Request.GetRequestStream())
				{
					StreamCopier.Copy(RequestBodyStream, RequestStream);
					RequestStream.Close();
				}
			}

			//
			// SEND THE REQUEST AND READ THE RESPONSE.
			//
			HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

			using (MemoryStream TargetStream = new MemoryStream())
			{
				using (Stream ResponseStream = Response.GetResponseStream())
				{
					StreamCopier.Copy(ResponseStream, TargetStream);
					ResponseStream.Close();
				}

				return Encoding.UTF8.GetString(TargetStream.ToArray());
			}
		}

		public void PutJson (string RequestPath, string RequestBody)
		{
			//
			// CREATE THE REQUEST BODY.
			//
			byte[] RequestBodyBytes = Encoding.UTF8.GetBytes(RequestBody);

			//
			// CREATE THE REQUEST.
			//
			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(BaseUrl + RequestPath);
			Request.Method = "PUT";
			Request.Headers["Authorization"] = Token;
			Request.ContentType = "application/json";
			Request.ContentLength = RequestBodyBytes.Length;

			//
			// SEND THE REQUEST.
			//
			using (MemoryStream RequestBodyStream = new MemoryStream(RequestBodyBytes))
			{
				using (Stream RequestStream = Request.GetRequestStream())
				{
					StreamCopier.Copy(RequestBodyStream, RequestStream);
					RequestStream.Close();
				}
			}

			//
			// SEND THE REQUEST AND READ THE RESPONSE.
			//
			HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

			using (MemoryStream TargetStream = new MemoryStream())
			{
				using (Stream ResponseStream = Response.GetResponseStream())
				{
					StreamCopier.Copy(ResponseStream, TargetStream);
					ResponseStream.Close();
				}

				string ResponseString = Encoding.UTF8.GetString(TargetStream.ToArray());

				/*
				using (StringReader ResponseStringReader = new StringReader(ResponseString))
				{
					return (VonageSendSmsResponseModel)JsonSerializer.CreateDefault().Deserialize(ResponseStringReader, typeof(VonageSendSmsResponseModel));
				}
				*/
			}
		}

		public bool TryPostJson (string RequestPath, string RequestBody)
		{
			//
			// CREATE THE REQUEST BODY.
			//
			byte[] RequestBodyBytes = Encoding.UTF8.GetBytes(RequestBody);

			//
			// CREATE THE REQUEST.
			//
			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(BaseUrl + RequestPath);
			Request.Method = "POST";
			Request.Headers["Authorization"] = Token;
			Request.ContentType = "application/json";
			Request.ContentLength = RequestBodyBytes.Length;

			//
			// WRITE THE REQUEST BODY.
			//
			using (MemoryStream RequestBodyStream = new MemoryStream(RequestBodyBytes))
			{
				using (Stream RequestStream = Request.GetRequestStream())
				{
					StreamCopier.Copy(RequestBodyStream, RequestStream);
					RequestStream.Close();
				}
			}

			//
			// SEND THE REQUEST AND READ THE RESPONSE.
			//
			try
			{
				HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
				return true;
			}
			catch (WebException E)
			{
				HttpWebResponse Response = (HttpWebResponse)E.Response;

				if (((int)Response.StatusCode) == 422)
					return false;

				throw;
			}
		}

		public void Delete (string RequestPath, string RequestBody)
		{
			//
			// CREATE THE REQUEST BODY.
			//
			byte[] RequestBodyBytes = Encoding.UTF8.GetBytes(RequestBody);

			//
			// CREATE THE REQUEST.
			//
			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(BaseUrl + RequestPath);
			Request.Method = "DELETE";
			Request.Headers["Authorization"] = Token;
			Request.ContentType = "application/json";
			Request.ContentLength = RequestBodyBytes.Length;

			//
			// SEND THE REQUEST.
			//
			using (MemoryStream RequestBodyStream = new MemoryStream(RequestBodyBytes))
			{
				using (Stream RequestStream = Request.GetRequestStream())
				{
					StreamCopier.Copy(RequestBodyStream, RequestStream);
					RequestStream.Close();
				}
			}

			//
			// SEND THE REQUEST AND READ THE RESPONSE.
			//
			HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

			using (MemoryStream TargetStream = new MemoryStream())
			{
				using (Stream ResponseStream = Response.GetResponseStream())
				{
					StreamCopier.Copy(ResponseStream, TargetStream);
					ResponseStream.Close();
				}

				string ResponseString = Encoding.UTF8.GetString(TargetStream.ToArray());

				/*
				using (StringReader ResponseStringReader = new StringReader(ResponseString))
				{
					return (VonageSendSmsResponseModel)JsonSerializer.CreateDefault().Deserialize(ResponseStringReader, typeof(VonageSendSmsResponseModel));
				}
				*/
			}
		}

		public string BaseUrl;
		public string Token;
	}
}
