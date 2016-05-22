using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HDT.Plugins.EndGame.Enums;
using Hearthstone_Deck_Tracker.Utility.Logging;
using HDTCard = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace HDT.Plugins.EndGame.Controls
{
	/// <summary>
	/// Interaction logic for ArchetypeDeckView.xaml
	/// </summary>
	public partial class ArchetypeDeckView : UserControl
	{
		private ArchetypeDeckViewModel _viewModel;

		public ArchetypeDeckView()
		{
			InitializeComponent();

			DataContextChanged += ArchetypeDeckView_DataContextChanged;
			_viewModel = (ArchetypeDeckViewModel)DataContext;

			ComboBoxKlass.ItemsSource = Enum.GetValues(typeof(PlayerClass));
			ComboBoxFormat.ItemsSource = Enum.GetValues(typeof(GameFormat));
		}

		public static readonly RoutedEvent DeckEditEvent = EventManager.RegisterRoutedEvent(
			"DeckEdit", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ArchetypeDeckView));

		public event RoutedEventHandler DeckEdit
		{
			add { AddHandler(DeckEditEvent, value); }
			remove { RemoveHandler(DeckEditEvent, value); }
		}

		private void RaiseDeckEditEvent()
		{
			RoutedEventArgs newEventArgs = new RoutedEventArgs(DeckEditEvent);
			RaiseEvent(newEventArgs);
		}

		private void ArchetypeDeckView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			_viewModel = (ArchetypeDeckViewModel)DataContext;
			Log.Info("DeckView context change: " + _viewModel.Name);
		}

		private void TextBoxCardSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = (TextBox)sender;
			if (textBox == null)
				return;
			if (string.IsNullOrEmpty(textBox.Text)) // show all cards
				return;

			SearchList.ItemsSource = _viewModel.ViableCardSearch(textBox.Text);
			SearchList.SelectedIndex = 0;
		}

		private void TextBoxCardSearch_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			var index = SearchList.SelectedIndex;
			switch (e.Key)
			{
				case Key.Down:
					if (index < SearchList.Items.Count - 1)
						SearchList.SelectedIndex += 1;
					break;

				case Key.Up:
					if (index > 0)
						SearchList.SelectedIndex -= 1;
					break;

				case Key.Enter:
					Log.Info(_viewModel.ToString());
					if (_viewModel != null)
					{
						Log.Info(SearchList.SelectedItem.ToString());
						if (SearchList.SelectedIndex >= 0)
						{
							var sc = ((HDTCard)SearchList.SelectedItem);
							_viewModel.AddCard(sc);
						}
					}
					break;
			}
			SearchList.ScrollIntoView(SearchList.SelectedItem);
		}

		private void ButtonAddCard_Click(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
			{
				Log.Info(SearchList.SelectedItem.ToString());
				if (SearchList.SelectedIndex >= 0)
				{
					_viewModel.AddCard((HDTCard)SearchList.SelectedItem);
				}
			}
		}

		private void ButtonRemoveCard_Click(object sender, RoutedEventArgs e)
		{
			if (_viewModel != null)
			{
				if (ListViewCards.SelectedIndex >= 0)
				{
					Log.Info(ListViewCards.SelectedItem.ToString());
					_viewModel.RemoveCard((HDTCard)ListViewCards.SelectedItem);
				}
			}
		}
	}
}