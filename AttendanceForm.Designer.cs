using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;

namespace Secugen_HU20
{
    partial class AttendanceForm
    {
        private System.ComponentModel.IContainer components = null;

        
        private Guna2ComboBox ddlExamGroup;
     
        private Guna2DataGridView dgvStudents;
       
       
        private Guna2TextBox textBoxLogs;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCentName;

        private Guna.UI2.WinForms.Guna2HtmlLabel lblExamDate;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatus;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttendanceForm));
            this.ddlExamGroup = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dgvStudents = new Guna.UI2.WinForms.Guna2DataGridView();
            this.lblExamDate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblStatus = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.textBoxLogs = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblCentName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.textBoxSearch1 = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();
            // 
            // ddlExamGroup
            // 
            this.ddlExamGroup.BackColor = System.Drawing.Color.Transparent;
            this.ddlExamGroup.BorderRadius = 8;
            this.ddlExamGroup.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlExamGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlExamGroup.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ddlExamGroup.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ddlExamGroup.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ddlExamGroup.ForeColor = System.Drawing.Color.Black;
            this.ddlExamGroup.ItemHeight = 30;
            this.ddlExamGroup.Items.AddRange(new object[] {
            "--SELECT EXAM GROUP--",
            "MORNING",
            "EVENING"});
            this.ddlExamGroup.Location = new System.Drawing.Point(15, 40);
            this.ddlExamGroup.Margin = new System.Windows.Forms.Padding(2);
            this.ddlExamGroup.Name = "ddlExamGroup";
            this.ddlExamGroup.Size = new System.Drawing.Size(201, 36);
            this.ddlExamGroup.StartIndex = 0;
            this.ddlExamGroup.TabIndex = 1;
            this.ddlExamGroup.SelectedIndexChanged += new System.EventHandler(this.ddlExamGroup_SelectedIndexChanged);
            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvStudents.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStudents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStudents.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStudents.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStudents.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStudents.Location = new System.Drawing.Point(15, 129);
            this.dgvStudents.Margin = new System.Windows.Forms.Padding(2);
            this.dgvStudents.MultiSelect = false;
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.RowTemplate.Height = 40;
            this.dgvStudents.Size = new System.Drawing.Size(743, 503);
            this.dgvStudents.TabIndex = 5;
            this.dgvStudents.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStudents.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvStudents.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvStudents.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvStudents.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvStudents.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvStudents.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStudents.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvStudents.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvStudents.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStudents.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvStudents.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStudents.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvStudents.ThemeStyle.ReadOnly = true;
            this.dgvStudents.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStudents.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvStudents.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvStudents.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvStudents.ThemeStyle.RowsStyle.Height = 40;
            this.dgvStudents.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStudents.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // lblExamDate
            // 
            this.lblExamDate.BackColor = System.Drawing.Color.Transparent;
            this.lblExamDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblExamDate.Location = new System.Drawing.Point(240, 49);
            this.lblExamDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExamDate.Name = "lblExamDate";
            this.lblExamDate.Size = new System.Drawing.Size(3, 2);
            this.lblExamDate.TabIndex = 2;
            this.lblExamDate.Text = " ";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(12, 75);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(3, 2);
            this.lblStatus.TabIndex = 33;
            this.lblStatus.Text = null;
            // 
            // textBoxLogs
            // 
            this.textBoxLogs.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxLogs.DefaultText = "";
            this.textBoxLogs.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.textBoxLogs.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.textBoxLogs.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.textBoxLogs.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.textBoxLogs.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.textBoxLogs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxLogs.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.textBoxLogs.Location = new System.Drawing.Point(12, 638);
            this.textBoxLogs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxLogs.Multiline = true;
            this.textBoxLogs.Name = "textBoxLogs";
            this.textBoxLogs.PlaceholderText = "Logs...";
            this.textBoxLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLogs.SelectedText = "";
            this.textBoxLogs.Size = new System.Drawing.Size(746, 110);
            this.textBoxLogs.TabIndex = 34;
            // 
            // lblCentName
            // 
            this.lblCentName.BackColor = System.Drawing.Color.Transparent;
            this.lblCentName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCentName.Location = new System.Drawing.Point(13, 9);
            this.lblCentName.Name = "lblCentName";
            this.lblCentName.Size = new System.Drawing.Size(3, 2);
            this.lblCentName.TabIndex = 35;
            this.lblCentName.Text = null;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.guna2Button1.BorderRadius = 8;
            this.guna2Button1.FillColor = System.Drawing.Color.Purple;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(577, 83);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(184, 35);
            this.guna2Button1.TabIndex = 36;
            this.guna2Button1.Text = "Check Count";
            this.guna2Button1.Visible = false;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // textBoxSearch1
            // 
            this.textBoxSearch1.BorderRadius = 8;
            this.textBoxSearch1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxSearch1.DefaultText = "";
            this.textBoxSearch1.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.textBoxSearch1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.textBoxSearch1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.textBoxSearch1.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.textBoxSearch1.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.textBoxSearch1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxSearch1.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.textBoxSearch1.Location = new System.Drawing.Point(577, 40);
            this.textBoxSearch1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxSearch1.Name = "textBoxSearch1";
            this.textBoxSearch1.PlaceholderText = "Search...";
            this.textBoxSearch1.SelectedText = "";
            this.textBoxSearch1.Size = new System.Drawing.Size(184, 36);
            this.textBoxSearch1.TabIndex = 31;
            this.textBoxSearch1.TextChanged += new System.EventHandler(this.textBoxSearch1_TextChanged);
            // 
            // AttendanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 761);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.lblCentName);
            this.Controls.Add(this.textBoxLogs);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.textBoxSearch1);
            this.Controls.Add(this.ddlExamGroup);
            this.Controls.Add(this.lblExamDate);
            this.Controls.Add(this.dgvStudents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "AttendanceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BISEGRW | EXAMINATION ATTENDANCE";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Guna2Button guna2Button1;
        private Guna2TextBox textBoxSearch1;
    }
}
