
# VTS-Memory-Compression
A BepInEx hack for VTube Studio that utilizes HarmonyX and BepInEx (same pair as you'd normally use when modding games like R.E.P.O. or PEAK) that allows VTube Studio to load DDS textures if they are present.

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
* Hardware capable of handling Direct3D 11 (which you need for VTS anyway on PC).
* DDS textures files (see section below).

Usage:
========
* Download a [new release](https://github.com/SuiMachine/VTS-Memory-Compression/releases) and extract it.
* Drop files in VTube Studio folder.
* If your model has DDS texture files present in the same folder as color textures, the plugin will try and load them instead!  

Creating DDS textures:
========
* Download [Nvidia's Texture Tools](https://developer.nvidia.com/texture-tools-exporter) and install them.
* Open **NVIDIA Texture Tools Exporter** and load your model's texture.
* As a format use **BC7 RGBA 8 bpp | explicit alpha**.
* Texture type: **2D Texture**
* Image type: **Color Map**.
* At the bottom in make sure to check **Flip Vertically**.
* Save the texture as dds with the same naming convention as original texture (i.e. if the original texture is `texture_00.png`, save it as `texture_00.dds`) and move it to a folder where the original texture is stored.

Issues:
========
* As mentioned in **Creating DDS texture** - textures need to be flipped vertically. I could technically prepare a compute shader that performs this on load, but as the objective in here is VRAM and load performance optimisation, this really should be done in texture itself!
* [DirectXTex](https://github.com/microsoft/DirectXTex) and its **texconv.exe** can be used in theory, but they often resulted in slightly different results, which sometimes can cause lines to appear on loaded models.

Credits:
========
* [Sui_VT](https://www.twitch.tv/sui_vt)

Thanks to:
========
* [Nathan Reed](https://www.reedbeta.com/blog/understanding-bcn-texture-compression-formats) - for his great blog entry about BCn compression.
* [Panzerhandschuh](https://github.com/Panzerhandschuh/UnityDds) for also figuring out how to do DDS imports to Unity - their code helped me figure out a bug with DXT10 header.
