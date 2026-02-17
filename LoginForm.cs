using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secugen_HU20
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            // Set default selections
            ddlClass.SelectedIndex = 0;
            ddlMode.SelectedIndex = 0;
            ddlLoginType.SelectedIndex = 0;
            ddlSess.SelectedIndex = 0;


            // Initialize ProgressBar
            progressBarLogin.Visible = false;
            //progressBarLogin.Style = ProgressBarStyle.Marquee;

            // Subscribe to TextChanged event for custom masking
            textBoxInstituteCode.TextChanged += TextBoxInstituteCode_TextChanged;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ddlLoginType.Focus();
        }


        private async void ButtonLogin_Click(object sender, EventArgs e)
        {
            progressBarLogin.Visible = true;


            // 🔹 Check Login Type
            if (ddlLoginType.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select Login Type (Institute / IT Teacher).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ddlLoginType.Focus();
                ddlLoginType.BorderColor = Color.Red;
                progressBarLogin.Visible = false;
                return;
            }
            else
            {
                ddlLoginType.BorderColor = Color.Silver;
            }

            string input = textBoxInstituteCode.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            string instituteCode = "";  // make this string to handle both CNIC and numeric code
            string loginType = ddlLoginType.SelectedItem?.ToString();

            if (loginType == "IT Teacher")
            {
                // CNIC validation (#####-#######-#)
                string cnicPattern = @"^\d{5}-\d{7}-\d{1}$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, cnicPattern))
                {
                    MessageBox.Show("Invalid CNIC format. Use #####-#######-#",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxInstituteCode.Focus();
                    textBoxInstituteCode.BorderColor = Color.Red;
                    progressBarLogin.Visible = false;
                    return;
                }

                instituteCode = input; // save CNIC as instituteCode
            }
            else if (loginType == "Supervisory Staff")
            {
                // Validation: Maximum 3 digits
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d{1,3}$"))
                {
                    MessageBox.Show("Invalid Center Number. Please enter up to 3 digits.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxInstituteCode.Focus();
                    textBoxInstituteCode.BorderColor = Color.Red;
                    progressBarLogin.Visible = false;
                    return;
                }

                instituteCode = input;
            }

            else
            {
                // Institute code validation (numeric only)
                if (!int.TryParse(input, out int numericCode) || numericCode <= 0 || input.Length != 6)
                {
                    MessageBox.Show("Invalid Institute Code. Please enter a valid number.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxInstituteCode.Focus();
                    progressBarLogin.Visible = false;
                    return;
                }

                instituteCode = numericCode.ToString(); // store number as string
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password cannot be empty.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassword.Focus();
                progressBarLogin.Visible = false;
                return;
            }

            if (loginType == "Supervisory Staff")
            {
                if (password != instituteCode)
                {
                    MessageBox.Show("For Supervisory Staff, the Password must be the same as the Center Number.",
                       "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPassword.Focus();
                    progressBarLogin.Visible = false;
                    return;
                }
            }

            // 🔹 Check Mode
            if (ddlMode.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select Mode (Registration / Examination).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ddlMode.Focus();
                ddlMode.BorderColor = Color.Red;
                progressBarLogin.Visible = false;
                return;
            }
            else
            {
                ddlMode.BorderColor = Color.Silver;
            }

            //Check Class
            if (ddlClass.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select Class (9th / 10th / 11th / 12th).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ddlClass.Focus();
                ddlClass.BorderColor = Color.Red;
                progressBarLogin.Visible = false;
                return;
            }
            else
            {
                ddlClass.BorderColor = Color.Silver;
            }

            // Check Session
            if (ddlSess.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select Session (1st Annual / 2nd Annual).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ddlSess.Focus();
                ddlSess.BorderColor = Color.Red;
                progressBarLogin.Visible = false;
                return;
            }
            else
            {
                ddlSess.BorderColor = Color.Silver;
            }

            int selectedClassIndex = ddlClass.SelectedIndex;
            int selectedSessIndex = ddlSess.SelectedIndex;
            int cls = 0;
            int sess = 0;

            switch (selectedClassIndex)
            {
                case 1:
                    cls = 9;
                    break;
                case 2:
                    cls = 10;
                    break;
                case 3:
                    cls = 11;
                    break;
                case 4:
                    cls = 12;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedClassIndex), "Invalid class selection");

            }
            progressBarLogin.Visible = false;

            switch (selectedSessIndex)
            {
                case 1:
                    sess = 1;
                    break;
                case 2:
                    sess = 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedSessIndex), "Invalid session selection");
            }
            progressBarLogin.Visible = false;

            int ryear = ConfigurationManager.GetYear(cls, sess);
            if (ryear == 0)
            {
                MessageBox.Show("This Session/Class is not active yet.",
                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                progressBarLogin.Visible = false;
                return;
            }

            NetworkHelper.ShowInternetWarningIfOffline();

            //Now instituteCode holds either CNIC (string) or numeric code (string)
            UserSession.InstituteCode = instituteCode.ToString();
            string selectedMode = ddlMode.SelectedItem?.ToString();

            // Save in session
            UserSession.Class = cls;
            UserSession.Sess = sess;

            progressBarLogin.Visible = true;

            // Run validation in background
            bool isValid = await Task.Run(() =>
                ValidateUser(instituteCode, password, cls, selectedMode, sess, loginType));

            progressBarLogin.Visible = false;

            if (isValid && loginType == "Institute")
            {
                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            }
            else if (isValid && loginType == "IT Teacher")
            {
                AttendanceForm AttendanceForm = new AttendanceForm();
                AttendanceForm.Show();
                this.Hide();
            }
            else if (isValid && loginType == "Supervisory Staff")
            {
                if (selectedMode == "Registration")
                {
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
                else if (selectedMode == "Examination")
                {
                    AttendanceForm AttendanceForm = new AttendanceForm();
                    AttendanceForm.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Invalid Code or Password.",
                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateUser(string instituteCode, string password, int selectedClassIndex, string selectedMode, int selectedSessIndex, string loginType)
        {
            bool isValid = false;

            try
            {
                int cls = selectedClassIndex;
                int sess = selectedSessIndex;
                UserSession.Class = cls;
                UserSession.Sess = sess;
                int ExamName = 0;
                int ExamType = 0;
                UserSession.Year = ConfigurationManager.GetYear(cls, sess);

                if (cls == 9 || cls == 10)
                {
                    ExamName = 1;
                }
                else if (cls == 11 || cls == 12)
                {
                    ExamName = 3;
                }

                if (sess == 1)
                {
                    ExamType = 0;
                }
                else if (sess == 2)
                {
                    ExamType = 1;
                }

                string query;
                SqlParameter[] parameters;

                if (loginType == "Institute")
                {
                    query = @"SELECT CONCAT(Inst_cd, ' - ', Name) Name ,null cent_cd 
                      FROM LiveDB..tblinstitutes_all 
                      WHERE Inst_cd = @InstituteCode 
                        AND Inst_pwd = @Password 
                        AND IsActive = 1";

                    parameters = new SqlParameter[]
                    {
                    new SqlParameter("@InstituteCode", instituteCode),
                    new SqlParameter("@Password", password)
                    };
                }
                else if (loginType == "IT Teacher")
                {
                    query = @"select a.FirstName Name, a.TeacherID,b.InstituteCode cent_cd from LiveDB..Teacher  a
                    inner join  LiveDB..Teacher_Duties  b on a.TeacherId = b.TeacherID
                    where a.TeacherID = @TeacherID and a.TeacherPassword =@Password and
                    b.IsActive = 1 and b.isIT = 1 and b.ExamName = @ExamName and b.ExamType = @ExamType and b.Year = @year";
                    parameters = new SqlParameter[]
                    {
                    new SqlParameter("@TeacherID", instituteCode),
                    new SqlParameter("@Password", password),
                    new SqlParameter("@year", UserSession.Year),
                    new SqlParameter("@ExamName", ExamName),
                    new SqlParameter("@ExamType", ExamType),
                    new SqlParameter("@ExamType", ExamType),
                    };
                }
                else if (loginType == "Supervisory Staff")
                {
                    // Bypass DB query as per user request
                    UserSession.InstituteCode = instituteCode;
                    UserSession.Cent_cd = instituteCode;
                    UserSession.Name = "Supervisory Staff - Center No. " + instituteCode;
                    UserSession.Mode = selectedMode;
                    UserSession.LoginType = loginType;
                    return true;
                }
                else
                {
                    MessageBox.Show("Invalid login type selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

                if (result.Rows.Count > 0)
                {
                    isValid = true;
                    UserSession.InstituteCode = instituteCode.ToString();
                    UserSession.Name = result.Rows[0]["Name"].ToString();

                    UserSession.Cent_cd = result.Rows[0].IsNull("cent_cd")
                      ? null
                      : result.Rows[0]["cent_cd"].ToString();
                    UserSession.Mode = selectedMode;
                    UserSession.LoginType = loginType;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while validating the user: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValid;
        }


        private void ddlLoginType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlLoginType.BorderColor = Color.Silver;

            if (ddlLoginType.SelectedItem != null)
            {
                string selectedType = ddlLoginType.SelectedItem.ToString();

                if (selectedType == "Institute")
                {
                    ddlMode.Enabled = true;
                    textBoxInstituteCode.Text = string.Empty;
                    textBoxInstituteCode.PlaceholderText = "Institute Code";
                    textBoxInstituteCode.MaxLength = 6;

                }
                else if (selectedType == "IT Teacher")
                {
                    ddlMode.SelectedItem = "Examination";
                    ddlMode.Enabled = false;
                    textBoxInstituteCode.Text = string.Empty;
                    textBoxInstituteCode.PlaceholderText = "Teacher ID";
                    textBoxInstituteCode.MaxLength = 15;

                }
                else if (selectedType == "Supervisory Staff")
                {
                    ddlMode.Enabled = true;
                    textBoxInstituteCode.Text = string.Empty;
                    textBoxInstituteCode.PlaceholderText = "Center Number";
                    textBoxInstituteCode.MaxLength = 3;
                }
            }
        }

        private void TextBoxInstituteCode_TextChanged(object sender, EventArgs e)
        {
            if (ddlLoginType.SelectedItem?.ToString() == "IT Teacher")
            {
                // Simple CNIC masking #####-#######-#
                string currentText = textBoxInstituteCode.Text;
                string numericText = currentText.Replace("-", "");

                if (System.Text.RegularExpressions.Regex.IsMatch(numericText, @"^\d+$"))
                {
                    string formatted = numericText;
                    if (numericText.Length >= 5)
                        formatted = formatted.Insert(5, "-");
                    if (numericText.Length >= 12) // 5 + 7
                        formatted = formatted.Insert(13, "-");

                    if (currentText != formatted)
                    {
                        textBoxInstituteCode.Text = formatted;
                        textBoxInstituteCode.SelectionStart = formatted.Length; // Move cursor to end
                    }
                }
            }
        }

    }
}
