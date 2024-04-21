namespace LunAdd
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblEintrag = new Label();
            txtSearchField = new TextBox();
            btnShowEMail = new Button();
            btnShowAddress = new Button();
            btnHomePhone = new Button();
            btnShowMobile = new Button();
            btnForward = new Button();
            btnBack = new Button();
            txtEntryInformation = new TextBox();
            btnOpening = new Button();
            numIndex = new NumericUpDown();
            btnWorkPhone = new Button();
            ((System.ComponentModel.ISupportInitialize)numIndex).BeginInit();
            SuspendLayout();
            // 
            // lblEintrag
            // 
            lblEintrag.AutoSize = true;
            lblEintrag.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblEintrag.Location = new Point(12, 9);
            lblEintrag.Name = "lblEintrag";
            lblEintrag.Size = new Size(96, 32);
            lblEintrag.TabIndex = 0;
            lblEintrag.Text = "Suchen";
            // 
            // txtSearchField
            // 
            txtSearchField.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            txtSearchField.Location = new Point(20, 48);
            txtSearchField.Name = "txtSearchField";
            txtSearchField.Size = new Size(750, 61);
            txtSearchField.TabIndex = 1;
            // 
            // btnShowEMail
            // 
            btnShowEMail.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            btnShowEMail.Location = new Point(878, 22);
            btnShowEMail.Name = "btnShowEMail";
            btnShowEMail.Size = new Size(138, 112);
            btnShowEMail.TabIndex = 2;
            btnShowEMail.Text = "@";
            btnShowEMail.UseVisualStyleBackColor = true;
            btnShowEMail.Click += BtnShowEMail_Click;
            // 
            // btnShowAddress
            // 
            btnShowAddress.Font = new Font("Segoe UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point);
            btnShowAddress.Location = new Point(1037, 22);
            btnShowAddress.Name = "btnShowAddress";
            btnShowAddress.Size = new Size(132, 112);
            btnShowAddress.TabIndex = 3;
            btnShowAddress.Text = "Haus";
            btnShowAddress.UseVisualStyleBackColor = true;
            // 
            // btnHomePhone
            // 
            btnHomePhone.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            btnHomePhone.Location = new Point(878, 143);
            btnHomePhone.Name = "btnHomePhone";
            btnHomePhone.Size = new Size(138, 112);
            btnHomePhone.TabIndex = 4;
            btnHomePhone.Text = "Tel";
            btnHomePhone.UseVisualStyleBackColor = true;
            btnHomePhone.Click += BtnHomePhone_Click;
            // 
            // btnShowMobile
            // 
            btnShowMobile.Font = new Font("Segoe UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point);
            btnShowMobile.Location = new Point(1037, 143);
            btnShowMobile.Name = "btnShowMobile";
            btnShowMobile.Size = new Size(132, 112);
            btnShowMobile.TabIndex = 5;
            btnShowMobile.Text = "Mob";
            btnShowMobile.UseVisualStyleBackColor = true;
            btnShowMobile.Click += BtnShowMobile_Click;
            // 
            // btnForward
            // 
            btnForward.Location = new Point(698, 127);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(72, 71);
            btnForward.TabIndex = 6;
            btnForward.Text = "next";
            btnForward.UseVisualStyleBackColor = true;
            btnForward.Click += BtnForward_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(571, 127);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(72, 71);
            btnBack.TabIndex = 7;
            btnBack.Text = "back";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += BtnBack_Click;
            // 
            // txtEntryInformation
            // 
            txtEntryInformation.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            txtEntryInformation.Location = new Point(20, 217);
            txtEntryInformation.Multiline = true;
            txtEntryInformation.Name = "txtEntryInformation";
            txtEntryInformation.ScrollBars = ScrollBars.Vertical;
            txtEntryInformation.Size = new Size(750, 672);
            txtEntryInformation.TabIndex = 8;
            // 
            // btnOpening
            // 
            btnOpening.Font = new Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point);
            btnOpening.Location = new Point(878, 289);
            btnOpening.Name = "btnOpening";
            btnOpening.Size = new Size(453, 95);
            btnOpening.TabIndex = 9;
            btnOpening.Text = "Informationen";
            btnOpening.UseVisualStyleBackColor = true;
            btnOpening.Click += BtnOpening_Click;
            // 
            // numIndex
            // 
            numIndex.Font = new Font("Segoe UI", 28.2F, FontStyle.Regular, GraphicsUnit.Point);
            numIndex.Location = new Point(20, 127);
            numIndex.Name = "numIndex";
            numIndex.Size = new Size(194, 70);
            numIndex.TabIndex = 10;
            numIndex.ValueChanged += NumIndex_ValueChanged;
            // 
            // btnWorkPhone
            // 
            btnWorkPhone.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            btnWorkPhone.Location = new Point(1193, 22);
            btnWorkPhone.Name = "btnWorkPhone";
            btnWorkPhone.Size = new Size(138, 233);
            btnWorkPhone.TabIndex = 11;
            btnWorkPhone.Text = "GTel";
            btnWorkPhone.UseVisualStyleBackColor = true;
            btnWorkPhone.Click += BtnWorkPhone_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SteelBlue;
            ClientSize = new Size(1357, 996);
            Controls.Add(btnWorkPhone);
            Controls.Add(numIndex);
            Controls.Add(btnOpening);
            Controls.Add(txtEntryInformation);
            Controls.Add(btnBack);
            Controls.Add(btnForward);
            Controls.Add(btnShowMobile);
            Controls.Add(btnHomePhone);
            Controls.Add(btnShowAddress);
            Controls.Add(btnShowEMail);
            Controls.Add(txtSearchField);
            Controls.Add(lblEintrag);
            Name = "Form1";
            Text = "Adressbuch";
            KeyDown += Element_KeyDown;
            KeyPress += Form1_KeyPress;
            ((System.ComponentModel.ISupportInitialize)numIndex).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblEintrag;
        private TextBox txtSearchField;
        private Button btnShowEMail;
        private Button btnShowAddress;
        private Button btnHomePhone;
        private Button btnShowMobile;
        private Button btnForward;
        private Button btnBack;
        private TextBox txtEntryInformation;
        private Button btnOpening;
        private NumericUpDown numIndex;
        private Button btnWorkPhone;
    }
}