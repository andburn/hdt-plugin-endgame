using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HDT.Plugins.EndGame.Archetype;

namespace HDT.Plugins.EndGame.Controls
{
	public class ArchetypeDeckListViewModel
	{
		public ObservableCollection<ArchetypeDeck> Decks { get; set; }

		public ArchetypeDeckListViewModel(List<ArchetypeDeck> decks)
		{
			Decks = new ObservableCollection<ArchetypeDeck>(decks);
		}

		public ArchetypeDeck GetDeck(object obj)
		{
			if (obj == null)
				return null;

			ArchetypeDeck d = obj as ArchetypeDeck;
			if (d == null)
				return null;

			return Decks.FirstOrDefault(x => x.Id == d.Id);
		}

		public void AddDeck(ArchetypeDeck deck)
		{
			Decks.Add(deck);
			ArchetypeManager.Instance.AddDeck(deck);
		}
	}
}