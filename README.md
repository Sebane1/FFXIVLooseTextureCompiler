# FFXIVLooseTextureCompiler
Official download and repository. Do not download from other sources unless you can verify the code on github.

Make loose texture mods less of a pain to set up. Automated cross conversion between body types

Download: 

https://github.com/Sebane1/FFXIVLooseTextureCompiler/releases

![FFXIVLTC](https://user-images.githubusercontent.com/7157688/218231662-a176dc91-3196-4d0c-b3d1-689bc2299d3d.png)

Video demonstration:

https://www.youtube.com/watch?v=YmxmL5ccL2Y

Video intro tutorial:

https://www.youtube.com/watch?v=8yjPDb32XMA

How to find original loose body textures:

https://docs.google.com/document/d/1QR0b1D6Dr_-UCoLIVKi82p2dMF69BmzXutxcpwfmJTo/edit?usp=sharing

Getting loose files into the game with the tool:

https://docs.google.com/document/d/1AR53LNy0dQ6X7L6NSfQY4PkZoUFgqfEYPzHPCcnW_YY/edit?usp=sharing

How to make your Bibo+, Gen3, or Gen2 body texture cross compatible:

https://docs.google.com/document/d/1jXWL5cE9bQL5KPbIzAXKdM7_UgSz8zz2hO9fj5xysHg/edit?usp=share_link

How to create autogenerated normal maps:

https://docs.google.com/document/d/1UMmHVM2Iqvw7jPQ1Ff3MIy_-Cqwam1dcywBcOdyrp8E/edit

Make sure you have .net desktop runtime 7.0 (not to be confused with .net runtime 7.0)
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-7.0.1-windows-x64-installer

You may need to install XNormal if you run into any errors that say stuff is missing (using universal texture set feature).
https://xnormal.net/

Select your body, race, texture, etc and hit generate. The tool should spit out files with all the nessecary path names.

Just hit rediscover in penumbra and use.

.tex generation is forked from Penumbra.

Requires Penumbra to use.

Core features:
- Near instant visual feedback thanks to using penumbra
- Drag and drop texture assignment
- Automatic path generation
- Automatic penumbra options, and ability to customize groups for exports to penumbra
- png, dds, bmp, and .tex support (yes you can compile existing makeup or body mods into this tool with just the .tex files) 
- Ability to add custom texture paths if this tool doesn't already autogenerate them
- Automatic redraw on generation
- Can automatically generate normals, if only a diffuse is provided, or alternatively merge the generated normal data with existing normals.
- Can be used to convert one body texture to other body textures with automated use of XNormal
- Low learning curve.


Support Discord: 

https://discord.gg/rtGXwMn7pX


If you wish to donate you can do so here:

https://ko-fi.com/sebastina
