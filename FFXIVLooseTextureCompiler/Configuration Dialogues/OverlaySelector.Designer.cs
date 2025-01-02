namespace FFXIVLooseTextureCompiler.Configuration_Dialogues {
    partial class OverlaySelector {
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
            label1 = new Label();
            addLayerButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(4, 67);
            label1.Name = "label1";
            label1.Size = new Size(661, 25);
            label1.TabIndex = 10;
            label1.Text = "Assigned Images Will Be Automatically Layered Bottom To Top On Export";
            label1.Click += label1_Click;
            // 
            // addLayerButton
            // 
            addLayerButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            addLayerButton.Location = new Point(563, 38);
            addLayerButton.Name = "addLayerButton";
            addLayerButton.Size = new Size(96, 23);
            addLayerButton.TabIndex = 11;
            addLayerButton.Text = "Add Layer";
            addLayerButton.UseVisualStyleBackColor = true;
            addLayerButton.Click += addLayerButton_Click;
            // 
            // OverlaySelector
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(670, 96);
            Controls.Add(addLayerButton);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "OverlaySelector";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Overlay Layer Assignment";
            Load += OverlaySelector_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Button addLayerButton;
    }
}