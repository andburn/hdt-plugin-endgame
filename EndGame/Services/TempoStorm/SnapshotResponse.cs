using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Services.TempoStorm
{
	public class SnapshotResponse
	{
		[JsonProperty("snapNum")]
		public int SnapNumber { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("snapshotType")]
		public string SnapshotType { get; set; }

		[JsonProperty("isActive")]
		public bool IsActive { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("createdDate")]
		public DateTime Created { get; set; }

		[JsonProperty("updatedDate")]
		public DateTime Updated { get; set; }

		[JsonProperty("slugs")]
		public List<SlugItem> Slugs { get; set; }

		[JsonProperty("error")]
		public Error Error { get; set; }

		[JsonProperty("deckTiers")]
		public List<DeckSnapshot> DeckTiers { get; set; }

		public override string ToString()
		{
			return $"{Title} {SnapshotType} {DeckTiers.Count}";
		}
	}
}