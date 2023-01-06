using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

using System.Drawing.Imaging;

using System.Drawing.Drawing2D;

namespace Log4OMQSLEmailer
{
    public partial class ENVDataLayoutByImage : Form
    {
        public string QSLImage = "";
        public string LayoutFile = "";
        public QSLLayout ql = new QSLLayout();
        public ENVDataLayoutByImage()
        {
            InitializeComponent();
        }

        private void DataLayout_Load(object sender, EventArgs e)
        {
            string sql = "";

            if (System.IO.File.Exists(LayoutFile))
            {
                //load values from file 
                JsonSerializer js = new JsonSerializer();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(LayoutFile))
                {
                    sql = sr.ReadToEnd();
                    ql = JsonConvert.DeserializeObject<QSLLayout>(sql);
                    sr.Close();

                }

                //txtBandX.Text = ql.Band.X.ToString();
                //txtBandY.Text = ql.Band.Y.ToString();
                //txtModeX.Text = ql.Mode.X.ToString();
                //txtModeY.Text = ql.Mode.Y.ToString();
                txtHiscallX.Text = ql.HisCallsign.X.ToString();
                txtHisCallY.Text = ql.HisCallsign.Y.ToString();
                checkBox1.Checked = ql.ignoreMyAddress;
                //txtRSTX.Text = ql.SentRST.X.ToString();
                //txtRSTY.Text = ql.SentRST.Y.ToString();
                //txtTimeX.Text = ql.Time.X.ToString();
                //txtTimeY.Text = ql.Time.Y.ToString();
                txtCallX.Text = ql.Callsign.X.ToString();
                txtCallY.Text = ql.Callsign.Y.ToString();
                txtFontSize.Text = ql.FontSize.ToString();
            


            }

            /*
            txtBandX.Text = Properties.Settings.Default.BAND_X;
            txtBandY.Text = Properties.Settings.Default.BAND_Y;
            txtModeX.Text = Properties.Settings.Default.MODE_X;
            txtModeY.Text = Properties.Settings.Default.MODE_Y;
            txtDateX.Text = Properties.Settings.Default.DATE_X;
            txtDateY.Text = Properties.Settings.Default.DATE_Y;
            txtRSTX.Text = Properties.Settings.Default.RST_X;
            txtRSTY.Text = Properties.Settings.Default.RST_Y;
            txtTimeX.Text = Properties.Settings.Default.TIME_X;
            txtTimeY.Text = Properties.Settings.Default.TIME_Y;
            txtCallX.Text = Properties.Settings.Default.CALL_X;
            txtCallY.Text = Properties.Settings.Default.CALL_Y;
            txtFontSize.Text = Properties.Settings.Default.FontSize;
        */
        }

        private void button1_Click(object sender, EventArgs e)
        {


            savelayout();
            this.Close();

        }

        void savelayout()
        {

            //ql.Band = new System.Drawing.PointF(int.Parse(txtBandX.Text), int.Parse(txtBandY.Text));
            //ql.Mode = new PointF(int.Parse(txtModeX.Text), int.Parse(txtModeY.Text));
            //ql.Date = new PointF(int.Parse(txtHiscallX.Text), int.Parse(txtHisCallY.Text));

            //ql.Time = new PointF(int.Parse(txtTimeX.Text), int.Parse(txtTimeY.Text));
            ql.ignoreMyAddress = this.checkBox1.Checked;
            ql.Callsign = new PointF(int.Parse(txtCallX.Text), int.Parse(txtCallY.Text));
            ql.HisCallsign = new PointF(int.Parse(txtHiscallX.Text), int.Parse(txtHisCallY.Text));
            //ql.SentRST = new PointF(int.Parse(txtRSTX.Text), int.Parse(txtRSTY.Text));
            //ql.FontSize = int.Parse(txtFontSize.Text);

            string sql = JsonConvert.SerializeObject(ql);

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(LayoutFile))
            {
                sw.Write(sql);
                sw.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
           
            if (txtHiscallX.Text.Equals(""))
            {
                txtHiscallX.Text = "0";
            }
            if (txtHisCallY.Text.Equals(""))
            {
                txtHisCallY.Text = "0";
            }
           
            if (txtCallX.Text.Equals(""))
            {
                txtCallX.Text = "0";
            }
            if (txtCallY.Text.Equals(""))
            {
                txtCallY.Text = "0";
            }

            if (txtFontSize.Text.Equals(""))
            {
                txtFontSize.Text = "100";
            }
            savelayout();

            
            ql.Callsign = new PointF(int.Parse(txtCallX.Text), int.Parse(txtCallY.Text));
            ql.HisCallsign = new PointF(int.Parse(txtHiscallX.Text), int.Parse(txtHisCallY.Text));
            ql.FontSize = int.Parse(txtFontSize.Text);




            Image img = Image.FromFile(QSLImage);

            lblImageSize.Text = img.Width.ToString() + "/" + img.Height.ToString();



            ImageWriter iw = new ImageWriter();

            string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, "TEST");
            if (this.checkBox1.Checked)
            {
                img = iw.writeENVImage(QSLImage, myfile, "", "HISVeryLong Named Person\r\n9000009 LONG STREET NAME OR AVE\r\nBOX #5555555555555555\r\nVERYLONG CITY, ST 99999-9999 ");
            }
            else
            {
                img = iw.writeENVImage(QSLImage, myfile, "VeryLong Named Person\r\n9000009 LONG STREET NAME OR AVE\r\nBOX #5555555555555555\r\nVERYLONG CITY, ST 99999-9999 ", "HISVeryLong Named Person\r\n9000009 LONG STREET NAME OR AVE\r\nBOX #5555555555555555\r\nVERYLONG CITY, ST 99999-9999 ");
            }
            /*
            //save PNG here
            Graphics g = Graphics.FromImage(img);
            Font font = new Font("Arial", ql.FontSize, FontStyle.Bold, GraphicsUnit.Pixel);

            g.DrawString("BAND", font, Brushes.Black, ql.Band);
            g.DrawString("CALL", font, Brushes.Black, ql.Callsign);
            g.DrawString("MODE", font, Brushes.Black, ql.Mode);

            g.DrawString("YYYY-MM-DD", font, Brushes.Black, ql.Date);
            g.DrawString("HH:MM", font, Brushes.Black, ql.Time);
            g.DrawString("RST", font, Brushes.Black, ql.SentRST);



            img.Save(System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, "TEST" + ".png"), System.Drawing.Imaging.ImageFormat.Png);
            */

            panel1.BackgroundImage = ResizeImage(img, panel1.Width, panel1.Height);
            
            panel1.Refresh();
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
