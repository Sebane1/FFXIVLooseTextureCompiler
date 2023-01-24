namespace FFXIVLooseTextureCompiler {
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
            this.textureList = new System.Windows.Forms.ListBox();
            this.texturePreview = new System.Windows.Forms.PictureBox();
            this.exportPNG = new System.Windows.Forms.Button();
            this.bulkImport = new System.Windows.Forms.Button();
            this.exportAllButton = new System.Windows.Forms.Button();
            this.clearListButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // textureList
            // 
            this.textureList.AllowDrop = true;
            this.textureList.FormattingEnabled = true;
            this.textureList.ItemHeight = 15;
            this.textureList.Location = new System.Drawing.Point(4, 4);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(168, 304);
            this.textureList.TabIndex = 0;
            this.textureList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.textureList.DragDrop += new System.Windows.Forms.DragEventHandler(this.texList_DragDrop);
            this.textureList.DragEnter += new System.Windows.Forms.DragEventHandler(this.texList_DragEnter);
            this.textureList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.texList_MouseDown);
            this.textureList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.texList_MouseMove);
            // 
            // texturePreview
            // 
            this.texturePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.texturePreview.Location = new System.Drawing.Point(176, 0);
            this.texturePreview.Name = "texturePreview";
            this.texturePreview.Size = new System.Drawing.Size(308, 308);
            this.texturePreview.TabIndex = 1;
            this.texturePreview.TabStop = false;
            this.texturePreview.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // exportPNG
            // 
            this.exportPNG.Location = new System.Drawing.Point(400, 312);
            this.exportPNG.Name = "exportPNG";
            this.exportPNG.Size = new System.Drawing.Size(80, 24);
            this.exportPNG.TabIndex = 3;
            this.exportPNG.Text = "Export PNG";
            this.exportPNG.UseVisualStyleBackColor = true;
            this.exportPNG.Click += new System.EventHandler(this.exportPNGButton_Click);
            // 
            // bulkImport
            // 
            this.bulkImport.Location = new System.Drawing.Point(4, 312);
            this.bulkImport.Name = "bulkImport";
            this.bulkImport.Size = new System.Drawing.Size(168, 23);
            this.bulkImport.TabIndex = 4;
            this.bulkImport.Text = "Recursive Bulk Import";
            this.bulkImport.UseVisualStyleBackColor = true;
            this.bulkImport.Click += new System.EventHandler(this.bulkImport_Click);
            // 
            // exportAllButton
            // 
            this.exportAllButton.Location = new System.Drawing.Point(264, 312);
            this.exportAllButton.Name = "exportAllButton";
            this.exportAllButton.Size = new System.Drawing.Size(132, 24);
            this.exportAllButton.TabIndex = 5;
            this.exportAllButton.Text = "Bulk Export To PNG";
            this.exportAllButton.UseVisualStyleBackColor = true;
            this.exportAllButton.Click += new System.EventHandler(this.exportAllButton_Click);
            // 
            // clearListButton
            // 
            this.clearListButton.Location = new System.Drawing.Point(176, 312);
            this.clearListButton.Name = "clearListButton";
            this.clearListButton.Size = new System.Drawing.Size(88, 24);
            this.clearListButton.TabIndex = 6;
            this.clearListButton.Text = "Clear List";
            this.clearListButton.UseVisualStyleBackColor = true;
            this.clearListButton.Click += new System.EventHandler(this.textureList_Click);
            // 
            // BulkTexManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 341);
            this.Controls.Add(this.clearListButton);
            this.Controls.Add(this.exportAllButton);
            this.Controls.Add(this.bulkImport);
            this.Controls.Add(this.exportPNG);
            this.Controls.Add(this.texturePreview);
            this.Controls.Add(this.textureList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BulkTexManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bulk Tex Manager";
            ((System.ComponentModel.ISupportInitialize)(this.texturePreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox textureList;
        private PictureBox texturePreview;
        private Button exportPNG;
        private Button bulkImport;
        private Button exportAllButton;
        private Button clearListButton;
    }
}