using Live2D.Cubism.Rendering;
using System.Collections.Generic;
using UnityEngine;

namespace VTSMemoryCompression
{
	public class ExporterGO : MonoBehaviour
	{
		[System.Serializable]
		public class SerializableData
		{
			public Vector3[] MeshPolys;
			public int[] Indencies;
			public Vector2[] UVs;

			public int RenderOrder;
			public int MaterialID;
			public string MaterialName;
			public string TextureUsed;
			public int TextureHash;
			public string ObjectName;
			public Vector3 position;
			public Vector3 rotation;
			public Vector3 scale;
		}

		private static ExporterGO m_instance;
		public static ExporterGO GetInstance()
		{
			if (m_instance != null)
				return m_instance;

			var go = new GameObject("ExporterGO")
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			m_instance = go.AddComponent<ExporterGO>();
			DontDestroyOnLoad(go);
			go.transform.position = new Vector3(0, 0, 0);
			return m_instance;
		}

		private void OnGUI()
		{
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.Label("We are here:");
			if (GUILayout.Button("Export?"))
			{
				var mainModel = VTubeStudioModelLoader.GetMainModel();
				if (mainModel != null)
				{
					Plugin.LogMessage("Storing");
					List<SerializableData> materialsToStore = new List<SerializableData>();

					var meshes = mainModel.GetComponentsInChildren<MeshFilter>();
					Plugin.LogMessage($"Meshes to store: {meshes.Length}");

					for (int i = 0; i < meshes.Length; i++)
					{
						var mf = meshes[i];
						var mr = meshes[i].GetComponent<MeshRenderer>();
						var cr = meshes[i].GetComponent<CubismRenderer>();

						if (mf == null || mr == null || cr == null)
							continue;

						if (cr.MainTexture == null)
							continue;

						var data = new SerializableData()
						{
							RenderOrder = mr.sortingOrder,
							MaterialID = mr.sharedMaterial.GetInstanceID(),
							MaterialName = cr.Material.name,
							TextureUsed = cr.MainTexture.name,
							TextureHash = cr.MainTexture.GetHashCode(),
							ObjectName = cr.name,
							position = cr.transform.localPosition,
							rotation = cr.transform.localEulerAngles,
							scale = cr.transform.localScale,
						};
						var mesh = mf.sharedMesh;
						data.MeshPolys = mesh.vertices;
						data.Indencies = mesh.triangles;
						data.UVs = mesh.uv;

						materialsToStore.Add(data);
					}
					Plugin.LogMessage($"Ready: {materialsToStore.Count}");

					Utils.Store("test.xml", materialsToStore);
					Plugin.LogMessage("Ready");

				}
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}
	}
}
