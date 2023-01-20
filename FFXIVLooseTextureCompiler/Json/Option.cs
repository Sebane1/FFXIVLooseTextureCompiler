using System.Collections.Generic;

namespace FFXIVVoicePackCreator.Json {
    public class Option {
        string name;
        int priority;
        Dictionary<string, string> files = new Dictionary<string, string>();
        Dictionary<string, string> fileSwaps = new Dictionary<string, string>();
        List<Manipulations> manipulations = new List<Manipulations>();

        public Option(string name, int priority) {
            this.name = name;
            this.priority = priority;
        }

        public string Name { get => name; set => name = value; }
        public int Priority { get => priority; set => priority = value; }
        public Dictionary<string, string> Files { get => files; set => files = value; }
        public Dictionary<string, string> FileSwaps { get => fileSwaps; set => fileSwaps = value; }
        public List<Manipulations> Manipulations { get => manipulations; set => manipulations = value; }
    }
}