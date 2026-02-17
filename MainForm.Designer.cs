using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace Secugen_HU20
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBoxLogs = new Guna.UI2.WinForms.Guna2TextBox();
            this.pictureBoxFinger = new Guna.UI2.WinForms.Guna2PictureBox();
            this.dataGridViewStudents = new Guna.UI2.WinForms.Guna2DataGridView();
            this.serialNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.labelFName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.labelBForm = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.labelFNIC = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.ButtonExit = new Guna.UI2.WinForms.Guna2Button();
            this.ButtonSaveTemplate = new Guna.UI2.WinForms.Guna2Button();
            this.buttonCapture = new Guna.UI2.WinForms.Guna2Button();
            this.btnVerify = new Guna.UI2.WinForms.Guna2Button();
            this.pictureBoxImage = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblFormno = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.textBoxSearch1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.ButtonCaptureFinger2 = new Guna.UI2.WinForms.Guna2Button();
            this.lblCount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.btnDelReq = new Guna.UI2.WinForms.Guna2Button();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFinger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStudents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxLogs
            // 
            this.textBoxLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLogs.BorderRadius = 8;
            this.textBoxLogs.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxLogs.DefaultText = "";
            this.textBoxLogs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxLogs.Location = new System.Drawing.Point(17, 623);
            this.textBoxLogs.Multiline = true;
            this.textBoxLogs.Name = "textBoxLogs";
            this.textBoxLogs.PlaceholderText = "Logs will appear here...";
            this.textBoxLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLogs.SelectedText = "";
            this.textBoxLogs.Size = new System.Drawing.Size(751, 110);
            this.textBoxLogs.TabIndex = 16;
            // 
            // pictureBoxFinger
            // 
            this.pictureBoxFinger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxFinger.BorderRadius = 12;
            this.pictureBoxFinger.ImageRotate = 0F;
            this.pictureBoxFinger.Location = new System.Drawing.Point(452, 345);
            this.pictureBoxFinger.Name = "pictureBoxFinger";
            this.pictureBoxFinger.Size = new System.Drawing.Size(316, 190);
            this.pictureBoxFinger.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFinger.TabIndex = 15;
            this.pictureBoxFinger.TabStop = false;
            // 
            // dataGridViewStudents
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridViewStudents.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewStudents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewStudents.ColumnHeadersHeight = 35;
            this.dataGridViewStudents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.serialNumberColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewStudents.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewStudents.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewStudents.Location = new System.Drawing.Point(17, 63);
            this.dataGridViewStudents.Name = "dataGridViewStudents";
            this.dataGridViewStudents.RowHeadersVisible = false;
            this.dataGridViewStudents.Size = new System.Drawing.Size(751, 276);
            this.dataGridViewStudents.TabIndex = 14;
            this.dataGridViewStudents.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewStudents.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dataGridViewStudents.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dataGridViewStudents.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dataGridViewStudents.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dataGridViewStudents.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewStudents.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewStudents.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dataGridViewStudents.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewStudents.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewStudents.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewStudents.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewStudents.ThemeStyle.HeaderStyle.Height = 35;
            this.dataGridViewStudents.ThemeStyle.ReadOnly = false;
            this.dataGridViewStudents.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewStudents.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewStudents.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewStudents.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dataGridViewStudents.ThemeStyle.RowsStyle.Height = 22;
            this.dataGridViewStudents.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dataGridViewStudents.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dataGridViewStudents.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewStudents_CellClick);
            
            // 
            // serialNumberColumn
            // 
            this.serialNumberColumn.HeaderText = "Sr. No";
            this.serialNumberColumn.Name = "serialNumberColumn";
            this.serialNumberColumn.ReadOnly = true;
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelName.BackColor = System.Drawing.Color.Transparent;
            this.labelName.Location = new System.Drawing.Point(14, 368);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(34, 15);
            this.labelName.TabIndex = 13;
            this.labelName.Text = "Name:";
            // 
            // labelFName
            // 
            this.labelFName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFName.BackColor = System.Drawing.Color.Transparent;
            this.labelFName.Location = new System.Drawing.Point(14, 391);
            this.labelFName.Name = "labelFName";
            this.labelFName.Size = new System.Drawing.Size(67, 15);
            this.labelFName.TabIndex = 12;
            this.labelFName.Text = "Father Name:";
            // 
            // labelBForm
            // 
            this.labelBForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelBForm.BackColor = System.Drawing.Color.Transparent;
            this.labelBForm.Location = new System.Drawing.Point(14, 417);
            this.labelBForm.Name = "labelBForm";
            this.labelBForm.Size = new System.Drawing.Size(39, 15);
            this.labelBForm.TabIndex = 11;
            this.labelBForm.Text = "B-Form:";
            // 
            // labelFNIC
            // 
            this.labelFNIC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFNIC.BackColor = System.Drawing.Color.Transparent;
            this.labelFNIC.Location = new System.Drawing.Point(14, 441);
            this.labelFNIC.Name = "labelFNIC";
            this.labelFNIC.Size = new System.Drawing.Size(30, 15);
            this.labelFNIC.TabIndex = 10;
            this.labelFNIC.Text = "FNIC:";
            // 
            // ButtonExit
            // 
            this.ButtonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonExit.BorderRadius = 8;
            this.ButtonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonExit.FillColor = System.Drawing.Color.DimGray;
            this.ButtonExit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonExit.ForeColor = System.Drawing.Color.White;
            this.ButtonExit.Location = new System.Drawing.Point(554, 541);
            this.ButtonExit.Name = "ButtonExit";
            this.ButtonExit.Size = new System.Drawing.Size(214, 76);
            this.ButtonExit.TabIndex = 9;
            this.ButtonExit.Text = "Exit";
            this.ButtonExit.Click += new System.EventHandler(this.ButtonExit_Click);
            // 
            // ButtonSaveTemplate
            // 
            this.ButtonSaveTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSaveTemplate.BorderRadius = 8;
            this.ButtonSaveTemplate.FillColor = System.Drawing.Color.SeaGreen;
            this.ButtonSaveTemplate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonSaveTemplate.ForeColor = System.Drawing.Color.White;
            this.ButtonSaveTemplate.Location = new System.Drawing.Point(218, 582);
            this.ButtonSaveTemplate.Name = "ButtonSaveTemplate";
            this.ButtonSaveTemplate.Size = new System.Drawing.Size(169, 35);
            this.ButtonSaveTemplate.TabIndex = 8;
            this.ButtonSaveTemplate.Text = "Save";
            this.ButtonSaveTemplate.Click += new System.EventHandler(this.ButtonSaveTemplate_Click);
            // 
            // buttonCapture
            // 
            this.buttonCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCapture.BorderRadius = 8;
            this.buttonCapture.FillColor = System.Drawing.Color.DodgerBlue;
            this.buttonCapture.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonCapture.ForeColor = System.Drawing.Color.White;
            this.buttonCapture.Location = new System.Drawing.Point(17, 582);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(195, 35);
            this.buttonCapture.TabIndex = 7;
            this.buttonCapture.Text = "Capture Thumb";
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVerify.BorderRadius = 8;
            this.btnVerify.FillColor = System.Drawing.Color.OrangeRed;
            this.btnVerify.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnVerify.ForeColor = System.Drawing.Color.White;
            this.btnVerify.Location = new System.Drawing.Point(393, 582);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(155, 35);
            this.btnVerify.TabIndex = 6;
            this.btnVerify.Text = "Verify";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxImage.BorderRadius = 12;
            this.pictureBoxImage.ImageRotate = 0F;
            this.pictureBoxImage.Location = new System.Drawing.Point(316, 345);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(130, 130);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 5;
            this.pictureBoxImage.TabStop = false;
            // 
            // lblFormno
            // 
            this.lblFormno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFormno.BackColor = System.Drawing.Color.Transparent;
            this.lblFormno.Location = new System.Drawing.Point(14, 465);
            this.lblFormno.Name = "lblFormno";
            this.lblFormno.Size = new System.Drawing.Size(46, 15);
            this.lblFormno.TabIndex = 4;
            this.lblFormno.Text = "Form No:";
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Location = new System.Drawing.Point(14, 489);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(74, 15);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Student Name:";
            // 
            // textBoxSearch1
            // 
            this.textBoxSearch1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearch1.BorderRadius = 8;
            this.textBoxSearch1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxSearch1.DefaultText = "";
            this.textBoxSearch1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxSearch1.Location = new System.Drawing.Point(584, 24);
            this.textBoxSearch1.Name = "textBoxSearch1";
            this.textBoxSearch1.PlaceholderText = "Search Form No....";
            this.textBoxSearch1.SelectedText = "";
            this.textBoxSearch1.Size = new System.Drawing.Size(184, 33);
            this.textBoxSearch1.TabIndex = 2;
            this.textBoxSearch1.TextChanged += new System.EventHandler(this.textBoxSearch1_TextChanged);
            // 
            // ButtonCaptureFinger2
            // 
            this.ButtonCaptureFinger2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCaptureFinger2.BorderRadius = 8;
            this.ButtonCaptureFinger2.FillColor = System.Drawing.Color.MediumPurple;
            this.ButtonCaptureFinger2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ButtonCaptureFinger2.ForeColor = System.Drawing.Color.White;
            this.ButtonCaptureFinger2.Location = new System.Drawing.Point(17, 541);
            this.ButtonCaptureFinger2.Name = "ButtonCaptureFinger2";
            this.ButtonCaptureFinger2.Size = new System.Drawing.Size(195, 35);
            this.ButtonCaptureFinger2.TabIndex = 0;
            this.ButtonCaptureFinger2.Text = "Capture Finger";
            this.ButtonCaptureFinger2.Visible = false;
            this.ButtonCaptureFinger2.Click += new System.EventHandler(this.ButtonCaptureFinger2_Click);
            // 
            // lblCount
            // 
            this.lblCount.BackColor = System.Drawing.Color.Transparent;
            this.lblCount.Location = new System.Drawing.Point(242, 33);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(3, 2);
            this.lblCount.TabIndex = 17;
            this.lblCount.Text = null;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.guna2Button1.BorderRadius = 8;
            this.guna2Button1.FillColor = System.Drawing.Color.MidnightBlue;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(218, 541);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(169, 35);
            this.guna2Button1.TabIndex = 18;
            this.guna2Button1.Text = "Check Count";
            this.guna2Button1.Visible = false;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2Button2
            // 
            this.guna2Button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.guna2Button2.BorderRadius = 8;
            this.guna2Button2.FillColor = System.Drawing.Color.Crimson;
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Location = new System.Drawing.Point(393, 541);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(155, 35);
            this.guna2Button2.TabIndex = 19;
            this.guna2Button2.Text = "Capture Verification";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // btnDelReq
            // 
            this.btnDelReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelReq.BorderRadius = 8;
            this.btnDelReq.FillColor = System.Drawing.Color.Red;
            this.btnDelReq.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDelReq.ForeColor = System.Drawing.Color.White;
            this.btnDelReq.Location = new System.Drawing.Point(316, 481);
            this.btnDelReq.Name = "btnDelReq";
            this.btnDelReq.Size = new System.Drawing.Size(130, 23);
            this.btnDelReq.TabIndex = 20;
            this.btnDelReq.Text = "Delete Request";
            this.btnDelReq.Visible = false;
            this.btnDelReq.Click += new System.EventHandler(this.btnDelReq_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.BorderRadius = 8;
            this.btnRefresh.FillColor = System.Drawing.Color.LightSeaGreen;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(316, 510);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(130, 23);
            this.btnRefresh.TabIndex = 21;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonCapture;
            this.CancelButton = this.ButtonExit;
            this.ClientSize = new System.Drawing.Size(784, 749);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelReq);
            this.Controls.Add(this.guna2Button2);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.ButtonCaptureFinger2);
            this.Controls.Add(this.textBoxSearch1);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblFormno);
            this.Controls.Add(this.pictureBoxImage);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.buttonCapture);
            this.Controls.Add(this.ButtonSaveTemplate);
            this.Controls.Add(this.ButtonExit);
            this.Controls.Add(this.labelFNIC);
            this.Controls.Add(this.labelBForm);
            this.Controls.Add(this.labelFName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.dataGridViewStudents);
            this.Controls.Add(this.pictureBoxFinger);
            this.Controls.Add(this.textBoxLogs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 766);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Board of Intermediate and Secondary Education Gujranwala Biometric";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFinger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStudents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Guna2TextBox textBoxLogs;
        private Guna2PictureBox pictureBoxFinger;
        private Guna2DataGridView dataGridViewStudents;
        private Guna2HtmlLabel labelName;
        private Guna2HtmlLabel labelFName;
        private Guna2HtmlLabel labelBForm;
        private Guna2HtmlLabel labelFNIC;
        private DataGridViewTextBoxColumn serialNumberColumn;
        private Guna2Button ButtonExit;
        private Guna2Button ButtonSaveTemplate;
        private Guna2Button buttonCapture;
        private Guna2Button btnVerify;
        private Guna2PictureBox pictureBoxImage;
        private Guna2HtmlLabel lblFormno;
        private Guna2HtmlLabel lblName;
        private Guna2TextBox textBoxSearch1;
        private Guna2Button ButtonCaptureFinger2;
        private Guna2HtmlLabel lblCount;
        private Guna2Button guna2Button1;
        private Guna2Button guna2Button2;
        private Guna2Button btnDelReq;
        private Guna2Button btnRefresh;
    }
}
