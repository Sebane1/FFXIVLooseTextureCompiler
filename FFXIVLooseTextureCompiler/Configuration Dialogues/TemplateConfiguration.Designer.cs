namespace FFXIVLooseTextureCompiler {
    partial class TemplateConfiguration {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateConfiguration));
            groupNameTextBox = new TextBox();
            label = new Label();
            confirmButton = new Button();
            SuspendLayout();
            // 
            // groupNameTextBox
            // 
            groupNameTextBox.Location = new Point(0, 44);
            groupNameTextBox.Name = "groupNameTextBox";
            groupNameTextBox.Size = new Size(360, 23);
            groupNameTextBox.TabIndex = 0;
            groupNameTextBox.Text = "Default";
            // 
            // label
            // 
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            label.Location = new Point(4, 4);
            label.Name = "label";
            label.Size = new Size(356, 37);
            label.TabIndex = 1;
            label.Text = "Group Name For Template";
            // 
            // confirmButton
            // 
            confirmButton.Location = new Point(284, 72);
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new Size(75, 23);
            confirmButton.TabIndex = 2;
            confirmButton.Text = "Confirm";
            confirmButton.UseVisualStyleBackColor = true;
            confirmButton.Click += confirmButton_Click;
            // 
            // TemplateConfiguration
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(359, 97);
            Controls.Add(confirmButton);
            Controls.Add(label);
            Controls.Add(groupNameTextBox);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TemplateConfiguration";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Template Configuration";
            Load += TemplateConfiguration_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private TextBox groupNameTextBox;
        private Label label;
        private Button confirmButton;
    }
}