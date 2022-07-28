﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
//using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace Log4OMQSLEmailer
{
    
    
    public partial class Form1 : Form
    {

        public string alllog = ""; 

        
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();

            f2.ShowDialog(this);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string d in System.IO.Directory.GetFiles(Properties.Settings.Default.QSLDir))
            {
                if (d.ToUpper().Contains(".PNG"))
                {
                    listBox1.Items.Add(d);
                }
            }
        }

        public int LookupQSLConformation(string qsoid)
        {

            MySqlConnector.MySqlConnectionStringBuilder b = new MySqlConnector.MySqlConnectionStringBuilder
            {
                Server = Properties.Settings.Default.DBHost,
                UserID = Properties.Settings.Default.DBUser,
                Password = Properties.Settings.Default.DBPassword,
                Database = Properties.Settings.Default.DBDatabase

            };
            MySqlConnector.MySqlConnection sqlcon = new MySqlConnector.MySqlConnection(b.ConnectionString);

            sqlcon.Open();
            string mysql = "select qsoconfirmations from log where qsoid = ?qsoid";
           
            MySqlConnector.MySqlCommand com = new MySqlConnector.MySqlCommand();
            com.Connection = sqlcon;
            com.CommandText = mysql;
            com.Parameters.Add("?qsoid", MySqlConnector.MySqlDbType.VarChar).Value = qsoid;
            
            MySqlConnector.MySqlDataReader reader = com.ExecuteReader();

            if (reader.Read())
            {
                string ret = reader["qsoconfirmations"].ToString();

                QSLConfirmation[] qsl = JsonConvert.DeserializeObject<QSLConfirmation[]>(ret);
                reader.Close();
                for (int idx = 0; idx<qsl.Count();idx++)
                { 

                    if (qsl[idx].CT.Equals("QSL"))
                    {
                        qsl[idx].S = "Yes";
                        qsl[idx].SV = "Electronic";
                        qsl[idx].SD = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss tt");
                        break;
                    }
                }

                //update code here 
                string qstr = JsonConvert.SerializeObject(qsl);

                mysql = "update log set qsoconfirmations = '" + qstr + "' where qsoid = ?qsoid2" ;
                com.Connection = sqlcon;
                
                com.CommandText = mysql;
                com.Parameters.Add("?qsoid2", MySqlConnector.MySqlDbType.VarChar).Value = qsoid;
                int rc = com.ExecuteNonQuery();

                sqlcon.Close();

                return rc;
            }
            else
            {

                return 0;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex == -1)
            {
                return;
            }
            if (listBox1.SelectedItem.ToString().Trim().Length == 0)
            {
                return;
            }
            string template = listBox1.SelectedItem.ToString();

            

            MySqlConnector.MySqlConnectionStringBuilder b = new MySqlConnector.MySqlConnectionStringBuilder
            {
                Server = Properties.Settings.Default.DBHost,
                UserID = Properties.Settings.Default.DBUser,
                Password = Properties.Settings.Default.DBPassword,
                Database = Properties.Settings.Default.DBDatabase

            };
            MySqlConnector.MySqlConnection sqlcon = new MySqlConnector.MySqlConnection(b.ConnectionString);
            sqlcon.Open();
            MySqlConnector.MySqlCommand com = new MySqlConnector.MySqlCommand();
            com.Connection = sqlcon;

            string mysql = "";

            if (txtStart.Text.Trim().Length > 0 & txtEnd.Text.Trim().Length == 0)
            {
                mysql = @"select qsoid, callsign, qsodate, email, band, mode, rstsent,name, j.* 
from log,JSON_TABLE(log.qsoconfirmations,'$[*]'
COLUMNS (
	ct VARCHAR(10) PATH '$.CT', S VARCHAR(10) PATH '$.S',
    R VARCHAR(10) PATH '$.R', 
      SV VARCHAR(100) PATH '$.SV',
      RV VARCHAR(100) PATH '$.RV',
      SD VARCHAR(100) PATH '$.SD',
      RD VARCHAR(100) PATH '$.RD' ) ) as j where j.ct = 'QSL' and qsodate > ?qsodate and j.S <> 'Yes' and email <> '';";

                com.CommandText = mysql;
                com.Parameters.Add("?qsodate", DbType.DateTime).Value = txtStart.Text;

            }

            if (txtStart.Text.Trim().Length > 0 & txtEnd.Text.Trim().Length > 0)
            {
                mysql = @"select qsoid, callsign, qsodate, email, band, mode, rstsent,name, j.* 
from log,JSON_TABLE(log.qsoconfirmations,'$[*]'
COLUMNS (
	ct VARCHAR(10) PATH '$.CT', S VARCHAR(10) PATH '$.S',
    R VARCHAR(10) PATH '$.R', 
      SV VARCHAR(100) PATH '$.SV',
      RV VARCHAR(100) PATH '$.RV',
      SD VARCHAR(100) PATH '$.SD',
      RD VARCHAR(100) PATH '$.RD' ) ) as j where j.ct = 'QSL' and qsodate > ?qsodate and qsodate < ?qsodate2  and j.S <> 'Yes' and email <> '';";

                com.CommandText = mysql;
                com.Parameters.Add("?qsodate", DbType.DateTime).Value = txtStart.Text;
                com.Parameters.Add("?qsodate2", DbType.DateTime).Value = txtEnd.Text;
            }

            //com.CommandText = "select * from log where callsign = 'KB9BVN' and email <> '' and qsodate <> '' LIMIT 2";
            MySqlConnector.MySqlDataReader reader = com.ExecuteReader();

            

            while (reader.Read())
            {

                try
                {
                    string mytime = "";
                    string mydate = "";
                    string mycall = reader["callsign"].ToString();
                    string myname = reader["name"].ToString();
                    string myemail = reader["email"].ToString();
                    string rst = reader["rstsent"].ToString();

                    if (Properties.Settings.Default.ExclusionList.Contains("," + mycall.ToUpper().Trim() + ","))
                    {
                        continue;
                    }                    
                    try
                    {
                        mytime = reader["qsodate"].ToString().Split(' ')[1].Substring(0).Trim();
                    }
                    catch (Exception ex)
                    {
                        mytime = "";
                    }
                    try
                    {

                        mydate = reader["qsodate"].ToString().Split(' ')[0];
                    }
                    catch (Exception ex)
                    {
                        mydate = "";
                    }
                    string band = reader["band"].ToString();
                    string mode = reader["mode"].ToString();
                    string myqsoid = reader["qsoid"].ToString();

                    //check to see if it is a DUP 
                    if (!checklog(  "," + myqsoid + ","))
                    {
                        continue; // skip printing and sending
                    }
                    writetolog("," +  myqsoid + ",");
                    Image img = Image.FromFile(listBox1.SelectedItem.ToString());
                    
                    string myfile = "c:\\tmp\\" + myqsoid + ".png";
                    //save PNG here
                    Graphics g = Graphics.FromImage(img);
                    Font font = new Font("Arial", int.Parse(Properties.Settings.Default.FontSize), FontStyle.Bold, GraphicsUnit.Pixel);

                    g.DrawString(band, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.BAND_X), int.Parse(Properties.Settings.Default.BAND_Y)));
                    g.DrawString(mycall, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.CALL_X), int.Parse(Properties.Settings.Default.CALL_Y)));
                    g.DrawString(mode, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.MODE_X), int.Parse(Properties.Settings.Default.MODE_Y)));

                    g.DrawString(mydate, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.DATE_X), int.Parse(Properties.Settings.Default.DATE_Y)));
                    g.DrawString(mytime, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.TIME_X), int.Parse(Properties.Settings.Default.TIME_Y)));
                    g.DrawString(rst, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.RST_X), int.Parse(Properties.Settings.Default.RST_Y)));



                    img.Save(@"c:\tmp\" + myqsoid + ".png", System.Drawing.Imaging.ImageFormat.Png);

                    this.MySendMail(myname, mycall, myfile, myemail,Properties.Settings.Default.MessageBody.Replace("<NAME>",myname));
                    int rc = LookupQSLConformation(myqsoid);
                    lstlog.Items.Add(mydate + " - " + mycall + " - " + band + " - " + mode + " - " + myqsoid);
                    
                    System.Windows.Forms.Application.DoEvents();
                    try
                    {
                        System.Threading.Thread.Sleep(50);
                   System.IO.File.Delete(myfile);

                }
                catch (Exception fex)
                {
                        //ignore this...
                    System.Console.WriteLine(fex.Message);

                }


            }
                catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);

            }

        
           
                System.Console.WriteLine(reader["callsign"].ToString());

            }
           
            MessageBox.Show("Complete");

        }

        void writetolog(string myqsl)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.Combine(Properties.Settings.Default.QSLDir, "log.txt"), true))
                {
                    sw.WriteLine(myqsl);
                    sw.Close();
                }
                alllog = alllog + "\r\n" + myqsl;
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Error writing to log: " + myqsl + " -- " + ex.Message);
                //its only a hobby - move on...             
            }
        }

        bool checklog(string myqsl)
        {
            
            if (alllog.Contains(myqsl))
            { return true; }
            else { return false; }

        }

        void MySendMail(string name, string call, string attachment, string email,string mybody)
        {
            using (System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage())
            {

                mm.To.Add(email);
                mm.From = new MailAddress(Properties.Settings.Default.SMTPUser);
                mm.Subject = "QSL for QSO with AC9HP";
                mm.Body = mybody;
                mm.Attachments.Add(new Attachment(attachment));

                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential("wrmichael@hotmail.com", "Ankle45DeepSecurity");
                smtp.Credentials = nc;
                try
                {
                    smtp.EnableSsl = true;
                    smtp.Send(mm);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {
            this.button1_Click(sender, e);
            if (System.IO.File.Exists(System.IO.Path.Combine(Properties.Settings.Default.QSLDir, "log.txt")))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(System.IO.Path.Combine(Properties.Settings.Default.QSLDir, "log.txt")))
                {
                    alllog = sr.ReadToEnd();
                    sr.Close();
                }
            }

        }

        private void layoytSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (DataLayout dl = new DataLayout())
            {
                dl.ShowDialog();
            }
        }
    }
}

