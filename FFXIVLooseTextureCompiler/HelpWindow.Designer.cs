namespace FFXIVLooseTextureCompiler {
    partial class HelpWindow {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpWindow));
            this.getBibo = new System.Windows.Forms.Button();
            this.getEve = new System.Windows.Forms.Button();
            this.getScalesPlus = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // getBibo
            // 
            this.getBibo.Location = new System.Drawing.Point(0, 4);
            this.getBibo.Name = "getBibo";
            this.getBibo.Size = new System.Drawing.Size(212, 24);
            this.getBibo.TabIndex = 0;
            this.getBibo.Text = "Get Bibo Textures";
            this.getBibo.UseVisualStyleBackColor = true;
            // 
            // getEve
            // 
            this.getEve.Location = new System.Drawing.Point(0, 32);
            this.getEve.Name = "getEve";
            this.getEve.Size = new System.Drawing.Size(212, 24);
            this.getEve.TabIndex = 1;
            this.getEve.Text = "Get Eve/T&&F/Gen3 Textures";
            this.getEve.UseVisualStyleBackColor = true;
            // 
            // getScalesPlus
            // 
            this.getScalesPlus.Location = new System.Drawing.Point(0, 60);
            this.getScalesPlus.Name = "getScalesPlus";
            this.getScalesPlus.Size = new System.Drawing.Size(212, 24);
            this.getScalesPlus.TabIndex = 2;
            this.getScalesPlus.Text = "Get Scales+Textures";
            this.getScalesPlus.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "Get TBSE Textures";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(212, 24);
            this.button2.TabIndex = 4;
            this.button2.Text = "Get HRBODY Textures";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 296);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.getScalesPlus);
            this.Controls.Add(this.getEve);
            this.Controls.Add(this.getBibo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelpWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button getBibo;
        private Button getEve;
        private Button getScalesPlus;
        private Button button1;
        private Button button2;
    }
}