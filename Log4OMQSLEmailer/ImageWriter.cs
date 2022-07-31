using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using Newtonsoft.Json;

namespace Log4OMQSLEmailer
{
    public class ImageWriter
    {

        public Image writeImage(string sourceimage, string newImageName, string band, string mode, string mycall, string rst, string mydate, string mytime)
        {
            string layoutfile = System.IO.Path.GetFileNameWithoutExtension(sourceimage) + ".layout";

            string sourceExt = System.IO.Path.GetExtension(sourceimage);


            Image img = Image.FromFile(sourceimage);

            string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, newImageName + "." + sourceExt);
            //save PNG here
            Graphics g = Graphics.FromImage(img);
            //Font font = new Font("Arial", int.Parse(Properties.Settings.Default.FontSize), FontStyle.Bold, GraphicsUnit.Pixel);


            QSLLayout ql = new QSLLayout();

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


            if (sourceExt.ToUpper().Equals(".PNG"))
            {
                img.Save(System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, newImageName  + sourceExt), System.Drawing.Imaging.ImageFormat.Png);
            }
            if (sourceExt.ToUpper().Equals(".JPG"))
            {
                img.Save(System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, newImageName  + sourceExt), System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            return img;

        }

    }
}
