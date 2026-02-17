using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Secugen_HU20
{
    public partial class UserDetailsPopup : Form
    {
        public UserDetailsPopup(string formNo, string stdid, string name, string fatherName, string bForm, string fnic)
        {
            InitializeComponent();
            NetworkHelper.ShowInternetWarningIfOffline();
            lblHeading.Font = new Font(lblHeading.Font.FontFamily, 14);
            lblHeading.Font = new Font("Arial", 14, FontStyle.Bold);


            lblstdid.Text = $"Student Id.: {stdid}";
            lblFormno.Text = $"Form No.: {formNo}";
            labelName.Text = $"Name: {name}";
            labelFName.Text = $"Father's Name: {fatherName}";
            labelBForm.Text = $"B-Form: {bForm}";
            labelFNIC.Text = $"FNIC: {fnic}";

            try
            {
                string baseUrl = string.Empty;

                if (UserSession.LoginType == "Supervisory Staff")
                {
                    string imageUrl = string.Empty;
                    string bFormVal = bForm; // Use local variable

                    string apiUrl =
                        $"https://services.bisegrw.edu.pk/android/Biometrics/getStaffProfileImage.php?bform={bFormVal}";

                    try
                    {
                        using (var wc = new WebClient())
                        {
                            wc.Encoding = Encoding.UTF8;
                            string json = wc.DownloadString(apiUrl);

                            var serializer = new JavaScriptSerializer();
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
                    }
                    return; // Exit here for Supervisory Staff
                }

                if (UserSession.Class == 9)
                {
                    baseUrl = "https://services.bisegrw.edu.pk/Share%20Images/uploads/SSC/";
                }
                else if (UserSession.Class == 10)
                {
                    baseUrl = "https://services.bisegrw.edu.pk/Share%20Images/OldPics/Pics-M/";
                }
                else if (UserSession.Class == 11)
                {
                    baseUrl = "https://services.bisegrw.edu.pk/Share%20Images/uploads/HSSC/registration/";
                }
                else if (UserSession.Class == 12)
                {
                    baseUrl = "https://services.bisegrw.edu.pk/Share%20Images/OldPics/Pics-I/";
                }

                int selectedStudentId = int.Parse(stdid);
                int divider = (int)Math.Ceiling((double)selectedStudentId / 50000);
                string imageUrlStudent = $"{baseUrl}{divider}/{selectedStudentId}.jpg"; // Renamed to avoid conflict

                // Configure PictureBox
                pictureBoxImage.Size = new Size(130, 130);
                pictureBoxImage.SizeMode = PictureBoxSizeMode.StretchImage;

                // Attach event to handle errors
                pictureBoxImage.LoadCompleted += (s, eArgs) =>
                {
                    try
                    {
                        if (eArgs.Error != null) // Image failed to load
                        {
                            string backupUrl = string.Empty;

                            if (UserSession.Class == 9 || UserSession.Class == 10)
                            {
                                backupUrl = "https://services.bisegrw.edu.pk/Share%20Images/OldPics/Pics-M/";
                            }
                            else if (UserSession.Class == 11 || UserSession.Class == 12)
                            {
                                backupUrl = "https://services.bisegrw.edu.pk/Share%20Images/OldPics/Pics-I/";
                            }

                            string backupImageUrl = $"{backupUrl}{divider}/{selectedStudentId}.jpg";
                            pictureBoxImage.LoadAsync(backupImageUrl);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading backup image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // Try loading the primary image
                pictureBoxImage.LoadAsync(imageUrlStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

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

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }

}
