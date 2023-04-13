namespace FFXIVLooseTextureCompiler.DataTypes {
    internal class RacialBodyIdentifiers {
        string name;
        List<string> raceIdentifiers;

        public RacialBodyIdentifiers(string name, List<string> raceIdentifiers) {
            Name = name;
            RaceIdentifiers = raceIdentifiers;
        }

        public string Name { get => name; set => name = value; }
        public List<string> RaceIdentifiers {
            get => raceIdentifiers; set => raceIdentifiers = value;
        }
    }
}
