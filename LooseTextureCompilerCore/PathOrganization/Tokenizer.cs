using System.Text;
namespace TypingConnector {
    public class Tokenizer {
        private string _input = null;
        private StringReader tokenReader = null;
        public Tokenizer(string inputString) {
            this._input = inputString;
            this.tokenReader = new StringReader(this._input + " ");
        }
        public string GetToken() {
            bool flag = false;
            StringBuilder stringBuilder = new StringBuilder();
            string result = null;
            for (int num = this.tokenReader.Read(); num != -1; num = this.tokenReader.Read()) {
                char c = (char)num;
                if (!flag) {
                    if (char.IsWhiteSpace(c)) {
                        result = stringBuilder.ToString();
                        stringBuilder.Remove(0, stringBuilder.Length);
                        break;
                    }
                    stringBuilder.Append(c);
                }
            }
            return result;
        }
        public string GetRestOfString() {
            return this.tokenReader.ReadToEnd();
        }
    }
}
