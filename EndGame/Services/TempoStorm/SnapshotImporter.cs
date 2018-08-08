﻿using HDT.Plugins.Common.Services;
using HDT.Plugins.EndGame.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDT.Plugins.EndGame.Services.TempoStorm
{
	public class SnapshotImporter : IArchetypeImporter
	{
		private const string BaseDeckUrl = "https://tempostorm.com/hearthstone/decks/";
		private const string BaseSnapshotUrl = "https://tempostorm.com/api/snapshots/findOne?filter=";

		private IHttpClient _http;
		private IDataRepository _tracker;
		private ILoggingService _logger;
		private JsonSerializerSettings _settings;

		public SnapshotImporter(IHttpClient http, IDataRepository tracker, ILoggingService logger)
		{
			_http = http;
			_tracker = tracker;
			_logger = logger;
			_settings = new JsonSerializerSettings()
			{
				NullValueHandling = NullValueHandling.Ignore
			};
		}

		public virtual async Task<Tuple<string, string>> GetSnapshotSlug(string type)
		{
			// build the Json query
			var metaReq = new SnapshotRequest();
			metaReq.SortOrder = "createdDate+DESC";
			metaReq.SetFields("id", "snapshotType", "isActive", "publishDate");
			metaReq.SetQuery("isActive:true", $"snapshotType:{type}");
			metaReq.Includes.Add(new IncludeItem("slugs"));
			// make the request and deserialize Json result
			var metaRespJson = await _http.JsonGet(
				BaseSnapshotUrl + JsonConvert.SerializeObject(metaReq, _settings));
			var metaResponse = JsonConvert.DeserializeObject<SnapshotResponse>(metaRespJson);
			// check there are no errors
			if (metaResponse.Error != null)
				throw new ImportException($"Getting the snapshot slug failed ({metaResponse.Error.Status})");
			// the slug needed to request the full snapshot (e.g. '2016-06-19')
			if (metaResponse.Slugs.Count != 1)
				throw new ImportException("Snapshot slug count greater than one");

			return new Tuple<string, string>(
				metaResponse.Slugs.Single().Slug,
				metaResponse.SnapshotType);
		}

		public virtual async Task<SnapshotResponse> GetSnapshot(Tuple<string, string> slug)
		{
			var snapReq = new SnapshotRequest();
			snapReq.SetQuery($"slug:{slug.Item1}", $"snapshotType:{slug.Item2}");
			// build the rest of the Json request, what sections to include/exclude
			// first, the tech cards
			var incCard = new IncludeItem("card", new Include("name"));
			var incCardTech = new IncludeItem("cardTech", incCard);
			var incDeckTech = new IncludeItem("deckTech", incCardTech);
			// then, acutal decks and cards
			var incSlugs = new IncludeItem("slugs", new Include("linked", "slug"));
			var inCardsCard = new IncludeItem("card", new Include(
				"id", "name", "name_ru", "cardType", "cost", "dust", "photoNames"));
			var incCards = new IncludeItem("cards", inCardsCard);
			var incDeck = new IncludeItem("deck", new Include()
			{
				Fields = new List<string> { "id", "name", "slug", "playerClass" },
				Items = new List<IncludeItem>() { incSlugs, incCards }
			});
			var incTiers = new IncludeItem("deckTiers", new Include()
			{
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

		public virtual async Task<int> ImportDecks(bool includeWild, bool archive, bool deletePrevious, bool removeClass)
		{
			// delete previous snapshot decks
			if (deletePrevious)
			{
				_logger.Info("Deleting previous meta decks");
				_tracker.DeleteAllDecksWithTag(Strings.PluginTag);
			}
			var deckCount = 0;
			deckCount += await ImportSnapshotDecks(Strings.MetaStandard, archive, removeClass);
			if (includeWild)
			{
				deckCount += await ImportSnapshotDecks(Strings.MetaWild, archive, removeClass);
			}
			return deckCount;
		}

		private async Task<int> ImportSnapshotDecks(string type, bool archive, bool removeClass)
		{
			int deckCount = 0;
			if (type == Strings.MetaStandard || type == Strings.MetaWild)
			{
				_logger.Info($"Starting meta deck import ({type})");
				// get the lastest meta snapshot slug/date
				var slug = await GetSnapshotSlug(type);
				// use the slug to request the actual snapshot details
				var snapshot = await GetSnapshot(slug);
				// add all decks to the tracker
				foreach (var dt in snapshot.DeckTiers)
				{
					var cards = "";
					_logger.Debug($"Importing deck ({dt.Name})");
					foreach (var cd in dt.Deck.Cards)
					{
						cards += cd.Detail.Name;
						// don't add count if only one
						if (cd.Quantity > 1)
							cards += $" x {cd.Quantity}";
						cards += "\n";
					}
					// remove trailing newline
					if (cards.Length > 1)
						cards = cards.Substring(0, cards.Length - 1);

					// optionally remove player class from deck name
					// e.g. 'Control Warrior' => 'Control'
					var deckName = dt.Name;
					if (removeClass)
						deckName = deckName.Replace(dt.Deck.PlayerClass, "").Trim();

					_tracker.AddDeck(deckName, dt.Deck.PlayerClass, cards, archive, Strings.ArchetypeTag, Strings.PluginTag);
					deckCount++;
				}
			}
			else
			{
				_logger.Info($"Unknown meta import type: {type}");
			}
			return deckCount;
		}

		public async Task<UpdateResult> CheckForUpdates(string stdDate, string wildDate)
		{
			var std = GetSnapshotSlug(Strings.MetaStandard);
			var wild = GetSnapshotSlug(Strings.MetaWild);
			return new UpdateResult(stdDate, (await std).Item1, wildDate, (await wild).Item1);
		}
	}
}