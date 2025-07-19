
using LanguageConversionProxy;
using Lumina.Data;
using RoleplayingQuestCore;

namespace FFXIVLooseTextureCompiler {
    public static class WFTranslator {
        static Form parentForm;

        public static Form ParentForm { get => parentForm; set => parentForm = value; }
        public static async void TranslateControl(Control parentControl) {
            var textBox = parentControl as TextBox;
            if (!int.TryParse(parentControl.Text, out var value)) {
                string text = await Translator.LocalizeText(parentControl.Text, Translator.UiLanguage,
                    LanguageConversionProxy.LanguageEnum.English);
                if (textBox == null || !textBox.Enabled) {
                    parentControl.Invoke(delegate {
                        parentControl.Text = text;
                    });
                }
                var listBox = parentControl as ListBox;
                if (listBox != null) {
                    for (int i = 0; i < listBox.Items.Count; i++) {
                        string textItem = listBox.Items[i] as string;
                        if (!int.TryParse(textItem, out var result))
                            text = await Translator.LocalizeText(textItem, Translator.UiLanguage,
                                LanguageConversionProxy.LanguageEnum.English);
                        parentControl?.Invoke(delegate {
                            if (text != null) {
                                listBox.Items[i] = text;
                            }
                        });
                    }
                }
                var comboBox = parentControl as ComboBox;
                if (comboBox != null) {
                    for (int i = 0; i < comboBox.Items.Count; i++) {
                        string textItem = comboBox.Items[i] as string;
                        if (!int.TryParse(textItem, out var result))
                            text = await Translator.LocalizeText(textItem, Translator.UiLanguage,
                                LanguageConversionProxy.LanguageEnum.English);
                        parentControl?.Invoke(delegate {
                            if (text != null) {
                                comboBox.Items[i] = text;
                            }
                        });
                    }
                }
            }
            foreach (Control control in parentControl.Controls) {
                TranslateControl(control);
            }
        }
        public static async Task<DialogResult> Show(string text) {
            string translatedText = await Translator.LocalizeText(text, Translator.UiLanguage, LanguageEnum.English);
            return MessageBox.Show(translatedText);
        }
        public static async Task<DialogResult> Show(string text, string title) {
            string translatedText = await Translator.LocalizeText(text, Translator.UiLanguage, LanguageEnum.English);
            string translatedTitle = await Translator.LocalizeText(title, Translator.UiLanguage, LanguageEnum.English);
            return MessageBox.Show(translatedText, translatedTitle);
        }

        public static async Task<DialogResult> Show(string text, string title, MessageBoxButtons messageBoxButtons) {
            string translatedText = await Translator.LocalizeText(text, Translator.UiLanguage, LanguageEnum.English);
            string translatedTitle = await Translator.LocalizeText(title, Translator.UiLanguage, LanguageEnum.English);
            return MessageBox.Show(translatedText, translatedTitle, messageBoxButtons);
        }

        public static async void TranslateMenuStrip(MenuStrip parentControl) {
            foreach (ToolStripMenuItem control in parentControl.Items) {
                string text = await Translator.LocalizeText(control.Text, Translator.UiLanguage,
                    LanguageConversionProxy.LanguageEnum.English);
                parentControl.Invoke(delegate {
                    control.Text = text;
                });
                TranslateToolStripItem(control);
            }
        }
        public static async void TranslateToolStripItem(ToolStripMenuItem parentControl) {
            foreach (ToolStripMenuItem control in parentControl.DropDownItems) {
                string text = await Translator.LocalizeText(control.Text, Translator.UiLanguage,
                    LanguageConversionProxy.LanguageEnum.English);
                parentForm.Invoke(delegate {
                    control.Text = text;
                });
                TranslateToolStripItem(control);
            }
        }
        public static async void TranslateToolStripDropDown(ToolStripDropDown parentControl) {
            foreach (ToolStripMenuItem control in parentControl.Items) {
                string text = await Translator.LocalizeText(control.Text, Translator.UiLanguage,
                    LanguageConversionProxy.LanguageEnum.English);
                parentForm.Invoke(delegate {
                    control.Text = text;
                });
                TranslateToolStripItem(control);
            }
        }

        public static async Task<string> String(string text) {
            return await Translator.LocalizeText(text, Translator.UiLanguage, LanguageEnum.English);
        }
    }
}
