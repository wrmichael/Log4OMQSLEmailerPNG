using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections;


namespace Log4OMQSLEmailer
{
    class ADIFReader
    {
        public string adifdata = "";
        //public string qsofields = "CALL,NAME,QSO_DATE,TIME_ON,RST_SENT,RST_RCVD,MODE,QTH,BAND,FREQ,SKCC,TX_PWR,COMMENT,QSLBYMAIL,EMAIL";
        public string ADIFPath { get; set; }
        public ArrayList records { get; set; }

        public int count = 0;
        public int index = -1;
        public ADIFRecord currentRecord { get; set; }
        public void start()
        {
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
                


                
                //if (gc.getQSLByMail(li.Text,k).Equals("1"))
                //{
                    rec.name = gc.getNameOnly(rec.call, k).Trim();
                
                if (!Properties.Settings.Default.ExclusionList.ToUpper().Contains("," + rec.call + ","))
                {
                    rec.email = gc.getEmailOnly(rec.call, k).Trim();
                    if (rec.email.Trim().Length > 0)
                    {
                        records.Add(rec);
                        idx++;
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
