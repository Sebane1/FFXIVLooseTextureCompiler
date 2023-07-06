namespace FFXIVLooseTextureCompiler {
    public partial class TemplateConfiguration : Form {
        private string groupName;

        public TemplateConfiguration() {
            InitializeComponent();
            AutoScaleDimensions = new SizeF(96, 96);
        }

        public string GroupName { get => groupName; set => groupName = value; }

        private void confirmButton_Click(object sender, EventArgs e) {
            groupName = groupNameTextBox.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
