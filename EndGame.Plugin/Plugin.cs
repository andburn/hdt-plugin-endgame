using System;
using System.Reflection;
using System.Windows.Controls;
using HDT.Plugins.Common.Providers.Metro;
using HDT.Plugins.Common.Providers.Tracker;
using HDT.Plugins.Common.Providers.Web;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Utils;
using Hearthstone_Deck_Tracker.Plugins;
using Ninject;

namespace HDT.Plugins.EndGame
{
	public class Plugin : IPlugin
	{
		private Version _version; 
		private IKernel _kernel;
		private IPluggable _plugin;

		public Plugin()
		{
			_version = GetVersion() ?? new Version(0, 0, 0, 0);
			_kernel = GetKernel();
			_plugin = new EndGame(_kernel, _version);
		}

		private MenuItem _menuItem;

		public MenuItem MenuItem
		{
			get
			{
				if (_menuItem == null)
					_menuItem = _plugin.CreateMenu();
				return _menuItem;
			}
		}

		public string Name => "End Game";

		public string Description => "Matches opponent's played cards to defined deck archetypes at the end of game.";

		public string ButtonText => "Settings";

		public string Author => "andburn";

		public Version Version => _version;

		public void OnButtonPress() => _plugin.ButtonPress();

		public void OnLoad() => _plugin.Load();

		public void OnUnload() => _plugin.Unload();

		public void OnUpdate() => _plugin.Repeat();

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
			return kernel;
		}

		private Version GetVersion()
		{
			return GitVersion.Get(Assembly.GetExecutingAssembly(), this);
		}
	}
}