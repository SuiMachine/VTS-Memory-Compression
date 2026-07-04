using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;

namespace VTSMemoryCompression
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
		public static ManualLogSource LoggerInstance { get; private set; }
		public static Harmony HarmonyInstance { get; private set; }
		public static PluginConfig ConfigVariables { get; private set; }


		private void Awake()
        {
			// Plugin startup logic
			LoggerInstance = base.Logger;
			HarmonyInstance = new HarmonyLib.Harmony("local.VTSMemoryCompression.SuiMachine");
			ConfigVariables = new PluginConfig(Config);

			Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
			if (ConfigVariables.Enabled.Value)
			{
				Logger.LogInfo($"Patching...");
				HarmonyInstance.PatchAll();
				Logger.LogInfo($"Patched!");
			}
			else
				Logger.LogInfo($"Skipped patching due to config variable...");
		}

		public static void LogInfo(string text) => LoggerInstance.LogInfo(text);

		public static void LogMessage(string text) => LoggerInstance.LogMessage(text);

		public static void LogError(string text) => LoggerInstance.LogError(text);

		public static void LogWarning(string text) => LoggerInstance.LogWarning(text);
	}
}
