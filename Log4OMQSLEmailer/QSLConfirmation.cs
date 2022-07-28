using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4OMQSLEmailer
{
    public class QSLConfirmation
    {
        /*[{"CT":"EQSL","S":"Yes","R":"Yes","SV":"Electronic","RV":"Electronic","SD":"2021-02-10T00:00:00Z","RD":"2020-12-01T00:00:00Z"}
         *{"CT":"LOTW","S":"Yes","R":"Yes","SV":"Electronic","RV":"Electronic","SD":"2020-07-09T00:00:00Z","RD":"2020-07-09T00:00:00Z"}
         *{"CT":"QSL","S":"No","R":"No","SV":"Electronic","RV":"Electronic"},
         *{"CT":"QRZCOM","S":"Yes","R":"No","SV":"Electronic","RV":"Electronic","SD":"2021-01-07T00:00:00Z"},
         *{"CT":"HAMQTH","S":"Requested","R":"No","SV":"Electronic","RV":"Electronic"},
         *{"CT":"HRDLOG","S":"Requested","R":"No","SV":"Electronic","RV":"Electronic"},
         *{"CT":"CLUBLOG","S":"Yes","R":"No","SV":"Electronic","RV":"Electronic","SD":"2021-06-22T00:00:00"}]
       */

        public string CT { get; set; }
        public string S { get; set; }
        public string R { get; set; }
        public string SV { get; set; }

        public string RV { get; set; }
        public string SD { get; set; }
        public string RD { get; set; }

    }
}
