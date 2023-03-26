using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LooseTextureCompilerFileRelay {
    public class FileIdentifier : IDisposable {
        Guid identifier;
        string modName;
        MemoryStream memoryStream;
        bool usedUp;

        public FileIdentifier(Guid identifier, string modName, MemoryStream memoryStream) {
            this.identifier = identifier;
            this.modName = modName;
            this.memoryStream = memoryStream;
        }

        public MemoryStream MemoryStream { get => memoryStream; set => memoryStream = value; }
        public string ModName { get => modName; set => modName = value; }
        public Guid Identifier { get => identifier; set => identifier = value; }
        public bool UsedUp { get => usedUp; set => usedUp = value; }

        public void Dispose() {
            memoryStream.Dispose();
            modName = null;
            identifier = Guid.Empty;
            usedUp = true;
        }
    }
}
