using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace VTSMemoryCompression.Hook
{
	[HarmonyPatch]
	public static class CubismTextureReplacerHook
	{
		public static Dictionary<int, List<TimeSpan>> BenchmarkValues = new Dictionary<int, List<TimeSpan>>();

		//[HarmonyPrefix]
		//[HarmonyPatch(typeof(IOHelper), "CubismLoadAssetFromStreamingAssets")]
		public static bool CubismLoadAssetFromStreamingAssetsDetourOG(ref object __result, Type assetType, string absolutePath)
		{
			//This is for benchmark purposes - could be reworked to offer optional DXT5 runtime compression, though
			if (assetType != typeof(Texture2D))
				return true;

			Stopwatch sw = Stopwatch.StartNew();
			Texture2D newTexture = new Texture2D(4, 4, TextureFormat.RGBA32, true);
			newTexture.wrapMode = TextureWrapMode.Clamp;

			var bytesLoaded = File.ReadAllBytes(absolutePath);
			newTexture.LoadImage(bytesLoaded);
			//newTexture.Compress(true);
			newTexture.Apply(false, true);
			sw.Stop();

			if (BenchmarkValues.TryGetValue(newTexture.width, out List<TimeSpan> foundBenchmarkValues))
			{
				foundBenchmarkValues.Add(sw.Elapsed);
			}
			else
			{
				foundBenchmarkValues = new List<TimeSpan>()
				{
					sw.Elapsed
				};
				BenchmarkValues.Add(newTexture.width, foundBenchmarkValues);
			}

			TimeSpan average = TimeSpan.Zero;
			foreach (TimeSpan element in foundBenchmarkValues)
				average += element;
			average /= foundBenchmarkValues.Count;

			Plugin.LogMessage($"LOAD BENCHMARK {newTexture.width} ({foundBenchmarkValues.Count} - {newTexture.format}) = {average}");

			__result = newTexture;

			return false;
		}


		[HarmonyPrefix]
		[HarmonyPatch(typeof(IOHelper), "CubismLoadAssetFromStreamingAssets")]
		public static bool CubismLoadAssetFromStreamingAssetsDetour(ref object __result, Type assetType, string absolutePath)
		{
			if (assetType != typeof(Texture2D))
				return true;

			var directory = Directory.GetParent(absolutePath).FullName;
			var fileName = Path.GetFileNameWithoutExtension(absolutePath) + ".dds";

			var fullPath = Path.Combine(directory, fileName);
			if (File.Exists(fullPath))
			{
				Plugin.LogMessage($"Trying to load replacement: {fullPath}");
				Stopwatch sw = Stopwatch.StartNew();
				Texture2D createdTexture = DDS_Util.LoadTexture(fullPath);

				//True allows for old code to be executed, false prevents it
				if (createdTexture != null)
				{
					sw.Start();
					if (BenchmarkValues.TryGetValue(createdTexture.width, out List<TimeSpan> foundBenchmarkValues))
					{
						foundBenchmarkValues.Add(sw.Elapsed);
					}
					else
					{
						foundBenchmarkValues = new List<TimeSpan>()
						{
							sw.Elapsed
						};
						BenchmarkValues.Add(createdTexture.width, foundBenchmarkValues);
					}

					TimeSpan average = TimeSpan.Zero;
					foreach (TimeSpan element in foundBenchmarkValues)
						average += element;
					average /= foundBenchmarkValues.Count;
					Plugin.LogMessage($"LOAD BENCHMARK {createdTexture.width} ({foundBenchmarkValues.Count} - {createdTexture.format}) = {average}");

					__result = createdTexture;
					GC.Collect();
					return false;
				}
				else
					return true;
			}

			return true;
		}
	}
}
