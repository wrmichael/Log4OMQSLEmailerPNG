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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            txtYourCallSign.Text = Properties.Settings.Default.YourCallSign;
            textBox1.Text = Properties.Settings.Default.QSLDir;
            txtDB.Text = Properties.Settings.Default.DBDatabase;
            txtDBPWD.Text = Properties.Settings.Default.DBPassword;
            txtDBUser.Text = Properties.Settings.Default.DBUser;
            txtDBServer.Text = Properties.Settings.Default.DBHost;

            txtSMTPHost.Text = Properties.Settings.Default.SMTPHost;
            txtSMTPUser.Text = Properties.Settings.Default.SMTPUser;
            txtSMTPPassword.Text = Properties.Settings.Default.SMTPPassword;
            txtSMTPPort.Text = Properties.Settings.Default.SMTPPort;
            txtBody.Text = Properties.Settings.Default.MessageBody;
            txtExclusionList.Text = Properties.Settings.Default.ExclusionList;
            txtTMP.Text = Properties.Settings.Default.TMPDIR;
            txtEnvelope.Text = Properties.Settings.Default.EnvelopePath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.YourCallSign = txtYourCallSign.Text;
            Properties.Settings.Default.QSLDir = textBox1.Text;
            Properties.Settings.Default.DBDatabase = txtDB.Text;
            Properties.Settings.Default.DBHost = txtDBServer.Text;
            Properties.Settings.Default.DBPassword = txtDBPWD.Text;
            Properties.Settings.Default.DBUser = txtDBUser.Text;

            Properties.Settings.Default.SMTPPassword = txtSMTPPassword.Text;
            Properties.Settings.Default.SMTPHost = txtSMTPHost.Text;
            Properties.Settings.Default.SMTPUser = txtSMTPUser.Text;
            Properties.Settings.Default.SMTPPort = txtSMTPPort.Text;
            Properties.Settings.Default.MessageBody = txtBody.Text;
            Properties.Settings.Default.ExclusionList = txtExclusionList.Text;
            Properties.Settings.Default.TMPDIR = txtTMP.Text;
            Properties.Settings.Default.EnvelopePath = txtEnvelope.Text;
            Properties.Settings.Default.Save();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            fbd.SelectedPath = textBox1.Text;
            fbd.ShowDialog();
            textBox1.Text = fbd.SelectedPath;
        }

        private void txtSMTPPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTmp_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = txtTMP.Text;
            fbd.ShowDialog();
            txtTMP.Text = fbd.SelectedPath;
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void btnEnv_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = textBox1.Text;
            fbd.ShowDialog();
            txtEnvelope.Text = fbd.SelectedPath;
        }
    }
}
