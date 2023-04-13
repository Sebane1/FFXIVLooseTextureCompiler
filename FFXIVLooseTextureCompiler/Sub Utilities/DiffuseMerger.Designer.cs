namespace FFXIVLooseTextureCompiler {
    partial class DiffuseMerger {
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
            this.image1 = new System.Windows.Forms.PictureBox();
            this.result = new System.Windows.Forms.PictureBox();
            this.image2 = new System.Windows.Forms.PictureBox();
            this.importButton1 = new System.Windows.Forms.Button();
            this.importButton2 = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.image1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.image2)).BeginInit();
            this.SuspendLayout();
            // 
            // image1
            // 
            this.image1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.image1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.image1.Location = new System.Drawing.Point(8, 12);
            this.image1.Name = "image1";
            this.image1.Size = new System.Drawing.Size(250, 250);
            this.image1.TabIndex = 0;
            this.image1.TabStop = false;
            this.image1.DragDrop += new System.Windows.Forms.DragEventHandler(this.import1_DragDrop);
            this.image1.DragEnter += new System.Windows.Forms.DragEventHandler(this.import1_DragEnter);
            // 
            // result
            // 
            this.result.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.result.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.result.Location = new System.Drawing.Point(272, 12);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(536, 528);
            this.result.TabIndex = 1;
            this.result.TabStop = false;
            // 
            // image2
            // 
            this.image2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.image2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.image2.Location = new System.Drawing.Point(8, 289);
            this.image2.Name = "image2";
            this.image2.Size = new System.Drawing.Size(250, 252);
            this.image2.TabIndex = 2;
            this.image2.TabStop = false;
            this.image2.DragDrop += new System.Windows.Forms.DragEventHandler(this.import2_DragDrop);
            this.image2.DragEnter += new System.Windows.Forms.DragEventHandler(this.import2_DragEnter);
            // 
            // importButton1
            // 
            this.importButton1.Location = new System.Drawing.Point(8, 264);
            this.importButton1.Name = "importButton1";
            this.importButton1.Size = new System.Drawing.Size(252, 23);
            this.importButton1.TabIndex = 3;
            this.importButton1.Text = "Import";
            this.importButton1.UseVisualStyleBackColor = true;
            this.importButton1.Click += new System.EventHandler(this.importButton1_Click);
            this.importButton1.DragDrop += new System.Windows.Forms.DragEventHandler(this.import1_DragDrop);
            this.importButton1.DragEnter += new System.Windows.Forms.DragEventHandler(this.import1_DragEnter);
            // 
            // importButton2
            // 
            this.importButton2.Location = new System.Drawing.Point(8, 543);
            this.importButton2.Name = "importButton2";
            this.importButton2.Size = new System.Drawing.Size(252, 23);
            this.importButton2.TabIndex = 4;
            this.importButton2.Text = "Import";
            this.importButton2.UseVisualStyleBackColor = true;
            this.importButton2.Click += new System.EventHandler(this.importButton2_Click);
            this.importButton2.DragDrop += new System.Windows.Forms.DragEventHandler(this.import2_DragDrop);
            this.importButton2.DragEnter += new System.Windows.Forms.DragEventHandler(this.import2_DragEnter);
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(272, 543);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(536, 23);
            this.exportButton.TabIndex = 5;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // DiffuseMerger
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(814, 574);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.importButton2);
            this.Controls.Add(this.importButton1);
            this.Controls.Add(this.image2);
            this.Controls.Add(this.result);
            this.Controls.Add(this.image1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "DiffuseMerger";
            this.Text = "Diffuse Merger (Experimental)";
            ((System.ComponentModel.ISupportInitialize)(this.image1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.result)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.image2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox image1;
        private PictureBox result;
        private PictureBox image2;
        private Button importButton1;
        private Button importButton2;
        private Button exportButton;
    }
}