using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Secugen_HU20
{
    public static class NetworkHelper
    {
        public static bool IsInternetAvailable()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 2000); // Google DNS
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void ShowInternetWarningIfOffline()
        {
            if (!IsInternetAvailable())
            {
                MessageBox.Show(
                    "Internet connection is not available.\nPlease check your internet connection.",
                    "Connectivity Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
