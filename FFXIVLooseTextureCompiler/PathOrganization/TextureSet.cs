using FFXIVLooseTextureCompiler.Export;

namespace FFXIVLooseTextureCompiler.PathOrganization {
    public class TextureSet {
        string materialSetName = "";
        string materialGroupName = "";

        string diffuse = "";
        string normal = "";
        string multi = "";

        string internalDiffusePath = "";
        string internalNormalPath = "";
        string internalMultiPath = "";
        private string normalMask = "";
        private string glow = "" ;

        private bool ignoreNormalGeneration;
        private bool ignoreMultiGeneration;
        private bool invertNormalGeneration;
        private bool omniExportMode;
        private BackupTexturePaths backupTexturePaths;
        public bool IsChildSet {
            get {
                return MaterialSetName.Contains("[IsChild]");
            }
        }

        List<TextureSet> childSets = new List<TextureSet>();
        private string normalCorrection;

        public string MaterialSetName { get => materialSetName; set => materialSetName = value; }
        public string Diffuse { get { if (diffuse == null) { diffuse = ""; } return diffuse; } set => diffuse = value; }
        public string Normal { get { if (normal == null) { normal = ""; } return normal; } set => normal = value; }
        public string Multi { get { if (multi == null) { multi = ""; } return multi; } set => multi = value; }
        public string NormalMask { get { if (normalMask == null) { normalMask = ""; } return normalMask; } set => normalMask = value; }
        public string Glow {
            get {
                if (glow == null) {
                    glow = "";
                }
                return glow;
            }
            set {
                if (!string.IsNullOrEmpty(value)) {
                    IgnoreMultiGeneration = false;
                }
                glow = value;
            }
        }
        public string InternalDiffusePath {
            get {
                return internalDiffusePath == null ? internalDiffusePath = "" : internalDiffusePath;
            }
            set => internalDiffusePath = value; }
        public string InternalNormalPath {
            get {
                return internalNormalPath == null ? internalNormalPath = "" : internalNormalPath;
            }
            set => internalNormalPath = value; }
        public string InternalMultiPath {
            get {
                return internalMultiPath == null ? internalMultiPath = "" : internalMultiPath;
            }
            set => internalMultiPath = value;
        }
        public string MaterialGroupName {
            get {
                if (string.IsNullOrEmpty(materialGroupName)) {
                    materialGroupName = materialSetName;
                }
                return materialGroupName;
            }
            set => materialGroupName = value;
        }

        public bool IgnoreMultiGeneration { get => ignoreMultiGeneration; set => ignoreMultiGeneration = value; }
        public bool IgnoreNormalGeneration { get => ignoreNormalGeneration; set => ignoreNormalGeneration = value; }
        public bool OmniExportMode { get => omniExportMode; set => omniExportMode = value; }
        public List<TextureSet> ChildSets { get => childSets; set => childSets = value; }
        public BackupTexturePaths BackupTexturePaths { get => backupTexturePaths; set => backupTexturePaths = value; }
        public string NormalCorrection { get => normalCorrection; set => normalCorrection = value; }
        public bool InvertNormalGeneration { get => invertNormalGeneration; set => invertNormalGeneration = value; }

        public override string ToString() {
            return materialSetName + (MaterialGroupName != materialSetName ? $" | Group({materialGroupName})" : "");
        }
    }
}
