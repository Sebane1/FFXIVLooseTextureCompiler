namespace FFXIVLooseTextureCompiler {
    partial class BaseMerger {
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
            image1 = new PictureBox();
            result = new PictureBox();
            image2 = new PictureBox();
            importButton1 = new Button();
            importButton2 = new Button();
            exportButton = new Button();
            ((System.ComponentModel.ISupportInitialize)image1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)result).BeginInit();
            ((System.ComponentModel.ISupportInitialize)image2).BeginInit();
            SuspendLayout();
            // 
            // image1
            // 
            image1.BackColor = SystemColors.ActiveCaption;
            image1.BackgroundImageLayout = ImageLayout.Zoom;
            image1.Location = new Point(8, 12);
            image1.Name = "image1";
            image1.Size = new Size(250, 250);
            image1.TabIndex = 0;
            image1.TabStop = false;
            image1.DragDrop += import1_DragDrop;
            image1.DragEnter += import1_DragEnter;
            // 
            // result
            // 
            result.BackColor = SystemColors.ActiveCaption;
            result.BackgroundImageLayout = ImageLayout.Zoom;
            result.Location = new Point(272, 12);
            result.Name = "result";
            result.Size = new Size(536, 528);
            result.TabIndex = 1;
            result.TabStop = false;
            // 
            // image2
            // 
            image2.BackColor = SystemColors.ActiveCaption;
            image2.BackgroundImageLayout = ImageLayout.Zoom;
            image2.Location = new Point(8, 289);
            image2.Name = "image2";
            image2.Size = new Size(250, 252);
            image2.TabIndex = 2;
            image2.TabStop = false;
            image2.DragDrop += import2_DragDrop;
            image2.DragEnter += import2_DragEnter;
            // 
            // importButton1
            // 
            importButton1.Location = new Point(8, 264);
            importButton1.Name = "importButton1";
            importButton1.Size = new Size(252, 23);
            importButton1.TabIndex = 3;
            importButton1.Text = "Import";
            importButton1.UseVisualStyleBackColor = true;
            importButton1.Click += importButton1_Click;
            importButton1.DragDrop += import1_DragDrop;
            importButton1.DragEnter += import1_DragEnter;
            // 
            // importButton2
            // 
            importButton2.Location = new Point(8, 543);
            importButton2.Name = "importButton2";
            importButton2.Size = new Size(252, 23);
            importButton2.TabIndex = 4;
            importButton2.Text = "Import";
            importButton2.UseVisualStyleBackColor = true;
            importButton2.Click += importButton2_Click;
            importButton2.DragDrop += import2_DragDrop;
            importButton2.DragEnter += import2_DragEnter;
            // 
            // exportButton
            // 
            exportButton.Location = new Point(272, 543);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(536, 23);
            exportButton.TabIndex = 5;
            exportButton.Text = "Export";
            exportButton.UseVisualStyleBackColor = true;
            exportButton.Click += exportButton_Click;
            // 
            // BaseMerger
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(814, 574);
            Controls.Add(exportButton);
            Controls.Add(importButton2);
            Controls.Add(importButton1);
            Controls.Add(image2);
            Controls.Add(result);
            Controls.Add(image1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MinimizeBox = false;
            Name = "BaseMerger";
            Text = "Base Merger (Experimental)";
            Load += BaseMerger_Load;
            ((System.ComponentModel.ISupportInitialize)image1).EndInit();
            ((System.ComponentModel.ISupportInitialize)result).EndInit();
            ((System.ComponentModel.ISupportInitialize)image2).EndInit();
            ResumeLayout(false);

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