using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Secugen_HU20
{
    public partial class LoginForm : Form
    {
        private Guna2TextBox textBoxInstituteCode;
        private Guna2TextBox textBoxPassword;
        private Guna2ComboBox ddlLoginType;
        private Guna2ComboBox ddlMode;
        private Guna2ComboBox ddlClass;
        private Guna2ComboBox ddlSess;
        private Guna2Button btnLogin;
        private Guna2HtmlLabel lblHeading;
        private Guna2ProgressIndicator progressBarLogin;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.lblHeading = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.ddlLoginType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.textBoxInstituteCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.textBoxPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.ddlMode = new Guna.UI2.WinForms.Guna2ComboBox();
            this.ddlClass = new Guna.UI2.WinForms.Guna2ComboBox();
            this.ddlSess = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnLogin = new Guna.UI2.WinForms.Guna2Button();
            this.progressBarLogin = new Guna.UI2.WinForms.Guna2ProgressIndicator();
            this.SuspendLayout();
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = false;
            this.lblHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblHeading.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeading.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.lblHeading.Location = new System.Drawing.Point(0, 0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(400, 70);
            this.lblHeading.TabIndex = 0;
            this.lblHeading.Text = "BISE Gujranwala Biometric";
            this.lblHeading.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ddlLoginType
            // 
            this.ddlLoginType.AutoRoundedCorners = true;
            this.ddlLoginType.BackColor = System.Drawing.Color.Transparent;
            this.ddlLoginType.BorderRadius = 17;
            this.ddlLoginType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlLoginType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLoginType.FocusedColor = System.Drawing.Color.Empty;
            this.ddlLoginType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ddlLoginType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.ddlLoginType.ItemHeight = 30;
            this.ddlLoginType.Items.AddRange(new object[] {
            "-- Select Login Type --",
            "Institute",
            "IT Teacher",
            "Supervisory Staff"});
            this.ddlLoginType.Location = new System.Drawing.Point(100, 90);
            this.ddlLoginType.Name = "ddlLoginType";
            this.ddlLoginType.Size = new System.Drawing.Size(200, 36);
            this.ddlLoginType.TabIndex = 1;
            this.ddlLoginType.SelectedIndexChanged += new System.EventHandler(this.ddlLoginType_SelectedIndexChanged);
            // 
            // textBoxInstituteCode
            // 
            this.textBoxInstituteCode.Animated = true;
            this.textBoxInstituteCode.AutoRoundedCorners = true;
            this.textBoxInstituteCode.BorderRadius = 17;
            this.textBoxInstituteCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxInstituteCode.DefaultText = "";
            this.textBoxInstituteCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxInstituteCode.Location = new System.Drawing.Point(100, 140);
            this.textBoxInstituteCode.MaxLength = 15;
            this.textBoxInstituteCode.Name = "textBoxInstituteCode";
            this.textBoxInstituteCode.PlaceholderText = "Institute Code";
            this.textBoxInstituteCode.SelectedText = "";
            this.textBoxInstituteCode.Size = new System.Drawing.Size(200, 36);
            this.textBoxInstituteCode.TabIndex = 2;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Animated = true;
            this.textBoxPassword.AutoRoundedCorners = true;
            this.textBoxPassword.BorderRadius = 17;
            this.textBoxPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxPassword.DefaultText = "";
            this.textBoxPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxPassword.Location = new System.Drawing.Point(100, 190);
            this.textBoxPassword.MaxLength = 50;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '●';
            this.textBoxPassword.PlaceholderText = "Password";
            this.textBoxPassword.SelectedText = "";
            this.textBoxPassword.Size = new System.Drawing.Size(200, 36);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // ddlMode
            // 
            this.ddlMode.AutoRoundedCorners = true;
            this.ddlMode.BackColor = System.Drawing.Color.Transparent;
            this.ddlMode.BorderRadius = 17;
            this.ddlMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlMode.FocusedColor = System.Drawing.Color.Empty;
            this.ddlMode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ddlMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.ddlMode.ItemHeight = 30;
            this.ddlMode.Items.AddRange(new object[] {
            "-- Select Mode --",
            "Registration",
            "Examination"});
            this.ddlMode.Location = new System.Drawing.Point(100, 240);
            this.ddlMode.Name = "ddlMode";
            this.ddlMode.Size = new System.Drawing.Size(200, 36);
            this.ddlMode.TabIndex = 4;
            // 
            // ddlClass
            // 
            this.ddlClass.AutoRoundedCorners = true;
            this.ddlClass.BackColor = System.Drawing.Color.Transparent;
            this.ddlClass.BorderRadius = 17;
            this.ddlClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlClass.FocusedColor = System.Drawing.Color.Empty;
            this.ddlClass.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ddlClass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.ddlClass.ItemHeight = 30;
            this.ddlClass.Items.AddRange(new object[] {
            "-- Select Class --",
            "9th",
            "10th",
            "11th",
            "12th"});
            this.ddlClass.Location = new System.Drawing.Point(100, 290);
            this.ddlClass.Name = "ddlClass";
            this.ddlClass.Size = new System.Drawing.Size(200, 36);
            this.ddlClass.TabIndex = 5;
            // 
            // ddlSess
            // 
            this.ddlSess.AutoRoundedCorners = true;
            this.ddlSess.BackColor = System.Drawing.Color.Transparent;
            this.ddlSess.BorderRadius = 17;
            this.ddlSess.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlSess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSess.FocusedColor = System.Drawing.Color.Empty;
            this.ddlSess.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ddlSess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.ddlSess.ItemHeight = 30;
            this.ddlSess.Items.AddRange(new object[] {
            "-- Select Session --",
            "1st Annual",
            "2nd Annual"});
            this.ddlSess.Location = new System.Drawing.Point(100, 340);
            this.ddlSess.Name = "ddlSess";
            this.ddlSess.Size = new System.Drawing.Size(200, 36);
            this.ddlSess.TabIndex = 6;
            // 
            // btnLogin
            // 
            this.btnLogin.Animated = true;
            this.btnLogin.AutoRoundedCorners = true;
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BorderRadius = 15;
            this.btnLogin.FillColor = System.Drawing.Color.MediumSlateBlue;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.IndicateFocus = true;
            this.btnLogin.Location = new System.Drawing.Point(100, 400);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(200, 33);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseTransparentBackground = true;
            this.btnLogin.Click += new System.EventHandler(this.ButtonLogin_Click);
            // 
            // progressBarLogin
            // 
            this.progressBarLogin.AutoStart = true;
            this.progressBarLogin.BackColor = System.Drawing.Color.Transparent;
            this.progressBarLogin.Location = new System.Drawing.Point(180, 460);
            this.progressBarLogin.Name = "progressBarLogin";
            this.progressBarLogin.ProgressColor = System.Drawing.Color.MediumSlateBlue;
            this.progressBarLogin.Size = new System.Drawing.Size(40, 40);
            this.progressBarLogin.TabIndex = 7;
            this.progressBarLogin.Visible = false;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 520);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.ddlLoginType);
            this.Controls.Add(this.textBoxInstituteCode);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.ddlMode);
            this.Controls.Add(this.ddlClass);
            this.Controls.Add(this.ddlSess);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.progressBarLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BISEGRW | LOGIN";
            this.ResumeLayout(false);

        }

      
    }
}
