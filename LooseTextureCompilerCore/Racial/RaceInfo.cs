using FFXIVLooseTextureCompiler.DataTypes;

namespace FFXIVLooseTextureCompiler.Racial {
    public static class RaceInfo {
        private static List<string> subRaces = new List<string>() { "Midlander" , "Highlander","Wildwood","Duskwight","Seeker",
            "Keeper", "Sea Wolf", "Hellsguard", "Plainsfolk", "Dunesfolk", "Raen", "Xaela", "Helions", "The Lost", "Rava", "Veena" };
        private static List<string> races = new List<string>() { "Midlander","Highlander","Elezen","Miqo'te","Roegadyn",
            "Lalafell","Raen","Xaela","Hrothgar","Viera",};

        private static RaceCode raceCodeBody = new RaceCode(new string[] {
            "0101","0301","0101","0101","0901","1101","1301","1301","1501","1701"}, new string[] {
            "0201","0401","0201","0201","0401","1101","1401","1401","1601","1801"});

        private static RaceCode raceCodeFace = new RaceCode(new string[] {
                "0101", "0301", "0501", "0501", "0701",
                "0701", "0901", "0901", "1101", "1101",
                "1301", "1301", "1501", "1501", "1701", "1701" }, new string[] {
                "0201", "0401", "0601", "0601", "0801",
                "0801", "1001", "1001", "1201", "1201",
                "1401", "1401", "1601", "1601", "1801", "1801" });

        private static List<RacialBodyIdentifiers> bodyIdentifiers = new List<RacialBodyIdentifiers>(){
            new RacialBodyIdentifiers("VANILLA",
                new List<string>() { "201", "401", "201", "201", "401", "1101", "1401", "1401", "Invalid", "1801" }),
            new RacialBodyIdentifiers("BIBO+",
                new List<string>() { "midlander", "highlander", "midlander", "midlander", "highlander", "Invalid", "raen", "xaela", "Invalid", "viera" }),
            new RacialBodyIdentifiers("EVE",
                new List<string>() { "middie", "buffie", "middie", "middie", "buffie", "Invalid", "lizard", "lizard2", "Invalid", "bunny" }),
            new RacialBodyIdentifiers("GEN3",
                new List<string>() { "mid", "high", "mid", "mid", "high", "Invalid", "raen", "xaela", "Invalid", "viera" }),
            new RacialBodyIdentifiers("SCALE+",
                new List<string>() { "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "raen", "xaela", "Invalid", "Invalid" }),
            new RacialBodyIdentifiers("TBSE/HRBODY",
                new List<string>() { "0101", "0301", "0101", "0101", "0901", "Invalid", "1301", "1301", "1501", "1701" }),
            new RacialBodyIdentifiers("TAIL",
                new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "", "" }) };

        public static RaceCode RaceCodeBody { get => raceCodeBody; set => raceCodeBody = value; }
        public static RaceCode RaceCodeFace { get => raceCodeFace; set => raceCodeFace = value; }
        internal static List<RacialBodyIdentifiers> BodyIdentifiers { get => bodyIdentifiers; set => bodyIdentifiers = value; }
        public static List<string> SubRaces { get => subRaces; set => subRaces = value; }
        public static List<string> Races { get => races; set => races = value; }

        public static string ReverseBodyLookup(string internalPath) {
            if (internalPath.Contains("bibo")) {
                return "bibo";
            } else if (internalPath.Contains("eve") || internalPath.Contains("gen3")) {
                return "gen3";
            } else if (internalPath.Contains("body")) {
                return "gen2";
            } else if (internalPath.Contains("skin_otopop") || internalPath.Contains("v01_c1101b0001_g")) {
                return "otopop";
            } else if (internalPath.Contains("1_b_d")) {
                return "tbse";
            }
            return "";
        }
        public static int ReverseRaceLookup(string path) {
            if (!string.IsNullOrEmpty(path)) {
                for (int i = 0; i < 10; i++) {
                    string bibo = bodyIdentifiers[1].RaceIdentifiers[i];
                    string eve = bodyIdentifiers[2].RaceIdentifiers[i];
                    string tnf = bodyIdentifiers[3].RaceIdentifiers[i];
                    string tbse = "c" + bodyIdentifiers[5].RaceIdentifiers[i];
                    if (path.Contains(bibo) || path.Contains(eve) || path.Contains(tnf) || path.Contains(tbse)) {
                        return i;
                    }
                }
                for (int i = 0; i < 9; i++) {
                    string vanilla = bodyIdentifiers[0].RaceIdentifiers[i];
                    if (!vanilla.Contains("Invalid")) {
                        if (path.Contains("c" + NumberPadder(int.Parse(vanilla)))) {
                            if (path.Contains("c1401b0001")) {
                                return 6;
                            } else if (path.Contains("c1401b0101")) {
                                return 7;
                            } else {
                                return i;
                            }
                        }
                    }
                }
                if (path.Contains("1101") || path.Contains("otopop")) {
                    return 5;
                }
            }
            return -1;
        }
        public static string NumberPadder(int value) {
            return value.ToString().PadLeft(4, '0');
        }
        public static int SubRaceToMainRace(int subRace) {
            switch (subRace) {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:
                case 3:
                    return 2;

                case 4:
                case 5:
                    return 3;

                case 6:
                case 7:
                    return 4;

                case 8:
                case 9:
                    return 5;

                case 10:
                    return 6;

                case 11:
                    return 7;

                case 12:
                case 13:
                    return 8;

                case 14:
                case 15:
                    return 9;

            }
            return -1;
        }
    }
}
