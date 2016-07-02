using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;
using Hearthstone_Deck_Tracker.Hearthstone;
using Newtonsoft.Json;

using HDTDeck = Hearthstone_Deck_Tracker.Hearthstone.Deck;

namespace HDT.Plugins.EndGame.Services.TempoStorm
{
	public class SnapshotImporter : IArchetypeImporter
	{
		private const string BaseDeckUrl = "https://tempostorm.com/hearthstone/decks/";
		private const string BaseSnapshotUrl = "https://tempostorm.com/api/snapshots/findOne?filter=";

		private IHttpClient _http;
		private JsonSerializerSettings _settings;

		public SnapshotImporter(IHttpClient http)
		{
			_http = http;
			_settings = new JsonSerializerSettings() {
				NullValueHandling = NullValueHandling.Ignore
			};
		}

		public async Task<Tuple<string, string>> GetSnapshotSlug()
		{
			// build the Json query
			var metaReq = new SnapshotRequest();
			metaReq.SortOrder = "createdDate DESC";
			metaReq.SetFields("id", "snapshotType", "isActive");
			metaReq.SetQuery("isActive:true");
			metaReq.Includes.Add(new IncludeItem("slugs"));
			// make the request and deserialize Json result
			var metaRespJson = await _http.JsonGet(
				BaseSnapshotUrl + JsonConvert.SerializeObject(metaReq, _settings));
			var metaResponse = JsonConvert.DeserializeObject<SnapshotResponse>(metaRespJson);
			// check there are no errors
			if (metaResponse.Error != null)
				throw new ImportException($"Getting the snapshot slug failed ({metaResponse.Error.Status})");
			// the slug needed to request the full snapshot (e.g. '2016-06-19')
			// should be a single item, unless wild is added
			if (metaResponse.Slugs.Count != 1)
				throw new ImportException("Snapshot slug count greater than one");

			return new Tuple<string, string>(
				metaResponse.Slugs.Single().Slug,
				metaResponse.SnapshotType);
		}

		public async Task<SnapshotResponse> GetSnapshot(Tuple<string, string> slug)
		{
			var snapReq = new SnapshotRequest();
			snapReq.SetQuery($"slug:{slug.Item1}", $"snapshotType:{slug.Item2}");
			snapReq.SetFields("snapNum", "title", "snapshotType", "isActive", "id", "createdDate", "updatedDate");
			// build the rest of the Json request, what sections to include/exclude
			// first, the tech cards
			var incCard = new IncludeItem("card", new Include("name"));
			var incCardTech = new IncludeItem("cardTech", incCard);
			var incDeckTech = new IncludeItem("deckTech", incCardTech);
			// then, acutal decks and cards
			var incSlugs = new IncludeItem("slugs", new Include("linked", "slug"));
			var inCardsCard = new IncludeItem("card", new Include("id", "name", "cardType", "cost"));
			var incCards = new IncludeItem("cards", inCardsCard);
			var incDeck = new IncludeItem("deck", new Include() {
				Fields = new List<string> { "id", "name", "slug", "playerClass" },
				Items = new List<IncludeItem>() { incSlugs, incCards }
			});
			var incTiers = new IncludeItem("deckTiers", new Include() {
				Fields = new List<string> { "name", "id", "deckId", "tier" },
				Items = new List<IncludeItem>() { incDeck, incDeckTech }
			});
			snapReq.Includes = new List<IncludeItem>() { incTiers };
			// make the request and deserialize
			var snapRespJson = await _http.JsonGet(BaseSnapshotUrl + JsonConvert.SerializeObject(snapReq, _settings));
			var snapResponse = JsonConvert.DeserializeObject<SnapshotResponse>(snapRespJson);
			// check there are no errors
			if (snapResponse.Error != null)
				throw new ImportException($"Getting the snapshot failed ({snapResponse.Error.Status})");

			return snapResponse;
		}

		public async Task<List<ArchetypeDeck>> GetArchetypes()
		{
			// first, get the lastest meta snapshot slug/date
			var slug = await GetSnapshotSlug();
			// next, use the slug to request the actual snapshot details
			var snapshot = await GetSnapshot(slug);

			var repo = new TrackerRepository();
			foreach (var dt in snapshot.DeckTiers)
			{
				HDTDeck deck = new HDTDeck();
				// TODO tags, note, url
				deck.Name = dt.Name;
				deck.Class = dt.Deck.PlayerClass;
				deck.Url = BaseDeckUrl + dt.Deck.Slugs.SingleOrDefault()?.Slug;
				deck.Tags.AddRange(new string[] { "Archetype", "EndGame" });
				foreach (var cd in dt.Deck.Cards)
				{
					var card = Database.GetCardFromName(cd.Detail.Name);
					// TODO check this comparison
					if (card != null || card == Database.UnknownCard)
					{
						card.Count = cd.Quantity;
						deck.Cards.Add(card);
					}
				}
				repo.AddDeck(deck);
			}

			return null;
		}
	}
}