using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
