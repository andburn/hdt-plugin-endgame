using System.Collections.Generic;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Services.TempoStorm
{
	public class Slugs
	{
		[JsonProperty("slugs")]
		public List<SlugItem> Items { get; set; }

		public Slugs()
		{
			Items = new List<SlugItem>();
		}
	}

	public class SlugItem
	{
		[JsonProperty("slug")]
		public string Slug { get; set; }

		[JsonProperty("linked")]
		public bool Linked { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("snapshotId")]
		public string SnapshotId { get; set; }

		[JsonProperty("deckId")]
		public string deckId { get; set; }
	}
}