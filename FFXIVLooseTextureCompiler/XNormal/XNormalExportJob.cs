namespace FFXIVLooseTextureCompiler {
    public class XNormalExportJob {
        private string internalPath;
        private string inputTexturePath;
        private string outputTexturePath;
        private string outputXMLPath;
        private string inputModel;
        private string outputModel;
        private bool isNormalMap;

        public XNormalExportJob(string internalPath, string inputTexturePath, string outputTexturePath,
            string inputModel, string outputModel, string outputXMLPath, bool isNormalMap) {
            this.internalPath = internalPath;
            this.inputTexturePath = inputTexturePath;
            this.outputTexturePath = outputTexturePath;
            this.outputXMLPath = outputXMLPath;
            this.inputModel = inputModel;
            this.outputModel = outputModel;
            this.isNormalMap = isNormalMap;
        }

        public string OutputTexturePath { get => outputTexturePath; set => outputTexturePath = value; }
        public string InputTexturePath { get => inputTexturePath; set => inputTexturePath = value; }
        public string InternalPath { get => internalPath; set => internalPath = value; }
        public string OutputXMLPath { get => outputXMLPath; set => outputXMLPath = value; }
        public string InputModel { get => inputModel; set => inputModel = value; }
        public string OutputModel { get => outputModel; set => outputModel = value; }
        public bool IsNormalMap { get => isNormalMap; set => isNormalMap = value; }
    }
}
