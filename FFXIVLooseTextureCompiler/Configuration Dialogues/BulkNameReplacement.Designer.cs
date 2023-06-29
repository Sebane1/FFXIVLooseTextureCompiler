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
            this.replaceAll = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.replacementTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.replaceBox = new System.Windows.Forms.TextBox();
            this.findBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // replaceAll
            // 
            this.replaceAll.Location = new System.Drawing.Point(171, 150);
            this.replaceAll.Name = "replaceAll";
            this.replaceAll.Size = new System.Drawing.Size(75, 23);
            this.replaceAll.TabIndex = 15;
            this.replaceAll.Text = "Replace All";
            this.replaceAll.UseVisualStyleBackColor = true;
            this.replaceAll.Click += new System.EventHandler(this.replaceAll_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(252, 150);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 24);
            this.cancel.TabIndex = 14;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(4, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = "Replace with this.";
            // 
            // replacementTypeComboBox
            // 
            this.replacementTypeComboBox.FormattingEnabled = true;
            this.replacementTypeComboBox.Items.AddRange(new object[] {
            "Search And Replace Inside Name",
            "Search And Replace Inside Group",
            "Find Name Then Change Whole Group",
            "Find In Group Then Change Whole Name",
            "Find In Name Then Replace Whole Name ",
            "Find In Group Then Change Whole Group"});
            this.replacementTypeComboBox.Location = new System.Drawing.Point(4, 124);
            this.replacementTypeComboBox.Name = "replacementTypeComboBox";
            this.replacementTypeComboBox.Size = new System.Drawing.Size(328, 22);
            this.replacementTypeComboBox.TabIndex = 12;
            this.replacementTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.replacementTypeComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(4, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 23);
            this.label2.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(-1, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Find this";
            // 
            // replaceBox
            // 
            this.replaceBox.Location = new System.Drawing.Point(4, 92);
            this.replaceBox.Name = "replaceBox";
            this.replaceBox.Size = new System.Drawing.Size(328, 22);
            this.replaceBox.TabIndex = 9;
            // 
            // findBox
            // 
            this.findBox.Location = new System.Drawing.Point(4, 36);
            this.findBox.Name = "findBox";
            this.findBox.Size = new System.Drawing.Size(328, 22);
            this.findBox.TabIndex = 8;
            // 
            // BulkNameReplacement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 175);
            this.Controls.Add(this.replaceAll);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.replacementTypeComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.replaceBox);
            this.Controls.Add(this.findBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BulkNameReplacement";
            this.Text = "BulkNameReplacement";
            this.Load += new System.EventHandler(this.BulkNameReplacement_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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