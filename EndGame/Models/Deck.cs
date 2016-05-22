using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Enums;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Models
{
	public abstract class Deck : ObservableObject
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		private PlayerClass _klass;

		[JsonProperty("class")]
		public PlayerClass Klass
		{
			get
			{
				return _klass;
			}
			set
			{
				Set(() => Klass, ref _klass, value);
			}
		}

		private GameFormat _format;

		[JsonProperty("format")]
		public GameFormat Format
		{
			get
			{
				return _format;
			}
			set
			{
				Set(() => Format, ref _format, value);
			}
		}

		[JsonProperty("cards")]
		public ObservableCollection<Card> Cards { get; set; }

		public Deck()
		{
			Id = Guid.NewGuid();
			Cards = new ObservableCollection<Card>();
		}

		public Deck(PlayerClass klass, GameFormat format, List<Card> cards)
			: this()
		{
			Klass = klass;
			Format = format;
			cards?.ForEach(c => Cards.Add(c));
		}

		public Deck(string klass, GameFormat format, List<Card> cards)
			: this()
		{
			Klass = Utils.KlassFromString(klass);
			Format = format;
			cards?.ForEach(c => Cards.Add(c));
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			Deck d = obj as Deck;
			if (d == null)
			{
				return false;
			}

			return Id.Equals(d.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public bool Matches(Deck d)
		{
			return Klass == d.Klass && Format == d.Format
				&& Cards.OrderBy(x => x).SequenceEqual(d.Cards.OrderBy(x => x));
		}
	}
}