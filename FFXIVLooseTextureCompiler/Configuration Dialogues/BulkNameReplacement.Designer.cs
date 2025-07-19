namespace FFXIVLooseTextureCompiler {
    partial class BulkNameReplacement {
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
            replaceAll = new Button();
            cancel = new Button();
            label3 = new Label();
            replacementTypeComboBox = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            replaceBox = new TextBox();
            findBox = new TextBox();
            SuspendLayout();
            // 
            // replaceAll
            // 
            replaceAll.Location = new Point(76, 161);
            replaceAll.Name = "replaceAll";
            replaceAll.Size = new Size(127, 25);
            replaceAll.TabIndex = 15;
            replaceAll.Text = "Replace All";
            replaceAll.UseVisualStyleBackColor = true;
            replaceAll.Click += replaceAll_Click;
            // 
            // cancel
            // 
            cancel.Location = new Point(209, 161);
            cancel.Name = "cancel";
            cancel.Size = new Size(118, 26);
            cancel.TabIndex = 14;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            cancel.Click += cancel_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 20.25F, FontStyle.Bold);
            label3.Location = new Point(4, 64);
            label3.Name = "label3";
            label3.Size = new Size(256, 33);
            label3.TabIndex = 13;
            label3.Text = "Replace with this.";
            // 
            // replacementTypeComboBox
            // 
            replacementTypeComboBox.FormattingEnabled = true;
            replacementTypeComboBox.Items.AddRange(new object[] { "Search And Replace Inside Name", "Search And Replace Inside Group", "Find Name Then Change Whole Group", "Find In Group Then Change Whole Name", "Find In Name Then Replace Whole Name ", "Find In Group Then Change Whole Group" });
            replacementTypeComboBox.Location = new Point(4, 133);
            replacementTypeComboBox.Name = "replacementTypeComboBox";
            replacementTypeComboBox.Size = new Size(328, 23);
            replacementTypeComboBox.TabIndex = 12;
            replacementTypeComboBox.SelectedIndexChanged += replacementTypeComboBox_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 14.25F);
            label2.Location = new Point(4, 77);
            label2.Name = "label2";
            label2.Size = new Size(0, 23);
            label2.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 20.25F, FontStyle.Bold);
            label1.Location = new Point(-1, -2);
            label1.Name = "label1";
            label1.Size = new Size(131, 33);
            label1.TabIndex = 10;
            label1.Text = "Find this";
            // 
            // replaceBox
            // 
            replaceBox.Location = new Point(4, 99);
            replaceBox.Name = "replaceBox";
            replaceBox.Size = new Size(328, 23);
            replaceBox.TabIndex = 9;
            // 
            // findBox
            // 
            findBox.Location = new Point(4, 39);
            findBox.Name = "findBox";
            findBox.Size = new Size(328, 23);
            findBox.TabIndex = 8;
            // 
            // BulkNameReplacement
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(331, 188);
            Controls.Add(replaceAll);
            Controls.Add(cancel);
            Controls.Add(label3);
            Controls.Add(replacementTypeComboBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(replaceBox);
            Controls.Add(findBox);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "BulkNameReplacement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bulk Text Replacement";
            Load += BulkNameReplacement_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button replaceAll;
        private Button cancel;
        private Label label3;
        private ComboBox replacementTypeComboBox;
        private Label label2;
        private Label label1;
        private TextBox replaceBox;
        private TextBox findBox;
    }
}