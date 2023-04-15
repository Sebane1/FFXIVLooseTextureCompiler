namespace FFXIVLooseTextureCompiler.Export {
    public class BackupTexturePaths {
        public BackupTexturePaths(string path) {
            _path = path;
            string cleanup = Diffuse.Replace(".ltct", ".tex");
            if (File.Exists(cleanup)) {
                File.Delete(cleanup);
            }
            cleanup = DiffuseRaen.Replace(".ltct", ".tex");
            if (File.Exists(cleanup)) {
                File.Delete(cleanup);
            }
            cleanup = Normal.Replace(".ltct", ".tex");
            if (File.Exists(cleanup)) {
                File.Delete(cleanup);
            }
        }
        const string _diffuse = "diffuse.ltct";
        const string _diffuseRaen = "diffuseRaen.ltct";
        const string _normal = "normal.ltct";
        string _path;

        public string Diffuse { get => _path + _diffuse; }
        public string DiffuseRaen { get => _path + _diffuseRaen; }
        public string Normal { get => _path + _normal; }
        public string Path {
            get => _path; set {
                _path = value;
            }
        }

        private static BackupTexturePaths biboPath = new BackupTexturePaths(@"res\textures\bibo\bibo\");
        private static BackupTexturePaths biboGen3Path = new BackupTexturePaths(@"res\textures\bibo\gen3\");
        private static BackupTexturePaths biboGen2Path = new BackupTexturePaths(@"res\textures\bibo\gen2\");

        private static BackupTexturePaths gen3BiboPath = new BackupTexturePaths(@"res\textures\gen3\bibo\");
        private static BackupTexturePaths gen3Path = new BackupTexturePaths(@"res\textures\gen3\gen3\");
        private static BackupTexturePaths gen3Gen2Path = new BackupTexturePaths(@"res\textures\gen3\gen2\");

        private static BackupTexturePaths tbsePath = new BackupTexturePaths(@"res\textures\tbse\tbse\");
        private static BackupTexturePaths tbsePathHighlander = new BackupTexturePaths(@"res\textures\tbse\highlander\");
        private static BackupTexturePaths tbsePathViera = new BackupTexturePaths(@"res\textures\tbse\viera\");

        private static BackupTexturePaths otopopLalaPath = new BackupTexturePaths(@"res\textures\otopop\otopop\");
        private static BackupTexturePaths vanillaLalaPath = new BackupTexturePaths(@"res\textures\otopop\vanilla\");

        public static BackupTexturePaths BiboPath { get => biboPath; set => biboPath = value; }
        public static BackupTexturePaths BiboGen3Path { get => biboGen3Path; set => biboGen3Path = value; }
        public static BackupTexturePaths BiboGen2Path { get => biboGen2Path; set => biboGen2Path = value; }
        public static BackupTexturePaths Gen3BiboPath { get => gen3BiboPath; set => gen3BiboPath = value; }
        public static BackupTexturePaths Gen3Path { get => gen3Path; set => gen3Path = value; }
        public static BackupTexturePaths Gen3Gen2Path { get => gen3Gen2Path; set => gen3Gen2Path = value; }
        public static BackupTexturePaths OtopopLalaPath { get => otopopLalaPath; set => otopopLalaPath = value; }
        public static BackupTexturePaths TbsePath { get => tbsePath; set => tbsePath = value; }
        public static BackupTexturePaths VanillaLalaPath { get => vanillaLalaPath; set => vanillaLalaPath = value; }
        public static BackupTexturePaths TbsePathHighlander { get => tbsePathHighlander; set => tbsePathHighlander = value; }
        public static BackupTexturePaths TbsePathViera { get => tbsePathViera; set => tbsePathViera = value; }

    }
}
