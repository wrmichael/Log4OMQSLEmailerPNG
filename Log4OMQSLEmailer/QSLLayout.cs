using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4OMQSLEmailer
{
    public class QSLLayout
    {
        public int FontSize; 
        public System.Drawing.PointF Callsign { get; set; }
        public System.Drawing.PointF HisCallsign { get; set; }
        public System.Drawing.PointF Date { get; set; }
        public System.Drawing.PointF Time { get; set; }
        public System.Drawing.PointF SentRST { get; set; }
        public System.Drawing.PointF Mode { get; set; }
        public System.Drawing.PointF Band { get; set; }
        public bool ignoreMyAddress { get; set; }

        public String ImagePath { get; set; }
        public System.Drawing.PointF ImageLoc { get; set; }

    }
}
