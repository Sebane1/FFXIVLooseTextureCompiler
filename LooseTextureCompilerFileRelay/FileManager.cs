using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LooseTextureCompilerFileRelay {
    public class FileManager {
        Dictionary<Guid, FileIdentifier> files = new Dictionary<Guid, FileIdentifier>();
        public Dictionary<Guid, FileIdentifier> Files { get => files; set => files = value; }
    }
}
