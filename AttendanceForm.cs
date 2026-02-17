using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using SecuGen.FDxSDKPro.Windows;


namespace Secugen_HU20
{
    public partial class AttendanceForm : Form
    {
        // Data / UI
        private DataTable cleanedDataTable;
        private DataTable presentList;
        private bool isFormLoading = true;
        private DateTime cachedServerDate;

        // Biometric
        private SGFingerPrintManager m_FPM;
        private int m_ImageWidth;
        private int m_ImageHeight;

        // Continuous capture
        private Timer fingerprintTimer;
        private bool isCapturing = false;

        // Cache: stdid -> template (for students in currently loaded grid)
        /*private readonly Dictionary<int, byte[]> fingerprintCache = new Dictionary<int, byte[]>();*/
        private readonly Dictionary<int, List<byte[]>> fingerprintCache = new Dictionary<int, List<byte[]>>();


        // Debounce
        private int lastMatchedStdId = -1;
        private DateTime lastMatchTime = DateTime.MinValue;
        private List<AttendanceRecord> pendingAttendance = new List<AttendanceRecord>();
        private Timer syncTimer;

        private readonly IAttendanceStore attendanceStore = new FileAttendanceStore();

        private Timer autoSyncTimer;
        private bool isSyncInProgress = false;

        private readonly string attendanceInsertQuery = @"
        IF NOT EXISTS (
            SELECT 1 FROM tbl_Attendance
            WHERE stdid = @stdid
            AND class = @class
            AND sess = @sess
            AND iyear = @iyear
            AND examDate = @examDate
            AND examGroup = @examGroup
        )
        BEGIN
            INSERT INTO tbl_Attendance
                (stdid, class, sess, iyear, examDate, examGroup, status, capturedBy, cent_cd)
            VALUES
                (@stdid, @class, @sess, @iyear, @examDate, @examGroup, 1, @capturedBy, @cent_cd);
        END";



        private void StartAutoSync()
        {
            autoSyncTimer = new Timer();
            autoSyncTimer.Interval = 120000; // ⏱ 2 minutes (safe default)
            autoSyncTimer.Tick += AutoSyncTimer_Tick;
            autoSyncTimer.Start();
        }

        private void StopAutoSync()
        {
            if (autoSyncTimer != null)
            {
                autoSyncTimer.Stop();
                autoSyncTimer.Tick -= AutoSyncTimer_Tick;
                autoSyncTimer.Dispose();
                autoSyncTimer = null;
            }
        }


        private void AutoSyncTimer_Tick(object sender, EventArgs e)
        {
            if (isSyncInProgress) return;

            try
            {
                isSyncInProgress = true;
                PushPendingAttendanceToServer();
            }
            finally
            {
                isSyncInProgress = false;
            }
        }

        private void PushPendingAttendanceToServer()
        {
            var pending = attendanceStore.LoadPending();
            if (pending.Count == 0) return;

            foreach (var rec in pending)
            {
                try
                {
                    SqlParameter[] prms = new SqlParameter[]
                    {
                new SqlParameter("@stdid", rec.StudentId),
                new SqlParameter("@class", UserSession.Class),
                new SqlParameter("@sess", UserSession.Sess),
                new SqlParameter("@iyear", UserSession.Year),
                new SqlParameter("@examDate", rec.ExamDate),
                new SqlParameter("@examGroup", rec.ExamGroup),
                new SqlParameter("@capturedBy", UserSession.InstituteCode),
                new SqlParameter("@cent_cd", UserSession.Cent_cd)
                    };

                    DatabaseHelper.ExecuteNonQuery(attendanceInsertQuery, prms);

                    // ✅ success → remove from file
                    attendanceStore.Remove(rec);

                    // optional UI update
                    MarkRowAsPresent(rec.StudentId);
                }
                catch
                {
                    // server still unavailable → retry next cycle
                    break;
                }
            }
        }


        /*private void btnSyncNow_Click(object sender, EventArgs e)
        {
            if (isSyncInProgress) return;
            AutoSyncTimer_Tick(null, null);
        }*/


