﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.PathOrganization {
    public class TextureSet {
        string materialSetName;
        string materialGroupName;

        string diffuse;
        string normal;
        string multi;

        string internalDiffusePath;
        string internalNormalPath;
        string internalMultiPath;
        private string normalMask;
        private string glow;

        public string MaterialSetName { get => materialSetName; set => materialSetName = value; }
        public string Diffuse { get => diffuse; set => diffuse = value; }
        public string Normal { get => normal; set => normal = value; }
        public string Multi { get => multi; set => multi = value; }
        public string NormalMask { get => normalMask; set => normalMask = value; }
        public string Glow { get => glow; set => glow = value; }
        public string InternalDiffusePath { get => internalDiffusePath; set => internalDiffusePath = value; }
        public string InternalNormalPath { get => internalNormalPath; set => internalNormalPath = value; }
        public string InternalMultiPath { get => internalMultiPath; set => internalMultiPath = value; }
        public string MaterialGroupName {
            get {
                if (string.IsNullOrEmpty(materialGroupName)) {
                    materialGroupName = materialSetName;
                }
                return materialGroupName;
            }
            set => materialGroupName = value;
        }

        public override string ToString() {
            return materialSetName + (MaterialGroupName != materialSetName ? $"| Group({materialGroupName})" : "");
        }
    }
}