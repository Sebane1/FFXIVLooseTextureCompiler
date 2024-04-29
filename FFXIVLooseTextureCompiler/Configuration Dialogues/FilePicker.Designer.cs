
using System.ComponentModel;
using System.Windows.Forms;

namespace FFXIVVoicePackCreator {
    partial class FilePicker {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            filePath = new TextBox();
            openButton = new Button();
            labelName = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            clearButton = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // filePath
            // 
            filePath.Dock = DockStyle.Fill;
            filePath.Location = new Point(78, 3);
            filePath.Margin = new Padding(4, 3, 4, 3);
            filePath.Name = "filePath";
            filePath.Size = new Size(471, 23);
            filePath.TabIndex = 0;
            filePath.TextChanged += filePath_TextChanged;
            filePath.DragDrop += filePath_DragDrop;
            filePath.DragEnter += filePath_DragEnter;
            filePath.KeyPress += filePath_KeyPress;
            filePath.Leave += filePath_Leave;
            filePath.MouseDown += filePicker_MouseDown;
            filePath.MouseMove += filePicker_MouseMove;
            // 
            // openButton
            // 
            openButton.Dock = DockStyle.Fill;
            openButton.Location = new Point(586, 3);
            openButton.Margin = new Padding(4, 3, 4, 3);
            openButton.Name = "openButton";
            openButton.Size = new Size(58, 22);
            openButton.TabIndex = 1;
            openButton.Text = "Select";
            openButton.UseVisualStyleBackColor = true;
            openButton.Click += openButton_Click;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Dock = DockStyle.Fill;
            labelName.Location = new Point(4, 0);
            labelName.Margin = new Padding(4, 0, 4, 0);
            labelName.Name = "labelName";
            labelName.Size = new Size(66, 28);
            labelName.TabIndex = 2;
            labelName.Text = "surprised";
            labelName.TextAlign = ContentAlignment.MiddleLeft;
            labelName.Click += labelName_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 74F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 29F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 66F));
            tableLayoutPanel1.Controls.Add(openButton, 3, 0);
            tableLayoutPanel1.Controls.Add(filePath, 1, 0);
            tableLayoutPanel1.Controls.Add(labelName, 0, 0);
            tableLayoutPanel1.Controls.Add(clearButton, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(648, 28);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // clearButton
            // 
            clearButton.BackColor = Color.IndianRed;
            clearButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            clearButton.Location = new Point(556, 3);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(23, 22);
            clearButton.TabIndex = 3;
            clearButton.Text = "X";
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += clearButton_Click;
            // 
            // FilePicker
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(300, 28);
            Name = "FilePicker";
            Size = new Size(648, 28);
            Load += filePicker_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox filePath;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Label labelName;
        private TableLayoutPanel tableLayoutPanel1;
        private Button clearButton;

        public Label LabelName { get => labelName; set => labelName = value; }
        public TextBox FilePath { get => filePath; set => filePath = value; }
        [
   Category("Index"),
   Description("Sort Order")
   ]
        public int Index { get => index; set => index = value; }
    }
}
