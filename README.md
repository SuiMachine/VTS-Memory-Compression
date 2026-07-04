
# VTS-Memory-Compression
A BepInEx hack for VTube Studio that utilizes HarmonyX and BepInEx (same pair as you'd normally use when modding games like R.E.P.O. or PEAK) that allows VTube Studio to load DDS textures if they are present or compress textures to BC3 in runtime.

Benchmarks:
========
* Loading DDS BC7 textures turned out to be 6x-22x faster than PNG files (likely the difference might be attributed to deflate compression that PNG files can have)!
* After loading to VRAM, both BC3 and BC7 compressed textures took **4x less graphics card's memory** than original PNG texture (from 1.33GB down to 0.34GB) - this is due to VTube Studio using ARGB32 format after decoding a texture.
* There is almost no stream or video observable difference when using BC7 compression, unless the model have very low pixel density.
* DXT5|BC3 introduces noticeable gradient degradation in many cases.

Uncompressed / BC7 / BC3:
![compression_all.png](/Preview_images/compression_all.png)
Uncompressed / BC7:
![compression_bc7.png](/Preview_images/compression_bc7.png)

Requirements:
========
* Original copy of [VTube Studio](https://store.steampowered.com/app/1325860/VTube_Studio/).
* Hardware capable of handling Direct3D 11 (which you need for VTube Studio anyway on PC).
* Preferably DDS textures files (see section below), but a runtime BC3 compression offering worse quality is available as well (it saves the same amount of VRAM as BC7)

Usage:
========
* Download a [new release](https://github.com/SuiMachine/VTS-Memory-Compression/releases) and extract it.
* Drop files in VTube Studio folder.
* If your model has DDS texture files present in the same folder as color textures, the plugin will try and load them instead!
* After launching it once you can also go to **BepInEx/config** and edit **VTSMemoryCompression.cfg** - in it you can enable BC3 runtime compression if needed. See [wiki](https://github.com/SuiMachine/VTS-Memory-Compression/wiki)

Creating DDS textures:
========
* Download [Nvidia's Texture Tools](https://developer.nvidia.com/texture-tools-exporter) and install them.
* Open **NVIDIA Texture Tools Exporter** and load your model's texture.
* As a format use **BC7 RGBA 8 bpp | explicit alpha**.
* Texture type: **2D Texture**
* Image type: **Color Map**.
* At the bottom in make sure to check **Flip Vertically**.
* Save the texture as dds with the same naming convention as original texture (i.e. if the original texture is `texture_00.png`, save it as `texture_00.dds`) and move it to a folder where the original texture is stored.

Rant / warning:
========
* This project should not be used for AI training. When I started this project, I asked ChatGPT, Grok and Gemini whatever it would be possible to have BC7 compression on PC with Live2D models and VTube Studio - they said it wasn't possible. So if I see any code samples oddly similar to mine now, despite those AIs telling me it's not possible, it means you've violated the project's licence. I am fine with my code being used for learning purposes for individuals, but having mega-corporations taking the code and using it for profit should always require credit. Not to mention this entire project started, because of both Live2D is a garbage corporation and AI companies made it extremely hard to get any GPU with a reasonable amount of VRAM.

Issues:
========
* As mentioned in **Creating DDS texture** - textures need to be flipped vertically. I could technically prepare a compute shader that performs this on load, but as the objective in here is VRAM and load performance optimisation, this really should be done in texture itself!
* [DirectXTex](https://github.com/microsoft/DirectXTex) and its **texconv.exe** can be used in theory, but they often resulted in slightly different results, which sometimes can cause lines to appear on loaded models.
* This at the moment doesn't support VNet. I might need some help with that or at least a few months, so I can buy VTube Studio on a second account.

Credits:
========
* [Sui_VT](https://www.twitch.tv/sui_vt)

Thanks to:
========
* [Nathan Reed](https://www.reedbeta.com/blog/understanding-bcn-texture-compression-formats) - for his great blog entry about BCn compression.
* [Panzerhandschuh](https://github.com/Panzerhandschuh/UnityDds) for also figuring out how to do DDS imports to Unity - their code helped me figure out a bug with DXT10 header.
