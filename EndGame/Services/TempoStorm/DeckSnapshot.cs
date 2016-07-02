using System.Collections.Generic;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Services.TempoStorm
{
	public class DeckSnapshot
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("tier")]
		public int Tier { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("deckId")]
		public string DeckId { get; set; }

		[JsonProperty("deck")]
		public Deck Deck { get; set; }
	}

	public class Deck
	{
		[JsonProperty("playerClass")]
		public string PlayerClass { get; set; }

		[JsonProperty("slugs")]
		public List<SlugItem> Slugs { get; set; }

		[JsonProperty("cards")]
		public List<Card> Cards { get; set; }
	}

	public class Card
	{
		[JsonProperty("cardQuantity")]
		public int Quantity { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("deckId")]
		public string DeckId { get; set; }

		[JsonProperty("cardId")]
		public string CardId { get; set; }

		[JsonProperty("card")]
		public CardDetail Detail { get; set; }
	}

	public class CardDetail
	{
		[JsonProperty("cardId")]
		public string CardId { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("cost")]
		public int Cost { get; set; }

		[JsonProperty("cardType")]
		public string CardType { get; set; }
	}
}