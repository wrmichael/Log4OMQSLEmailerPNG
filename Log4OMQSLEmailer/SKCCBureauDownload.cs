using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;
using System.IO;
//using System.Web; 

namespace Log4OMQSLEmailer
{
    internal class SKCCBureauDownload
    {

        string SKCC_BUREAU = "https://skccgroup.com/member_services/qsl_buro/";

        public string Bureau_users = "";

        public string[] getAmateurs()
        {
            string[] callsigns = null; ;


            using (var client = new HttpClient())
            {
                var response = client.GetAsync(SKCC_BUREAU).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string data = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
                    //first thing  - split the table for callsigns 
                    string table = data.Substring(data.IndexOf(@"<table><tr><th>Callsign</th><th>SKCC#</th><th>Name</th><th>Other Callsigns</th><th>SPC</th><th>Envelopes</th></tr>"));
                    table = table.Substring(0, table.IndexOf(@"</td></tr></table>"));
                    callsigns = table.Split(new string[] { "<tr><td>" },StringSplitOptions.None);
                    for (int i = 1; i < callsigns.Length; i++)
                    {
                        callsigns[i] = callsigns[i].Substring(0, callsigns[i].IndexOf("</td>"));
                    }

                }
            }

            return callsigns; 

        }

    }
}
