#region Assembly OtterTex, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\stel9\source\repos\Penumbra\Penumbra\lib\OtterTex.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;

namespace OtterTex {
    [Flags]
    public enum ColorPaletteFlags : ulong {
        None = 0x0uL,
        LegacyDWORD = 0x1uL,
        Paragraph = 0x2uL,
        YMM = 0x4uL,
        ZMM = 0x8uL,
        Page4k = 0x200uL,
        BadDXTnTails = 0x1000uL,
        _24BPP = 0x10000uL,
        _16BPP = 0x20000uL,
        _8BPP = 0x40000uL
    }
}
#if false // Decompilation log
'172' items in cache
------------------
Resolve: 'System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '7.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\7.0.2\ref\net7.0\System.Runtime.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime.InteropServices, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '7.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\7.0.2\ref\net7.0\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '7.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\7.0.2\ref\net7.0\System.Threading.dll'
------------------
Resolve: 'System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\7.0.2\ref\net7.0\System.Runtime.dll'
#endif
