using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Services.TempoStorm
{
	public class SnapshotRequest
	{
		[JsonProperty("order")]
		public string SortOrder { get; set; }

		[JsonProperty("fields")]
		public List<string> Fields { get; set; }

		[JsonProperty("where")]
		private Dictionary<string, string> Query { get; set; }

		[JsonProperty("include")]
		public List<IncludeItem> Includes { get; set; }

		public SnapshotRequest()
		{
			Query = new Dictionary<string, string>();
			Includes = new List<IncludeItem>();
		}

		public void SetFields(params string[] fields)
		{
			Fields = new List<string>(fields);
		}

		public void SetQuery(params string[] fields)
		{
			foreach (var f in fields)
			{
				var pair = f.Split(':');
				if (pair.Length != 2)
					throw new ArgumentException("Unexpected format received");
				if (!Query.ContainsKey(pair[0]))
					Query[pair[0]] = pair[1];
			}
		}
	}
}