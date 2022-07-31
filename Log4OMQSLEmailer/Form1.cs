using System;
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
            if (Properties.Settings.Default.QSLDir.Trim().Length == 0)
            {
                return;
            }

            foreach (string d in System.IO.Directory.GetFiles(Properties.Settings.Default.QSLDir))
            {


                if (d.ToUpper().Contains(".PNG") || d.ToUpper().Contains(".JPG"))
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

        string fixADIFTime(string inTime)
        {
            return inTime.Substring(0, 2) + ":" + inTime.Substring(2, 2) + ":" + inTime.Substring(4, 2); 
        }

        string fixDate(string inDate)
        {
            return inDate.Substring(4, 2) + "/" + inDate.Substring(6, 2) + "/" + inDate.Substring(0, 4);
        }

        private void ProcessADIF()
        {

            if (System.IO.File.Exists(txtADIFFile.Text))
            {
                ADIFReader ar = new ADIFReader();
                ar.ADIFPath = txtADIFFile.Text;

                ar.start();
                foreach (ADIFRecord rec in ar.records)
                {

                    if (rec.email.Trim().Length == 0)
                    {
                        continue;
                    }

                    try
                    {
                        string mytime =  fixADIFTime(rec.time);
                        string mydate = fixDate(rec.date);
                        string mycall = rec.call;
                        string myname = rec.name;
                        string myemail = rec.email;
                        string rst = rec.sent;

                        string band = rec.band;
                        string mode = rec.mode;
                        string myqsoid = rec.call + "_" + rec.mode + "_" + rec.band + "_" + rec.date + "_" + rec.time + "_" + rec.band;

                        //strip any non filename values... 
                        string exclude_char = "*()!@#$%^&+={}[]|\\;:'\"?/.,<>~`";

                        foreach(char c in exclude_char.ToArray())
                        {
                            myqsoid = myqsoid.Replace(c, '_');
                        }


                        if (Properties.Settings.Default.ExclusionList.Contains("," + mycall.ToUpper().Trim() + ","))
                        {
                            continue;
                        }
                        
                        
                        //check to see if it is a DUP 
                        
                        if (checklog("," + myqsoid + ","))
                        {
                            continue; // skip printing and sending
                        }
                        //writetolog("," + myqsoid + ",");

                        //Image img = Image.FromFile(listBox1.SelectedItem.ToString());

                        string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, myqsoid);

                        string imgext = System.IO.Path.GetExtension(listBox1.SelectedItem.ToString());

                        ImageWriter iw = new ImageWriter();
                        
                            iw.writeImage(listBox1.SelectedItem.ToString(), myfile, band, mode, mycall, rst, mydate, mytime);
                        
                        /*
                        //save PNG here
                        Graphics g = Graphics.FromImage(img);

                        QSLLayout ql = new QSLLayout();

                        string layoutfile = listBox1.SelectedItem.ToString();
                        layoutfile = System.IO.Path.GetFileNameWithoutExtension(layoutfile) + ".layout";
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(layoutfile))
                        {
                            string sql = sr.ReadToEnd();
                            ql = JsonConvert.DeserializeObject<QSLLayout>(sql);
                            sr.Close();
                        }
                        Font font = new Font("Arial", ql.FontSize, FontStyle.Bold, GraphicsUnit.Pixel);


                        g.DrawString(band, font, Brushes.Black, ql.Band);
                        g.DrawString(mycall, font, Brushes.Black, ql.Callsign);
                        g.DrawString(mode, font, Brushes.Black, ql.Mode);

                        g.DrawString(mydate, font, Brushes.Black, ql.Date);
                        g.DrawString(mytime, font, Brushes.Black, ql.Time);
                        g.DrawString(rst, font, Brushes.Black, ql.SentRST);
                        */



                        /*
                        Font font = new Font("Arial", int.Parse(Properties.Settings.Default.FontSize), FontStyle.Bold, GraphicsUnit.Pixel);

                        g.DrawString(band, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.BAND_X), int.Parse(Properties.Settings.Default.BAND_Y)));
                        g.DrawString(mycall, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.CALL_X), int.Parse(Properties.Settings.Default.CALL_Y)));
                        g.DrawString(mode, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.MODE_X), int.Parse(Properties.Settings.Default.MODE_Y)));

                        g.DrawString(mydate, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.DATE_X), int.Parse(Properties.Settings.Default.DATE_Y)));
                        g.DrawString(mytime, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.TIME_X), int.Parse(Properties.Settings.Default.TIME_Y)));
                        g.DrawString(rst, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.RST_X), int.Parse(Properties.Settings.Default.RST_Y)));
                        */


                        //img.Save(System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, myqsoid + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                        this.MySendMail(myname, mycall, myfile + imgext, myemail, Properties.Settings.Default.MessageBody.Replace("<NAME>", myname));
                        //int rc = LookupQSLConformation(myqsoid);
                        lstlog.Items.Add(mydate + " - " + mycall + " - " + band + " - " + mode + " - " + myqsoid);
                        writetolog(","+ myqsoid +",");
                        System.Windows.Forms.Application.DoEvents();
                        try
                        {
                            System.Threading.Thread.Sleep(50);
                            System.IO.File.Delete(myfile + imgext);

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

                }


            }
         }
        
        private void btnQuery_Click(object sender, EventArgs e)
        {

            string imgext = System.IO.Path.GetExtension(listBox1.SelectedItem.ToString());


            string layoutfile = listBox1.SelectedItem.ToString();
            layoutfile = System.IO.Path.GetFileNameWithoutExtension(layoutfile) + ".layout";
            if (!System.IO.File.Exists(layoutfile))
            {
                MessageBox.Show("Missing layout settings for QSL Image");
                return;
            }

            if (txtADIFFile.Text.Trim().Length > 0)
            {
                this.ProcessADIF();
                MessageBox.Show("complete");
                return;
            }

            if (listBox1.SelectedIndex == -1)
            {
                return;
            }
            if (listBox1.SelectedItem.ToString().Trim().Length == 0)
            {
                return;
            }

            if (Properties.Settings.Default.QSLDir.Trim().Length == 0)
            {
                return;
            }

            if (Properties.Settings.Default.TMPDIR.Trim().Length ==0)
            {
                return;
            }

            if (!System.IO.Directory.Exists(Properties.Settings.Default.TMPDIR))
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
                    if (checklog(  "," + myqsoid + ","))
                    {
                        continue; // skip printing and sending
                    }
                    
                    //Image img = Image.FromFile(listBox1.SelectedItem.ToString());
                    
                    string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR  ,  myqsoid );
                    //save PNG here
                    
                    ImageWriter iw = new ImageWriter();
                    
                    Image img = iw.writeImage(listBox1.SelectedItem.ToString(), myfile, band, mode, mycall, rst, mydate, mytime);


                    //Graphics g = Graphics.FromImage(img);
                    //Font font = new Font("Arial", int.Parse(Properties.Settings.Default.FontSize), FontStyle.Bold, GraphicsUnit.Pixel);

                    /*
                    QSLLayout ql = new QSLLayout();

                    using (System.IO.StreamReader sr = new System.IO.StreamReader(layoutfile))
                    {
                        string sql = sr.ReadToEnd();
                        ql = JsonConvert.DeserializeObject<QSLLayout>(sql);
                        sr.Close();
                    }*/
                    
                    /*
                    Font font = new Font("Arial", ql.FontSize, FontStyle.Bold, GraphicsUnit.Pixel);


                    g.DrawString(band, font, Brushes.Black, ql.Band);
                    g.DrawString(mycall, font, Brushes.Black, ql.Callsign);
                    g.DrawString(mode, font, Brushes.Black, ql.Mode);

                    g.DrawString(mydate, font, Brushes.Black, ql.Date);
                    g.DrawString(mytime, font, Brushes.Black, ql.Time);
                    g.DrawString(rst, font, Brushes.Black, ql.SentRST);



                    /*
                    g.DrawString(band, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.BAND_X), int.Parse(Properties.Settings.Default.BAND_Y)));
                    g.DrawString(mycall, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.CALL_X), int.Parse(Properties.Settings.Default.CALL_Y)));
                    g.DrawString(mode, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.MODE_X), int.Parse(Properties.Settings.Default.MODE_Y)));

                    g.DrawString(mydate, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.DATE_X), int.Parse(Properties.Settings.Default.DATE_Y)));
                    g.DrawString(mytime, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.TIME_X), int.Parse(Properties.Settings.Default.TIME_Y)));
                    g.DrawString(rst, font, Brushes.Black, new PointF(int.Parse(Properties.Settings.Default.RST_X), int.Parse(Properties.Settings.Default.RST_Y)));
                    


                    img.Save(System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, myqsoid + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                    */
                    this.MySendMail(myname, mycall, myfile + imgext, myemail,Properties.Settings.Default.MessageBody.Replace("<NAME>",myname));
                    int rc = LookupQSLConformation(myqsoid);
                    writetolog("," + myqsoid + ",");
                    lstlog.Items.Add(mydate + " - " + mycall + " - " + band + " - " + mode + " - " + myqsoid);
                    
                    System.Windows.Forms.Application.DoEvents();
                    try
                    {
                        System.Threading.Thread.Sleep(50);
                   System.IO.File.Delete(myfile + imgext);

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

            //cmbImageType.Text = "JPG";
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            
            ofd.ShowDialog();
            txtADIFFile.Text = ofd.FileName;

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lookupInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ADIFLookupInfo lk = new ADIFLookupInfo();
            lk.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem.ToString().Trim().Length == 0)
            {
                return;    
            }

            string layoutfile = this.listBox1.SelectedItem.ToString();

            layoutfile = System.IO.Path.GetFileNameWithoutExtension(layoutfile) + ".layout";

            DataLayoutByImage dblbi = new DataLayoutByImage();
            dblbi.QSLImage = this.listBox1.SelectedItem.ToString();
            dblbi.LayoutFile = layoutfile;
            dblbi.ShowDialog();

        }
    }
}

