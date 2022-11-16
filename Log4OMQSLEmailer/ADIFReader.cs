using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace Log4OMQSLEmailer
{
    class ADIFReader
    {
        public Form1 my_form1; 

        public string adifdata = "";
        //public string qsofields = "CALL,NAME,QSO_DATE,TIME_ON,RST_SENT,RST_RCVD,MODE,QTH,BAND,FREQ,SKCC,TX_PWR,COMMENT,QSLBYMAIL,EMAIL";
        public string ADIFPath { get; set; }
        public int limit_count { get; set; }
        public ArrayList records { get; set; }

        public int count = 0;
        public int index = -1;
        public ADIFRecord currentRecord { get; set; }
        public void start(bool mailimage =true)
        {
            int my_limit_count = 0;
            GlobalClassSmall gc = new GlobalClassSmall();
            if (ADIFPath.Trim().ToString().Length == 0)
            {
                return;
            }
        
            string adfi = openADFI(ADIFPath).ToUpper();

            records = new ArrayList();
            


        

            gc.username = Properties.Settings.Default.QRZUser;
            gc.password = Properties.Settings.Default.QRZPassword;

            
            string k = gc.GetKey();


            string mycall = "";
            
            int idx = 0;

            string special = "";

            foreach (string r in adfi.Split(new string[] { "<EOR>" }, StringSplitOptions.None))
            {

                ADIFRecord rec = new ADIFRecord();
                int x = 0;


                rec.call = getvalue(r, "CALL");
                rec.band = getvalue(r, "BAND");
                rec.mode = getvalue(r, "MODE");
                rec.sent = getvalue(r, "RST_SENT");
                rec.time = getvalue(r, "TIME_ON");
                rec.date = getvalue(r, "QSO_DATE");


                //check QSO id to see if need to skip this record.  don't waste time on a lookup
                string c_mytime = my_form1.fixADIFTime(rec.time);
                string c_mydate = my_form1.fixDate(rec.date);
                string c_mycall = rec.call;
                string c_myname = rec.name;
                string c_myemail = rec.email;
                string c_rst = rec.sent;

                string c_band = rec.band;
                string c_mode = rec.mode;
                string c_myqsoid = rec.call + "_" + rec.mode + "_" + rec.band + "_" + rec.date + "_" + rec.time + "_" + rec.band;

                //strip any non filename values... 
                string exclude_char = "*()!@#$%^&+={}[]|\\;:'\"?/.,<>~`";

                foreach (char c in exclude_char.ToArray())
                {
                    c_myqsoid = c_myqsoid.Replace(c, '_');
                }

                if (my_form1.checklog("," + c_myqsoid + ","))
                {
                    continue;
                }


                    //if (gc.getQSLByMail(li.Text,k).Equals("1"))
                    //{
                    if (mailimage)
                {
                    rec.name = gc.getNameOnly(rec.call, k).Trim();
                }
                
                if (!Properties.Settings.Default.ExclusionList.ToUpper().Contains("," + rec.call + ","))
                {
                    if (mailimage)
                    {
                        rec.email = gc.getEmailOnly(rec.call, k).Trim();

                        if (rec.email.Trim().Length > 0)
                        {
                            records.Add(rec);
                            idx++;
                            my_limit_count++;
                        }
                    }
                    else
                    {
                        records.Add(rec);
                        idx++;
                    }
                }
                if (this.limit_count > 0)
                {
                    if (my_limit_count > this.limit_count)
                    {
                        break;
                    }
                }
            }
            this.index = 0;
            this.count = idx;
        }

        public string getvalue(string s, string f)
        {

            string t = "";
            string ns = "";
            int cc = 0;
            if (s.IndexOf("<" + f + ":") > 0)

            {
                //look for value 
                t = s.Substring(s.IndexOf("<" + f + ":") + f.Length + 2);

                //t = t.Substring(0, t.IndexOf(":"));
                cc = int.Parse(t.Substring(0, t.IndexOf(">")));
                t = t.Substring(t.IndexOf(">") + 1);
                ns = t.Substring(0, cc);


            }
            return ns;
        }

        public string openADFI(string f)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(f);
            string adfi = sr.ReadToEnd();
            sr.Close();
            this.adifdata = adfi;
            return adfi;
        }

    }
}
