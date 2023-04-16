namespace FFXIVLooseTextureCompiler {
    partial class CustomPathDialog {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomPathDialog));
            this.materialSetNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.diffuseLabel = new System.Windows.Forms.Label();
            this.internalDiffusePathTextBox = new System.Windows.Forms.TextBox();
            this.normalLabel = new System.Windows.Forms.Label();
            this.internalNormalPathTextBox = new System.Windows.Forms.TextBox();
            this.multiLabel = new System.Windows.Forms.Label();
            this.internalMultiPathTextbox = new System.Windows.Forms.TextBox();
            this.acceptChangesButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupNameTextBox = new System.Windows.Forms.TextBox();
            this.ignoreNormalsCheckbox = new System.Windows.Forms.CheckBox();
            this.ignoreMultiCheckbox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.normalCorrection = new System.Windows.Forms.TextBox();
            this.invertNormals = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // materialSetNameTextBox
            // 
            this.materialSetNameTextBox.Location = new System.Drawing.Point(128, 36);
            this.materialSetNameTextBox.Name = "materialSetNameTextBox";
            this.materialSetNameTextBox.Size = new System.Drawing.Size(324, 23);
            this.materialSetNameTextBox.TabIndex = 0;
            this.materialSetNameTextBox.TextChanged += new System.EventHandler(this.materialSetNameTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Texture Set Name";
            // 
            // diffuseLabel
            // 
            this.diffuseLabel.AutoSize = true;
            this.diffuseLabel.Location = new System.Drawing.Point(4, 68);
            this.diffuseLabel.Name = "diffuseLabel";
            this.diffuseLabel.Size = new System.Drawing.Size(87, 15);
            this.diffuseLabel.TabIndex = 3;
            this.diffuseLabel.Text = "Internal Diffuse";
            // 
            // internalDiffusePathTextBox
            // 
            this.internalDiffusePathTextBox.Location = new System.Drawing.Point(128, 64);
            this.internalDiffusePathTextBox.Name = "internalDiffusePathTextBox";
            this.internalDiffusePathTextBox.Size = new System.Drawing.Size(324, 23);
            this.internalDiffusePathTextBox.TabIndex = 2;
            // 
            // normalLabel
            // 
            this.normalLabel.AutoSize = true;
            this.normalLabel.Location = new System.Drawing.Point(4, 96);
            this.normalLabel.Name = "normalLabel";
            this.normalLabel.Size = new System.Drawing.Size(90, 15);
            this.normalLabel.TabIndex = 5;
            this.normalLabel.Text = "Internal Normal";
            // 
            // internalNormalPathTextBox
            // 
            this.internalNormalPathTextBox.Location = new System.Drawing.Point(128, 92);
            this.internalNormalPathTextBox.Name = "internalNormalPathTextBox";
            this.internalNormalPathTextBox.Size = new System.Drawing.Size(324, 23);
            this.internalNormalPathTextBox.TabIndex = 4;
            // 
            // multiLabel
            // 
            this.multiLabel.AutoSize = true;
            this.multiLabel.Location = new System.Drawing.Point(4, 124);
            this.multiLabel.Name = "multiLabel";
            this.multiLabel.Size = new System.Drawing.Size(78, 15);
            this.multiLabel.TabIndex = 7;
            this.multiLabel.Text = "Internal Multi";
            // 
            // internalMultiPathTextbox
            // 
            this.internalMultiPathTextbox.Location = new System.Drawing.Point(128, 120);
            this.internalMultiPathTextbox.Name = "internalMultiPathTextbox";
            this.internalMultiPathTextbox.Size = new System.Drawing.Size(324, 23);
            this.internalMultiPathTextbox.TabIndex = 6;
            // 
            // acceptChangesButton
            // 
            this.acceptChangesButton.Location = new System.Drawing.Point(280, 200);
            this.acceptChangesButton.Name = "acceptChangesButton";
            this.acceptChangesButton.Size = new System.Drawing.Size(111, 23);
            this.acceptChangesButton.TabIndex = 8;
            this.acceptChangesButton.Text = "Confirm Changes";
            this.acceptChangesButton.UseVisualStyleBackColor = true;
            this.acceptChangesButton.Click += new System.EventHandler(this.acceptChangesButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(392, 200);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "Group";
            // 
            // groupNameTextBox
            // 
            this.groupNameTextBox.Location = new System.Drawing.Point(128, 8);
            this.groupNameTextBox.Name = "groupNameTextBox";
            this.groupNameTextBox.Size = new System.Drawing.Size(324, 23);
            this.groupNameTextBox.TabIndex = 10;
            // 
            // ignoreNormalsCheckbox
            // 
            this.ignoreNormalsCheckbox.AutoSize = true;
            this.ignoreNormalsCheckbox.Location = new System.Drawing.Point(128, 180);
            this.ignoreNormalsCheckbox.Name = "ignoreNormalsCheckbox";
            this.ignoreNormalsCheckbox.Size = new System.Drawing.Size(150, 19);
            this.ignoreNormalsCheckbox.TabIndex = 12;
            this.ignoreNormalsCheckbox.Text = "Dont Generate Normals";
            this.ignoreNormalsCheckbox.UseVisualStyleBackColor = true;
            // 
            // ignoreMultiCheckbox
            // 
            this.ignoreMultiCheckbox.AutoSize = true;
            this.ignoreMultiCheckbox.Location = new System.Drawing.Point(316, 180);
            this.ignoreMultiCheckbox.Name = "ignoreMultiCheckbox";
            this.ignoreMultiCheckbox.Size = new System.Drawing.Size(133, 19);
            this.ignoreMultiCheckbox.TabIndex = 13;
            this.ignoreMultiCheckbox.Text = "Dont Generate Multi";
            this.ignoreMultiCheckbox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "Normal Correction";
            // 
            // normalCorrection
            // 
            this.normalCorrection.Location = new System.Drawing.Point(127, 148);
            this.normalCorrection.Name = "normalCorrection";
            this.normalCorrection.Size = new System.Drawing.Size(324, 23);
            this.normalCorrection.TabIndex = 14;
            // 
            // invertNormals
            // 
            this.invertNormals.AutoSize = true;
            this.invertNormals.Location = new System.Drawing.Point(8, 180);
            this.invertNormals.Name = "invertNormals";
            this.invertNormals.Size = new System.Drawing.Size(104, 19);
            this.invertNormals.TabIndex = 16;
            this.invertNormals.Text = "Invert Normals";
            this.invertNormals.UseVisualStyleBackColor = true;
            // 
            // CustomPathDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(455, 227);
            this.Controls.Add(this.invertNormals);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.normalCorrection);
            this.Controls.Add(this.ignoreMultiCheckbox);
            this.Controls.Add(this.ignoreNormalsCheckbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupNameTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.acceptChangesButton);
            this.Controls.Add(this.multiLabel);
            this.Controls.Add(this.internalMultiPathTextbox);
            this.Controls.Add(this.normalLabel);
            this.Controls.Add(this.internalNormalPathTextBox);
            this.Controls.Add(this.diffuseLabel);
            this.Controls.Add(this.internalDiffusePathTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.materialSetNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CustomPathDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Path";
            this.Load += new System.EventHandler(this.CustomPathDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox materialSetNameTextBox;
        private Label label1;
        private Label diffuseLabel;
        private TextBox internalDiffusePathTextBox;
        private Label normalLabel;
        private TextBox internalNormalPathTextBox;
        private Label multiLabel;
        private TextBox internalMultiPathTextbox;
        private Button acceptChangesButton;
        private Button button1;
        private Label label5;
        private TextBox groupNameTextBox;
        private CheckBox ignoreNormalsCheckbox;
        private CheckBox ignoreMultiCheckbox;
        private Label label2;
        private TextBox normalCorrection;
        private CheckBox invertNormals;
    }
}