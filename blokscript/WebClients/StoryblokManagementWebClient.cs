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
		private static HttpClient _HttpClient;

		static StoryblokManagementWebClient()
		{
			_HttpClient = new HttpClient();
			//_HttpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
		}

		private HttpRequestMessage CreateHttpRequestMessage(string RequestPath)
		{
			/* https://api-us.storyblok.com/v1/spaces/1019179
				curl "https://mapi.storyblok.com/v1/spaces/606/" \
				-X GET \
				-H "Authorization: YOUR_OAUTH_TOKEN" \
				-H "Content-Type: application/json"
			*/
			HttpRequestMessage CreatedRequestMessage = new HttpRequestMessage();
			CreatedRequestMessage.RequestUri = new Uri(BaseUrl + RequestPath);
			CreatedRequestMessage.Headers.Add("Authorization", Token);
			return CreatedRequestMessage;
		}

		private HttpRequestMessage CreateHttpRequestMessage (string RequestPath, byte[] RequestBodyBytes)
		{
			/* https://api-us.storyblok.com/v1/spaces/1019179
				curl "https://mapi.storyblok.com/v1/spaces/606/" \
				-X GET \
				-H "Authorization: YOUR_OAUTH_TOKEN" \
				-H "Content-Type: application/json"
			*/
			HttpRequestMessage CreatedRequestMessage = CreateHttpRequestMessage(RequestPath);
			CreatedRequestMessage.Content = new ByteArrayContent(RequestBodyBytes);
			CreatedRequestMessage.Content.Headers.ContentType.MediaType = "application/json";
			CreatedRequestMessage.Content.Headers.ContentLength = RequestBodyBytes.Length;
			return CreatedRequestMessage;
		}

		private HttpRequestMessage CreateHttpRequestMessage (string RequestPath, string RequestBody)
		{
			return CreateHttpRequestMessage(RequestPath, Encoding.UTF8.GetBytes(RequestBody));
		}

		public string GetString(string RequestPath)
		{
			_HttpClient.Timeout = TimeSpan.FromMilliseconds(TimeoutMs);

			HttpRequestMessage CreatedRequestMessage = CreateHttpRequestMessage(RequestPath);
			CreatedRequestMessage.Method = HttpMethod.Get;

			Task<HttpResponseMessage> RequestTask = Task.Run<HttpResponseMessage>(async () => await _HttpClient.SendAsync(CreatedRequestMessage));
			RequestTask.Wait();

			using (Stream ResponseStream = RequestTask.Result.Content.ReadAsStream())
			{
				return StreamCopier.CopyStreamToString(ResponseStream);
			}
		}

		public bool GetOK (string RequestPath)
		{
			_HttpClient.Timeout = TimeSpan.FromMilliseconds(TimeoutMs);

			HttpRequestMessage CreatedRequestMessage = CreateHttpRequestMessage(RequestPath);
			CreatedRequestMessage.Method = HttpMethod.Get;

			Task<HttpResponseMessage> RequestTask = Task.Run<HttpResponseMessage>(async () => await _HttpClient.SendAsync(CreatedRequestMessage));
			RequestTask.Wait();
			HttpResponseMessage ResponseMessage = RequestTask.Result;

			return ResponseMessage.StatusCode == HttpStatusCode.OK;
		}

		public string PostJson (string RequestPath, string RequestBody)
		{
			_HttpClient.Timeout = TimeSpan.FromMilliseconds(TimeoutMs);

			HttpRequestMessage CreatedRequestMessage = CreateHttpRequestMessage(RequestPath, RequestBody);
			CreatedRequestMessage.Method = HttpMethod.Post;

			Task<HttpResponseMessage> RequestTask = Task.Run<HttpResponseMessage>(async () => await _HttpClient.SendAsync(CreatedRequestMessage));
			RequestTask.Wait();

			using (Stream ResponseStream = RequestTask.Result.Content.ReadAsStream())
			{
				return StreamCopier.CopyStreamToString(ResponseStream);
			}
		}

		public string PutJson (string RequestPath, string RequestBody)
		{
			_HttpClient.Timeout = TimeSpan.FromMilliseconds(TimeoutMs);

			HttpRequestMessage CreatedRequestMessage = CreateHttpRequestMessage(RequestPath, RequestBody);
			CreatedRequestMessage.Method = HttpMethod.Put;

			Task<HttpResponseMessage> RequestTask = Task.Run<HttpResponseMessage>(async () => await _HttpClient.SendAsync(CreatedRequestMessage));
			RequestTask.Wait();

			using (Stream ResponseStream = RequestTask.Result.Content.ReadAsStream())
			{
				return StreamCopier.CopyStreamToString(ResponseStream);
			}
		}

		public bool TryPostJson (string RequestPath, string RequestBody)
		{
			_HttpClient.Timeout = TimeSpan.FromMilliseconds(TimeoutMs);

			HttpRequestMessage CreatedRequestMessage = CreateHttpRequestMessage(RequestPath, RequestBody);
			CreatedRequestMessage.Method = HttpMethod.Post;

			Task<HttpResponseMessage> RequestTask = Task.Run<HttpResponseMessage>(async () => await _HttpClient.SendAsync(CreatedRequestMessage));
			RequestTask.Wait();
			HttpResponseMessage ResponseMessage = RequestTask.Result;

			return ResponseMessage.StatusCode == HttpStatusCode.OK;
		}

		public void Delete (string RequestPath, string RequestBody)
		{
			_HttpClient.Timeout = TimeSpan.FromMilliseconds(TimeoutMs);

			HttpRequestMessage CreatedRequestMessage = CreateHttpRequestMessage(RequestPath, RequestBody);
			CreatedRequestMessage.Method = HttpMethod.Delete;

			Task<HttpResponseMessage> RequestTask = Task.Run<HttpResponseMessage>(async () => await _HttpClient.SendAsync(CreatedRequestMessage));
			RequestTask.Wait();
		}

		public string BaseUrl;
		public string Token;

		public int TimeoutMs;
		public int ThrottleMs;
		public int RetryCount;
	}
}
