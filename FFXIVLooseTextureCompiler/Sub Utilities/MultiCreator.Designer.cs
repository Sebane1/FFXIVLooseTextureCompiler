namespace FFXIVLooseTextureCompiler {
    partial class MultiCreator {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiCreator));
            this.redImage = new System.Windows.Forms.PictureBox();
            this.result = new System.Windows.Forms.PictureBox();
            this.importButton1 = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.greenImage = new System.Windows.Forms.PictureBox();
            this.blueImage = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.alphaImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.redImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaImage)).BeginInit();
            this.SuspendLayout();
            // 
            // redImage
            // 
            this.redImage.BackColor = System.Drawing.Color.DarkRed;
            this.redImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.redImage.Location = new System.Drawing.Point(8, 32);
            this.redImage.Name = "redImage";
            this.redImage.Size = new System.Drawing.Size(92, 84);
            this.redImage.TabIndex = 0;
            this.redImage.TabStop = false;
            this.redImage.DragDrop += new System.Windows.Forms.DragEventHandler(this.import1_DragDrop);
            this.redImage.DragEnter += new System.Windows.Forms.DragEventHandler(this.import1_DragEnter);
            // 
            // result
            // 
            this.result.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.result.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.result.Location = new System.Drawing.Point(108, 12);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(504, 504);
            this.result.TabIndex = 1;
            this.result.TabStop = false;
            // 
            // importButton1
            // 
            this.importButton1.Location = new System.Drawing.Point(8, 116);
            this.importButton1.Name = "importButton1";
            this.importButton1.Size = new System.Drawing.Size(92, 23);
            this.importButton1.TabIndex = 3;
            this.importButton1.Text = "Import";
            this.importButton1.UseVisualStyleBackColor = true;
            this.importButton1.Click += new System.EventHandler(this.importButton1_Click);
            this.importButton1.DragDrop += new System.Windows.Forms.DragEventHandler(this.import1_DragDrop);
            this.importButton1.DragEnter += new System.Windows.Forms.DragEventHandler(this.import1_DragEnter);
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(108, 520);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(504, 23);
            this.exportButton.TabIndex = 5;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Red Channel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Green Channel";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Import";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.importButton2_Click);
            this.button1.DragDrop += new System.Windows.Forms.DragEventHandler(this.import2_DragEnter);
            this.button1.DragEnter += new System.Windows.Forms.DragEventHandler(this.import2_DragDrop);
            // 
            // greenImage
            // 
            this.greenImage.BackColor = System.Drawing.Color.ForestGreen;
            this.greenImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.greenImage.Location = new System.Drawing.Point(8, 168);
            this.greenImage.Name = "greenImage";
            this.greenImage.Size = new System.Drawing.Size(92, 84);
            this.greenImage.TabIndex = 9;
            this.greenImage.TabStop = false;
            this.greenImage.DragDrop += new System.Windows.Forms.DragEventHandler(this.import2_DragEnter);
            this.greenImage.DragEnter += new System.Windows.Forms.DragEventHandler(this.import2_DragDrop);
            // 
            // blueImage
            // 
            this.blueImage.BackColor = System.Drawing.Color.MidnightBlue;
            this.blueImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.blueImage.Location = new System.Drawing.Point(8, 304);
            this.blueImage.Name = "blueImage";
            this.blueImage.Size = new System.Drawing.Size(92, 84);
            this.blueImage.TabIndex = 9;
            this.blueImage.TabStop = false;
            this.blueImage.DragDrop += new System.Windows.Forms.DragEventHandler(this.import3_DragEnter);
            this.blueImage.DragEnter += new System.Windows.Forms.DragEventHandler(this.import3_DragDrop);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 388);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Import";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.importButton3_Click);
            this.button2.DragDrop += new System.Windows.Forms.DragEventHandler(this.import3_DragEnter);
            this.button2.DragEnter += new System.Windows.Forms.DragEventHandler(this.import3_DragDrop);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 284);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Blue Channel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 416);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Alpha Channel";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 520);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(92, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "Import";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.importButton4_Click);
            this.button3.DragDrop += new System.Windows.Forms.DragEventHandler(this.import4_DragEnter);
            this.button3.DragEnter += new System.Windows.Forms.DragEventHandler(this.import4_DragDrop);
            // 
            // alphaImage
            // 
            this.alphaImage.BackColor = System.Drawing.Color.Black;
            this.alphaImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.alphaImage.Location = new System.Drawing.Point(8, 436);
            this.alphaImage.Name = "alphaImage";
            this.alphaImage.Size = new System.Drawing.Size(92, 84);
            this.alphaImage.TabIndex = 12;
            this.alphaImage.TabStop = false;
            this.alphaImage.DragDrop += new System.Windows.Forms.DragEventHandler(this.import4_DragEnter);
            this.alphaImage.DragEnter += new System.Windows.Forms.DragEventHandler(this.import4_DragDrop);
            // 
            // MultiCreator
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(622, 551);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.alphaImage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.blueImage);
            this.Controls.Add(this.greenImage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.importButton1);
            this.Controls.Add(this.result);
            this.Controls.Add(this.redImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiCreator";
            this.Text = "RGBA Merger (Experimental)";
            ((System.ComponentModel.ISupportInitialize)(this.redImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.result)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox redImage;
        private PictureBox result;
        private Button importButton1;
        private Button exportButton;
        private Label label1;
        private Label label2;
        private Button button1;
        private PictureBox greenImage;
        private PictureBox blueImage;
        private Button button2;
        private Label label3;
        private Label label4;
        private Button button3;
        private PictureBox alphaImage;
    }
}