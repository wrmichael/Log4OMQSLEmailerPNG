using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log4OMQSLEmailer
{
    public partial class DataLayout : Form
    {
        public DataLayout()
        {
            InitializeComponent();
        }

        private void DataLayout_Load(object sender, EventArgs e)
        {
            txtBandX.Text = Properties.Settings.Default.BAND_X;
            txtBandY.Text = Properties.Settings.Default.BAND_Y;
            txtModeX.Text = Properties.Settings.Default.MODE_X;
            txtModeY.Text = Properties.Settings.Default.MODE_Y;
            txtDateX.Text = Properties.Settings.Default.DATE_X;
            txtDateY.Text = Properties.Settings.Default.DATE_Y;
            txtRSTX.Text = Properties.Settings.Default.RST_X;
            txtRSTY.Text = Properties.Settings.Default.RST_Y;
            txtTimeX.Text = Properties.Settings.Default.TIME_X;
            txtTimeY.Text = Properties.Settings.Default.TIME_Y;
            txtCallX.Text = Properties.Settings.Default.CALL_X;
            txtCallY.Text = Properties.Settings.Default.CALL_Y;
            txtFontSize.Text = Properties.Settings.Default.FontSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
             Properties.Settings.Default.BAND_X = txtBandX.Text;
            Properties.Settings.Default.BAND_Y = txtBandY.Text;
            Properties.Settings.Default.MODE_X= txtModeX.Text  ;
             Properties.Settings.Default.MODE_Y= txtModeY.Text;
             Properties.Settings.Default.DATE_X = txtDateX.Text;
             Properties.Settings.Default.DATE_Y = txtDateY.Text;
             Properties.Settings.Default.RST_X = txtRSTX.Text;
              Properties.Settings.Default.RST_Y =txtRSTY.Text;
             Properties.Settings.Default.TIME_X = txtTimeX.Text;
             Properties.Settings.Default.TIME_Y= txtTimeY.Text;
             Properties.Settings.Default.CALL_X= txtCallX.Text;
             Properties.Settings.Default.CALL_Y = txtCallY.Text;
            Properties.Settings.Default.FontSize = txtFontSize.Text;
            Properties.Settings.Default.Save();
            this.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
