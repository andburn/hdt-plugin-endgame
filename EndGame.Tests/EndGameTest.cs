using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace HDT.Plugins.EndGame.Tests
{
	[TestFixture]
	public class EndGameTest
	{
		private EndGame _plugin;

		[OneTimeSetUp]
		public void Setup()
		{
			_plugin = new EndGame();
		}

		[Test]
		public void Attributes_Are_Correct()
		{
			Assert.AreEqual("End Game", _plugin.Name);
			Assert.AreEqual("Settings", _plugin.ButtonText);
			Assert.AreEqual("andburn", _plugin.Author);
			Assert.IsNotEmpty(_plugin.Description);
		}

		[Test]
		public void PluginVersion_Equals_AssemblyVersion()
		{
			var assembly = Assembly.GetAssembly(typeof(EndGame));
			var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			var version = fvi.FileVersion;
			Assert.AreEqual(version, _plugin.Version.ToString());
		}
	}
}
