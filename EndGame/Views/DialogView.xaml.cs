using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;

namespace HDT.Plugins.EndGame.Views
{
	public partial class DialogView : UserControl
	{
		private Flyout _container;
		private Regex regex = new Regex(@"(?<pre>[^\[]*)\[(?<text>[^\]\(]+)\]\((?<url>[^\)]+)\)\s*(?<post>.*)", RegexOptions.Compiled);

		public DialogView(Flyout container, string title, string message, int autoClose)
		{
			InitializeComponent();

			_container = container;

			TitleText.Text = title;

			var match = regex.Match(message);
			if (match.Success)
			{
				Log.Debug("matched: ");
				MessageText.Inlines.Clear();
				MessageText.Inlines.Add(match.Groups["pre"].Value);
				Hyperlink hyperLink = new Hyperlink() {
					Foreground = System.Windows.Media.Brushes.White,
					NavigateUri = new Uri(match.Groups["url"].Value)
				};
				hyperLink.Inlines.Add(match.Groups["text"].Value);
				hyperLink.RequestNavigate += HyperLink_RequestNavigate;
				MessageText.Inlines.Add(hyperLink);
				MessageText.Inlines.Add(" " + match.Groups["post"].Value);
			}
			else
			{
				MessageText.Text = message;
			}

			AutoClose(autoClose);
		}

		public void SetUtilityButton(Action action, string icon)
		{
			var unicode = string.Empty;
			switch (icon.ToLower())
			{
				case "download":
					unicode = "\ue9c5"; break;
				case "error":
					unicode = "\uea0e"; break;
				case "github":
					unicode = "\ueab0"; break;
				case "info":
				default:
					unicode = "\uea08"; break;
			}

			UtilityButton.Content = unicode;
			UtilityButton.IsEnabled = true;
			if (action != null)
				UtilityButton.Click += (s, e) => { action.Invoke(); };
			else
				UtilityButton.IsEnabled = false;
			UtilityButton.UpdateLayout();
		}

		private void HyperLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(e.Uri.ToString());
		}

		private async Task AutoClose(int seconds)
		{
			// zero means no auto close
			if (seconds <= 0)
				return;
			await Task.Delay(seconds * 1000);
			_container.IsOpen = false;
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			_container.IsOpen = false;
		}
	}
}