        private void MarkRowAsPresent(string stdid)
        {
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells["stdid"].Value?.ToString() == stdid)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200);
                    row.Cells["status"].Value = "Present";
                    break;
                }
            }
        }


        public AttendanceForm()
        {
            InitializeComponent();

            // Prevent events from firing while initializing
            isFormLoading = true;

            InitBiometricDevice();
            StartSyncTimer();
            // do not auto select group at startup
            ddlExamGroup.SelectedIndex = 0;
            NetworkHelper.ShowInternetWarningIfOffline();

            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.DefaultCellStyle.SelectionBackColor = dgvStudents.DefaultCellStyle.BackColor;
            dgvStudents.DefaultCellStyle.SelectionForeColor = dgvStudents.DefaultCellStyle.ForeColor;

            dgvStudents.DataBindingComplete += (s, e) => ApplyRowStyling();
            dgvStudents.Sorted += (s, e) => ApplyRowStyling();


            isFormLoading = false;
        }

        #region Biometric init + capture loop

        private void InitBiometricDevice()
        {
            try
            {

                lblCentName.Location = new Point(10, 10);
                lblCentName.MaximumSize = new Size(800, 0);
                lblCentName.TextAlignment = ContentAlignment.TopLeft;
                lblCentName.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblCentName.ForeColor = Color.MediumSlateBlue;
                lblCentName.Text = "Center No. " + UserSession.Cent_cd + " - IT Teacher Name: " + UserSession.Name;

                lblStatus.Location = new Point(10, 85);
                lblStatus.MaximumSize = new Size(800, 0);
                lblStatus.TextAlignment = ContentAlignment.TopLeft;
                lblStatus.Font = new Font("Segoe UI", 12, FontStyle.Bold);


                m_FPM = new SGFingerPrintManager();

                int err = m_FPM.Init(SGFPMDeviceName.DEV_AUTO);
                if (err != (int)SGFPMError.ERROR_NONE)
                {
                    var error = ErrorHelper.GetError(err);
                    LogMessage($"❌ Fingerprint device init error: {error.Code} - {error.Message}");
                    return;
                }

                err = m_FPM.OpenDevice((int)SGFPMPortAddr.USB_AUTO_DETECT);
                if (err != (int)SGFPMError.ERROR_NONE)
                {
                    var error = ErrorHelper.GetError(err);
                    LogMessage($"❌ Fingerprint device open error: {error.Code} - {error.Message}");
                    return;
                }

                var info = new SGFPMDeviceInfoParam();
                m_FPM.GetDeviceInfo(info);
                m_ImageWidth = info.ImageWidth;
                m_ImageHeight = info.ImageHeight;

                // ✅ Device ready log
                LogMessage($"✅ Fingerprint device ready.");

            }
            catch (Exception ex)
            {
                LogMessage("❌ InitBiometricDevice error: " + ex.Message);
            }
        }

        private void LogMessage(string message)
        {
            string text = $"{DateTime.Now:HH:mm:ss} → {message}{Environment.NewLine}";
            if (textBoxLogs.InvokeRequired)
            {
                textBoxLogs.Invoke((MethodInvoker)(() => textBoxLogs.AppendText(text)));
            }
            else
            {
                textBoxLogs.AppendText(text);
            }
        }


        private void StartFingerprintListener()
        {
            // guard: don't start multiple timers
            if (fingerprintTimer != null) return;

            fingerprintTimer = new Timer();
            fingerprintTimer.Interval = 700; // ms, tweak as needed
            fingerprintTimer.Tick += FingerprintTimer_Tick;
            fingerprintTimer.Start();
        }

        private void StopFingerprintListener()
        {
            if (fingerprintTimer != null)
            {
                fingerprintTimer.Stop();
                fingerprintTimer.Tick -= FingerprintTimer_Tick;
                fingerprintTimer.Dispose();
                fingerprintTimer = null;
            }
        }

        private void FingerprintTimer_Tick(object sender, EventArgs e)
        {
            // Safety guards
            if (isFormLoading) return;
            if (ddlExamGroup == null || ddlExamGroup.SelectedIndex < 1) return;
            if (cleanedDataTable == null || cleanedDataTable.Rows.Count == 0) return;
            if (fingerprintCache == null || fingerprintCache.Count == 0) return;
            if (isCapturing) return;

            isCapturing = true;
            try
            {
                byte[] template = CaptureFingerprint();
                if (template == null) return;

                int matchedStdId = MatchFingerprint(template);

                if (matchedStdId > 0)
                {
                    // debounce: ignore repeat matches within 3 seconds for same student
                    if (matchedStdId == lastMatchedStdId && (DateTime.Now - lastMatchTime).TotalSeconds < 3)
                        return;

                    lastMatchedStdId = matchedStdId;
                    lastMatchTime = DateTime.Now;

                    // Lookup student details from cleanedDataTable
                    DataRow[] foundRows = cleanedDataTable.Select($"stdid = {matchedStdId}");
                    if (foundRows.Length > 0)
                    {
                        string rollNo = foundRows[0]["rno"].ToString();
                        string studentName = foundRows[0]["student_name"].ToString();

                        MarkPresent(matchedStdId.ToString());

                        // optional status label update
                        var lbl = this.Controls.Find("lblStatus", true).FirstOrDefault() as Guna2HtmlLabel;
                        if (lbl != null)
                        {
                            this.Invoke((MethodInvoker)(() =>
                            {
                                System.Media.SystemSounds.Asterisk.Play();
                                LogMessage($"✔ Marked: Roll No. {rollNo} - {studentName}");
                                lbl.Text = $"✔ Marked: Roll No. {rollNo} - {studentName}";
                                lbl.ForeColor = Color.Green;
                            }));
                        }
                    }
                }
                else
                {
                    var lbl = this.Controls.Find("lblStatus", true).FirstOrDefault() as Guna2HtmlLabel;
                    if (lbl != null)
                    {
                        this.Invoke((MethodInvoker)(() =>
                        {
                            System.Media.SystemSounds.Hand.Play();
                            lbl.Text = "❌ Not recognized";
                            lbl.ForeColor = Color.Red;
                        }));
                    }
                }
            }
            catch
            {
                // swallow periodic errors; consider logging
            }
            finally
            {
                isCapturing = false;
            }
        }


        private byte[] CaptureFingerprint()
        {
            try
            {
                // Get image buffer according to device dimensions
                byte[] fpImage = new byte[m_ImageWidth * m_ImageHeight];

                int err = m_FPM.GetImage(fpImage);
                if (err != (int)SGFPMError.ERROR_NONE)
                {
                    //var error = ErrorHelper.GetError(err);
                    //LogMessage($"❌ GetImage error: {error.Code} - {error.Message}");
                    return null;
                }

                // Get template max size then create template
                int maxTemplateSize = 0;
                m_FPM.GetMaxTemplateSize(ref maxTemplateSize);
                byte[] template = new byte[maxTemplateSize];

                // Use the same CreateTemplate signature your SDK supports
                err = m_FPM.CreateTemplate(fpImage, template);
                if (err != (int)SGFPMError.ERROR_NONE)
                {
                    var error = ErrorHelper.GetError(err);
                    LogMessage($"❌ CreateTemplate error: {error.Code} - {error.Message}");
                    return null;
                }

                // trim to actual size if GetMaxTemplateSize returned max and CreateTemplate produced less;
                // some SDKs put template length in last bytes or require GetMaxTemplateSize only. We'll return full template buffer.
                return template;
            }
            catch
            {
                return null;
            }
        }

        private int MatchFingerprint(byte[] capturedTemplate)
        {
            if (capturedTemplate == null || fingerprintCache.Count == 0) return -1;

            // 🔧 change: more lenient security level for better matching
            SGFPMSecurityLevel secu = SGFPMSecurityLevel.HIGH;

            foreach (var kv in fingerprintCache)
            {
                try
                {
                    foreach (var tpl in kv.Value) // ✅ check all stored templates per student
                    {
                        bool matched = false;
                        int err = m_FPM.MatchTemplate(capturedTemplate, tpl, secu, ref matched);

                        if (err != (int)SGFPMError.ERROR_NONE)
                        {
                            var error = ErrorHelper.GetError(err);
                            LogMessage($"❌ MatchTemplate error (stdid {kv.Key}): {error.Code} - {error.Message}");
                            continue;
                        }

                        // ✅ log diagnostic
                        //LogMessage($"Compare: stdid={kv.Key}, tplSize={tpl.Length}, matched={matched}");
                        if (matched) return kv.Key;
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("❌ MatchFingerprint exception: " + ex.Message);
                    continue;
                }
            }

            return -1;
        }


        #endregion

        #region Loading students / cache / styling

        private void ddlExamGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Prevent auto-run during initialization
            if (isFormLoading) return;

            // When no valid selection (e.g., "Select Group")
            if (ddlExamGroup.SelectedIndex < 1)
            {
                StopAutoSync(); // ⛔ stop background sync

                guna2Button1.Visible = false;

                // Stop fingerprint capture
                StopFingerprintListener();

                // Clear table & cache
                cleanedDataTable = null;
                dgvStudents.DataSource = null;
                fingerprintCache.Clear();
                return;
            }

            guna2Button1.Visible = true;

            // Stop any existing capture loop before reload
            StopFingerprintListener();

            // ✅ Display loading message
            lblStatus.Text = "⏳ Loading data, please wait...";
            lblStatus.ForeColor = Color.DarkOrange;
            lblStatus.Refresh();

            DateTime cachedServerDate;
            try
            {
                cachedServerDate = DatabaseHelper.GetServerDate(); // single DB call
            }
            catch
            {
                cachedServerDate = DateTime.Now; // fallback
            }

            int selectedGroup = ddlExamGroup.SelectedIndex;

            // ✅ Background load (no UI freeze)
            Task.Run(() =>
            {
                try
                {
                    // Run DB and cache loading off UI thread
                    var students = LoadStudents(cachedServerDate, selectedGroup);
                    var fingerprints = LoadFingerprintCacheScoped(cachedServerDate, selectedGroup);

                    // ✅ Return to UI thread
                    this.Invoke((MethodInvoker)(() =>
                    {
                        cleanedDataTable = students;
                        dgvStudents.DataSource = cleanedDataTable;
                        MarkAlreadyPresent(cachedServerDate);
                        ApplyRowStyling();

                        // ✅ Restart fresh listener after load
                        StartFingerprintListener();
                        StartAutoSync();

                        lblStatus.Text = "✅ Data loaded successfully. Scanner ready.";
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Refresh();

                        LogMessage("✅ Data loaded successfully for selected group.");
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        lblStatus.Text = "❌ Error while loading data.";
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Refresh();

                        LogMessage("❌ Error while loading group: " + ex.Message);
                        MessageBox.Show("Error loading data:\n" + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private DataTable LoadStudents(DateTime serverDate, int selectedGroup)
        {
            // exactly the same SQL as before — only using passed parameters
            if (selectedGroup < 1) return new DataTable();

            int subGrp = selectedGroup == 1 ? 1 : 2;
            string examDate = serverDate.ToString("dd-MM-yyyy");
            string query = @"
                SELECT rno,
                       formno,
                       stdid,
                       name AS student_name,
                       fname AS father_name,
                       (CASE subGrp WHEN 1 THEN 'Morning' WHEN 2 THEN 'Evening' ELSE '' END) AS exam_group,
                       SUB_NAME AS subject_name,
                       date AS exam_date
                FROM LiveDB..BioMetric_Atten_std
                WHERE [date] = @examDate
                  AND subGrp = @subGrp
                  AND cent_cd = @cent_cd
                ORDER BY rno;";

            SqlParameter[] prms = new SqlParameter[]
            {
        new SqlParameter("@examDate", examDate),
        new SqlParameter("@subGrp", subGrp),
        new SqlParameter("@cent_cd", UserSession.Cent_cd)
            };

            try
            {
                return DatabaseHelper.ExecuteQuery(query, prms);
            }
            catch (Exception ex)
            {
                LogMessage("❌ Error loading students: " + ex.Message);
                return new DataTable();
            }
        }

        private DataTable LoadFingerprintCacheScoped(DateTime serverDate, int selectedGroup)
        {
            fingerprintCache.Clear();

            int subGrp = selectedGroup == 1 ? 1 : 2;
            string examDate = serverDate.ToString("dd-MM-yyyy");

            string query = @"
                SELECT f.stdid, f.FingerprintTemplate
                FROM OnlineOpr..tbl_Fingerprint f
                INNER JOIN LiveDB..BioMetric_Atten_std s
                    ON s.stdid = f.stdid
                WHERE f.class   = @class
                  AND f.iyear   = @iyear
                  AND f.sess = @sess  
                  AND s.cent_cd = @cent_cd
                  AND s.subGrp  = @subGrp
                  AND s.sess = @sess
                  AND s.[date] = @examDate;";

            SqlParameter[] prms = new SqlParameter[]
            {
        new SqlParameter("@class", UserSession.Class),
        new SqlParameter("@iyear", ConfigurationManager.GetYear(UserSession.Class, UserSession.Sess)),
        new SqlParameter("@sess",  UserSession.Sess),
        new SqlParameter("@cent_cd", UserSession.Cent_cd),
        new SqlParameter("@subGrp", subGrp),
        new SqlParameter("@examDate", examDate)
            };

            try
            {
                DataTable t = DatabaseHelper.ExecuteQuery(query, prms);

                foreach (DataRow r in t.Rows)
                {
                    if (r["FingerprintTemplate"] == DBNull.Value) continue;

                    int stdid = Convert.ToInt32(r["stdid"]);
                    byte[] tpl = (byte[])r["FingerprintTemplate"];

                    if (!fingerprintCache.ContainsKey(stdid))
                        fingerprintCache[stdid] = new List<byte[]>();

                    fingerprintCache[stdid].Add(tpl);
                }

                LogMessage($"✅ Loaded {t.Rows.Count} fingerprint templates.");
                return t;
            }
            catch (Exception ex)
            {
                LogMessage("❌ Fingerprint load error: " + ex.Message);
                return new DataTable();
            }
        }

        private void MarkAlreadyPresent(DateTime serverDate)
        {
            if (ddlExamGroup.SelectedIndex < 1) return;

            int examGroup = ddlExamGroup.SelectedIndex == 1 ? 1 : 2;
            string examDate = serverDate.ToString("dd-MM-yyyy");

            string query = @"
            SELECT stdid
            FROM tbl_Attendance
            WHERE class = @class
            AND sess = @sess
            AND iyear = @iyear
            AND examDate = @examDate
            AND examGroup = @examGroup;";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@class", UserSession.Class),
                new SqlParameter("@sess", UserSession.Sess),
                new SqlParameter("@iyear", UserSession.Year),
                new SqlParameter("@examDate", examDate),
                new SqlParameter("@examGroup", examGroup)
            };

            try
            {
                presentList = DatabaseHelper.ExecuteQuery(query, prms);
                // no UI changes here - ApplyRowStyling will run on UI thread
            }
            catch (Exception ex)
            {
                LogMessage("❌ MarkAlreadyPresent error: " + ex.Message);
                presentList = new DataTable();
            }
        }


        private void ApplyRowStyling()
        {
            if (dgvStudents.Rows.Count == 0) return;

            if (!dgvStudents.Columns.Contains("status"))
                dgvStudents.Columns.Add("status", "Status");

            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                string stdid = row.Cells["stdid"].Value?.ToString();

                // Default = Absent (Red background + White text)
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 102, 102);
                row.DefaultCellStyle.ForeColor = Color.White;
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 102, 102);
                row.DefaultCellStyle.SelectionForeColor = Color.White;
                row.Cells["status"].Value = "Absent";

                // Present (Green background + Black text)
                if (presentList != null &&
                    presentList.AsEnumerable().Any(r => r["stdid"].ToString() == stdid))
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200);
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 255, 200);
                    row.DefaultCellStyle.SelectionForeColor = Color.Black;
                    row.Cells["status"].Value = "Present";
                }
            }
        }


        #endregion

        #region MarkPresent + DB duplication protection

        private void MarkPresent(string stdid)
        {
            if (string.IsNullOrWhiteSpace(stdid)) return;
            if (ddlExamGroup.SelectedIndex < 1) return;

            // memory check
            if (presentList != null && presentList.AsEnumerable().Any(r => r["stdid"].ToString() == stdid))
                return;

            int examGroup = 0; // default

            if (ddlExamGroup.SelectedIndex == 1)
                examGroup = 1;
            else if (ddlExamGroup.SelectedIndex == 2)
                examGroup = 2;

            try
            {
                cachedServerDate = DatabaseHelper.GetServerDate(); // may fail if offline
            }
            catch
            {
                // fallback to system date if server is not reachable
                cachedServerDate = DateTime.Now;
            }

            string examDate = cachedServerDate.ToString("dd-MM-yyyy");

            // ✅ STRICT LOCAL SAVE (NO LIVE DB HIT)
            attendanceStore.SaveLocal(new AttendanceRecord
            {
                StudentId = stdid,
                ExamDate = examDate,
                ExamGroup = examGroup,
                MarkedAt = cachedServerDate
            });


            // ✅ update presentList memory
            if (presentList == null)
            {
                presentList = new DataTable();
                presentList.Columns.Add("stdid", typeof(string));
            }
            if (!presentList.AsEnumerable().Any(r => r["stdid"].ToString() == stdid))
            {
                var newRow = presentList.NewRow();
                newRow["stdid"] = stdid;
                presentList.Rows.Add(newRow);
            }

            // ✅ update UI row
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells["stdid"].Value?.ToString() == stdid)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200); // medium green
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    if (!dgvStudents.Columns.Contains("status"))
                        dgvStudents.Columns.Add("status", "Status");
                    row.Cells["status"].Value = "Present";
                    break;
                }
            }

            // ✅ add: refresh present list and UI to reflect instantly
            MarkAlreadyPresent(cachedServerDate);
            ApplyRowStyling();
        }

        #endregion

        #region Search (keeps styling)

        private void textBoxSearch1_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = textBoxSearch1.Text.Trim();

            if (cleanedDataTable != null)
            {
                DataView dv = new DataView(cleanedDataTable);

                dv.RowFilter = string.Format(
                    "Convert([rno], 'System.String') LIKE '%{0}%' OR " +
                    "Convert([formno], 'System.String') LIKE '%{0}%' OR " +
                    "Convert([stdid], 'System.String') LIKE '%{0}%' OR " +
                    "[student_name] LIKE '%{0}%' OR " +
                    "[father_name] LIKE '%{0}%'",
                    searchQuery.Replace("'", "''")
                );

                dgvStudents.DataSource = dv;
                ApplyRowStyling();
            }
        }

        #endregion

        #region Cleanup

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopSyncTimer();
            // stop timer and close device
            StopFingerprintListener();
            StopAutoSync();

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

        // Start the sync timer (call this e.g. in ctor or Form_Load)
        private void StartSyncTimer()
        {
            if (syncTimer != null) return;

            syncTimer = new Timer();
            syncTimer.Interval = 30000; // 30 seconds
            syncTimer.Tick += SyncPendingAttendance; // EventHandler signature
            syncTimer.Start();
        }

        // Stop the sync timer (call in OnFormClosing)
        private void StopSyncTimer()
        {
            if (syncTimer == null) return;

            syncTimer.Stop();
            syncTimer.Tick -= SyncPendingAttendance;
            syncTimer.Dispose();
            syncTimer = null;
        }

        // Event handler — correct signature for Timer.Tick
        private void SyncPendingAttendance(object sender, EventArgs e)
        {
            if (pendingAttendance == null || pendingAttendance.Count == 0) return;

            // copy to avoid modifying collection while iterating
            var toAttempt = pendingAttendance.ToList();
            var synced = new List<AttendanceRecord>();

            foreach (var rec in toAttempt)
            {
                try
                {
                    // build same guarded insert you already use in MarkPresent
                    string query = @"
                IF NOT EXISTS (
                        SELECT 1 FROM tbl_Attendance
                        WHERE stdid = @stdid
                        AND class = @class
                        AND sess = @sess
                        AND iyear = @iyear
                        AND examDate = @examDate
                        AND examGroup = @examGroup
                )
                BEGIN
                    INSERT INTO tbl_Attendance
                        (stdid, class, sess, iyear, examDate, examGroup, status, capturedBy, cent_cd)
                    VALUES
                        (@stdid, @class, @sess, @iyear, @examDate, @examGroup, 1, @capturedBy, @cent_cd);
                END";

                    // Use the same date format as your MarkPresent (dd-MM-yyyy)
                    string examDate = rec.MarkedAt.ToString("dd-MM-yyyy");
                    if (ddlExamGroup.SelectedIndex < 1) return;
                    int examGroup = 0; // default

                    if (ddlExamGroup.SelectedIndex == 1)
                        examGroup = 1;
                    else if (ddlExamGroup.SelectedIndex == 2)
                        examGroup = 2;


                    SqlParameter[] prms = new SqlParameter[]
                    {
                    new SqlParameter("@stdid", rec.StudentId),
                    new SqlParameter("@class", UserSession.Class),
                    new SqlParameter("@sess", UserSession.Sess),
                    new SqlParameter("@iyear", UserSession.Year),
                    new SqlParameter("@examDate", examDate),
                    new SqlParameter("@examGroup", examGroup),
                    new SqlParameter("@capturedBy", UserSession.InstituteCode),
                    new SqlParameter("@cent_cd", UserSession.Cent_cd)
                    };

                    // try to insert using your DatabaseHelper
                    DatabaseHelper.ExecuteNonQuery(query, prms);

                    // if no exception -> success
                    synced.Add(rec);
                    LogMessage($"✅ Synced pending attendance for StdID: {rec.StudentId}");


                    // update UI row to "Present" (thread-safe)
                    UpdateRowStatus(rec.StudentId, "Present", Color.LightGreen);
                }
                catch (Exception ex)
                {
                    LogMessage("❌ Database error while marking present: " + ex.Message);
                    // still offline / DB error — keep it in pending list
                }
            }

            // remove synced entries from pending list
            foreach (var s in synced)
                pendingAttendance.Remove(s);
        }

        // Helper to update DataGridView row status safely (call from any thread)
        private void UpdateRowStatus(string stdid, string status, Color backColor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() => UpdateRowStatus(stdid, status, backColor)));
                return;
            }

            if (!dgvStudents.Columns.Contains("status"))
                dgvStudents.Columns.Add("status", "Status");

            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells["stdid"].Value?.ToString() == stdid)
                {
                    row.Cells["status"].Value = status;
                    row.DefaultCellStyle.BackColor = backColor;
                    row.DefaultCellStyle.ForeColor = Color.White;
                    break;
                }
            }
        }

        #endregion

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            LoadStats();
        }

        private void ShowStatsDialog(int total, int registered, decimal percentReg, int remaining, int attendance, int missing, decimal percentAtt)
        {
            Guna2MessageDialog msgDialog = new Guna2MessageDialog
            {
                Buttons = MessageDialogButtons.OK,
                Icon = MessageDialogIcon.Information,
                Style = MessageDialogStyle.Light,  // Light | Dark
                Caption = "📊 Center Stats",
                Text =
                    $"Total Candidates: {total}\n" +
                    $"Enrolled: {registered} ({percentReg}%)\n" +
                    $"Remaining: {remaining}\n" +
                    $"Attendance: {attendance} ({percentAtt}%)\n" +
                    $"Missing Attendance: {missing}",

                Parent = this
            };

            msgDialog.Show();
        }

        private DataTable LoadStats()
        {

            int examGroup = 0; // default

            if (ddlExamGroup.SelectedIndex == 1)
                examGroup = 1;
            else if (ddlExamGroup.SelectedIndex == 2)
                examGroup = 2;


            DateTime serverDate;

            try
            {
                serverDate = DatabaseHelper.GetServerDate(); // may fail if offline
            }
            catch
            {
                // fallback to system date if server is not reachable
                serverDate = DateTime.Now;
            }

            string examDate = serverDate.ToString("dd-MM-yyyy");

            try
            {
                string query = @"
                WITH CandidateSummary AS (
                    SELECT
                        a.cent_cd,
                        COUNT(DISTINCT a.stdid) AS total_candidates,
                        COUNT(DISTINCT CASE WHEN f.stdid IS NOT NULL THEN a.stdid END) AS registered_biometric,
                        COUNT(DISTINCT CASE WHEN f.stdid IS NOT NULL AND ta.status = 1 THEN a.stdid END) AS attendance_marked
                    FROM LiveDB..BioMetric_Atten_std a
                    LEFT JOIN onlineopr..tbl_Fingerprint f
                        ON f.stdid = a.stdid 
                       AND f.class = a.class 
                       AND f.iyear = a.iyear 
                       AND f.sess = a.sess
                    LEFT JOIN OnlineOpr..tbl_Attendance ta
                        ON ta.stdid = a.stdid 
                       AND ta.class = a.class 
                       AND ta.iyear = a.iyear 
                       AND ta.sess = a.sess
                       AND ta.examdate = @ExamDate
                       AND ta.examgroup IN (@ExamGroup)
                    WHERE a.iyear = @Year
                      AND a.sess = @Sess
                      AND a.class = @Class
                      AND a.cent_cd = @InstCode
                      AND a.date = @ExamDate
                      AND a.subgrp IN (@SubGrp)
                    GROUP BY a.cent_cd
                )
                SELECT
                    cs.cent_cd,
                    cs.total_candidates,
                    cs.registered_biometric AS registered_candidates,
                    (cs.total_candidates - cs.registered_biometric) AS remaining_candidates,
                    CAST(
                         CASE WHEN cs.total_candidates = 0 THEN 0
                              ELSE (cs.registered_biometric * 100.0 / cs.total_candidates) END 
                         AS DECIMAL(5,2)
                    ) AS percentage_registration,
                    cs.attendance_marked AS attendance_candidates,
                    (cs.registered_biometric - cs.attendance_marked) AS missing_attendance_candidates,
                    CAST(
                         CASE WHEN cs.registered_biometric = 0 THEN 0
                              ELSE (cs.attendance_marked * 100.0 / cs.registered_biometric) END 
                         AS DECIMAL(5,2)
                    ) AS percentage_attendance
                FROM CandidateSummary cs
                ORDER BY cs.cent_cd ASC;
";

                SqlParameter[] parameters = {
                new SqlParameter("@Year", UserSession.Year),
                new SqlParameter("@Sess", UserSession.Sess),
                new SqlParameter("@Class", UserSession.Class),
                new SqlParameter("@InstCode", UserSession.Cent_cd),
                new SqlParameter("@ExamDate", examDate),
                new SqlParameter("@ExamGroup", examGroup),
                new SqlParameter("@SubGrp", examGroup),
                };

                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                // Update label dynamically
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    int total = Convert.ToInt32(row["total_candidates"]);
                    int registered = Convert.ToInt32(row["registered_candidates"]);
                    int remaining = Convert.ToInt32(row["remaining_candidates"]);
                    decimal percentReg = Convert.ToDecimal(row["percentage_registration"]);
                    int attendance = Convert.ToInt32(row["attendance_candidates"]);
                    int missing = Convert.ToInt32(row["missing_attendance_candidates"]);
                    decimal percentAtt = Convert.ToDecimal(row["percentage_attendance"]);

                    /* lblCount.MaximumSize = new Size(800, 0);
                     lblCount.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                     lblCount.ForeColor = Color.DarkGreen;
                     lblCount.Text =
                         $"Candidates: {total}, Enrolled: {registered} ({percentReg}%), " +
                         $"Remaining: {remaining}, Attendance: {attendance} ({percentAtt}%), Missing Attendance: {missing}";*/

                    /* MessageBox.Show(
                         $"Total: {total}\n" +
                         $"Enrolled: {registered} ({percentReg}%)\n" +
                         $"Remaining: {remaining}\n" +
                         $"Attendance: {attendance} ({percentAtt}%)\n" +
                         $"Missing Attendance: {missing}",
                         "Center Stats",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information
                     );*/

                    ShowStatsDialog(total, registered, percentReg, remaining, attendance, missing, percentAtt);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stats:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

    }
}