using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log4OMQSLEmailer
{
    public partial class DXSearch : Form
    {

        public Form1 form1;

        public DXSearch()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }


        public void BureauDXQuery()
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


            /*
             
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
             
             */


            mysql = @"select qsoid, callsign, DATE_FORMAT(qsodate,'%Y-%m-%d %T') as qsodate, email,mysiginfo, band, mode, rstsent,name, j.* 
from log,JSON_TABLE(log.qsoconfirmations,'$[*]'
COLUMNS (
	ct VARCHAR(10) PATH '$.CT', S VARCHAR(10) PATH '$.S',
    R VARCHAR(10) PATH '$.R', 
      SV VARCHAR(100) PATH '$.SV',
      RV VARCHAR(100) PATH '$.RV',
      SD VARCHAR(100) PATH '$.SD',
      RD VARCHAR(100) PATH '$.RD' ) ) as j where j.ct = 'QSL' and   qsodate > ?qsodate and qsodate < ?qsodate2  and j.S <> 'Yes'   and dxcc <> mydxcc ";

            com.CommandText = mysql;

            //com.Parameters.Add("?callsign", DbType.DateTime).Value = txtDxStart.Text;
            com.Parameters.Add("?qsodate", DbType.DateTime).Value = txtDxStart.Text;
            com.Parameters.Add("?qsodate2", DbType.DateTime).Value = txtDXEnd.Text;


            MySqlConnector.MySqlDataReader reader = com.ExecuteReader();


            while (reader.Read())
            {



                try
                {
                    string mytime = "";
                    string mydate = "";
                    string mycall = reader["callsign"].ToString();
                    if (mycall.ToUpper().Equals("CO2VDD"))
                    {
                        System.Console.Write("TEST");
                    }

                    GlobalClassSmall g = new GlobalClassSmall();
                    g.username = Properties.Settings.Default.QRZUser;
                    g.password = Properties.Settings.Default.QRZPassword;
                    string k = g.GetKey(); 
                    string UseBureau = g.getQSLByBureau(mycall, k);
                    if (UseBureau.Contains("NO BUREAU")|| UseBureau.Contains("NO BURO") )
                    {
                        // no reason to list or print 
                        continue;
                    }
                    if (UseBureau.Equals("NOT FOUND -> "))
                    {
                        continue;
                    }

                    if (UseBureau.StartsWith("NOT FOUND ->"))
                    {
                        if ((!UseBureau.Contains("BUREAU")) && (!UseBureau.Contains("BURO")))
                        {
                            continue;
                        }
                    }

                    string myname = reader["name"].ToString();
                    string myemail = reader["email"].ToString();
                    string mysiginfo = reader["mysiginfo"].ToString();
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
                    string myqsoid = reader["qsoid"].ToString();


                    //"qsoid,callsign,qsodate,email,band,mode,rstsent,name"
                    ListViewItem li = new ListViewItem();
                    li.Text = myqsoid;
                    li.SubItems.Add(mycall);
                    li.SubItems.Add(mydate);
                    li.SubItems.Add(mytime);
                    li.SubItems.Add(mysiginfo);
                    li.SubItems.Add(myemail);
                    li.SubItems.Add(band);
                    li.SubItems.Add(mode);
                    li.SubItems.Add(rst);
                    li.SubItems.Add(myname);
                    li.SubItems.Add(UseBureau);

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
            try
            { reader.Close(); }
            catch (Exception dex)
            { }
            try
            {
                com.Dispose();
            }
            catch (Exception dex)
            { }

            try
            {
                sqlcon.Close();
            }
            catch (Exception dex)
            { }

            System.GC.Collect();

        }
//        public int sQSLBefore(string callsign)
//        {

//            try
//            {
//                //connect to databsae 
//                MySqlConnector.MySqlConnectionStringBuilder b = new MySqlConnector.MySqlConnectionStringBuilder
//                {
//                    Server = Properties.Settings.Default.DBHost,
//                    UserID = Properties.Settings.Default.DBUser,
//                    Password = Properties.Settings.Default.DBPassword,
//                    Database = Properties.Settings.Default.DBDatabase,
//                    DateTimeKind = MySqlConnector.MySqlDateTimeKind.Utc


//                };
//                //if all the above return AllowDrop record we have a dup
//                //connect to databsae 
//                MySqlConnector.MySqlConnection sqlcon2 = new MySqlConnector.MySqlConnection(b.ConnectionString);
//                sqlcon2.Open();

//                MySqlConnector.MySqlCommand com = new MySqlConnector.MySqlCommand();
//                com.Connection = sqlcon2;

//                string mysql = "";

//                mysql = @"select j.* 
//from log,JSON_TABLE(log.qsoconfirmations,'$[*]'
//COLUMNS (
//	ct VARCHAR(10) PATH '$.CT', S VARCHAR(10) PATH '$.S',
//    R VARCHAR(10) PATH '$.R', 
//      SV VARCHAR(100) PATH '$.SV',
//      RV VARCHAR(100) PATH '$.RV',
//      SD VARCHAR(100) PATH '$.SD',
//      RD VARCHAR(100) PATH '$.RD' ) ) as j where j.ct = 'QSL' and callsign = ?callsign  and j.S = 'Yes'";
//                com.CommandText = mysql;

//                com.Parameters.Add("?callsign", DbType.String).Value = callsign;
//                //com.Parameters.Add("?band", DbType.String).Value = mode;
//                //com.Parameters.Add("?mode", DbType.String).Value = band;


//                MySqlConnector.MySqlDataReader reader = com.ExecuteReader();


//                int rows = 0;
//                while (reader.Read())
//                {
//                    rows++;

//                }
//                reader.Close();
//                com.Dispose();
//                sqlcon2.Close();
//                System.GC.Collect();
//                return rows;

//                try
//                {
//                    reader.Close();
//                    com.Dispose();
//                    sqlcon2.Close();
//                    System.GC.Collect();
//                }
//                catch (Exception ex)
//                {
//                    return -1;
//                }
//            }
//            catch (Exception exs)
//            {

//            }
//            return -1;
//        }

//        public int rQSLBefore(string callsign)
//        {

//            try
//            {
//                //connect to databsae 
//                MySqlConnector.MySqlConnectionStringBuilder b = new MySqlConnector.MySqlConnectionStringBuilder
//                {
//                    Server = Properties.Settings.Default.DBHost,
//                    UserID = Properties.Settings.Default.DBUser,
//                    Password = Properties.Settings.Default.DBPassword,
//                    Database = Properties.Settings.Default.DBDatabase,
//                    DateTimeKind = MySqlConnector.MySqlDateTimeKind.Utc


//                };
//                //if all the above return AllowDrop record we have a dup
//                //connect to databsae 
//                MySqlConnector.MySqlConnection sqlcon2 = new MySqlConnector.MySqlConnection(b.ConnectionString);
//                sqlcon2.Open();

//                MySqlConnector.MySqlCommand com = new MySqlConnector.MySqlCommand();
//                com.Connection = sqlcon2;

//                string mysql = "";

//                mysql = @"select j.* 
//from log,JSON_TABLE(log.qsoconfirmations,'$[*]'
//COLUMNS (
//	ct VARCHAR(10) PATH '$.CT', S VARCHAR(10) PATH '$.S',
//    R VARCHAR(10) PATH '$.R', 
//      SV VARCHAR(100) PATH '$.SV',
//      RV VARCHAR(100) PATH '$.RV',
//      SD VARCHAR(100) PATH '$.SD',
//      RD VARCHAR(100) PATH '$.RD' ) ) as j where j.ct = 'QSL' and   callsign =  ?callsign  and j.R = 'Yes'";
//                com.CommandText = mysql;

//                com.Parameters.Add("?callsign", DbType.String).Value = callsign;
//                //com.Parameters.Add("?band", DbType.String).Value = mode;
//                //com.Parameters.Add("?mode", DbType.String).Value = band;


//                MySqlConnector.MySqlDataReader reader = com.ExecuteReader();


//                int rows = 0;
//                while (reader.Read())
//                {
//                    rows++;

//                }
//                reader.Close();
//                com.Dispose();
//                sqlcon2.Close();
//                System.GC.Collect();
//                return rows;

//                try
//                {
//                    reader.Close();
//                    com.Dispose();
//                    sqlcon2.Close();
//                    System.GC.Collect();
//                }
//                catch (Exception ex)
//                {
//                    return -1;
//                }
//            }
//            catch (Exception exs)
//            {

//            }
//            return -1;
//        }


        private void DXSearch_Load(object sender, EventArgs e)
        {
            string[] qsofields = "qsoid,callsign,qsodate,qsotime,mysiginfo,email,band,mode,rstsent,name,Bureau,Duplicate,rQSL,sQSL,QSL Notes".Split(',');


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
            this.BureauDXQuery();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Refresh();
            MessageBox.Show("Query Complete");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count ==0) return;

            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.Selected)
                {
                    string layout = listBox1.SelectedItem.ToString();
                    string mycall = lvi.SubItems[1].Text;
                    string mydate = lvi.SubItems[2].Text;
                    string mytime = lvi.SubItems[3].Text;
                    string myband = lvi.SubItems[6].Text;
                    string mymode = lvi.SubItems[7].Text;
                    string myrst = lvi.SubItems[8].Text;

                    layout = System.IO.Path.GetDirectoryName(layout) + "\\" + System.IO.Path.GetFileNameWithoutExtension(layout) + ".layout";
                    string imagefile = listBox1.SelectedItem.ToString();
                    form1.WriteQSLCard(lvi.Text, layout, imagefile, mycall, myband, mymode, myrst, mydate, mytime);
                    form1.UpdateQSLConfirmation(lvi.Text, "Bureau");
                    

                }
                GC.Collect();
            }
            MessageBox.Show("Saving complete");
            
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
            try
            {
                Process.Start(Properties.Settings.Default.TMPDIR);
            }
            catch (Win32Exception win32Exception)
            {
                //The system cannot find the file specified...
                Console.WriteLine(win32Exception.Message);
            }
        }
    }
}
