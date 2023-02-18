// © Anamnesis.
// Licensed under the MIT license.

namespace Anamnesis.Penumbra;

using FFBardMusicPlayer.FFXIV;
using FFXIVLooseTextureCompiler.Penumbra;
using System.Threading.Tasks;

public static partial class PenumbraHttpApi {
    public static async Task Redraw(int targetIndex, FFXIVHook hook) {
        RedrawData data = new();
        data.ObjectTableIndex = targetIndex;
        data.Type = RedrawData.RedrawType.Redraw;

        await PenumbraApi.Post("/redraw", data, hook);

        await Task.Delay(500);
    }
    public static async Task Reload(string modPath, string modName, FFXIVHook hook) {
        ReloadData data = new();
        data.ModPath = modPath;
        data.ModName = modName;

        await PenumbraApi.Post("/reload", data, hook);

        await Task.Delay(500);
    }

    public class RedrawData {
        public enum RedrawType {
            Redraw,
            AfterGPose,
        }

        public string Name { get; set; } = string.Empty;
        public int ObjectTableIndex { get; set; } = -1;
        public RedrawType Type { get; set; } = RedrawType.Redraw;
    }
}
