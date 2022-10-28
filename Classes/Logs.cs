using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ServerBlockedPurchases.Classes
{
    public static class Log
    {
        public static RichTextBox rtb;
        public static void SetLogsRichTb(RichTextBox richtb)
        {
            rtb = richtb;
        }

        private delegate void DelegLog(string msg);

        public static void Msg(string msg)
        {
            try
            {
                if (rtb.InvokeRequired)
                {
                    DelegLog d = new DelegLog(Msg);
                    rtb.Invoke(d, new object[] { msg });
                }
                else
                {
                    if (!string.IsNullOrEmpty(msg))
                    {
                        string time = DateTime.Now.ToString("dd.MM HH:mm:ss");
                        StringBuilder sb = new StringBuilder(time + "  " + msg);
                        sb.AppendLine();
                        sb.AppendLine(rtb.Text);
                        rtb.Text = sb.ToString();
                    }
                }
            }
            catch { }
        }

    }
}
