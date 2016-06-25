using System;
using System.Drawing;
using System.Windows.Forms;

namespace Poker_Server_v1._1
{
    static class Globals
    {
        public static RichTextBox txtLogsHandle;
        private delegate void ControlStringConsumer(string text, Color color);

        public static Database DB;

        public static Core GameCore;

        public static ServerStatus serverStatus;
        public static void addLog(string text, Color color)
        {
            if (txtLogsHandle != null)
            {

                if (txtLogsHandle.InvokeRequired)
                {
                    txtLogsHandle.Invoke(new ControlStringConsumer(addLog), new object[] { text, color });
                }
                else
                {
                    if (txtLogsHandle.Text.Length >= txtLogsHandle.MaxLength)
                        txtLogsHandle.Text = "";

                    txtLogsHandle.SelectionStart = txtLogsHandle.TextLength;
                    txtLogsHandle.SelectionLength = 0;
                    txtLogsHandle.SelectionColor = color;
                    txtLogsHandle.AppendText(text + Environment.NewLine);
                    txtLogsHandle.SelectionColor = txtLogsHandle.ForeColor;

                    txtLogsHandle.SelectionStart = txtLogsHandle.Text.Length;
                    // scroll it automatically
                    txtLogsHandle.ScrollToCaret();
                }
            }
        }
    }
}
