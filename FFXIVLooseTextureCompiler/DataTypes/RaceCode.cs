namespace FFXIVLooseTextureCompiler.DataTypes {
    public class RaceCode {
        string[] masculine;
        string[] feminine;

        public RaceCode(string[] masculine, string[] feminine) {
            this.masculine = masculine;
            this.feminine = feminine;
        }

        public string[] Feminine { get => feminine; set => feminine = value; }
        public string[] Masculine { get => masculine; set => masculine = value; }
    }
}
