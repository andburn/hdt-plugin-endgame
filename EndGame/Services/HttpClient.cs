using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.EndGame.Services
{
	public class HttpClient : IHttpClient
	{
		private async Task<string> JsonRequest(string url, string data = null)
		{
			using (var wc = new WebClient())
			{
				wc.Encoding = Encoding.UTF8;
				wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");

				var response = "";
				if (string.IsNullOrWhiteSpace(data))
					response = await wc.DownloadStringTaskAsync(new Uri(url));
				else
					response = await wc.UploadStringTaskAsync(new Uri(url), data);

				return response;
			}
		}

		public async Task<string> JsonGet(string url)
		{
			return await JsonRequest(url);
		}

		public async Task<string> JsonPost(string url, string data)
		{
			return await JsonRequest(url, data);
		}

		public Task<string> Get(string url)
		{
			throw new NotImplementedException();
		}

		public Task<string> Post(string url, string data)
		{
			throw new NotImplementedException();
		}
	}
}