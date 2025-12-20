using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace VTSMemoryCompression
{
	public enum DXGI_FORMAT : uint
	{
		DXGI_FORMAT_UNKNOWN = 0,
		DXGI_FORMAT_R32G32B32A32_TYPELESS = 1,
		DXGI_FORMAT_R32G32B32A32_FLOAT = 2,
		DXGI_FORMAT_R32G32B32A32_UINT = 3,
		DXGI_FORMAT_R32G32B32A32_SINT = 4,
		DXGI_FORMAT_R32G32B32_TYPELESS = 5,
		DXGI_FORMAT_R32G32B32_FLOAT = 6,
		DXGI_FORMAT_R32G32B32_UINT = 7,
		DXGI_FORMAT_R32G32B32_SINT = 8,
		DXGI_FORMAT_R16G16B16A16_TYPELESS = 9,
		DXGI_FORMAT_R16G16B16A16_FLOAT = 10,
		DXGI_FORMAT_R16G16B16A16_UNORM = 11,
		DXGI_FORMAT_R16G16B16A16_UINT = 12,
		DXGI_FORMAT_R16G16B16A16_SNORM = 13,
		DXGI_FORMAT_R16G16B16A16_SINT = 14,
		DXGI_FORMAT_R32G32_TYPELESS = 15,
		DXGI_FORMAT_R32G32_FLOAT = 16,
		DXGI_FORMAT_R32G32_UINT = 17,
		DXGI_FORMAT_R32G32_SINT = 18,
		DXGI_FORMAT_R32G8X24_TYPELESS = 19,
		DXGI_FORMAT_D32_FLOAT_S8X24_UINT = 20,
		DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS = 21,
		DXGI_FORMAT_X32_TYPELESS_G8X24_UINT = 22,
		DXGI_FORMAT_R10G10B10A2_TYPELESS = 23,
		DXGI_FORMAT_R10G10B10A2_UNORM = 24,
		DXGI_FORMAT_R10G10B10A2_UINT = 25,
		DXGI_FORMAT_R11G11B10_FLOAT = 26,
		DXGI_FORMAT_R8G8B8A8_TYPELESS = 27,
		DXGI_FORMAT_R8G8B8A8_UNORM = 28,
		DXGI_FORMAT_R8G8B8A8_UNORM_SRGB = 29,
		DXGI_FORMAT_R8G8B8A8_UINT = 30,
		DXGI_FORMAT_R8G8B8A8_SNORM = 31,
		DXGI_FORMAT_R8G8B8A8_SINT = 32,
		DXGI_FORMAT_R16G16_TYPELESS = 33,
		DXGI_FORMAT_R16G16_FLOAT = 34,
		DXGI_FORMAT_R16G16_UNORM = 35,
		DXGI_FORMAT_R16G16_UINT = 36,
		DXGI_FORMAT_R16G16_SNORM = 37,
		DXGI_FORMAT_R16G16_SINT = 38,
		DXGI_FORMAT_R32_TYPELESS = 39,
		DXGI_FORMAT_D32_FLOAT = 40,
		DXGI_FORMAT_R32_FLOAT = 41,
		DXGI_FORMAT_R32_UINT = 42,
		DXGI_FORMAT_R32_SINT = 43,
		DXGI_FORMAT_R24G8_TYPELESS = 44,
		DXGI_FORMAT_D24_UNORM_S8_UINT = 45,
		DXGI_FORMAT_R24_UNORM_X8_TYPELESS = 46,
		DXGI_FORMAT_X24_TYPELESS_G8_UINT = 47,
		DXGI_FORMAT_R8G8_TYPELESS = 48,
		DXGI_FORMAT_R8G8_UNORM = 49,
		DXGI_FORMAT_R8G8_UINT = 50,
		DXGI_FORMAT_R8G8_SNORM = 51,
		DXGI_FORMAT_R8G8_SINT = 52,
		DXGI_FORMAT_R16_TYPELESS = 53,
		DXGI_FORMAT_R16_FLOAT = 54,
		DXGI_FORMAT_D16_UNORM = 55,
		DXGI_FORMAT_R16_UNORM = 56,
		DXGI_FORMAT_R16_UINT = 57,
		DXGI_FORMAT_R16_SNORM = 58,
		DXGI_FORMAT_R16_SINT = 59,
		DXGI_FORMAT_R8_TYPELESS = 60,
		DXGI_FORMAT_R8_UNORM = 61,
		DXGI_FORMAT_R8_UINT = 62,
		DXGI_FORMAT_R8_SNORM = 63,
		DXGI_FORMAT_R8_SINT = 64,
		DXGI_FORMAT_A8_UNORM = 65,
		DXGI_FORMAT_R1_UNORM = 66,
		DXGI_FORMAT_R9G9B9E5_SHAREDEXP = 67,
		DXGI_FORMAT_R8G8_B8G8_UNORM = 68,
		DXGI_FORMAT_G8R8_G8B8_UNORM = 69,
		DXGI_FORMAT_BC1_TYPELESS = 70,
		DXGI_FORMAT_BC1_UNORM = 71,
		DXGI_FORMAT_BC1_UNORM_SRGB = 72,
		DXGI_FORMAT_BC2_TYPELESS = 73,
		DXGI_FORMAT_BC2_UNORM = 74,
		DXGI_FORMAT_BC2_UNORM_SRGB = 75,
		DXGI_FORMAT_BC3_TYPELESS = 76,
		DXGI_FORMAT_BC3_UNORM = 77,
		DXGI_FORMAT_BC3_UNORM_SRGB = 78,
		DXGI_FORMAT_BC4_TYPELESS = 79,
		DXGI_FORMAT_BC4_UNORM = 80,
		DXGI_FORMAT_BC4_SNORM = 81,
		DXGI_FORMAT_BC5_TYPELESS = 82,
		DXGI_FORMAT_BC5_UNORM = 83,
		DXGI_FORMAT_BC5_SNORM = 84,
		DXGI_FORMAT_B5G6R5_UNORM = 85,
		DXGI_FORMAT_B5G5R5A1_UNORM = 86,
		DXGI_FORMAT_B8G8R8A8_UNORM = 87,
		DXGI_FORMAT_B8G8R8X8_UNORM = 88,
		DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM = 89,
		DXGI_FORMAT_B8G8R8A8_TYPELESS = 90,
		DXGI_FORMAT_B8G8R8A8_UNORM_SRGB = 91,
		DXGI_FORMAT_B8G8R8X8_TYPELESS = 92,
		DXGI_FORMAT_B8G8R8X8_UNORM_SRGB = 93,
		DXGI_FORMAT_BC6H_TYPELESS = 94,
		DXGI_FORMAT_BC6H_UF16 = 95,
		DXGI_FORMAT_BC6H_SF16 = 96,
		DXGI_FORMAT_BC7_TYPELESS = 97,
		DXGI_FORMAT_BC7_UNORM = 98,
		DXGI_FORMAT_BC7_UNORM_SRGB = 99,
		DXGI_FORMAT_AYUV = 100,
		DXGI_FORMAT_Y410 = 101,
		DXGI_FORMAT_Y416 = 102,
		DXGI_FORMAT_NV12 = 103,
		DXGI_FORMAT_P010 = 104,
		DXGI_FORMAT_P016 = 105,
		DXGI_FORMAT_420_OPAQUE = 106,
		DXGI_FORMAT_YUY2 = 107,
		DXGI_FORMAT_Y210 = 108,
		DXGI_FORMAT_Y216 = 109,
		DXGI_FORMAT_NV11 = 110,
		DXGI_FORMAT_AI44 = 111,
		DXGI_FORMAT_IA44 = 112,
		DXGI_FORMAT_P8 = 113,
		DXGI_FORMAT_A8P8 = 114,
		DXGI_FORMAT_B4G4R4A4_UNORM = 115,
		DXGI_FORMAT_P208 = 130,
		DXGI_FORMAT_V208 = 131,
		DXGI_FORMAT_V408 = 132,
		DXGI_FORMAT_SAMPLER_FEEDBACK_MIN_MIP_OPAQUE = 189,
		DXGI_FORMAT_SAMPLER_FEEDBACK_MIP_REGION_USED_OPAQUE = 190,
		DXGI_FORMAT_FORCE_UINT = 0xffffffff
	};

	public enum D3D10_RESOURCE_DIMENSION : uint
	{
		D3D10_RESOURCE_DIMENSION_UNKNOWN = 0,
		D3D10_RESOURCE_DIMENSION_BUFFER = 1,
		D3D10_RESOURCE_DIMENSION_TEXTURE1D = 2,
		D3D10_RESOURCE_DIMENSION_TEXTURE2D = 3,
		D3D10_RESOURCE_DIMENSION_TEXTURE3D = 4
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct DDS_PIXELFORMAT
	{
		public uint dwSize;         // 32
		public uint dwFlags;
		public uint dwFourCC;
		public uint dwRGBBitCount;
		public uint dwRBitMask;
		public uint dwGBitMask;
		public uint dwBBitMask;
		public uint dwABitMask;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct DDS_HEADER
	{
		public uint dwSize;                 // 124
		public uint dwFlags;
		public uint dwHeight;
		public uint dwWidth;
		public uint dwPitchOrLinearSize;
		public uint dwDepth;
		public uint dwMipMapCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public uint[] dwReserved1;
		public DDS_PIXELFORMAT ddspf;
		public uint dwCaps;
		public uint dwCaps2;
		public uint dwCaps3;
		public uint dwCaps4;
		public uint dwReserved2;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct DDS_HEADER_DXT10
	{
		public DXGI_FORMAT dxgiFormat;
		public D3D10_RESOURCE_DIMENSION resourceDimension;
		public uint miscFlag;
		public uint arraySize;
		public uint miscFlags2;
	}

	public static class DDS_Util
	{
		public static Texture2D LoadTexture(string absolutePath)
		{
			if (!File.Exists(absolutePath))
			{
				Plugin.LogError($"No DDS file found at path {absolutePath}!");
				return null;
			}

			using var fs = new FileStream(absolutePath, FileMode.Open);

			byte[] chunk = new byte[4];
			long ptr = 0;

			_ = fs.Read(chunk, 0, chunk.Length);
			ptr = fs.Position;

			if (!Enumerable.SequenceEqual(chunk, new byte[] { (byte)'D', (byte)'D', (byte)'S', (byte)' ' }))
			{
				Plugin.LogError("Invalid DDS header!");
				return null;
			}

			_ = fs.Read(chunk, 0, chunk.Length);
			ptr = fs.Position;

			int headerSize = BitConverter.ToInt32(chunk, 0);
			if (headerSize != 124)
			{
				Plugin.LogError("Invalid DDS header size!");
				return null;
			}

			//Rewind to the beginning
			fs.Position = ptr - chunk.Length;

			chunk = new byte[headerSize];
			_ = fs.Read(chunk, 0, chunk.Length);
			ptr = fs.Position;

			DDS_HEADER header;

			GCHandle handle = GCHandle.Alloc(chunk, GCHandleType.Pinned);
			try
			{
				var tempCopy = Marshal.PtrToStructure<DDS_HEADER>(handle.AddrOfPinnedObject());
				header = tempCopy;
			}
			catch (Exception e)
			{
				Plugin.LogError($"Failed to load DDS header: {e}");
				handle.Free();
				return null;
			}
			finally
			{
				handle.Free();
			}

			Texture2D newTexture = null;

			//Has DDS_HEADER_DXT10
			if ((header.ddspf.dwFlags & 0x00000004) != 0 && header.ddspf.dwFourCC == 0x30315844)
			{
				chunk = new byte[Marshal.SizeOf<DDS_HEADER_DXT10>()];
				_ = fs.Read(chunk, 0, chunk.Length);
				handle = GCHandle.Alloc(chunk, GCHandleType.Pinned);
				try
				{
					var DXT10_Header = Marshal.PtrToStructure<DDS_HEADER_DXT10>(handle.AddrOfPinnedObject());

					GraphicsFormat? unityFormat = DXT10_Header.dxgiFormat switch
					{
						DXGI_FORMAT.DXGI_FORMAT_BC7_UNORM => GraphicsFormat.RGBA_BC7_UNorm,
						DXGI_FORMAT.DXGI_FORMAT_BC7_TYPELESS or DXGI_FORMAT.DXGI_FORMAT_BC7_UNORM_SRGB => GraphicsFormat.RGBA_BC7_SRGB,
						DXGI_FORMAT.DXGI_FORMAT_BC3_UNORM => GraphicsFormat.RGBA_DXT5_UNorm,
						DXGI_FORMAT.DXGI_FORMAT_BC3_UNORM_SRGB or DXGI_FORMAT.DXGI_FORMAT_BC3_TYPELESS => GraphicsFormat.RGBA_DXT5_SRGB,
						_ => null
					};

					if (unityFormat == null)
					{
						Plugin.LogError($"Unhandled DDS format!");
						handle.Free();
						return null;
					}

					newTexture = new Texture2D((int)header.dwWidth, (int)header.dwHeight, unityFormat.Value, (int)header.dwMipMapCount, TextureCreationFlags.None);
				}
				catch (Exception e)
				{
					Plugin.LogError($"Failed to load DDS DXT10 header: {e}");
					return null;
				}
				finally
				{
					handle.Free();
				}
			}
			else if ((header.ddspf.dwFlags & 0x00000004) != 0 && header.ddspf.dwFourCC == 0x35545844) // BC3
			{
				newTexture = new Texture2D((int)header.dwWidth, (int)header.dwHeight, TextureFormat.DXT5, (int)header.dwMipMapCount, false, true);
			}

			if (newTexture != null)
			{
				chunk = new byte[fs.Length - fs.Position];
				_ = fs.Read(chunk, 0, chunk.Length);

				newTexture.LoadRawTextureData(chunk);
				newTexture.Apply(false, true);

				return newTexture;
			}
			else
				return null;
		}
	}
}
