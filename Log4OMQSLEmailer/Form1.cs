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
            listBox1.Items.Clear();
            if (Properties.Settings.Default.QSLDir.Trim().Length == 0)
            {
                return;
            }

            try
            {
                foreach (string d in System.IO.Directory.GetFiles(Properties.Settings.Default.QSLDir))
                {


                    if (d.ToUpper().Contains(".PNG") || d.ToUpper().Contains(".JPG"))
                    {
                        listBox1.Items.Add(d);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading QSL folder:" + Properties.Settings.Default.QSLDir + "\r\n"+ ex.Message);
            }
        }


        
        public bool QSLBefore(string callsign, string band, string mode)
        {
            bool found =false;

            MySqlConnector.MySqlConnectionStringBuilder b = new MySqlConnector.MySqlConnectionStringBuilder
            {
                Server = Properties.Settings.Default.DBHost,
                UserID = Properties.Settings.Default.DBUser,
                Password = Properties.Settings.Default.DBPassword,
                Database = Properties.Settings.Default.DBDatabase

            };
            MySqlConnector.MySqlConnection sqlcon = new MySqlConnector.MySqlConnection(b.ConnectionString);

            sqlcon.Open();
            string mysql = "select qsoconfirmations from log where callsign = ?callsign and  band = ?band and mode = ?mode";

            MySqlConnector.MySqlCommand com = new MySqlConnector.MySqlCommand();
            com.Connection = sqlcon;
            com.CommandText = mysql;
            com.Parameters.Add("?callsign", MySqlConnector.MySqlDbType.VarChar).Value = callsign;
            com.Parameters.Add("?band", MySqlConnector.MySqlDbType.VarChar).Value = band;
            com.Parameters.Add("?mode", MySqlConnector.MySqlDbType.VarChar).Value = mode;

            MySqlConnector.MySqlDataReader reader = com.ExecuteReader();

            while(reader.Read())
            {
                string ret = reader["qsoconfirmations"].ToString();

                QSLConfirmation[] qsl = JsonConvert.DeserializeObject<QSLConfirmation[]>(ret);
                //reader.Close();
                for (int idx = 0; idx < qsl.Count(); idx++)
                {

                    if (qsl[idx].CT.Equals("QSL"))
                    {
                        if (qsl[idx].S.Equals("Yes"))
                        {
                            found = true;
                            break;
                        }

                    }
                }
                if (found)
                {
                    break;
                }
                
            }

            reader.Close();
            

            return found;
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

        public string fixADIFTime(string inTime)
        {
            string rt;
            try
            {
                rt = inTime.Substring(0, 2) + ":" + inTime.Substring(2, 2) + ":" + inTime.Substring(4, 2);
            } catch (Exception ex)
            {
                rt = inTime;
                writetodebuglog("error with time format for: " + inTime);
            }
            return rt;
        }

        public string fixDate(string inDate)
        {
            string rt;

            try
            {
                rt =  inDate.Substring(4, 2) + "/" + inDate.Substring(6, 2) + "/" + inDate.Substring(0, 4);

            }catch(Exception ex)
            {
                rt = inDate;
                writetodebuglog("error with date format for: " + inDate);
            }

            return rt;

        }

        private void ProcessADIF(bool deleteimage=true, bool mailimage=true)
        {
            int limit_count = 0;
            if (System.IO.File.Exists(txtADIFFile.Text))
            {
                if (Program.X_DEBUG)
                {
                    writetodebuglog("ADIF exists ");
                }
                ADIFReader ar = new ADIFReader();
                ar.ADIFPath = txtADIFFile.Text;
                if (this.ck_limit.Checked)
                {
                    ar.limit_count = 499;    
                }
                else {
                    ar.limit_count = 0;
                }
                ar.my_form1 = this; 
                ar.start(mailimage);

                if (Program.X_DEBUG)
                {
                    writetodebuglog("Before record loop");
                    writetodebuglog("count:" + ar.count.ToString());
                }
                

                foreach (ADIFRecord rec in ar.records)
                {
                    if (Program.X_DEBUG)
                    {
                        writetodebuglog("Loop Start ADIF ");
                    }
                    if (rec.email.Trim().Length == 0)
                    {
                        if (Program.X_DEBUG)
                        {
                            writetodebuglog("missing email 1");
                            
                        }
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

                        if (mytime.Equals("") && mydate.Equals(""))
                        {
                            //do not send a card -- some sort of error 
                            writetodebuglog("Missing date time for qso:" + myqsoid);
                            continue;
                        }

                            //strip any non filename values... 
                            string exclude_char = "*()!@#$%^&+={}[]|\\;:'\"?/.,<>~`";

                        foreach(char c in exclude_char.ToArray())
                        {
                            myqsoid = myqsoid.Replace(c, '_');
                        }


                        if (Properties.Settings.Default.ExclusionList.Contains("," + mycall.ToUpper().Trim() + ","))
                        {
                            if (Program.X_DEBUG)
                            {
                                writetodebuglog("in excluded list:" + mycall);

                            }
                            continue;
                        }


                        //check to see if it is a DUP
                        //No reason to add to log if making a local copy only 
                        if (mailimage)
                        {
                            if (checklog("," + myqsoid + ","))
                            {
                                if (Program.X_DEBUG)
                                {
                                    writetodebuglog("Already send qsoid " + myqsoid);
                                }

                                continue; // skip printing and sending
                            }
                        }
                        //writetolog("," + myqsoid + ",");

                        //Image img = Image.FromFile(listBox1.SelectedItem.ToString());

                        string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, myqsoid);

                        string imgext = System.IO.Path.GetExtension(listBox1.SelectedItem.ToString());
                        if (Program.X_DEBUG)
                        {
                            writetodebuglog("Before Image write ");
                        }

                        ImageWriter iw = new ImageWriter();
                        
                            iw.writeImage(listBox1.SelectedItem.ToString(), myfile, band, mode, mycall, rst, mydate, mytime);

                        if (Program.X_DEBUG)
                        {
                            writetodebuglog("After Image Write  ");
                        }

                        if (mailimage)
                        {
                            if (this.MySendMail(myname, mycall, myfile + imgext, myemail, Properties.Settings.Default.MessageBody.Replace("<NAME>", myname)))
                            {
                                //int rc = LookupQSLConformation(myqsoid);
                                lstlog.Items.Add(mydate + " - " + mycall + " - " + band + " - " + mode + " - " + myqsoid);
                                writetolog("," + myqsoid + ",");
                                if (Program.X_DEBUG)
                                {
                                    writetodebuglog("Mail message success!");

                                }
                                limit_count++;
                            }
                            else
                            {
                                lstlog.Items.Add("Check outbox -> Failed to send email for qsoid: " + myqsoid);
                                writetodebuglog("Failed to send mail for QSOID: " + myqsoid);
                                //force a sleep 
                                System.Threading.Thread.Sleep(3000);
                            }
                        }
                        System.Windows.Forms.Application.DoEvents();
                        try
                        {
                            if (deleteimage)
                            {
                                System.Threading.Thread.Sleep(50);
                                System.IO.File.Delete(myfile + imgext);
                            }
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
                if (Program.X_DEBUG)
                {
                    writetodebuglog("After record loop");

                }
                if (ck_limit.Checked)
                {
                    if (limit_count >= 499)
                    {
                        return;
                    }
                }
            }
         }
        

        
        private void btnQuery_Click(object sender, EventArgs e)
        {
            ProcessImages();

            return; 
            //sold code here.   

            string imgext = System.IO.Path.GetExtension(listBox1.SelectedItem.ToString());

            string layoutfile = this.listBox1.SelectedItem.ToString();

            layoutfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(layoutfile), System.IO.Path.GetFileNameWithoutExtension(layoutfile) + ".layout");

            //layoutfile = System.IO.Path.GetFileNameWithoutExtension(layoutfile) + ".layout";
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
                mysql = @"select qsoid, callsign,  DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name, j.* 
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
                mysql = @"select qsoid, callsign,  DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name, j.* 
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
                    string band = reader["band"].ToString();
                    string mode = reader["mode"].ToString();
                    string myqsoid = reader["qsoid"].ToString();

                    //Exclude people who don't want QSL cards
                    if (Properties.Settings.Default.ExclusionList.Contains("," + mycall.ToUpper().Trim() + ","))
                    {
                        continue;
                    }

                    //Check QSL Before 
                    if (this.ckQSLBefore.Checked)
                    {
                        if (QSLBefore(mycall, band, mode))
                        {
                            //mark it as NO and skip it 
                            continue;
                        }
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


        void ProcessImages(bool deleteimage = true, bool mailimage = true)
        {
            int email_count = 0;

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Select a template first!");
                return;
            }

            string imgext = System.IO.Path.GetExtension(listBox1.SelectedItem.ToString());

            string layoutfile = this.listBox1.SelectedItem.ToString();

            layoutfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(layoutfile), System.IO.Path.GetFileNameWithoutExtension(layoutfile) + ".layout");

            //layoutfile = System.IO.Path.GetFileNameWithoutExtension(layoutfile) + ".layout";
            if (!System.IO.File.Exists(layoutfile))
            {
                MessageBox.Show("Missing layout settings for QSL Image");
                return;
            }

            if (txtADIFFile.Text.Trim().Length > 0)
            {
                if (Program.X_DEBUG)
                {
                    writetodebuglog("Starting ADIF ");
                }
                lstlog.Items.Add("Processing ADIF file: " + txtADIFFile.Text.Trim());
                this.ProcessADIF(deleteimage, mailimage);
                if (Program.X_DEBUG)
                {
                    writetodebuglog("ADIF Complete");
                }
                MessageBox.Show("complete");
                return;
            }

            if (listBox1.SelectedIndex == -1)
            {
                lstlog.Items.Add("Must select a template first");
                return;
            }
            if (listBox1.SelectedItem.ToString().Trim().Length == 0)
            {
                lstlog.Items.Add("Must select a template first");
                return;
            }

            if (Properties.Settings.Default.QSLDir.Trim().Length == 0)
            {
                lstlog.Items.Add("QSL Folder is not defined. See settings");
                return;
            }

            if (Properties.Settings.Default.TMPDIR.Trim().Length == 0)
            {
                lstlog.Items.Add("TMP directory must be defined. See settings");
                return;
            }

            if (!System.IO.Directory.Exists(Properties.Settings.Default.TMPDIR))
            {
                lstlog.Items.Add("TMP directory does not exist.  Please create or select a new folder.");
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
                mysql = @"select qsoid, callsign,  DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name, j.* 
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
                mysql = @"select qsoid, callsign,  DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name, j.* 
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

                    if (myemail.Contains(" "))
                    {
                        lstlog.Items.Add("Invalid Email for " + mycall + " - " +myemail);
                        //invalid email - skip it 
                        continue;
                    }
                    if (!myemail.Contains("@"))
                    {
                        lstlog.Items.Add("Invalid Email for " + mycall + " - " + myemail);
                        //invalid email - skip it 
                        continue;
                    }
                    if (myemail.ToUpper().Contains("ARRL.NET"))  //TO DO MAKE THIS OPTIONAL 
                    {
                        lstlog.Items.Add("Skipping - ARRL Blocks as SPAM - Invalid Email for " + mycall + " - " + myemail);
                        continue;
                    }

                    if (Properties.Settings.Default.ExclusionList.Contains("," + mycall.ToUpper().Trim() + ","))
                    {
                        lstlog.Items.Add( mycall + " - Exclusion list - do not process");
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


                    //Check QSL Before 
                    if (this.ckQSLBefore.Checked)
                    {
                        if (QSLBefore(mycall, band, mode))
                        {
                            lstlog.Items.Add(mycall + " - QSL Before (Band/mode/call)");
                            //mark it as NO and skip it 
                            continue;
                        }
                    }



                    if (mailimage)
                    {
                        //check to see if it is a DUP 
                        if (checklog("," + myqsoid + ","))
                        {
                            lstlog.Items.Add(myqsoid + " - Duplicate processing,  skipping!");
                            continue; // skip printing and sending
                        }
                    }
                    //Image img = Image.FromFile(listBox1.SelectedItem.ToString());

                    string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, myqsoid);
                    //save PNG here

                    ImageWriter iw = new ImageWriter();

                    Image img = iw.writeImage(listBox1.SelectedItem.ToString(), myfile, band, mode, mycall, rst, mydate, mytime);

                    if (mailimage)
                    {
                        this.MySendMail(myname, mycall, myfile + imgext, myemail, Properties.Settings.Default.MessageBody.Replace("<NAME>", myname));

                        int rc = LookupQSLConformation(myqsoid);
                        writetolog("," + myqsoid + ",");
                        lstlog.Items.Add(mydate + " - " + mycall + " - " + band + " - " + mode + " - " + myqsoid);
                        System.Threading.Thread.Sleep(1500); // help reduce issues with MASS mailing 
                    }
                    System.Windows.Forms.Application.DoEvents();
                    try
                    {
                        System.Threading.Thread.Sleep(350);
                        if (deleteimage)
                        {
                            System.IO.File.Delete(myfile + imgext);
                        }
                    }
                    catch (Exception fex)
                    {
                        lstlog.Items.Add(mycall + " - QSOID:" + myqsoid + " - " + myfile + imgext + " error deleting file: " + fex.Message);
                        //ignore this...
                        System.Console.WriteLine(fex.Message);

                    }


                }
                catch (Exception ex)
                {
                    lstlog.Items.Add("General error: " + ex.Message);
                         
                    MessageBox.Show("Error:" + ex.Message);

                }



                System.Console.WriteLine(reader["callsign"].ToString());

            }

            MessageBox.Show("Complete");


        }

        public void writetodebuglog(string m)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.Combine(Properties.Settings.Default.QSLDir, "debug_log.txt"), true))
                {
                    sw.WriteLine(m);
                    sw.Close();
                }
             
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Error writing to debug log: " + m + " -- " + ex.Message);
                //its only a hobby - move on...             
            }
        }

        


        public void writetolog(string myqsl)
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

        public bool checklog(string myqsl)
        {
            
            if (alllog.Contains(myqsl))
            { return true; }
            else { return false; }

        }

        public bool MySendMail(string name, string call, string attachment, string email,string mybody)
        {
            using (System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage())
            {

                mm.To.Add(email);
                mm.From = new MailAddress(Properties.Settings.Default.SMTPUser);
                mm.Subject = "QSL for QSO";
                mm.Body = mybody;
                mm.Attachments.Add(new Attachment(attachment));



                SmtpClient smtp = new SmtpClient(Properties.Settings.Default.SMTPHost, int.Parse(Properties.Settings.Default.SMTPPort));
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(Properties.Settings.Default.SMTPUser, Properties.Settings.Default.SMTPPassword);
                smtp.Credentials = nc;
                try
                {
                    smtp.EnableSsl = true;
                    smtp.Send(mm);
                    if (Program.X_DEBUG)
                    {
                        writetodebuglog("SMPT Call made!");

                    }
                    return true;
                }
                catch (Exception ex)
                {
                    writetodebuglog("Error sending mail: " + ex.ToString());
                    return false;
                }
            }
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {

            string tmppath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string qslpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (Properties.Settings.Default.TMPDIR.Trim().Length == 0)
            {
                //predefine the TMP and QSL folders in documents 
                
                tmppath = System.IO.Path.Combine(tmppath, "TMP");
                Properties.Settings.Default.TMPDIR = tmppath;
                Properties.Settings.Default.Save();
                
            }
            try
            {
                if (!System.IO.Directory.Exists(Properties.Settings.Default.TMPDIR))
                {
                    System.IO.Directory.CreateDirectory(Properties.Settings.Default.TMPDIR);
                }
            }
            catch
            { //ignore for now
              }



                if (Properties.Settings.Default.QSLDir.Trim().Length == 0) 
            { 
                
                qslpath = System.IO.Path.Combine(qslpath, "QSL");
                Properties.Settings.Default.QSLDir = qslpath;
                Properties.Settings.Default.Save();


            }
                try
                {
                    if (!System.IO.Directory.Exists(Properties.Settings.Default.QSLDir))
                    {
                        System.IO.Directory.CreateDirectory(Properties.Settings.Default.QSLDir);
                    }
                }
                catch
                { //ignore for now
                  }



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

            layoutfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(layoutfile), System.IO.Path.GetFileNameWithoutExtension(layoutfile) + ".layout");


            DataLayoutByImage dblbi = new DataLayoutByImage();
            dblbi.QSLImage = this.listBox1.SelectedItem.ToString();
            dblbi.LayoutFile = layoutfile;
            dblbi.ShowDialog();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProcessImages(false,false);
            MessageBox.Show("Your files should be in " + Properties.Settings.Default.TMPDIR);
            
        }

        private void callsignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ByCallSign bcs = new ByCallSign();
            bcs.form1 = this;
            bcs.ShowDialog();

        }
    }
}

