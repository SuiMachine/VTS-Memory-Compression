using HarmonyLib;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace VTSMemoryCompression.Hook
{
	[HarmonyPatch]
	public static class CubismTextureReplacerHook
	{
		[HarmonyPrefix]
		[HarmonyPatch(typeof(IOHelper), "CubismLoadAssetFromStreamingAssets")]
		public static bool CubismLoadAssetFromStreamingAssetsDetour(ref object __result, Type assetType, string absolutePath)
		{
			if (assetType != typeof(Texture2D))
				return true;

			var directory = Directory.GetParent(absolutePath).FullName;
			var fileName = Path.GetFileNameWithoutExtension(absolutePath) + ".dds";
			//var fileNameBin = Path.GetFileNameWithoutExtension(absolutePath) + ".bin";

/*			var fullPath = Path.Combine(directory, fileNameBin);
			if (File.Exists(fullPath))
			{
				Plugin.LogMessage($"Trying to load replacement: {fullPath}");

				Texture2D createdTexture = LoadHeaderlessTexture(fullPath);
				//True allows for old code to be executed, false prevents it
				if (createdTexture != null)
				{
					Plugin.LogMessage($"Replaced texture with {createdTexture.format}");
					__result = createdTexture;
					GC.Collect();
					return false;
				}
			}*/

			var fullPath = Path.Combine(directory, fileName);
			if (File.Exists(fullPath))
			{
				Plugin.LogMessage($"Trying to load replacement: {fullPath}");
				Texture2D createdTexture = DDS_Util.LoadTexture(fullPath);

				//True allows for old code to be executed, false prevents it
				if (createdTexture != null)
				{
					Plugin.LogMessage($"Replaced texture with {createdTexture.format}");
					__result = createdTexture;
					GC.Collect();
					return false;
				}
				else
					return true;
			}

			return true;
		}

		/*		[HarmonyPatch(typeof(CubismBuiltinPickers), nameof(CubismBuiltinPickers.TexturePicker))]
				[HarmonyPostfix]*/
		public static void TexturePickerDetour(ref Texture2D __result, CubismModel3Json sender, CubismDrawable drawable)
		{
			if (sender == null)
				return;

			if (string.IsNullOrEmpty(sender.DisplayInfo3Json))
				return;


			if (__result == null)
				return;

			switch (__result.format)
			{
				case TextureFormat.RGBA32:
				case TextureFormat.ARGB32:
					if (__result.width < 4096 || __result.height < 4096)
						break;

					for (int i = 0; i < sender.Textures.Length; i++)
					{
						if (sender.Textures[i] == null)
							continue;
						if (sender.Textures[i] == __result)
						{
							Plugin.LogError($"Creating new texture!");
							var newTexture = Compress(__result, __result.format, __result.mipmapCount);
							Plugin.LogError($"Created new format: {newTexture.format}!");
							Texture2D.Destroy(__result);
							sender.Textures[i] = newTexture;
							__result = newTexture;
							GC.Collect();
						}
					}
					break;
			}

			Plugin.LogError($"Texture picker format {__result.format}!");
		}

		private static Texture2D LoadHeaderlessTexture(string absolutePath)
		{
			if (!File.Exists(absolutePath))
			{
				Plugin.LogError($"No DDS file found at path {absolutePath}!");
				return null;
			}

			var loadedBytes = File.ReadAllBytes(absolutePath);
			var folder = int.Parse(Directory.GetParent(absolutePath).Name.Split('.').Last());

			Plugin.LogMessage(folder.ToString());
			var texture = new Texture2D(folder, folder, TextureFormat.BC7, true, false, true);
			texture.LoadRawTextureData(loadedBytes);
			texture.Apply(true, true);
			return texture;
		}

		private static Texture2D Compress(Texture2D source, TextureFormat format, int mipChainCount)
		{
			RenderTexture renderTex = RenderTexture.GetTemporary(
						source.width,
						source.height,
						0,
						RenderTextureFormat.Default,
						RenderTextureReadWrite.Linear);
			Graphics.Blit(source, renderTex);
			RenderTexture previous = RenderTexture.active;
			RenderTexture.active = renderTex;
			Texture2D readableText = new Texture2D(source.width, source.height, format, mipChainCount, true);
			readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
			readableText.Apply();
			readableText.Compress(true);
			RenderTexture.active = previous;
			RenderTexture.ReleaseTemporary(renderTex);
			return readableText;
		}
	}
}
