using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.PathOrganization
{
    public class MaterialSet
    {
        string materialSetName;

        string diffuse;
        string normal;
        string multi;

        string internalDiffusePath;
        string internalNormalPath;
        string internalMultiPath;

        public string MaterialSetName { get => materialSetName; set => materialSetName = value; }
        public string Diffuse { get => diffuse; set => diffuse = value; }
        public string Normal { get => normal; set => normal = value; }
        public string Multi { get => multi; set => multi = value; }
        public string InternalDiffusePath { get => internalDiffusePath; set => internalDiffusePath = value; }
        public string InternalNormalPath { get => internalNormalPath; set => internalNormalPath = value; }
        public string InternalMultiPath { get => internalMultiPath; set => internalMultiPath = value; }

        public override string ToString() {
            return materialSetName;
        }
    }
}
