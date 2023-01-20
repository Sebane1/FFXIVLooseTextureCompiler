using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.DataTypes {
    internal class RaceCode {
        string[] masculine;
        string[] feminine;

        public string[] Feminine { get => feminine; set => feminine = value; }
        public string[] Masculine { get => masculine; set => masculine = value; }
    }
}
