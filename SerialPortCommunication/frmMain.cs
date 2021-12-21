using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PCComm;
namespace PCComm
{
    public partial class frmMain : Form
    {
        CommunicationManager comm = new CommunicationManager();
        string transType = string.Empty;
        public frmMain()
        {
            try {
                InitializeComponent();
                this.txtSend.KeyPress += txtSend_KeyPress;
            }catch(Exception ex) { }
        }

        void txtSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.KeyChar.Equals((char)13))
            {
                sendDataToPort();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                LoadValues();
                SetDefaults();
                SetControlState();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Failure occcured while loading!\n" +
                    //ex.Message);
            }
        }

        private void cmdOpen_Click(object sender, EventArgs e)
        {
            try
            {
                comm.Parity = cboParity.Text;
                comm.StopBits = cboStop.Text;
                comm.DataBits = cboData.Text;
                comm.BaudRate = cboBaud.Text;
                comm.DisplayWindow = rtbDisplay;
                comm.PortName = cboPort.SelectedItem.ToString();
                comm.OpenPort();

                cmdOpen.Enabled = false;
                cmdClose.Enabled = true;
                cmdSend.Enabled = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Method to initialize serial port
        /// values to standard defaults
        /// </summary>
        private void SetDefaults()
        {
            cboPort.SelectedIndex = 0;
            cboBaud.SelectedIndex = 5;
            cboParity.SelectedIndex = 0;
            cboStop.SelectedIndex = 1;
            cboData.SelectedIndex = 1;
        }

        /// <summary>
        /// methos to load our serial
        /// port option values
        /// </summary>
        private void LoadValues()
        {
            try {
                comm.SetPortNameValues(cboPort);
                comm.SetParityValues(cboParity);
                comm.SetStopBitValues(cboStop);
            }catch(Exception ex) { }
        }

        /// <summary>
        /// method to set the state of controls
        /// when the form first loads
        /// </summary>
        private void SetControlState()
        {
            rdoText.Checked = true;
            cmdSend.Enabled = false;
            cmdClose.Enabled = false;
        }

        private void sendDataToPort()
        {
            try
            {
                string TxText = txtSend.Text;
                if (cbxCR.Checked) TxText += "\r";
                if (cbxLF.Checked) TxText += "\n";
                comm.WriteData(TxText);
            }
            catch (Exception ex)
            {
            }
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            sendDataToPort();
        }

        private void rdoHex_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoHex.Checked == true)
            {
                comm.CurrentTransmissionType = PCComm.CommunicationManager.TransmissionType.Hex;
            }
            else
            {
                comm.CurrentTransmissionType = PCComm.CommunicationManager.TransmissionType.Text;
            }
        }

        void closeComPort()
        {
            try
            {
                comm.closePort();
                //cmdOpen.Enabled = true;
            }
            catch (Exception ex)
            {
                cmdOpen.Enabled = true;
            }
            finally
            {
                cmdOpen.Enabled = true;
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            closeComPort();
        }

        private void zoomdec_Click(object sender, EventArgs e)
        {
            try
            {
                rtbDisplay.ZoomFactor = (rtbDisplay.ZoomFactor - (float)0.2);
            }
            catch (Exception ex) { }
        }

        public void onAppClosing()
        {
            try
            {
                comm.closePort();
            }
            catch (Exception ex) { }
        }
        private void ZoomInc_Click(object sender, EventArgs e)
        {
            try
            {
                rtbDisplay.ZoomFactor = (rtbDisplay.ZoomFactor + (float)0.2);
            }
            catch (Exception ex) { }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.rtbDisplay.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.cboPort.Items.Clear();
                this.cboParity.Items.Clear();
                this.cboStop.Items.Clear();

                LoadValues();
                SetDefaults();
                SetControlState();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Failed to open\n" + ex.Message);
            }
        }
    }
}