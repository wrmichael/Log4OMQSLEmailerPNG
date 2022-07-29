using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4OMQSLEmailer
{
    class ADIFRecord
    {

        public string call { get; set; }
        public string date { get; set; }
        public string freq { get; set; }
        public string email { get; set; }
        public string name { get; set; }

        public string band { get; set; }
        public string mode { get; set; }
        public string power { get; set; }
        public string time { get; set; }
        public string sent { get; set; }
        public string rcv { get; set; }
        public string qth { get; set; }


    }
}
