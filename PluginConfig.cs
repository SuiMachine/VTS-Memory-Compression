using BepInEx.Configuration;

namespace VTSMemoryCompression
{
	public class PluginConfig
	{
		public static PluginConfig Instance { get; private set; }

		public ConfigEntry<bool> Enabled { get; private set; }
		public ConfigEntry<bool> RuntimeCompression { get; private set; }
		public ConfigEntry<bool> RuntimeCompressionHighQuality { get; private set; }


		public PluginConfig(ConfigFile config)
		{
			if (Instance != null)
				return;

			Instance = this;

			Enabled = config.Bind("General", "Enable", true, "Enables hooking code to be able to load DDS file replacements.");
			RuntimeCompression = config.Bind("General", "RuntimeCompression", false, "(Optional) Enables compressing textures to BC3 in runtime - this will only work on textures that don't have DDS files ready. HOWEVER: the BC3 quality is going to be worse than manually created BC7 and the loading time will be longer, since not only an original texture has to be loaded, but it will then have to be compressed to BC3!");
			RuntimeCompressionHighQuality = config.Bind("General", "RuntimeCompressionHighQuality", true, "(Optional) If a runtime compression is enabled, this affects whatever dithering will be applied to a source texture, which - according to Unity - should increase BC3 compression's quality a bit, but will make the process longer. You should still be using BC7 and premade DDS textures if possible.");
		}
	}
}
