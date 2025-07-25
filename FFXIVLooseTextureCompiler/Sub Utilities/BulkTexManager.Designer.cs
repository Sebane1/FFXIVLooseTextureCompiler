﻿namespace FFXIVLooseTextureCompiler {
    partial class BulkTexManager {
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BulkTexManager));
            textureList = new ListBox();
            texturePreview = new PictureBox();
            exportPNG = new Button();
            bulkImport = new Button();
            exportAllButton = new Button();
            clearListButton = new Button();
            pathTextBox = new TextBox();
            openPathButton = new Button();
            label1 = new Label();
            newFileCheck = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)texturePreview).BeginInit();
            SuspendLayout();
            // 
            // textureList
            // 
            textureList.AllowDrop = true;
            textureList.FormattingEnabled = true;
            textureList.ItemHeight = 15;
            textureList.Location = new Point(4, 4);
            textureList.Name = "textureList";
            textureList.Size = new Size(168, 589);
            textureList.TabIndex = 0;
            textureList.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            textureList.DragDrop += texList_DragDrop;
            textureList.DragEnter += texList_DragEnter;
            textureList.MouseDown += texList_MouseDown;
            textureList.MouseMove += texList_MouseMove;
            // 
            // texturePreview
            // 
            texturePreview.BackgroundImageLayout = ImageLayout.Zoom;
            texturePreview.Location = new Point(176, 0);
            texturePreview.Name = "texturePreview";
            texturePreview.Size = new Size(584, 593);
            texturePreview.TabIndex = 1;
            texturePreview.TabStop = false;
            texturePreview.Click += pictureBox1_Click;
            // 
            // exportPNG
            // 
            exportPNG.Location = new Point(481, 597);
            exportPNG.Name = "exportPNG";
            exportPNG.Size = new Size(140, 24);
            exportPNG.TabIndex = 3;
            exportPNG.Text = "Export PNG";
            exportPNG.UseVisualStyleBackColor = true;
            exportPNG.Click += exportPNGButton_Click;
            // 
            // bulkImport
            // 
            bulkImport.Location = new Point(4, 597);
            bulkImport.Name = "bulkImport";
            bulkImport.Size = new Size(168, 23);
            bulkImport.TabIndex = 4;
            bulkImport.Text = "Recursive Bulk Import";
            bulkImport.UseVisualStyleBackColor = true;
            bulkImport.Click += bulkImport_Click;
            // 
            // exportAllButton
            // 
            exportAllButton.Location = new Point(343, 597);
            exportAllButton.Name = "exportAllButton";
            exportAllButton.Size = new Size(132, 24);
            exportAllButton.TabIndex = 5;
            exportAllButton.Text = "Bulk Export To PNG";
            exportAllButton.UseVisualStyleBackColor = true;
            exportAllButton.Click += exportAllButton_Click;
            // 
            // clearListButton
            // 
            clearListButton.Location = new Point(176, 596);
            clearListButton.Name = "clearListButton";
            clearListButton.Size = new Size(161, 24);
            clearListButton.TabIndex = 6;
            clearListButton.Text = "Clear List";
            clearListButton.UseVisualStyleBackColor = true;
            clearListButton.Click += textureList_Click;
            // 
            // pathTextBox
            // 
            pathTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pathTextBox.Location = new Point(72, 624);
            pathTextBox.Name = "pathTextBox";
            pathTextBox.Size = new Size(688, 23);
            pathTextBox.TabIndex = 7;
            pathTextBox.TextChanged += pathTextBox_TextChanged;
            // 
            // openPathButton
            // 
            openPathButton.Location = new Point(627, 597);
            openPathButton.Name = "openPathButton";
            openPathButton.Size = new Size(133, 24);
            openPathButton.TabIndex = 8;
            openPathButton.Text = "Open Path";
            openPathButton.UseVisualStyleBackColor = true;
            openPathButton.Click += openPathButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 628);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 9;
            label1.Text = "Base Path";
            // 
            // newFileCheck
            // 
            newFileCheck.Enabled = true;
            newFileCheck.Interval = 5000;
            newFileCheck.Tick += newFileCheck_Tick;
            // 
            // BulkTexManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(764, 651);
            Controls.Add(label1);
            Controls.Add(openPathButton);
            Controls.Add(pathTextBox);
            Controls.Add(clearListButton);
            Controls.Add(exportAllButton);
            Controls.Add(bulkImport);
            Controls.Add(exportPNG);
            Controls.Add(texturePreview);
            Controls.Add(textureList);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "BulkTexManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bulk Tex Manager";
            Load += BulkTexManager_Load;
            ((System.ComponentModel.ISupportInitialize)texturePreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox textureList;
        private PictureBox texturePreview;
        private Button exportPNG;
        private Button bulkImport;
        private Button exportAllButton;
        private Button clearListButton;
        private TextBox pathTextBox;
        private Button openPathButton;
        private Label label1;
        private System.Windows.Forms.Timer newFileCheck;
    }
}