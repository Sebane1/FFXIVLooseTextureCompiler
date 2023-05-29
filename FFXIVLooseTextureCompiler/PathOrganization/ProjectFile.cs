namespace FFXIVLooseTextureCompiler.PathOrganization {
    public class ProjectFile {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Version { get; set; }

        public Dictionary<string, int> GroupOptionTypes { get; set; }
        public bool BakeMissingNormals { get; set; }
        public bool GenerateMulti { get; set; }

        public bool SimpleMode { get; set; }

        public int SimpleBodyType { get; set; }
        public int SimpleSubRaceType { get; set; }
        public int SimpleFaceType { get; set; }
        public int SimpleNormalGeneration { get; set; }

        public int ExportType { get; set; }

        /// <summary>
        /// This property is only here for backwards compatibility, please use TextureSets instead.
        /// </summary>
        [Obsolete("This property is only here for backwards compatibility, please use TextureSets instead.")]
        public List<TextureSet> MaterialSets {
            set { TextureSets = value; }
        }
        public List<TextureSet> TextureSets { get; set; }
    }
}
