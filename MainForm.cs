using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SecuGen.FDxSDKPro.Windows;


namespace Secugen_HU20
{
    public partial class MainForm : Form
    {
        private SGFingerPrintManager m_FPM;
        private int m_ImageWidth;
        private int m_ImageHeight;
        private byte[] f1 = null;
        private byte[] f2 = null;
        private int selectedStudentId = 0;
        private DataTable cleanedDataTable;
        private Timer fingerprintTimer;


        public MainForm()
        {
            InitializeComponent();
            InitilizingDevice();
            NetworkHelper.ShowInternetWarningIfOffline();
            LoadStudentData();
            dataGridViewStudents.DataBindingComplete -= dataGridViewStudents_DataBindingComplete;
            dataGridViewStudents.DataBindingComplete += dataGridViewStudents_DataBindingComplete;

            dataGridViewStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewStudents.MultiSelect = false;
            dataGridViewStudents.DataBindingComplete += (s, e) =>
            {
                foreach (DataGridViewRow row in dataGridViewStudents.Rows)
                {
                    row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor;
                }
                dataGridViewStudents.ClearSelection(); // no row selected initially
            };
        }

        private void dataGridViewStudents_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (UserSession.Mode == "Examination")
            {
                ApplyRowStyling();

            }

            dataGridViewStudents.ClearSelection(); // no row selected on load
        }
        private void InitilizingDevice()
        {

            try
            {
                SGFPMDeviceName device_name = SGFPMDeviceName.DEV_AUTO;
                m_FPM = new SGFingerPrintManager();

                LogMessage("Initializing fingerprint device...");

                int port_addr = (int)SGFPMPortAddr.USB_AUTO_DETECT;


                // Initialize the device
                int iError = m_FPM.Init(device_name);
                if (iError == (int)SGFPMError.ERROR_NONE)
                    LogMessage("Initialization Success.");
                else
                {
                    var error = ErrorHelper.GetError(iError);
                    LogMessage($"Initialization Error: {error.Code} - {error.Message}");
                    return;
                }

                // Open the device
                iError = m_FPM.OpenDevice(port_addr);
                if (iError == (int)SGFPMError.ERROR_NONE)
                    LogMessage("Device Opened Successfully.");
                else
                {
                    var error = ErrorHelper.GetError(iError);
                    LogMessage($"Initialization Error: {error.Code} - {error.Message}");
                    return;
                }

                // Get device info
                SGFPMDeviceInfoParam pInfo = new SGFPMDeviceInfoParam();
                iError = m_FPM.GetDeviceInfo(pInfo);

                if (iError == (int)SGFPMError.ERROR_NONE)
                {
                    m_ImageWidth = pInfo.ImageWidth;
                    m_ImageHeight = pInfo.ImageHeight;
                }
                else
                {
                    var error = ErrorHelper.GetError(iError);
                    LogMessage($"Initialization Error: {error.Code} - {error.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                LogMessage("❌ InitBiometricDevice error: " + ex.Message);
            }
        }
        private byte[] CaptureAndDisplayImage()
        {
            // 🔧 FIX: Capture and create template with dynamic size & trimming
            byte[] img = new byte[m_ImageWidth * m_ImageHeight];
            int err = m_FPM.GetImage(img);
            if (err != (int)SGFPMError.ERROR_NONE)
            {
                LogMessage("❌ Finger capture failed. Error code: " + err);
                return null;
            }

            // ✅ get device max template size dynamically
            int maxTemplateSize = 0;
            m_FPM.GetMaxTemplateSize(ref maxTemplateSize);
            byte[] rawTemplate = new byte[maxTemplateSize];

            err = m_FPM.CreateTemplate(img, rawTemplate);
            if (err != (int)SGFPMError.ERROR_NONE)
            {
                LogMessage("❌ Template creation failed: " + err);
                return null;
            }

            // ✅ trim trailing zeros
            int actualSize = rawTemplate.Length;
            for (int i = rawTemplate.Length - 1; i >= 0; i--)
            {
                if (rawTemplate[i] != 0)
                {
                    actualSize = i + 1;
                    break;
                }
            }
            byte[] finalTemplate = new byte[actualSize];
            Array.Copy(rawTemplate, finalTemplate, actualSize);

            // ✅ show preview
            pictureBoxFinger.Image = ConvertToBitmap(img, m_ImageWidth, m_ImageHeight);
            LogMessage($"✅ Template captured. Size={finalTemplate.Length}");
            return finalTemplate;

        }

        // Method to load student data into the DataGridView
        private void LoadStudentData()
        {
            int ryear = ConfigurationManager.GetYear(UserSession.Class, UserSession.Sess);
            string query = string.Empty;

            // Name string get karna
            string fullName = UserSession.Name?.ToString() ?? "User Name";

            // 56 characters tak cut karna
            string displayName = fullName.Length > 60
                ? fullName.Substring(0, 60) + "..."
                : fullName;

            // Label settings
            lblName.Location = new Point(10, 10);
            lblName.MaximumSize = new Size(800, 0);
            lblName.TextAlignment = ContentAlignment.TopLeft;
            lblName.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblName.ForeColor = Color.MediumSlateBlue;
            lblName.Text = displayName;

            if (UserSession.LoginType == "Supervisory Staff")
            {
                if (UserSession.Mode == "Registration")
                {
                    int ExamName = 0;
                    int ExamType = 0;

                    if (UserSession.Class == 9 || UserSession.Class == 10)
                    {
                        ExamName = 1;
                    }
                    else if (UserSession.Class == 11 || UserSession.Class == 12)
                    {
                        ExamName = 3;
                    }

                    if (UserSession.Sess == 1)
                    {
                        ExamType = 0;
                    }
                    else if (UserSession.Sess == 2)
                    {
                        ExamType = 1;
                    }

                    query = @"
                    SELECT TeacherId formNo, BoardId stdid, FirstName name, FatherName fname, CNIC bform, CNIC fnic
                    FROM BiseDutyAssignment..View_TeacherDuties 
                    WHERE centrecode = @InstituteCode and year = 2023 and examtype = " + ExamType + " and examname = " + ExamName + " and isactiveduty =1 and isdeleted = 0";
                }
                else if (UserSession.Mode == "Examination")
                {
                    query = @"
                        SELECT e.formNo, e.stdid, e.name, e.fname, e.bform, e.fnic,
                        (SELECT COUNT(*) 
                        FROM Onlineopr..tbl_Fingerprint f 
                        WHERE f.class = e.class 
                        AND f.sess = e.sess 
                        AND f.inst_cd = e.Inst_cd 
                        AND f.stdid = e.stdID) AS isfinger,
                        STUFF((
                        SELECT ',' + CAST(FingerNo AS VARCHAR)
                        FROM Onlineopr..tbl_Fingerprint f
                        WHERE f.class = e.class 
                        AND f.sess = e.sess 
                        AND f.inst_cd = e.Inst_cd 
                        AND f.stdid = e.stdID
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''
                        ) AS enrolledFingers,
                        CASE 
                        WHEN EXISTS (
                        SELECT 1 
                        FROM OnlineOpr..DeleteRequestsBiometric d
                        WHERE d.StdID = e.StdID 
                        AND d.class = e.class 
                        AND d.sess = e.sess
                        AND CONVERT(date, d.RequestDate) = CONVERT(date, GETDATE())
                        ) THEN 1
                        ELSE 0
                        END AS HasDeleteRequest
                        FROM LiveDB..ExaminationCenterStudents e
                        WHERE e.iyear = @Ryear
                        AND e.sess = @sess
                        AND e.class = @class
                        AND e.inst_cd = @InstituteCode";
                }
            }

            else
            {
                if (UserSession.Mode == "Registration")
                {
                    // ---------------- Registration Mode ----------------
                    if (UserSession.Class == 9)
                    {
                        query = @"
                            SELECT formNo, stdid, name, fname, bform, fnic
                            FROM Onlineopr..vw9threg 
                            WHERE ryear = @Ryear 
                            AND inst_cd = @InstituteCode 
                            AND regpvt = 1
                            AND batch_id IS NOT NULL 
                            AND (isDeleted IS NULL OR isDeleted = 0) 
                            AND isBiometric IS NULL";
                    }
                    else if (UserSession.Class == 11)
                    {
                        query = @"
                            SELECT formNo, stdid, name, fname, bformNo as bform, fnic
                            FROM Onlineopr..vw11threg 
                            WHERE ryear = @Ryear 
                            AND inst_cd = @InstituteCode 
                            AND regpvt = 1
                            AND batch_id IS NOT NULL 
                            AND (isDeleted IS NULL OR isDeleted = 0) 
                            AND isBiometric IS NULL";
                    }
                }

                else if (UserSession.Mode == "Examination")
                {
                    // ---------------- Examination Mode ----------------

                    ButtonCaptureFinger2.Visible = true;
                    guna2Button1.Visible = true;
                    btnDelReq.Visible = true;

                    query = @"
                        SELECT e.formNo, e.stdid, e.name, e.fname, e.bform, e.fnic,
                        (SELECT COUNT(*) 
                        FROM Onlineopr..tbl_Fingerprint f 
                        WHERE f.class = e.class 
                        AND f.sess = e.sess 
                        AND f.inst_cd = e.Inst_cd 
                        AND f.stdid = e.stdID) AS isfinger,
                        STUFF((
                        SELECT ',' + CAST(FingerNo AS VARCHAR)
                        FROM Onlineopr..tbl_Fingerprint f
                        WHERE f.class = e.class 
                        AND f.sess = e.sess 
                        AND f.inst_cd = e.Inst_cd 
                        AND f.stdid = e.stdID
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''
                        ) AS enrolledFingers,
                        CASE 
                        WHEN EXISTS (
                        SELECT 1 
                        FROM OnlineOpr..DeleteRequestsBiometric d
                        WHERE d.StdID = e.StdID 
                        AND d.class = e.class 
                        AND d.sess = e.sess
                        AND CONVERT(date, d.RequestDate) = CONVERT(date, GETDATE())
                        ) THEN 1
                        ELSE 0
                        END AS HasDeleteRequest
                        FROM LiveDB..ExaminationCenterStudents e
                        WHERE e.iyear = @Ryear
                        AND e.sess = @sess
                        AND e.class = @class
                        AND e.inst_cd = @InstituteCode";
                }
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                LogMessage("No valid query found for current Mode/Class.");
                return;
            }

            SqlParameter[] parameters =
                {
                new SqlParameter("@Ryear", ryear),
                new SqlParameter("@sess", UserSession.Sess),
                new SqlParameter("@class", UserSession.Class),
                new SqlParameter("@InstituteCode", UserSession.InstituteCode)
                };

            DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            cleanedDataTable = new DataTable();
            cleanedDataTable.Columns.Add("srno", typeof(int));
            cleanedDataTable.Columns.Add("formNo", typeof(string));
            cleanedDataTable.Columns.Add("stdid", typeof(string));
            cleanedDataTable.Columns.Add("name", typeof(string));
            cleanedDataTable.Columns.Add("fname", typeof(string));
            cleanedDataTable.Columns.Add("bform", typeof(string));
            cleanedDataTable.Columns.Add("fnic", typeof(string));

            if (UserSession.Mode == "Examination")
            {
                cleanedDataTable.Columns.Add("isfinger", typeof(int));
                cleanedDataTable.Columns.Add("enrolledFingers", typeof(string));
                cleanedDataTable.Columns.Add("HasDeleteRequest", typeof(bool));
            }


            int serialNumber = 1;
            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = cleanedDataTable.NewRow();
                newRow["srno"] = serialNumber++;
                newRow["formNo"] = row["formNo"];
                newRow["stdid"] = row["stdid"];
                newRow["name"] = row["name"];
                newRow["fname"] = row["fname"];
                newRow["bform"] = row["bform"];
                newRow["fnic"] = row["fnic"];

                if (UserSession.Mode == "Examination")
                {
                    newRow["isfinger"] = row["isfinger"];
                    newRow["enrolledFingers"] = row["enrolledFingers"];

                    // ✅ Add HasDeleteRequest column from query
                    bool hasDeleteRequest = false;
                    if (dataTable.Columns.Contains("HasDeleteRequest") && row["HasDeleteRequest"] != DBNull.Value)
                    {
                        hasDeleteRequest = Convert.ToBoolean(row["HasDeleteRequest"]);
                    }

                    newRow["HasDeleteRequest"] = hasDeleteRequest;
                }


                cleanedDataTable.Rows.Add(newRow);
            }

            dataGridViewStudents.AutoGenerateColumns = false;
            dataGridViewStudents.DataSource = cleanedDataTable;
            dataGridViewStudents.Columns.Clear();

            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "srno", HeaderText = "Serial No", DataPropertyName = "srno", ReadOnly = true });
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "formNo", HeaderText = "Form No", DataPropertyName = "formNo", ReadOnly = true });
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "stdid", HeaderText = "ID", DataPropertyName = "stdid", ReadOnly = true });
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "name", HeaderText = "Name", DataPropertyName = "name", ReadOnly = true });
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "fname", HeaderText = "Father's Name", DataPropertyName = "fname", ReadOnly = true });
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "bform", HeaderText = "B-Form", DataPropertyName = "bform", ReadOnly = true });
            dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "fnic", HeaderText = "FNIC", DataPropertyName = "fnic", ReadOnly = true });


            // ---------------- Styling ----------------
            dataGridViewStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewStudents.EnableHeadersVisualStyles = false;
            dataGridViewStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 220, 220);
            dataGridViewStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridViewStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dataGridViewStudents.RowTemplate.Height = 28;
            dataGridViewStudents.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dataGridViewStudents.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dataGridViewStudents.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridViewStudents.GridColor = Color.LightGray;
            dataGridViewStudents.BackgroundColor = Color.White;

            if (UserSession.Mode == "Examination")
            {
                dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "isfinger",
                    DataPropertyName = "isfinger",
                    Visible = false
                });

                dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "enrolledFingers",
                    DataPropertyName = "enrolledFingers",
                    Visible = false   // hide if you don’t want to show raw data
                });

                dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "isfinger", DataPropertyName = "isfinger", Visible = false });
                dataGridViewStudents.Columns.Add(new DataGridViewTextBoxColumn { Name = "enrolledFingers", DataPropertyName = "enrolledFingers", Visible = false });
            }

            dataGridViewStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 👉 Apply highlighting after data is bound
            if (UserSession.Mode == "Examination")
            {
                ApplyRowStyling();
            }

        }

        private DataTable LoadSingleInstituteStats()
        {
            try
            {
                string query = @"
            SELECT 
                COUNT(DISTINCT ecs.StdID) AS TotalCandidates,
                COUNT(DISTINCT f.StdID) AS EnrolledCandidates,
                COUNT(DISTINCT ecs.StdID) - COUNT(DISTINCT f.StdID) AS Remaining,
                CAST(
                    (COUNT(DISTINCT f.StdID) * 100.0) / NULLIF(COUNT(DISTINCT ecs.StdID), 0) 
                    AS DECIMAL(5,2)
                ) AS EnrolledPercentage
            FROM LiveDB..ExaminationCenterStudents ecs
            LEFT JOIN Onlineopr..tbl_Fingerprint f
                ON ecs.StdID = f.StdID
                AND f.iyear = @Year
                AND f.sess = @Sess
                AND f.class = @Class
            WHERE ecs.Inst_cd = @InstCode;
        ";

                SqlParameter[] parameters = {
                new SqlParameter("@InstCode", UserSession.InstituteCode),
                new SqlParameter("@Year",UserSession.Year ),
                new SqlParameter("@Sess",UserSession.Sess ),
                new SqlParameter("@Class",UserSession.Class ),
                };

                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                // Update label
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    int total = Convert.ToInt32(row["TotalCandidates"]);
                    int enrolled = Convert.ToInt32(row["EnrolledCandidates"]);
                    int remaining = Convert.ToInt32(row["Remaining"]);
                    decimal percent = Convert.ToDecimal(row["EnrolledPercentage"]);

                    lblCount.MaximumSize = new Size(800, 0);
                    lblCount.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    lblCount.ForeColor = Color.DarkGreen;
                    lblCount.Text = $"Enrolled: {enrolled}/{total} ({percent}%), Remaining: {remaining}";
                }

                return dataTable;
            }
            catch (NotImplementedException nie)
            {
                MessageBox.Show("DatabaseHelper.ExecuteQuery is not implemented yet.\n" + nie.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading institute stats:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        private void ApplyRowStyling()
        {
            if (dataGridViewStudents.Rows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewStudents.Rows)
            {
                if (row.Cells["isfinger"].Value == null) continue;

                int fingerCount = 0;
                int.TryParse(row.Cells["isfinger"].Value.ToString(), out fingerCount);

                // default
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.DefaultCellStyle.BackColor = Color.White; // default light background

                if (fingerCount <= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 102, 102); // medium red
                    row.DefaultCellStyle.ForeColor = Color.White;

                }
                else if (fingerCount == 1)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 102); // medium yellow/orange
                    row.DefaultCellStyle.ForeColor = Color.Black;

                }
                else // 2+
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200); // medium green
                    row.DefaultCellStyle.ForeColor = Color.Black;

                }
            }
        }
        // Method to save the captured fingerprint template to the database
        private void SaveTemplateToDatabase(byte[] template, int FingerNo)
        {
            // Validate that a student is selected
            if (selectedStudentId == 0)
            {
                MessageBox.Show("No student selected. Please select a student first.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMessage("No student selected. Please select a student first.");
                return;
            }

            // Validate that the template is not null or empty
            if (template == null || template.Length == 0)
            {
                MessageBox.Show("Fingerprint template is empty. Please scan a valid fingerprint.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMessage("Fingerprint template is empty. Please scan a valid fingerprint.");
                return;
            }

            // Check if the fingerprint is already enrolled (duplicate across candidates)
            if (IsFingerprintDuplicate(template, selectedStudentId, FingerNo))
            {
                MessageBox.Show("This fingerprint is already enrolled for another student.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMessage("This fingerprint is already enrolled for another student.");
                return;
            }

            // Get the size of the template in bytes
            int templateSize = template.Length;

            // Optionally check for the maximum template size (this is an example limit, adjust as needed)
            const int MaxTemplateSize = 1024; // Set a max template size if required (in bytes)
            if (templateSize > MaxTemplateSize)
            {
                MessageBox.Show("Fingerprint template size exceeds the maximum allowed size.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogMessage("Fingerprint template size exceeds the maximum allowed size.");
                return;
            }

            // Now save the template along with its quality and size
            try
            {
                int type = 1;
                if (UserSession.LoginType == "Supervisory Staff")
                {
                    type = 2;
                }

                // Define the SQL query with the necessary columns
                string query = "INSERT INTO tbl_Fingerprint (stdid, FingerprintTemplate, TemplateSize, EnrollQuality,class,iyear,sess,inst_cd,isActive,FingerNo,type) " +
                           "VALUES (@UserID, @FingerprintTemplate, @TemplateSize, @EnrollQuality,@class,@iyear,@sess,@inst_cd,1,@FingerNo,@type)";

                // Define the parameters to prevent SQL injection
                SqlParameter[] parameters = {
            new SqlParameter("@UserID", selectedStudentId),
            new SqlParameter("@FingerprintTemplate", template),
            new SqlParameter("@TemplateSize", templateSize),
            new SqlParameter("@EnrollQuality", 100),
            new SqlParameter("@class", UserSession.Class),
            new SqlParameter("@iyear", UserSession.Year),
            new SqlParameter("@sess", UserSession.Sess),
            new SqlParameter("@inst_cd", UserSession.InstituteCode),
            new SqlParameter("@FingerNo", FingerNo),
            new SqlParameter("@type", type),
        };

                // Execute the query using DatabaseHelper (which you must have defined)
                int result = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (result > 0)
                {
                    LogMessage($"Success: {result} row(s) updated.");
                    if (UserSession.Mode == "Registration" && UserSession.LoginType == "Institute")
                    {
                        UpdateBiometricStatus(selectedStudentId);
                        LoadStudentData();
                        ResetUI();
                    }

                    if (UserSession.Mode == "Examination")
                    {
                        UpdateRowCacheAndUI(selectedStudentId, FingerNo);

                    }

                    LogMessage("Fingerprint template saved successfully.");
                }
                else
                {
                    LogMessage("No rows were updated.");
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle database-related errors (e.g., connection issues, etc.)
                LogMessage("Database error while saving the fingerprint template: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // Handle other general errors
                LogMessage("Error saving to database: " + ex.Message);
            }
        }
        private void UpdateRowCacheAndUI(int stdid, int savedFingerNo)
        {
            // 1) DataTable row update (isfinger + enrolledFingers)
            DataRow[] rows = cleanedDataTable.Select($"stdid = '{stdid}'");
            if (rows.Length > 0)
            {
                var dr = rows[0];

                // enrolledFingers update
                string ef = dr["enrolledFingers"]?.ToString() ?? "";
                var list = new HashSet<string>((ef ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                list.Add(savedFingerNo.ToString());
                dr["enrolledFingers"] = string.Join(",", list);

                // isfinger = COUNT (list.Count)
                dr["isfinger"] = list.Count;

                // ✅ Sync UI grid cells (if selected row visible)
                if (dataGridViewStudents.SelectedRows.Count > 0)
                {
                    var sel = dataGridViewStudents.SelectedRows[0];
                    sel.Cells["enrolledFingers"].Value = dr["enrolledFingers"];
                    sel.Cells["isfinger"].Value = dr["isfinger"];
                }

                // ✅ Immediately disable captured finger button
                if (savedFingerNo == 1)
                    buttonCapture.Enabled = false;
                else if (savedFingerNo == 2)
                    ButtonCaptureFinger2.Enabled = false;

            }

            // 2) Row re-color
            ApplyRowStyling();

            // 3) Buttons disable (selected row pe based)
            if (dataGridViewStudents.SelectedRows.Count > 0)
            {
                var sel = dataGridViewStudents.SelectedRows[0];
                string enrolledFingers = sel.Cells["enrolledFingers"].Value?.ToString() ?? "";
                buttonCapture.Enabled = !enrolledFingers.Split(',').Contains("1");
                ButtonCaptureFinger2.Enabled = !enrolledFingers.Split(',').Contains("2");
            }
        }
        // Method to update biometric status of a student
        private void UpdateBiometricStatus(int studentId)
        {
            string updateQuery = string.Empty;

            if (UserSession.Class == 9)
            {
                updateQuery = "UPDATE tblRegBioData SET isBiometric = 1 WHERE stdid = @stdid";
            }

            else if (UserSession.Class == 11)
            {
                updateQuery = "UPDATE tblRegBioDataInter SET isBiometric = 1 WHERE stdid = @stdid";
            }

            SqlParameter[] parameters = {
                new SqlParameter("@stdid", studentId)
            };

            DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
        }
        // Event handler when a student is selected in the DataGridView
        private void DataGridViewStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //ResetUI();

            if (e.RowIndex >= 0)
            {
                dataGridViewStudents.Rows[e.RowIndex].Selected = true;
            }
            else
            {
                return;
            }

            if (e.ColumnIndex >= 0)
            {
                dataGridViewStudents.CurrentCell = dataGridViewStudents.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
            else
            {
                return;
            }

            var row = dataGridViewStudents.Rows[e.RowIndex];

            // Student info
            selectedStudentId = Convert.ToInt32(row.Cells["stdid"].Value);
            lblFormno.Text = $"Form No: {row.Cells["formNo"].Value}";
            labelName.Text = $"Name: {row.Cells["name"].Value}";
            labelFName.Text = $"Father's Name: {row.Cells["fname"].Value}";
            labelBForm.Text = $"B-Form: {row.Cells["bform"].Value}";
            labelFNIC.Text = $"FNIC: {row.Cells["fnic"].Value}";


            // ✅ Reset only fingerprints, not full UI
            f1 = null;
            f2 = null;
            pictureBoxFinger.Image = null;

            // ✅ Bold font
            Font boldFont = new Font("Segoe UI", 8, FontStyle.Bold);
            labelName.Font = labelFName.Font = labelBForm.Font = labelFNIC.Font = lblFormno.Font = boldFont;

            // ✅ Enable/disable capture buttons based on enrolled fingers


            if (UserSession.Mode == "Examination")
            {
                string enrolledFingers = row.Cells["enrolledFingers"].Value?.ToString() ?? "";
                buttonCapture.Enabled = !enrolledFingers.Contains("1");
                ButtonCaptureFinger2.Enabled = !enrolledFingers.Contains("2");

                // ✅ Delete button: enable only if at least one finger is enrolled
                int fingerCount = 0;
                int.TryParse(row.Cells["isfinger"].Value?.ToString(), out fingerCount);
                btnDelReq.Enabled = fingerCount > 0; // only enable if enrolled
            }
            else
            {
                buttonCapture.Enabled = true;
                ButtonCaptureFinger2.Enabled = true;
                btnDelReq.Enabled = true;
            }



            // ✅ Load image
            try
            {
                string baseUrl = string.Empty;
                string imageUrl = string.Empty;

                if (UserSession.LoginType == "Supervisory Staff")
                {
                    string bFormVal = row.Cells["bform"].Value.ToString();

                    string apiUrl =
                        $"https://services.bisegrw.edu.pk/android/Biometrics/getStaffProfileImage.php?bform={bFormVal}";

                    try
                    {
                        using (var wc = new WebClient())
                        {
                            wc.Encoding = Encoding.UTF8;
                            string json = wc.DownloadString(apiUrl);

                            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var obj = (Dictionary<string, object>)serializer.DeserializeObject(json);

                            string imageName = null;
                            if (obj != null && obj.ContainsKey("image"))
                            {
                                imageName = obj["image"]?.ToString();
                            }

                            imageUrl = null;

                            if (!string.IsNullOrEmpty(imageName))
                            {
                                imageUrl = $"https://services.bisegrw.edu.pk/Share%20Images/uploads/Marking/{bFormVal}/profile/{imageName}";
                            }
                            else
                            {
                                // fallback URL if API returns null
                                imageUrl = "https://services.bisegrw.edu.pk/Share%20Images/uploads/Marking/noimage.jpg";
                            }

                            pictureBoxImage.LoadCompleted -= PictureBoxImage_LoadCompleted;
                            pictureBoxImage.LoadCompleted += PictureBoxImage_LoadCompleted;

                            pictureBoxImage.Size = new Size(130, 130);
                            pictureBoxImage.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBoxImage.LoadAsync(imageUrl);
                        }
                    }
                    catch (Exception ex)
                    {
                        // If even API call fails, load default image
                        pictureBoxImage.LoadCompleted -= PictureBoxImage_LoadCompleted;
                        pictureBoxImage.LoadCompleted += PictureBoxImage_LoadCompleted;

                        pictureBoxImage.Size = new Size(130, 130);
                        pictureBoxImage.SizeMode = PictureBoxSizeMode.StretchImage;

                        pictureBoxImage.LoadAsync("https://services.bisegrw.edu.pk/Share%20Images/uploads/Marking/noimage.jpg");
                        // Optional: log or show error
                        // MessageBox.Show($"Error loading Supervisory Staff image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    if (UserSession.Class == 9)
                        baseUrl = "https://services.bisegrw.edu.pk/Share%20Images/uploads/SSC/";
                    else if (UserSession.Class == 10)
                        baseUrl = "https://services.bisegrw.edu.pk/Share%20Images/OldPics/Pics-M/";
                    else if (UserSession.Class == 11)
                        baseUrl = "https://services.bisegrw.edu.pk/Share%20Images/uploads/HSSC/registration/";
                    else if (UserSession.Class == 12)
                        baseUrl = "https://services.bisegrw.edu.pk/Share%20Images/OldPics/Pics-I/";

                    int divider = (int)Math.Ceiling((double)selectedStudentId / 50000);
                    imageUrl = $"{baseUrl}{divider}/{selectedStudentId}.jpg";
                }


                // ✅ Ensure event not added multiple times
                pictureBoxImage.LoadCompleted -= PictureBoxImage_LoadCompleted;
                pictureBoxImage.LoadCompleted += PictureBoxImage_LoadCompleted;

                pictureBoxImage.Size = new Size(130, 130);
                pictureBoxImage.SizeMode = PictureBoxSizeMode.StretchImage;

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    pictureBoxImage.LoadAsync(imageUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ✅ Backup image loader (only once attached)
        private void PictureBoxImage_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null) // Primary failed, try backup
            {
                try
                {
                    string backupImageUrl = null;

                    if (UserSession.LoginType == "Supervisory Staff")
                    {
                        // Supervisory Staff backup - optional default image
                        backupImageUrl = "https://services.bisegrw.edu.pk/Share%20Images/uploads/Marking/noimage.jpg";
                    }
                    else
                    {
                        // Students
                        string backupUrl = (UserSession.Class == 9 || UserSession.Class == 10)
                            ? "https://services.bisegrw.edu.pk/Share%20Images/OldPics/Pics-M/"
                            : "https://services.bisegrw.edu.pk/Share%20Images/OldPics/Pics-I/";

                        int divider = (int)Math.Ceiling((double)selectedStudentId / 50000);
                        backupImageUrl = $"{backupUrl}{divider}/{selectedStudentId}.jpg";
                    }

                    if (!string.IsNullOrEmpty(backupImageUrl))
                    {
                        pictureBoxImage.LoadAsync(backupImageUrl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading backup image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Save Fingerprint Template
        private void ButtonSaveTemplate_Click(object sender, EventArgs e)
        {
            if (selectedStudentId == 0)
            {
                MessageBox.Show("No student selected. Please select a student first.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //string enrolledFingers = dataGridViewStudents.SelectedRows[0].Cells["enrolledFingers"].Value?.ToString() ?? "";

            DataRow[] rows = cleanedDataTable.Select($"stdid = '{selectedStudentId}'");
            string enrolledFingers = "";
            bool isExamination = (UserSession.Mode == "Examination");

            if (isExamination && rows.Length > 0)
            {
                enrolledFingers = rows[0]["enrolledFingers"]?.ToString() ?? "";
            }

            bool saved = false;

            // Save Finger 1 only if captured and not already in DB
            if (f1 != null && (!isExamination || !enrolledFingers.Contains("1")))
            {
                SaveTemplateToDatabase(f1, 1);
                saved = true;
            }

            // Save Finger 2 only if captured and not already in DB
            if (f2 != null && (!isExamination || !enrolledFingers.Contains("2")))
            {
                SaveTemplateToDatabase(f2, 2);
                saved = true;
            }

            if (!saved)
            {
                MessageBox.Show("No new fingerprint to save. Already enrolled or not captured.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogMessage("No new fingerprint to save.");
            }
        }
        private void LogMessage(string message)
        {
            textBoxLogs.AppendText($"{DateTime.Now:HH:mm:ss} → {message}{Environment.NewLine}");
            //textBoxLogs.AppendText(message + Environment.NewLine);
        }

        // ✅ ADD: Self check for corrupt templates


        private Bitmap ConvertToBitmap(byte[] imageData, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int colorValue = imageData[y * width + x];
                    bmp.SetPixel(x, y, Color.FromArgb(colorValue, colorValue, colorValue));
                }
            }
            return bmp;
        }
        private void buttonCapture_Click(object sender, EventArgs e)
        {
            f1 = CaptureAndDisplayImage();
        }
        private void ButtonCaptureFinger2_Click(object sender, EventArgs e)
        {
            f2 = CaptureAndDisplayImage();
        }
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void StopFingerprintListener()
        {
            if (fingerprintTimer != null)
            {
                fingerprintTimer.Stop();
                fingerprintTimer.Dispose();
                fingerprintTimer = null;
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // stop timer and close device
            StopFingerprintListener();

            try
            {
                if (m_FPM != null)
                {
                    try { m_FPM.CloseDevice(); } catch { }
                }
            }
            catch { }

            base.OnFormClosing(e);
        }
        // Method to reset UI components
        private void ResetUI()
        {
            // Clear fingerprint image in the picture box
            pictureBoxFinger.Image = null;
            pictureBoxImage.Image = null;

            // Reset the selected student ID
            selectedStudentId = 0;

            // Clear the labels
            labelName.Text = "Name:";
            labelFName.Text = "Father's Name:";
            labelBForm.Text = "B-Form:";
            labelFNIC.Text = "FNIC:";
            lblFormno.Text = "";
            // Clear the search bar
            textBoxSearch1.Text = string.Empty;

            // Clear the logs 
            textBoxLogs.Clear();


            // Reset captured fingerprint templates
            f1 = null;
            f2 = null;
        }
        private void btnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                if (f1 == null && f2 == null)
                {
                    LogMessage("Please capture at least one fingerprint first.");
                    return;
                }

                DataTable fingerprintData = GetInstituteFingerprints();
                bool isMatched = false;
                int matchedStudentId = -1;
                SGFPMSecurityLevel securityLevel = SGFPMSecurityLevel.HIGH;

                foreach (DataRow row in fingerprintData.Rows)
                {
                    byte[] storedTemplate = (byte[])row["FingerprintTemplate"];
                    bool matched = false;

                    // First finger check
                    if (f1 != null)
                    {
                        //m_FPM.MatchTemplate(f1, storedTemplate, securityLevel, ref matched);

                        int iError = m_FPM.MatchTemplate(f1, storedTemplate, securityLevel, ref matched);
                        if (iError != (int)SGFPMError.ERROR_NONE)
                        {
                            var error = ErrorHelper.GetError(iError);
                            LogMessage($"MatchTemplate Error: {error.Code} - {error.Message}");
                        }

                        if (matched)
                        {
                            isMatched = true;
                            matchedStudentId = Convert.ToInt32(row["stdid"]);
                            break;
                        }
                    }

                    // Second finger check (agar pehle se match ni mila to)
                    if (!isMatched && f2 != null)
                    {
                        int iError = m_FPM.MatchTemplate(f2, storedTemplate, securityLevel, ref matched);
                        if (iError != (int)SGFPMError.ERROR_NONE)
                        {
                            var error = ErrorHelper.GetError(iError);
                            LogMessage($"MatchTemplate Error: {error.Code} - {error.Message}");
                        }


                        if (matched)
                        {
                            isMatched = true;
                            matchedStudentId = Convert.ToInt32(row["stdid"]);
                            break;
                        }
                    }
                }

                if (isMatched)
                {
                    LogMessage($"Fingerprint matched! Student ID: {matchedStudentId}");

                    // -------- Fetch user details (same as your code) ----------
                    string biodataQuery = string.Empty;

                    if (UserSession.Class == 9 && UserSession.Mode == "Registration")
                    {
                        biodataQuery = @"SELECT formNo, stdid, name, fname, bform, fnic
                                 FROM Onlineopr..vw9threg 
                                 WHERE stdid = @StdId 
                                 AND ryear = @Ryear 
                                 AND inst_cd = @InstituteCode 
                                 AND batch_id IS NOT NULL 
                                 AND (isDeleted IS NULL OR isDeleted = 0)
                                 AND isBiometric = 1";
                    }
                    else if (UserSession.Class == 11 && UserSession.Mode == "Registration")
                    {
                        biodataQuery = @"SELECT formNo, stdid, name, fname, bformNo as bform, fnic
                                 FROM Onlineopr..vw11threg 
                                 WHERE stdid = @StdId
                                 AND ryear = @Ryear 
                                 AND inst_cd = @InstituteCode 
                                 AND batch_id IS NOT NULL 
                                 AND (isDeleted IS NULL OR isDeleted = 0)
                                 AND isBiometric = 1";
                    }
                    else if (UserSession.Mode == "Examination")
                    {
                        biodataQuery = @"SELECT top 1 formNo, stdid, name, fname, bform, fnic
                                 FROM LiveDB..ExaminationCenterStudents  
                                 WHERE stdid = @StdId
                                 AND iyear = @Ryear 
                                 AND inst_cd = @InstituteCode    
                                 AND sess= @sess";
                    }
                    if (UserSession.LoginType == "Supervisory Staff")
                    {

                        int ExamName = 0;
                        int ExamType = 0;

                        if (UserSession.Class == 9 || UserSession.Class == 10)
                        {
                            ExamName = 1;
                        }
                        else if (UserSession.Class == 11 || UserSession.Class == 12)
                        {
                            ExamName = 3;
                        }

                        if (UserSession.Sess == 1)
                        {
                            ExamType = 0;
                        }
                        else if (UserSession.Sess == 2)
                        {
                            ExamType = 1;
                        }

                        biodataQuery = @"SELECT top 1 TeacherId formNo, BoardId stdid, FirstName name, FatherName fname, CNIC bform, CNIC fnic
                    FROM BiseDutyAssignment..View_TeacherDuties 
                    WHERE centrecode = @InstituteCode and year = 2023 and examtype = " + ExamType + " and examname = " + ExamName + " and isactiveduty =1 and isdeleted = 0 and BoardId = @StdId";
                    }

                    SqlParameter[] biodataParameters = {
                new SqlParameter("@StdId", matchedStudentId),
                new SqlParameter("@Ryear", ConfigurationManager.GetYear(UserSession.Class, UserSession.Sess)),
                new SqlParameter("@InstituteCode", UserSession.InstituteCode),
                new SqlParameter("@sess", UserSession.Sess)
            };

                    DataTable userBiodata = DatabaseHelper.ExecuteQuery(biodataQuery, biodataParameters);

                    if (userBiodata.Rows.Count > 0)
                    {
                        var userRow = userBiodata.Rows[0];
                        string stdid = userRow["stdid"].ToString();
                        string formNo = userRow["formNo"].ToString();
                        string name = userRow["name"].ToString();
                        string fatherName = userRow["fname"].ToString();
                        string bForm = userRow["bform"].ToString();
                        string fnic = userRow["fnic"].ToString();

                        UserDetailsPopup popup = new UserDetailsPopup(formNo, stdid, name, fatherName, bForm, fnic);
                        popup.ShowDialog();
                    }
                    else
                    {
                        ResetUI();
                        LogMessage("No matching user found in the database.");
                    }
                }
                else
                {
                    ResetUI();
                    LogMessage("Fingerprint not matched with any record.");
                }
            }
            catch (Exception ex)
            {
                LogMessage("Verification error: " + ex.Message);
            }
        }
        private DataTable GetInstituteFingerprints()
        {
            string query = @"
            SELECT stdid, FingerprintTemplate,FingerNo 
            FROM Onlineopr..tbl_Fingerprint 
            WHERE inst_cd = @InstituteCode 
            AND class = @class 
            AND iyear = @iyear
            AND sess = @sess";

            SqlParameter[] parameters = {
            new SqlParameter("@InstituteCode", UserSession.InstituteCode),
            new SqlParameter("@class", UserSession.Class),
            new SqlParameter("@iyear", ConfigurationManager.GetYear(UserSession.Class,UserSession.Sess)),
            new SqlParameter("@sess", UserSession.Sess)
    };

            DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

            /*if (result.Rows.Count == 0)
            {
                LogMessage("No fingerprints found for the given institute, class, and year.");
            }*/

            return result;
        }
        private bool IsFingerprintDuplicate(byte[] newTemplate, int currentStudentId, int currentFingerNo)
        {
            DataTable fingerprintData = GetInstituteFingerprints();
            SGFPMSecurityLevel securityLevel = SGFPMSecurityLevel.HIGH;

            foreach (DataRow row in fingerprintData.Rows)
            {
                int studentId = Convert.ToInt32(row["stdid"]);
                int fingerNo = Convert.ToInt32(row["FingerNo"]);

                // 👉 Apne hi same finger ko skip kar do (update case)
                if (studentId == currentStudentId && fingerNo == currentFingerNo)
                    continue;

                byte[] storedTemplate = (byte[])row["FingerprintTemplate"];
                bool matched = false;

                //m_FPM.MatchTemplate(newTemplate, storedTemplate, securityLevel, ref matched);

                int iError = m_FPM.MatchTemplate(newTemplate, storedTemplate, securityLevel, ref matched);

                if (iError != (int)SGFPMError.ERROR_NONE)
                {
                    // 🔹 SDK error ko log karo with ErrorHelper
                    var error = ErrorHelper.GetError(iError);
                    LogMessage($"❌ MatchTemplate Error (stdid={studentId}): {error.Code} - {error.Message}");
                    continue; // skip this template and check next
                }

                if (matched)
                {
                    // ❌ Apne dusre finger ke saath match ho gaya
                    if (studentId == currentStudentId)
                        return true;

                    // ❌ Kisi aur student ke finger ke saath match ho gaya
                    if (studentId != currentStudentId)
                        return true;
                }
            }

            return false; // ✅ Bilkul unique hai
        }


        private void textBoxSearch1_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = textBoxSearch1.Text.Trim();

            if (string.IsNullOrEmpty(searchQuery))
            {
                dataGridViewStudents.DataSource = cleanedDataTable;
                dataGridViewStudents.ClearSelection();
                return;
            }

            // -------------------------
            // ✅ Examination Mode
            // -------------------------
            if (UserSession.Mode == "Examination")
            {

                if (!long.TryParse(searchQuery, out long numericValue))
                {
                    // Input is not numeric, so don't query yet
                    dataGridViewStudents.DataSource = null;
                    MessageBox.Show("Enter Form No. Only .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                bool foundInGrid = false;

                foreach (DataGridViewRow row in dataGridViewStudents.Rows)
                {
                    if (row.Cells["formNo"].Value != null &&
                        row.Cells["formNo"].Value.ToString().Trim() == searchQuery)
                    {
                        // Grid me match mil gaya
                        ApplyRowStyling();
                        SelectAndStyleRow(row);
                        foundInGrid = true;
                        break;
                    }
                }

                // ✅ Agar grid me nahi mila → DB query
                if (!foundInGrid && searchQuery.Length == 6)
                {
                    string sql = @"
                SELECT e.formNo, e.stdid, e.name, e.fname, e.bform, e.fnic,
                (SELECT COUNT(*) 
                FROM Onlineopr..tbl_Fingerprint f 
                WHERE f.class = e.class 
                AND f.sess = e.sess 
                AND f.stdid = e.stdID) AS isfinger,
                STUFF((
                SELECT ',' + CAST(FingerNo AS VARCHAR)
                FROM Onlineopr..tbl_Fingerprint f
                WHERE f.class = e.class 
                AND f.sess = e.sess 
                AND f.stdid = e.stdID
                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''
                ) AS enrolledFingers
                FROM LiveDB..ExaminationCenterStudents e
                WHERE e.iyear = @Ryear
                AND e.sess = @sess
                AND e.class = @class
                AND e.formNo = @formNo;";

                    SqlParameter[] parameters = {
                new SqlParameter("@Ryear", ConfigurationManager.GetYear(UserSession.Class, UserSession.Sess)),
                new SqlParameter("@sess", UserSession.Sess),
                new SqlParameter("@class", UserSession.Class),
                new SqlParameter("@formNo", searchQuery)
            };

                    DataTable result = DatabaseHelper.ExecuteQuery(sql, parameters);

                    if (result.Rows.Count > 0)
                    {
                        dataGridViewStudents.DataSource = result;
                        dataGridViewStudents.ClearSelection();

                        foreach (DataGridViewRow row in dataGridViewStudents.Rows)
                        {
                            if (row.Cells["formNo"].Value != null &&
                                row.Cells["formNo"].Value.ToString().Trim() == searchQuery)
                            {
                                ApplyRowStyling();
                                SelectAndStyleRow(row);
                                break;
                            }
                        }
                    }
                }

                else
                {
                    // ❌ Non-numeric input → Examination mode me ignore karo
                    return;
                }
            }

            // -------------------------
            // ✅ Registration Mode
            // -------------------------
            else if (UserSession.Mode == "Registration")
            {
                if (cleanedDataTable != null)
                {
                    string safeQuery = searchQuery.Replace("'", "''");

                    // Pehle exact formNo check
                    DataRow[] exactFormMatch = cleanedDataTable.Select(
                        $"Convert(formNo, 'System.String') = '{safeQuery}'");

                    if (exactFormMatch.Length == 1)
                    {
                        dataGridViewStudents.DataSource = cleanedDataTable;
                        dataGridViewStudents.ClearSelection();

                        foreach (DataGridViewRow row in dataGridViewStudents.Rows)
                        {
                            if (row.Cells["formNo"].Value != null &&
                                row.Cells["formNo"].Value.ToString().Trim() == searchQuery)
                            {
                                SelectAndStyleRow(row);
                                break;
                            }
                        }
                    }
                    else
                    {
                        DataView dv = new DataView(cleanedDataTable);
                        dv.RowFilter = string.Format(
                            "Convert(formNo, 'System.String') LIKE '%{0}%' OR " +
                            "Convert(stdid, 'System.String') LIKE '%{0}%' OR " +
                            "name LIKE '%{0}%' OR " +
                            "fname LIKE '%{0}%' OR " +
                            "bform LIKE '%{0}%' OR " +
                            "fnic LIKE '%{0}%'", safeQuery);

                        dataGridViewStudents.DataSource = dv;
                        dataGridViewStudents.ClearSelection();
                    }
                }
            }
        }


        private void SelectAndStyleRow(DataGridViewRow row)
        {
            dataGridViewStudents.ClearSelection();

            row.Selected = true;
            dataGridViewStudents.CurrentCell = row.Cells["formNo"];

            // ✅ Row styling
            row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor;
            row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor;

            // ✅ Call cell click for details load
            DataGridViewStudents_CellClick(
                dataGridViewStudents,
                new DataGridViewCellEventArgs(row.Cells["formNo"].ColumnIndex, row.Index)
            );
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            LoadSingleInstituteStats();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            f1 = CaptureAndDisplayImage();
        }

        private void btnDelReq_Click(object sender, EventArgs e)
        {
            if (dataGridViewStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a candidate first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Get selected row
            DataGridViewRow selectedRow = dataGridViewStudents.SelectedRows[0];
            string formNo = selectedRow.Cells["formNo"].Value?.ToString();
            string stdid = selectedRow.Cells["stdid"].Value?.ToString();
            string name = selectedRow.Cells["name"].Value?.ToString();
            string fname = selectedRow.Cells["fname"].Value?.ToString();

            if (string.IsNullOrEmpty(formNo) || string.IsNullOrEmpty(stdid))
            {
                MessageBox.Show("Invalid candidate data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Confirmation
            DialogResult dr = MessageBox.Show(
                $"Are you sure you want to send delete request for:\n\nForm No: {formNo}\nStdID: {stdid}\nName: {name}\nFather's Name: {fname}?",
                "Confirm Delete Request",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );


            if (dr == DialogResult.No)
                return;

            try
            {
                string sql = @"
                INSERT INTO OnlineOpr..DeleteRequestsBiometric 
                    (FormNo, StdID, IYear, Class, Sess, RequestDate, RequestedBy)
                VALUES 
                    (@FormNo, @StdID, @IYear, @Class, @Sess, GETDATE(), @RequestedBy);";

                SqlParameter[] parameters = {
                    new SqlParameter("@FormNo", SqlDbType.BigInt) { Value = long.Parse(formNo) },
                    new SqlParameter("@StdID", SqlDbType.BigInt) { Value = long.Parse(stdid) },
                    new SqlParameter("@IYear", SqlDbType.Int) { Value = UserSession.Year },
                    new SqlParameter("@Class", SqlDbType.Int) { Value = UserSession.Class },
                    new SqlParameter("@Sess", SqlDbType.Int) { Value = UserSession.Sess },
                    new SqlParameter("@RequestedBy", SqlDbType.NVarChar, 100) { Value = UserSession.InstituteCode }
                };


                int rows = DatabaseHelper.ExecuteNonQuery(sql, parameters);

                if (rows > 0)
                {
                    MessageBox.Show("Delete request has been sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to send delete request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetUI();
            LoadStudentData();
            dataGridViewStudents.DataBindingComplete -= dataGridViewStudents_DataBindingComplete;
            dataGridViewStudents.DataBindingComplete += dataGridViewStudents_DataBindingComplete;

            dataGridViewStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewStudents.MultiSelect = false;
            dataGridViewStudents.DataBindingComplete += (s, eArgs) =>
            {
                foreach (DataGridViewRow row in dataGridViewStudents.Rows)
                {
                    row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor;
                }
                dataGridViewStudents.ClearSelection(); // no row selected initially
            };
        }

        /*  private void RunStoredTemplateSelfCheck()
          {
              var dt = GetInstituteFingerprints();
              foreach (DataRow row in dt.Rows)
              {
                  int stdid = Convert.ToInt32(row["stdid"]);
                  byte[] t = (byte[])row["FingerprintTemplate"];
                  if (t == null || t.Length == 0)
                  {
                      LogMessage($"Std:{stdid} → EMPTY TEMPLATE");
                      continue;
                  }

                  bool matched = false;
                  int score = 0;
                  int err = m_FPM.MatchTemplate(t, t, SGFPMSecurityLevel.HIGH, ref matched);
                  m_FPM.GetMatchingScore(t, t, ref score);

                  string status = matched ? "✅ TRUE" : "⚠️ FALSE (possible minor mismatch)";
                  LogMessage($"StdID={stdid} | Matched={status} | Score={score}");
              }
          }


          private void textBoxLogs_MouseDoubleClick(object sender, MouseEventArgs e)
          {
              RunStoredTemplateSelfCheck();
          }*/
    }
}
