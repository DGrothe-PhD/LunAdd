namespace LunAdd
{
    partial class Element
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtContent = new TextBox();
            lblTitle = new Label();
            SuspendLayout();
            // 
            // txtContent
            // 
            txtContent.AccessibleName = "Inhalt";
            txtContent.Font = new Font("Segoe UI", 125F, FontStyle.Regular, GraphicsUnit.Point);
            txtContent.Location = new Point(29, 57);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.ReadOnly = true;
            txtContent.ScrollBars = ScrollBars.Vertical;
            txtContent.Size = new Size(1293, 974);
            txtContent.TabIndex = 0;
            txtContent.MouseEnter += txtContent_MouseEnter;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.Location = new Point(29, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(118, 31);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Feldname";
            // 
            // Element
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSeaGreen;
            ClientSize = new Size(1349, 1053);
            Controls.Add(lblTitle);
            Controls.Add(txtContent);
            Name = "Element";
            Text = "Element";
            FormClosed += Element_FormClosed;
            KeyDown += Element_KeyDown;
            KeyPress += Element_KeyPress;
            KeyUp += Element_KeyUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtContent;
        private Label lblTitle;
    }
}