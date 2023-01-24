using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.PathOrganization {
    public class ProjectFile {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Version { get; set; }

        public bool BakeMissingNormals { get; set; }
        public bool GenerateMulti { get; set; }

        public int ExportType { get; set; }
        public List<MaterialSet> MaterialSets { get; set; }
    }
}
