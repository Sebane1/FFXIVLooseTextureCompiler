namespace FFXIVLooseTextureCompiler {
    partial class MaskCreator {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaskCreator));
            redImage = new PictureBox();
            result = new PictureBox();
            importButton1 = new Button();
            exportButton = new Button();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            greenImage = new PictureBox();
            blueImage = new PictureBox();
            button2 = new Button();
            label3 = new Label();
            label4 = new Label();
            button3 = new Button();
            alphaImage = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)redImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)result).BeginInit();
            ((System.ComponentModel.ISupportInitialize)greenImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)blueImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)alphaImage).BeginInit();
            SuspendLayout();
            // 
            // redImage
            // 
            redImage.BackColor = Color.DarkRed;
            redImage.BackgroundImageLayout = ImageLayout.Zoom;
            redImage.Location = new Point(8, 32);
            redImage.Name = "redImage";
            redImage.Size = new Size(92, 84);
            redImage.TabIndex = 0;
            redImage.TabStop = false;
            redImage.DragDrop += import1_DragDrop;
            redImage.DragEnter += import1_DragEnter;
            // 
            // result
            // 
            result.BackColor = SystemColors.ActiveCaption;
            result.BackgroundImageLayout = ImageLayout.Zoom;
            result.Location = new Point(108, 12);
            result.Name = "result";
            result.Size = new Size(504, 504);
            result.TabIndex = 1;
            result.TabStop = false;
            // 
            // importButton1
            // 
            importButton1.Location = new Point(8, 116);
            importButton1.Name = "importButton1";
            importButton1.Size = new Size(92, 23);
            importButton1.TabIndex = 3;
            importButton1.Text = "Import";
            importButton1.UseVisualStyleBackColor = true;
            importButton1.Click += importButton1_Click;
            importButton1.DragDrop += import1_DragDrop;
            importButton1.DragEnter += import1_DragEnter;
            // 
            // exportButton
            // 
            exportButton.Location = new Point(108, 520);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(504, 23);
            exportButton.TabIndex = 5;
            exportButton.Text = "Export";
            exportButton.UseVisualStyleBackColor = true;
            exportButton.Click += exportButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 12);
            label1.Name = "label1";
            label1.Size = new Size(74, 15);
            label1.TabIndex = 8;
            label1.Text = "Red Channel";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 148);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 11;
            label2.Text = "Green Channel";
            // 
            // button1
            // 
            button1.Location = new Point(8, 252);
            button1.Name = "button1";
            button1.Size = new Size(92, 23);
            button1.TabIndex = 10;
            button1.Text = "Import";
            button1.UseVisualStyleBackColor = true;
            button1.Click += importButton2_Click;
            button1.DragDrop += import2_DragEnter;
            button1.DragEnter += import2_DragDrop;
            // 
            // greenImage
            // 
            greenImage.BackColor = Color.ForestGreen;
            greenImage.BackgroundImageLayout = ImageLayout.Zoom;
            greenImage.Location = new Point(8, 168);
            greenImage.Name = "greenImage";
            greenImage.Size = new Size(92, 84);
            greenImage.TabIndex = 9;
            greenImage.TabStop = false;
            greenImage.DragDrop += import2_DragEnter;
            greenImage.DragEnter += import2_DragDrop;
            // 
            // blueImage
            // 
            blueImage.BackColor = Color.MidnightBlue;
            blueImage.BackgroundImageLayout = ImageLayout.Zoom;
            blueImage.Location = new Point(8, 304);
            blueImage.Name = "blueImage";
            blueImage.Size = new Size(92, 84);
            blueImage.TabIndex = 9;
            blueImage.TabStop = false;
            blueImage.DragDrop += import3_DragEnter;
            blueImage.DragEnter += import3_DragDrop;
            // 
            // button2
            // 
            button2.Location = new Point(8, 388);
            button2.Name = "button2";
            button2.Size = new Size(92, 23);
            button2.TabIndex = 10;
            button2.Text = "Import";
            button2.UseVisualStyleBackColor = true;
            button2.Click += importButton3_Click;
            button2.DragDrop += import3_DragEnter;
            button2.DragEnter += import3_DragDrop;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 284);
            label3.Name = "label3";
            label3.Size = new Size(77, 15);
            label3.TabIndex = 11;
            label3.Text = "Blue Channel";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 416);
            label4.Name = "label4";
            label4.Size = new Size(85, 15);
            label4.TabIndex = 14;
            label4.Text = "Alpha Channel";
            // 
            // button3
            // 
            button3.Location = new Point(8, 520);
            button3.Name = "button3";
            button3.Size = new Size(92, 23);
            button3.TabIndex = 13;
            button3.Text = "Import";
            button3.UseVisualStyleBackColor = true;
            button3.Click += importButton4_Click;
            button3.DragDrop += import4_DragEnter;
            button3.DragEnter += import4_DragDrop;
            // 
            // alphaImage
            // 
            alphaImage.BackColor = Color.Black;
            alphaImage.BackgroundImageLayout = ImageLayout.Zoom;
            alphaImage.Location = new Point(8, 436);
            alphaImage.Name = "alphaImage";
            alphaImage.Size = new Size(92, 84);
            alphaImage.TabIndex = 12;
            alphaImage.TabStop = false;
            alphaImage.DragDrop += import4_DragEnter;
            alphaImage.DragEnter += import4_DragDrop;
            // 
            // MaskCreator
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(622, 551);
            Controls.Add(label4);
            Controls.Add(button3);
            Controls.Add(alphaImage);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(blueImage);
            Controls.Add(greenImage);
            Controls.Add(label1);
            Controls.Add(exportButton);
            Controls.Add(importButton1);
            Controls.Add(result);
            Controls.Add(redImage);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MaskCreator";
            Text = "RGBA Merger (Experimental)";
            Load += MaskCreator_Load;
            ((System.ComponentModel.ISupportInitialize)redImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)result).EndInit();
            ((System.ComponentModel.ISupportInitialize)greenImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)blueImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)alphaImage).EndInit();
            ResumeLayout(false);
            PerformLayout();

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