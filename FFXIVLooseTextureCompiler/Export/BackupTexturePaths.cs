using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.Export {
    public class BackupTexturePaths {
        public BackupTexturePaths(string path) {
            _path = path;
        }
        const string _diffuse = "diffuse.tex";
       // const string _diffuseRaen = "diffuseRaen.tex";
        const string _normal = "normal.tex";
       // const string _normalAuRa = "normalAuRa.tex";
        string _path;

        public string Diffuse { get => _path + _diffuse; }
        // public string DiffuseRaen { get => _path + _diffuseRaen; }
        public string Normal { get => _path + _normal; }
        // public string NormalAuRa { get => _path + _normalAuRa; }
    }
}
