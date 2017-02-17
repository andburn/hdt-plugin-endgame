using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.Common.Controls.SlidePanels;
using HDT.Plugins.Common.Plugin;
using HDT.Plugins.Common.Providers;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Settings;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Services.TempoStorm;
using HDT.Plugins.EndGame.Utilities;
using HDT.Plugins.EndGame.ViewModels;
using HDT.Plugins.EndGame.Views;

namespace HDT.Plugins.EndGame
{
	[Name("End Game")]
	[Description("Matches opponent's played cards to defined deck archetypes at the end of game.")]
	public class EndGame : PluginBase
	{
		public static readonly IUpdateService Updater;
		public static readonly ILoggingService Logger;
		public static readonly IDataRepository Data;
		public static readonly IEventsService Events;
		public static readonly IGameClientService Client;
		public static readonly IConfigurationRepository Config;
		public static readonly Settings Settings;

		static EndGame()
		{
			// initialize services
			var resolver = Injector.Instance.Container;
			Updater = resolver.GetInstance<IUpdateService>();
			Logger = resolver.GetInstance<ILoggingService>();
			Data = resolver.GetInstance<IDataRepository>();
			Events = resolver.GetInstance<IEventsService>();
			Client = resolver.GetInstance<IGameClientService>();
			Config = resolver.GetInstance<IConfigurationRepository>();
			// load settings
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "HDT.Plugins.EndGame.Resources.Default.ini";
			Settings = new Settings(assembly.GetManifestResourceStream(resourceName), "EndGame");
		}

		public EndGame()
			: base(Assembly.GetExecutingAssembly())
		{
		}

		private MenuItem _menuItem;

		public override MenuItem MenuItem
		{
			get
			{
				if (_menuItem == null)
					_menuItem = CreatePluginMenu();
				return _menuItem;
			}
		}

		private MenuItem CreatePluginMenu()
		{
			var pm = new PluginMenu("End Game", "trophy");
			pm.Append("Import Meta Decks",
				new RelayCommand(async () => await ImportMetaDecks()));
			pm.Append("Settings",
				new RelayCommand(() => ShowSettings()));
			return pm.Menu;
		}

		public override void OnButtonPress()
		{
			ShowSettings();
		}

		public override async void OnLoad()
		{
			try
			{
				// disable the built in note dialog
				Config.Set("ShowNoteDialogAfterGame", false);
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
			// check for plugin update
			await UpdateCheck("EndGame", "hdt-plugin-endgame");
			// set the action to run on the game end event
			Events.OnGameEnd(Run);
		}

		public override void OnUnload()
		{
			CloseOpenNoteWindows();
			CloseSettings();
		}

		public async static void Run()
		{
			try
			{
				var mode = Data.GetGameMode();
				// close any already open note windows
				CloseOpenNoteWindows();
				// check what features are enabled
				if (IsModeEnabledForArchetypes(mode))
				{
					//var viewModel = new NoteViewModel();
					//var view = new NoteView();
					//view.DataContext = viewModel;
					var view = new MainView();
					await WaitUntilInMenu();
					view.Show();
				}
			}
			catch (Exception e)
			{
				Logger.Error(e);
				Notify("EndGame Error", e.Message, 15, "error", null);
			}
		}

		public static void CloseSettings()
		{
		}

		public static void ShowSettings()
		{
			CloseOpenNoteWindows();
			var view = new MainView();
			view.Show();
		}

		public static void CloseOpenNoteWindows()
		{
			foreach (var x in Application.Current.Windows.OfType<MainView>())
				x.Close();
		}

		public static void Notify(string title, string message, int autoClose, string icon = null, Action action = null)
		{
			SlidePanelManager
				.Notification(title, message, icon, action)
				.AutoClose(autoClose);
		}

		public static async Task ImportMetaDecks()
		{
			try
			{
				IArchetypeImporter importer = new SnapshotImporter(new HttpClient(), Data, Logger);
				var count = await importer.ImportDecks(
					Settings.Get(Strings.AutoArchiveArchetypes).Bool,
					Settings.Get(Strings.DeletePreviouslyImported).Bool,
					Settings.Get(Strings.RemoveClassFromName).Bool);
				Notify("Import Complete", $"{count} decks imported", 10);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				Notify("Import Failed", e.Message, 15, "error", null);
			}
		}

		private static bool IsModeEnabledForArchetypes(string mode)
		{
			switch (mode.ToLowerInvariant())
			{
				case "ranked":
					return Settings.Get(Strings.RecordRankedArchetypes).Bool;

				case "casual":
					return Settings.Get(Strings.RecordCasualArchetypes).Bool;

				case "brawl":
					return Settings.Get(Strings.RecordBrawlArchetypes).Bool;

				case "friendly":
					return Settings.Get(Strings.RecordFriendlyArchetypes).Bool;

				case "arena":
					return Settings.Get(Strings.RecordArenaArchetypes).Bool;

				default:
					return Settings.Get(Strings.RecordOtherArchetypes).Bool;
			}
		}

		private static async Task WaitUntilInMenu()
		{
			var timeout = 30000;
			var wait = 1000;
			var elapsed = 0;
			while (!Client.IsInMenu())
			{
				await Task.Delay(wait);
				elapsed += wait;
				if (elapsed >= timeout)
					return;
			}
		}

		private async Task UpdateCheck(string name, string repo)
		{
			var uri = new Uri($"https://api.github.com/repos/andburn/{repo}/releases");
			Logger.Debug("update uri = " + uri);
			try
			{
				var latest = await Updater.CheckForUpdate(uri, Version);
				if (latest.HasUpdate)
				{
					Logger.Info($"Plugin Update available ({latest.Version})");
					SlidePanelManager
						.Notification("Plugin Update Available",
							$"[DOWNLOAD]({latest.DownloadUrl}) {name} v{latest.Version}",
							"download3",
							() => Process.Start(latest.DownloadUrl))
						.AutoClose(10);
				}
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
		}
	}
}