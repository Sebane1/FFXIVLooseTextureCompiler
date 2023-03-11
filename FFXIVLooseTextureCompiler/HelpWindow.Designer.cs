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
            this.getTBSE = new System.Windows.Forms.Button();
            this.getHRBODY = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.getOtopop = new System.Windows.Forms.Button();
            this.getRedefinedLala = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // getBibo
            // 
            this.getBibo.Location = new System.Drawing.Point(0, 36);
            this.getBibo.Name = "getBibo";
            this.getBibo.Size = new System.Drawing.Size(212, 24);
            this.getBibo.TabIndex = 0;
            this.getBibo.Text = "Get Bibo+ Textures";
            this.getBibo.UseVisualStyleBackColor = true;
            this.getBibo.Click += new System.EventHandler(this.getBibo_Click);
            // 
            // getEve
            // 
            this.getEve.Location = new System.Drawing.Point(0, 64);
            this.getEve.Name = "getEve";
            this.getEve.Size = new System.Drawing.Size(212, 24);
            this.getEve.TabIndex = 1;
            this.getEve.Text = "Get Eve/T&&F/Gen3 Textures";
            this.getEve.UseVisualStyleBackColor = true;
            this.getEve.Click += new System.EventHandler(this.getGen3_Click);
            // 
            // getScalesPlus
            // 
            this.getScalesPlus.Location = new System.Drawing.Point(0, 92);
            this.getScalesPlus.Name = "getScalesPlus";
            this.getScalesPlus.Size = new System.Drawing.Size(212, 24);
            this.getScalesPlus.TabIndex = 2;
            this.getScalesPlus.Text = "Get Scales+Textures";
            this.getScalesPlus.UseVisualStyleBackColor = true;
            this.getScalesPlus.Click += new System.EventHandler(this.getScalesPlus_Click);
            // 
            // getTBSE
            // 
            this.getTBSE.Location = new System.Drawing.Point(0, 120);
            this.getTBSE.Name = "getTBSE";
            this.getTBSE.Size = new System.Drawing.Size(212, 24);
            this.getTBSE.TabIndex = 3;
            this.getTBSE.Text = "Get TBSE Textures";
            this.getTBSE.UseVisualStyleBackColor = true;
            this.getTBSE.Click += new System.EventHandler(this.getTBSE_Click);
            // 
            // getHRBODY
            // 
            this.getHRBODY.Location = new System.Drawing.Point(0, 148);
            this.getHRBODY.Name = "getHRBODY";
            this.getHRBODY.Size = new System.Drawing.Size(212, 24);
            this.getHRBODY.TabIndex = 4;
            this.getHRBODY.Text = "Get HRBODY Textures";
            this.getHRBODY.UseVisualStyleBackColor = true;
            this.getHRBODY.Click += new System.EventHandler(this.getHRBODY_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "These link to direct downloads";
            // 
            // getOtopop
            // 
            this.getOtopop.Location = new System.Drawing.Point(0, 176);
            this.getOtopop.Name = "getOtopop";
            this.getOtopop.Size = new System.Drawing.Size(212, 24);
            this.getOtopop.TabIndex = 6;
            this.getOtopop.Text = "Get Otopop Textures";
            this.getOtopop.UseVisualStyleBackColor = true;
            this.getOtopop.Click += new System.EventHandler(this.getOtopop_Click);
            // 
            // getRedefinedLala
            // 
            this.getRedefinedLala.Location = new System.Drawing.Point(0, 204);
            this.getRedefinedLala.Name = "getRedefinedLala";
            this.getRedefinedLala.Size = new System.Drawing.Size(212, 24);
            this.getRedefinedLala.TabIndex = 7;
            this.getRedefinedLala.Text = "Get Redefined Lala Textures";
            this.getRedefinedLala.UseVisualStyleBackColor = true;
            this.getRedefinedLala.Click += new System.EventHandler(this.getRedefinedLala_Click);
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 231);
            this.Controls.Add(this.getRedefinedLala);
            this.Controls.Add(this.getOtopop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.getHRBODY);
            this.Controls.Add(this.getTBSE);
            this.Controls.Add(this.getScalesPlus);
            this.Controls.Add(this.getEve);
            this.Controls.Add(this.getBibo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "HelpWindow";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.HelpWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button getBibo;
        private Button getEve;
        private Button getScalesPlus;
        private Button getTBSE;
        private Button getHRBODY;
        private Label label1;
        private Button getOtopop;
        private Button getRedefinedLala;
    }
}