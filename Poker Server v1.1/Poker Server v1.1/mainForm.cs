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
        private void btnInstall_Click(object sender, EventArgs e)
        {
            Globals.addLog("Installing...", Color.Yellow);
            if (!Globals.DB.installDatabaseColumns()) {
                Globals.addLog("an error accured while installing Database..", Color.Red);
                return;
            }
            Globals.addLog("Database installed successfully",Color.Green);
            Globals.addLog("Installation ended successfully", Color.Green);
        }

        private void btnAddHNL_Click(object sender, EventArgs e)
        {
            Globals.DB.addTable(txtAddIdHNL.Text, txtAddNameHNL.Text, "HNL",txtAddMaxHNL.Text,txtAddMinHNL.Text,txtAddBigBlindHNL.Text,(int)nudAddSeatsHNL.Value);
            Globals.addLog("New No Limit Holdem Created..", Color.Green);
        }

        private void btnAddHL_Click(object sender, EventArgs e)
        {
            Globals.DB.addTable(txtAddIdHL.Text, txtAddNameHL.Text, "HL", txtAddMaxHL.Text, txtAddMinHL.Text, txtAddBigBlindHL.Text, (int)nudAddSeatsHL.Value);
            Globals.addLog("New Limited Holdem Created..", Color.Green);
        }

        private void btnAddONL_Click(object sender, EventArgs e)
        {
            Globals.DB.addTable(txtAddIdONL.Text, txtAddNameONL.Text, "ONL", txtAddMaxONL.Text, txtAddMinONL.Text, txtAddBigBlindONL.Text, (int)nudAddSeatsONL.Value);
            Globals.addLog("New No Limit Omaha Created..", Color.Green);
        }

        private void btnAddOL_Click(object sender, EventArgs e)
        {
            Globals.DB.addTable(txtAddIdOL.Text, txtAddNameOL.Text, "ONL", txtAddMaxOL.Text, txtAddMinOL.Text, txtAddBigBlindOL.Text, (int)nudAddSeatsOL.Value);
            Globals.addLog("New Limited Omaha Created..", Color.Green);
        }
    }
}
