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
    public partial class ADIFLookupInfo : Form
    {
        public ADIFLookupInfo()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.QRZCQUser= txtQRZCQUser.Text;
            Properties.Settings.Default.QRZCQPassword = txtQRZCWPwd.Text;
            Properties.Settings.Default.QRZPassword = txtQRZPwd.Text;
            Properties.Settings.Default.QRZUser = txtQRZUser.Text;

            Properties.Settings.Default.Save();

            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ADIFLookupInfo_Load(object sender, EventArgs e)
        {
            txtQRZCQUser.Text = Properties.Settings.Default.QRZCQUser;
            txtQRZCWPwd.Text = Properties.Settings.Default.QRZCQPassword;
            txtQRZPwd.Text = Properties.Settings.Default.QRZPassword;
            txtQRZUser.Text = Properties.Settings.Default.QRZUser;

        }
    }
}
