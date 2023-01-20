using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVVoicePackCreator.Json {
    public class Group {
        string name;
        string description;
        int priority;
        string type = "Multi";
        int defaultSettings;
        List<Option> options = new List<Option>();

        public Group(string name, string description, int priority, string type, int defaultSettings) {
            this.name = name;
            this.description = description;
            this.priority = priority;
            this.type = type;
            this.defaultSettings = defaultSettings;
        }

        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int Priority { get => priority; set => priority = value; }
        public string Type { get => type; set => type = value; }
        public int DefaultSettings { get => defaultSettings; set => defaultSettings = value; }
        public List<Option> Options { get => options; set => options = value; }
    }
}
