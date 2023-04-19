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
    public partial class BySKCC : Form
    {

        public Form1 form1;

        public string BureuUsers = ""; 

        public void QueryByCallSign(bool ignoreQSLStatus = false, bool ignoreEmail = false)
        {

            //connect to databsae 
            MySqlConnector.MySqlConnectionStringBuilder b = new MySqlConnector.MySqlConnectionStringBuilder
            {
                Server = Properties.Settings.Default.DBHost,
                UserID = Properties.Settings.Default.DBUser,
                Password = Properties.Settings.Default.DBPassword,
                Database = Properties.Settings.Default.DBDatabase,
                DateTimeKind = MySqlConnector.MySqlDateTimeKind.Utc
                

            };
//            b.DateTimeKind = MySqlConnector.MySqlDateTimeKind.Utc;
            MySqlConnector.MySqlConnection sqlcon = new MySqlConnector.MySqlConnection(b.ConnectionString);
            sqlcon.Open();
            MySqlConnector.MySqlCommand com = new MySqlConnector.MySqlCommand();
            com.Connection = sqlcon;

            string mysql = "";

            if (ignoreQSLStatus)
            {
                if (ignoreEmail)
                {
                    mysql = @"select qsoid, callsign, DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name from log where callsign in (" + BureuUsers + ")";
                }
                else
                {
                    mysql = @"select qsoid, callsign, DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name from log where  email <> '' and callsign in (" + BureuUsers + ")";
                }
            }
            else
            {
                if (ignoreEmail)
                {
                    mysql = @"select qsoid, callsign, DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name, j.* 
from log,JSON_TABLE(log.qsoconfirmations,'$[*]'
COLUMNS (
	ct VARCHAR(10) PATH '$.CT', S VARCHAR(10) PATH '$.S',
    R VARCHAR(10) PATH '$.R', 
      SV VARCHAR(100) PATH '$.SV',
      RV VARCHAR(100) PATH '$.RV',
      SD VARCHAR(100) PATH '$.SD',
      RD VARCHAR(100) PATH '$.RD' ) ) as j where j.ct = 'QSL' and j.S <> 'Yes' and callsign in (" + BureuUsers + ")";
                }
                else
                { 
                mysql = @"select qsoid, callsign, DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name, j.* 
from log,JSON_TABLE(log.qsoconfirmations,'$[*]'
COLUMNS (
	ct VARCHAR(10) PATH '$.CT', S VARCHAR(10) PATH '$.S',
    R VARCHAR(10) PATH '$.R', 
      SV VARCHAR(100) PATH '$.SV',
      RV VARCHAR(100) PATH '$.RV',
      SD VARCHAR(100) PATH '$.SD',
      RD VARCHAR(100) PATH '$.RD' ) ) as j where j.ct = 'QSL' and j.S <> 'Yes' and email <> '' and callsign in (" + BureuUsers + ")";
                }
            }
            com.CommandText = mysql;
            
            //com.Parameters.Add("?callsign", DbType.String).Value = txtCallSign.Text;
            
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

                    if (ckcw.Checked)
                    {
                        if (!mode.ToUpper().Equals("CW"))
                        {
                            continue;
                        }
                    
                    }

                    string myqsoid = reader["qsoid"].ToString();
                    //if (mycall.Equals("WM4P"))
                    //{
                    //    System.Console.Write("Stop");
                    //}

                    //if (form1.duplicateByDateCheck(mycall, mode, band))
                    //{
                    //    if (ckQSOB4.Checked)
                    //    {
                    //        continue;
                    //    }
                    //}


                    //double check to make sure we don't have  a duplicate for this day. 



                    //"qsoid,callsign,qsodate,email,band,mode,rstsent,name"
                    ListViewItem li = new ListViewItem();
                    li.Text = myqsoid;
                    li.SubItems.Add(mycall);
                    li.SubItems.Add(mydate);
                    li.SubItems.Add(mytime);
                    li.SubItems.Add(myemail);
                    li.SubItems.Add(band);
                    li.SubItems.Add(mode);
                    li.SubItems.Add(rst);
                    li.SubItems.Add(myname);

                    bool duplicate = form1.duplicateByDateCheck(mycall, mode, band);
                    if (duplicate)
                    {
                        if (ckQSOB4.Checked)
                        {
                            continue;
                        }
                        else
                        {

                        }
                    }

                    li.SubItems.Add(duplicate.ToString());
                    int sqsl = form1.sQSLBefore(mycall);
                    int rqsl = form1.rQSLBefore(mycall);

                    li.SubItems.Add(rqsl.ToString());
                    li.SubItems.Add(sqsl.ToString());

                    //show liars  (wants cards but return cards)
                    if (sqsl > 0 && rqsl <= 0)
                    {
                        li.BackColor = Color.Red;
                        if (ckDeadBeat.Checked)
                        {
                            continue;
                        }
                    }
                    if (rst.Trim().Length == 0)
                    {
                        //this would make a bad print so warn with a color 
                        li.BackColor = Color.MistyRose;
                    }

                    listView1.Items.Add(li);


                }
                catch (Exception exx)
                {
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Refresh();

             }
        }

        public bool duplicateByDateCheck(string callsign, string mode, string band)
        {

            try
            {
                //connect to databsae 
                MySqlConnector.MySqlConnectionStringBuilder b = new MySqlConnector.MySqlConnectionStringBuilder
                {
                    Server = Properties.Settings.Default.DBHost,
                    UserID = Properties.Settings.Default.DBUser,
                    Password = Properties.Settings.Default.DBPassword,
                    Database = Properties.Settings.Default.DBDatabase,
                    DateTimeKind = MySqlConnector.MySqlDateTimeKind.Utc


                };
                //if all the above return AllowDrop record we have a dup
                //connect to databsae 
                MySqlConnector.MySqlConnection sqlcon2 = new MySqlConnector.MySqlConnection(b.ConnectionString);
                sqlcon2.Open();

                MySqlConnector.MySqlCommand com = new MySqlConnector.MySqlCommand();
                com.Connection = sqlcon2;

                string mysql = "";

                mysql = @"select qsoid, callsign, DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email, band, mode, rstsent,name, j.* 
from log,JSON_TABLE(log.qsoconfirmations,'$[*]'
COLUMNS (
	ct VARCHAR(10) PATH '$.CT', S VARCHAR(10) PATH '$.S',
    R VARCHAR(10) PATH '$.R', 
      SV VARCHAR(100) PATH '$.SV',
      RV VARCHAR(100) PATH '$.RV',
      SD VARCHAR(100) PATH '$.SD',
      RD VARCHAR(100) PATH '$.RD' ) ) as j where j.ct = 'QSL' and j.S = 'Yes' and callsign = ?callsign and band = ?band and ?mode = mode ";
                com.CommandText = mysql;

                com.Parameters.Add("?callsign", DbType.String).Value = callsign;
                com.Parameters.Add("?band", DbType.String).Value = band;
                com.Parameters.Add("?mode", DbType.String).Value = mode;


                MySqlConnector.MySqlDataReader reader = com.ExecuteReader();


                while (reader.Read())
                {
                    reader.Close();
                    com.Dispose();
                    sqlcon2.Close();
                    System.GC.Collect();
                    return true;
                }
                try
                {
                    reader.Close();
                    com.Dispose();
                    sqlcon2.Close();
                    System.GC.Collect();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception exs)
            { 
            
            }
            return false; 
        }

        public BySKCC()
        {
            InitializeComponent();
        }

        private void ByCallSign_Load(object sender, EventArgs e)
        {

            string[] qsofields = "qsoid,callsign,qsodate,qsotime,email,band,mode,rstsent,name,Duplicate,rQSL,sQSL".Split(',');


            listView1.Items.Clear();
            listView1.View = System.Windows.Forms.View.Details;
            listView1.FullRowSelect = true;
            listView1.MultiSelect = true;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.Refresh();
            foreach (string s in qsofields)
            {
                if (s.Length > 0)
                {
                    listView1.Columns.Add(s);
                }
            }
            button3_Click(sender, e);

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            listView1.Items.Clear();
            this.QueryByCallSign(ckIgnoreQSL.Checked);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string imgext = System.IO.Path.GetExtension(this.listBox1.SelectedItem.ToString());

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                //"qsoid,callsign,qsodate,email,band,mode,rstsent,name"
                string mycall = item.SubItems[1].Text;
                string myqsoid = item.SubItems[0].Text;
                string band = item.SubItems[5].Text;
                string mydate = item.SubItems[2].Text;
                string myname = item.SubItems[8].Text;
                string rst = item.SubItems[7].Text.Trim();
                string mode = item.SubItems[6].Text;    
                string myemail = item.SubItems[4].Text;
                string mytime = item.SubItems[3].Text;

               



                ImageWriter iw = new ImageWriter();
                string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, myqsoid);

                Image img = iw.writeImage(this.listBox1.SelectedItem.ToString(), myfile, band, mode, mycall, rst, mydate, mytime);


                form1.MySendMail(myname, mycall, myfile + imgext, myemail, Properties.Settings.Default.MessageBody.Replace("<NAME>", myname), Properties.Settings.Default.YourCallSign);

                int rc = form1.LookupQSLConformation(myqsoid);
                form1.writetolog("," + myqsoid + ",");
                form1.lstlog.Items.Add(mydate + " - " + mycall + " - " + band + " - " + mode + " - " + myqsoid);

                System.Windows.Forms.Application.DoEvents();
                try
                {
                    System.Threading.Thread.Sleep(50);                   
                    System.IO.File.Delete(myfile + imgext);
                   
                }
                catch (Exception fex)
                {
                    form1.lstlog.Items.Add(mycall + " - QSOID:" + myqsoid + " - " + myfile + imgext + " error deleting file: " + fex.Message);
                    //ignore this...
                    System.Console.WriteLine(fex.Message);

                }
            }

        }

        private void txtCallSign_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCallSign_KeyPress(object sender, KeyPressEventArgs e)
       {
            if (e.KeyChar == (char)Keys.Enter )
            {
                button1_Click(sender, new EventArgs());

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show("Error reading QSL folder:" + Properties.Settings.Default.QSLDir + "\r\n" + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Pleae select a card first");
                return;
            }

            string imgext = System.IO.Path.GetExtension(this.listBox1.SelectedItem.ToString());

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                //"qsoid,callsign,qsodate,email,band,mode,rstsent,name"
                string mycall = item.SubItems[1].Text;
                string myqsoid = item.SubItems[0].Text;
                string band = item.SubItems[5].Text;
                string mydate = item.SubItems[2].Text;
                string myname = item.SubItems[8].Text;
                string rst = item.SubItems[7].Text.Trim();
                string mode = item.SubItems[6].Text;
                string myemail = item.SubItems[4].Text;
                string mytime = item.SubItems[3].Text;

                ImageWriter iw = new ImageWriter();
                string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, mycall.Replace('/','_') + "_" + band + "_" + mode + "_" + mydate.Replace('/','-') + "_" + mytime.Replace(':',' ') + "_" + myqsoid);

                Image img = iw.writeImage(this.listBox1.SelectedItem.ToString(), myfile, band, mode, mycall, rst, mydate, mytime);


                //form1.MySendMail(myname, mycall, myfile + imgext, myemail, Properties.Settings.Default.MessageBody.Replace("<NAME>", myname), Properties.Settings.Default.YourCallSign);

                int rc = form1.UpdateQSLConfirmation(myqsoid,"Bureau");
                form1.writetolog("," + myqsoid + ",");
                form1.lstlog.Items.Add(mydate + " - " + mycall + " - " + band + " - " + mode + " - " + myqsoid);

                System.Windows.Forms.Application.DoEvents();
                
               
            }
        }
    }
}
