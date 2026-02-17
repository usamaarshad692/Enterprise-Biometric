using System;
using System.Windows.Forms;

namespace Secugen_HU20
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm()); // Start the MainForm as the Windows Form application
        }
    }
}
