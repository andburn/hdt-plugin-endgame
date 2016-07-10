using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Services;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Utilities
{
	public class Github
	{
		private static readonly ILoggingService _logger = new TrackerLogger();

		// Check if there is a newer release on Github than current
		public static async Task<GithubRelease> CheckForUpdate(string user, string repo, Version version)
		{
			try
			{
				var latest = await GetLatestRelease(user, repo);

				// tag needs to be in strict version format: e.g. 0.0.0
				Version v = new Version(latest.tag_name);

				// check if latest is newer than current
				if (v.CompareTo(version) > 0)
				{
					return latest;
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				EndGame.Notify("Plugin Update Check Failed", ex.Message, 15, "error", null);
			}
			return null;
		}

		// Use the Github API to get the latest release for a repo
		public static async Task<GithubRelease> GetLatestRelease(string user, string repo)
		{
			var url = String.Format("https://api.github.com/repos/{0}/{1}/releases", user, repo);
			try
			{
				var json = "";
				using (WebClient wc = new WebClient())
				{
					// API requires user-agent string, user name or app name preferred
					wc.Headers.Add(HttpRequestHeader.UserAgent, user);
					json = await wc.DownloadStringTaskAsync(url);
				}
				var releases = JsonConvert.DeserializeObject<List<GithubRelease>>(json);

				return releases.FirstOrDefault<GithubRelease>();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Basic release info for JSON deserialization
		public class GithubRelease
		{
			public string html_url { get; set; }
			public string tag_name { get; set; }
			public string prerelease { get; set; }
			public string published_at { get; set; }
		}
	}
}