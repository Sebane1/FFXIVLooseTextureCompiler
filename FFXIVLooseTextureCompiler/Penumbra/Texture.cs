using OtterTex;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace Penumbra.Import.Textures;

public sealed class Texture : IDisposable {
    public enum FileType {
        Unknown,
        Dds,
        Tex,
        Png,
        Bitmap,
    }

    // Path to the file we tried to load.
    public string Path = string.Empty;

    // If the load failed, an exception is stored.
    public Exception? LoadError = null;

    // The pixels of the main image in RGBA order.
    // Empty if LoadError != null or Path is empty.
    public byte[] RGBAPixels = Array.Empty<byte>();

    // The ImGui wrapper to load the image.
    // null if LoadError != null or Path is empty.
    // The base image in whatever format it has.
    public object? BaseImage = null;

    // Original File Type.
    public FileType Type = FileType.Unknown;

    // Whether the file is successfully loaded and drawable.
    // The ImGui wrapper to load the image.
    // null if LoadError != null or Path is empty.
    public TextureWrap? TextureWrap = null;
    // Whether the file is successfully loaded and drawable.
    public bool IsLoaded
        => TextureWrap != null;

    private void Clean() {
        RGBAPixels = Array.Empty<byte>();
        (BaseImage as IDisposable)?.Dispose();
        BaseImage = null;
        Type = FileType.Unknown;
        Loaded?.Invoke(false);
    }

    public void Dispose()
        => Clean();

    public event Action<bool>? Loaded;

    private void Load(string path) {
        _tmpPath = null;
        if (path == Path) {
            return;
        }

        Path = path;
        Clean();
        if (path.Length == 0) {
            return;
        }

        try {
            var _ = System.IO.Path.GetExtension(Path).ToLowerInvariant() switch {
                ".dds" => LoadDds(),
                ".png" => LoadPng(),
                ".tex" => LoadTex(),
                _ => throw new Exception($"Extension {System.IO.Path.GetExtension(Path)} unknown."),
            };
            Loaded?.Invoke(true);
        } catch (Exception e) {
            LoadError = e;
            Clean();
        }
    }

    private bool LoadDds() {
        Type = FileType.Dds;
        var scratch = ScratchImage.LoadDDS(Path);
        BaseImage = scratch;
        var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
        RGBAPixels = rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray();
        return true;
    }

    private bool LoadPng() {
        Type = FileType.Png;
        BaseImage = null;
        using var stream = File.OpenRead(Path);
        using var png = Image.Load<Rgba32>(stream);
        RGBAPixels = new byte[png.Height * png.Width * 4];
        png.CopyPixelDataTo(RGBAPixels);
        return true;
    }

    private bool LoadTex() {
        Type = FileType.Tex;
        using var stream = OpenTexStream();
        var scratch = TexFileParser.Parse(stream);
        BaseImage = scratch;
        var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
        RGBAPixels = rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray();
        return true;
    }

    private Stream OpenTexStream() {
        if (System.IO.Path.IsPathRooted(Path)) {
            return File.OpenRead(Path);
        }
        return null;
    }


    private string? _tmpPath;
}