using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poker_Server_v1._1
{
    public partial class mainForm : Form
    {
        private Server server;
        public mainForm()
        {
            InitializeComponent();
            Globals.txtLogsHandle = this.txtLogs;
            server = new Server();
            Globals.DB = new Database();
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            if (server.State == ServerStatus.Stoped)
            {
                if (server.Start(this.txtIpAddress.Text, Int16.Parse(this.nudPort.Value.ToString())))
                {
                    txtIpAddress.Enabled = false;
                    nudPort.Enabled = false;
                    btnRun.Text = "Stop";
                    lblStatus.Text = "Running";
                    lblStatus.ForeColor = Color.Green;
                }
            }
            else if(server.State == ServerStatus.Running)
            {
                if (server.Stop())
                {
                    txtIpAddress.Enabled = true;
                    nudPort.Enabled = true;
                    btnRun.Text = "Start";
                    lblStatus.Text = "Disconnected";
                    lblStatus.ForeColor = Color.Red;
                }                
            }
        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            
        }
        private void btnRemuse_Click(object sender, EventArgs e)
        {

        }
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
                server.Stop();
        }
    }
}
