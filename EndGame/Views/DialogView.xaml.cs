using System.Threading.Tasks;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace HDT.Plugins.EndGame.Views
{
	public partial class DialogView : UserControl
	{
		private Flyout _container;

		public DialogView(Flyout container, string title, string message, int autoClose)
		{
			InitializeComponent();

			_container = container;

			TitleText.Text = title;
			MessageText.Text = message;

			AutoClose(autoClose);
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