using GalaSoft.MvvmLight.Command;
using HDT.Plugins.Common.Controls;
using HDT.Plugins.Common.Providers.Metro;
using HDT.Plugins.Common.Providers.Tracker;
using HDT.Plugins.Common.Providers.Web;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Settings;
using HDT.Plugins.Common.Utils;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Services.TempoStorm;
using HDT.Plugins.EndGame.Utilities;
using HDT.Plugins.EndGame.ViewModels;
using HDT.Plugins.EndGame.Views;
using Hearthstone_Deck_Tracker.Plugins;
using Ninject;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HDT.Plugins.EndGame
{
	public class EndGame : IPlugin
	{
		private static IKernel _kernel;
		private static MainViewModel _viewModel;

		public static IUpdateService Updater;
		public static ILoggingService Logger;
		public static IDataRepository Data;
		public static IEventsService Events;
		public static IGameClientService Client;
		public static IConfigurationRepository Config;
		public static Settings Settings;

		public EndGame()
		{
			_kernel = GetKernel();
			// initialize services
			Updater = _kernel.Get<IUpdateService>();
			Logger = _kernel.Get<ILoggingService>();
			Data = _kernel.Get<IDataRepository>();
			Events = _kernel.Get<IEventsService>();
			Client = _kernel.Get<IGameClientService>();
			Config = _kernel.Get<IConfigurationRepository>();
			// load settings
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "HDT.Plugins.EndGame.Resources.Default.ini";
			Settings = new Settings(assembly.GetManifestResourceStream(resourceName), "EndGame");
			// set logger name and pass object down to common
			Logger.SetDumpFileName("EndGame");
			UpdateLogger();
			Common.Common.Log = Logger;
			// main view model
			_viewModel = new MainViewModel();
		}

		public string Name => "End Game";

		public string Description => "Matches opponent's played cards to defined deck archetypes at the end of game.";

		public string ButtonText => "Settings";

		public string Author => "andburn";

		private Version _version;

		public Version Version
		{
			get
			{
				if (_version == null)
					_version = GetVersion() ?? new Version(0, 0, 0, 0);
				return _version;
			}
		}

		private MenuItem _menuItem;

		public MenuItem MenuItem
		{
			get
			{
				if (_menuItem == null)
					_menuItem = CreateMenu();
				return _menuItem;
			}
		}

		public async void OnButtonPress()
		{
			await ShowSettings();
		}

		public async void OnLoad()
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
			// update meta decks if an update is available
			await ImportMetaDecks(false);
			// check for plugin update
			await UpdateCheck("andburn", "hdt-plugin-endgame");
			// set the action to run on the game end event
			Events.OnGameEnd(Run);
		}

		public void OnUnload()
		{
			CloseMainView();
		}

		public void OnUpdate()
		{
		}

		public async static void Run()
		{
			try
			{
				if (Settings.Get(Strings.WaitUntilBackInMenu).Bool)
					await WaitUntilInMenu();
				await ShowMainView(Strings.NavNote);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				Notify("EndGame Error", e.Message, 15, IcoMoon.Warning, null);
			}
		}

		public static async Task ShowSettings()
		{
			await ShowMainView(Strings.NavSettings);
		}

		public static async Task ShowNote()
		{
			await ShowMainView(Strings.NavNote);
		}

		public static async Task ShowStats()
		{
			await ShowMainView(Strings.NavStats);
		}

		public static async Task ShowMainView(string location)
		{
			Logger.Debug($"EndGame: Showing MainView ({location})");

			if (location.ToLower() == Strings.NavNote
				&& !NoteBoxShouldBeDisplayed())
			{
				Logger.Debug($"EndGame: Aborting main view navigation on '{Strings.NavNote}'");
				return;
			}

			MainView view = null;
			// check for any open windows
			var open = Application.Current.Windows.OfType<MainView>();
			if (open.Count() == 1)
			{
				view = open.FirstOrDefault();
			}
			else
			{
				CloseMainView();
				// create view
				view = new MainView()
				{
					DataContext = _viewModel
				};
			}
			// show the window, and restore if needed
			view.Show();
			if (view.WindowState == WindowState.Minimized)
				view.WindowState = WindowState.Normal;
			view.Activate();

			// navigate to location
			await _viewModel.OnNavigation(location);
		}

		public static void CloseMainView()
		{
			foreach (var x in Application.Current.Windows.OfType<MainView>())
				x.Close();
		}

		public static void Notify(string title, string message, int autoClose, string icon = null, Action action = null)
		{
			SlidePanelManager
				.Notification(_kernel.Get<ISlidePanel>(), title, message, icon, action)
				.AutoClose(autoClose);
		}

		public static async Task ImportMetaDecks(bool forced)
		{
			try
			{
				IArchetypeImporter importer = _kernel.Get<SnapshotImporter>();
				bool incWild = Settings.Get(Strings.IncludeWild).Bool;
				// check if there is an update available
				UpdateResult update = await importer.CheckForUpdates(
					Settings.Get(Strings.LastSnapshotStandard),
					Settings.Get(Strings.LastSnapshotWild));
				// import the decks if there is an update or forced
				if (forced || (update != null && update.HasUpdates(incWild)))
				{
					var count = await importer.ImportDecks(incWild,
						Settings.Get(Strings.AutoArchiveArchetypes).Bool,
						Settings.Get(Strings.DeletePreviouslyImported).Bool,
						Settings.Get(Strings.RemoveClassFromName).Bool);

					if (forced)
						Logger.Debug($"EndGame: forced meta update ({count})");
					else
						Logger.Debug($"EndGame: meta update available ({count})");

					// on success save date and show notification
					if (count > 0)
					{
						Settings.Set(Strings.LastSnapshotStandard, update.StandardLatest);
						if (incWild)
							Settings.Set(Strings.LastSnapshotWild, update.WildLatest);
						Notify("Meta Decks Updated", $"{count} decks imported", 10, IcoMoon.Notification, null);
					}
					else
					{
						Logger.Error("Update error, no decks imported");
						Notify("Import Failed", "Update contained no decks", 15, IcoMoon.Warning, null);
					}
				}
				else
					Logger.Debug("EndGame: No meta updates available");
			}
			catch (Exception e)
			{
				Logger.Error(e);
				Notify("Import Failed", e.Message, 15, IcoMoon.Warning, null);
			}
		}

		public static void UpdateLogger()
		{
			Logger.Debug($"EndGame: Updating Logger");
			if (Settings.Get(Strings.DebugLog).Bool)
				Logger.EnableDumpToFile();
			else
				Logger.DisableDumpToFile();
		}

		// Check if note box should be displayed before navigating to
		// the MainView and showing the note, avoids it popping up
		// and ignoring other settings
		public static bool NoteBoxShouldBeDisplayed()
		{
			if (Settings.Get(Strings.DeveloperMode).Bool)
			{
				return true;
			}
			else if (ViewModelHelper.IsDeckAvailable())
			{
				var mode = Data.GetGameMode();
				if (ViewModelHelper.IsModeEnabledForArchetypes(mode))
				{
					return ViewModelHelper.ModeIsEffectedByRank(mode);
				}
				else if (Settings.Get(Strings.ShowRegularNoteBox).Bool)
				{
					return ViewModelHelper.ModeIsEffectedByRank(mode);
				}
			}
			return false;
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
				{
					Logger.Debug($"EndGame: InMenu wait timed out after {timeout / 1000}s");
					return;
				}
			}
		}

		private async Task UpdateCheck(string user, string repo)
		{
			try
			{
				var latest = await Updater.CheckForUpdate(user, repo, Version);
				if (latest.HasUpdate)
				{
					Logger.Info($"Plugin Update available ({latest.Version})");
					Notify("Plugin Update Available",
						$"[DOWNLOAD]({latest.DownloadUrl}) {Name} v{latest.Version}",
						10, IcoMoon.Download3, () => Process.Start(latest.DownloadUrl));
				}
			}
			catch (Exception e)
			{
				Logger.Error($"Github update failed: {e.Message}");
			}
		}

		private MenuItem CreateMenu()
		{
			var pm = new PluginMenu("End Game", IcoMoon.Target);
			pm.Append("Import Meta Decks", IcoMoon.Download2,
				new RelayCommand(async () => await ImportMetaDecks(true)));
			pm.Append("Stats", IcoMoon.StatsDots,
				new RelayCommand(async () => await ShowStats()));
			pm.Append("Settings", IcoMoon.Cog,
				new RelayCommand(async () => await ShowSettings()));
			if (Settings.Get(Strings.DeveloperMode).Bool)
			{
				pm.Append("Note", IcoMoon.FileText2,
					new RelayCommand(async () => await ShowNote()));
			}
			return pm.Menu;
		}

		private Version GetVersion()
		{
			return GitVersion.Get(Assembly.GetExecutingAssembly(), this);
		}

		private IKernel GetKernel()
		{
			var kernel = new StandardKernel();
			kernel.Bind<IDataRepository>().To<TrackerDataRepository>().InSingletonScope();
			kernel.Bind<IUpdateService>().To<GitHubUpdateService>().InSingletonScope();
			kernel.Bind<ILoggingService>().To<TrackerLoggingService>().InSingletonScope();
			kernel.Bind<IEventsService>().To<TrackerEventsService>().InSingletonScope();
			kernel.Bind<IGameClientService>().To<TrackerClientService>().InSingletonScope();
			kernel.Bind<IConfigurationRepository>().To<TrackerConfigRepository>().InSingletonScope();
			kernel.Bind<ISlidePanel>().To<MetroSlidePanel>();
			kernel.Bind<IHttpClient>().To<HttpClient>();
			return kernel;
		}
	}
}