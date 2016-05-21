using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hearthstone_Deck_Tracker.Stats;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Archetype
{
	public class ArchetypeManager
	{
		private const string DECKS_FILE = @".\deck.json";

		private static ArchetypeManager _instance;

		public static ArchetypeManager Instance
		{
			get
			{
				if (_instance == null)
					_instance = new ArchetypeManager();
				return _instance;
			}
		}

		private List<ArchetypeDeck> _archetypes;

		public List<ArchetypeDeck> Decks
		{
			get { return _archetypes; }
		}

		private List<ArchetypeStyle> _defaultStyles;
		private List<ArchetypeStyle> _customStyles;

		public List<ArchetypeStyle> Styles
		{
			get
			{
				var styles = new List<ArchetypeStyle>(_defaultStyles);
				styles.AddRange(_customStyles);
				return styles;
			}
		}

		private ArchetypeManager()
		{
			_archetypes = new List<ArchetypeDeck>();
			_defaultStyles = new List<ArchetypeStyle>() {
				ArchetypeStyles.AGGRO,
				ArchetypeStyles.COMBO,
				ArchetypeStyles.CONTROL,
				ArchetypeStyles.MIDRANGE
			};
			_customStyles = new List<ArchetypeStyle>();
		}

		public void Reset()
		{
			_archetypes.Clear();
			_customStyles.Clear();
		}

		public void LoadDecks(string file = null)
		{
			List<ArchetypeDeck> decks = new List<ArchetypeDeck>();
			try
			{
				decks = JsonConvert.DeserializeObject<List<ArchetypeDeck>>(
					File.ReadAllText(file ?? DECKS_FILE));
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
			if (decks.Count > 0)
			{
				Decks.Clear();
				foreach (var d in decks)
					AddDeck(d);
			}
		}

		public void SaveDecks(string file = null)
		{
			try
			{
				File.WriteAllText(file ?? DECKS_FILE,
					JsonConvert.SerializeObject(_archetypes));
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public void AddDeck(ArchetypeDeck deck)
		{
			// Skip duplicate decks
			if (!_archetypes.Any(x => x.Matches(deck)))
			{
				_archetypes.Add(deck);
			}
		}

		public void RemoveDeck(ArchetypeDeck deck)
		{
			// Remove actual deck
			var success = _archetypes.Remove(deck);
			// else remove matching decks
			if (!success)
				_archetypes.RemoveAll(x => x.Matches(deck));
		}

		public ArchetypeDeck GetDeck(Guid id)
		{
			return _archetypes.FirstOrDefault(x => x.Id == id);
		}

		public ArchetypeDeck GetDeck(object obj)
		{
			if (obj == null)
				return null;

			ArchetypeDeck d = obj as ArchetypeDeck;
			if (d == null)
				return null;

			return GetDeck(d.Id);
		}

		public List<ArchetypeDeck> Find(GameStats game)
		{
			var deck = new PlayedDeck(game.OpponentHero, game.Format, game.Turns, game.OpponentCards);
			return Find(deck);
		}

		public List<ArchetypeDeck> Find(PlayedDeck deck)
		{
			// TODO remove any 0 and/or use a a threshold
			return _archetypes.OrderByDescending(x => deck.Similarity(x)).ToList();
		}

		public void AddStyle(ArchetypeStyle style)
		{
			if (!_customStyles.Contains(style))
				_customStyles.Add(style);
		}

		public void RemoveStyle(ArchetypeStyle style)
		{
			_customStyles.Remove(style);
		}
	}
}