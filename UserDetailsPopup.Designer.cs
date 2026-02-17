namespace Secugen_HU20
{
    partial class UserDetailsPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserDetailsPopup));
            this.labelFNIC = new System.Windows.Forms.Label();
            this.labelBForm = new System.Windows.Forms.Label();
            this.labelFName = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.lblHeading = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.lblFormno = new System.Windows.Forms.Label();
            this.lblstdid = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFNIC
            // 
            this.labelFNIC.AutoSize = true;
            this.labelFNIC.Location = new System.Drawing.Point(21, 188);
            this.labelFNIC.Name = "labelFNIC";
            this.labelFNIC.Size = new System.Drawing.Size(31, 13);
            this.labelFNIC.TabIndex = 20;
            this.labelFNIC.Text = "FNIC";
            // 
            // labelBForm
            // 
            this.labelBForm.AutoSize = true;
            this.labelBForm.Location = new System.Drawing.Point(21, 164);
            this.labelBForm.Name = "labelBForm";
            this.labelBForm.Size = new System.Drawing.Size(40, 13);
            this.labelBForm.TabIndex = 19;
            this.labelBForm.Text = "B-Form";
            // 
            // labelFName
            // 
            this.labelFName.AutoSize = true;
            this.labelFName.Location = new System.Drawing.Point(21, 139);
            this.labelFName.Name = "labelFName";
            this.labelFName.Size = new System.Drawing.Size(68, 13);
            this.labelFName.TabIndex = 18;
            this.labelFName.Text = "Father Name";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(21, 116);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 17;
            this.labelName.Text = "Name";
            // 
            // lblHeading
            // 
            this.lblHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeading.AutoSize = true;
            this.lblHeading.Location = new System.Drawing.Point(21, 27);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(192, 13);
            this.lblHeading.TabIndex = 24;
            this.lblHeading.Text = "Showing the Detail of Verified Biometric";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(15, 261);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(379, 35);
            this.btnOk.TabIndex = 25;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxImage.Location = new System.Drawing.Point(264, 72);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(130, 130);
            this.pictureBoxImage.TabIndex = 26;
            this.pictureBoxImage.TabStop = false;
            // 
            // lblFormno
            // 
            this.lblFormno.AutoSize = true;
            this.lblFormno.Location = new System.Drawing.Point(21, 95);
            this.lblFormno.Name = "lblFormno";
            this.lblFormno.Size = new System.Drawing.Size(50, 13);
            this.lblFormno.TabIndex = 27;
            this.lblFormno.Text = "Form No.";
            // 
            // lblstdid
            // 
            this.lblstdid.AutoSize = true;
            this.lblstdid.Location = new System.Drawing.Point(21, 72);
            this.lblstdid.Name = "lblstdid";
            this.lblstdid.Size = new System.Drawing.Size(21, 13);
            this.lblstdid.TabIndex = 28;
            this.lblstdid.Text = "ID.";
            // 
            // UserDetailsPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 308);
            this.Controls.Add(this.lblstdid);
            this.Controls.Add(this.lblFormno);
            this.Controls.Add(this.pictureBoxImage);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.labelFNIC);
            this.Controls.Add(this.labelBForm);
            this.Controls.Add(this.labelFName);
            this.Controls.Add(this.labelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UserDetailsPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BISEGRW | CANDIDATE DETAILS";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelFNIC;
        private System.Windows.Forms.Label labelBForm;
        private System.Windows.Forms.Label labelFName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Label lblFormno;
        private System.Windows.Forms.Label lblstdid;
    }
}