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
        int setText = -1;

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

            button5.BackColor = Color.LightBlue;
            button4.BackColor = Color.LightBlue;

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
                txtImagePath.Text = ql.ImagePath;
                txtImageX.Text = ql.ImageLoc.X.ToString();
                txtImageY.Text = ql.ImageLoc.Y.ToString();
                txtDEx.Text = ql.DE.X.ToString();

                txtDEy.Text = ql.DE.Y.ToString();

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
            ql.DE = new PointF(int.Parse(txtDEx.Text), int.Parse(txtDEy.Text));
            //ql.SentRST = new PointF(int.Parse(txtRSTX.Text), int.Parse(txtRSTY.Text));
            ql.FontSize = int.Parse(txtFontSize.Text);
            try
            {
                ql.ImageLoc = new PointF(int.Parse(txtImageX.Text), int.Parse(txtImageY.Text));
            }
            catch (Exception ex)
            { 
                //ignore it 
            }

            ql.ImagePath = txtImagePath.Text;

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

            if (txtImageX.Text.Equals(""))
            {
                txtImageX.Text = "0";
            }
            if (txtImageY.Text.Equals(""))
            {
                txtImageY.Text = "0";
            }
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
            if (txtDEx.Text.Equals(""))
            {
                txtDEx.Text = "100";
            }
            if (txtDEy.Text.Equals(""))
            {
                txtDEy.Text = "100";
            }
            savelayout();

            
            ql.Callsign = new PointF(int.Parse(txtCallX.Text), int.Parse(txtCallY.Text));
            ql.ImageLoc = new PointF(int.Parse(txtImageX.Text), int.Parse(txtImageY.Text));
            ql.ImagePath = txtImagePath.Text;
            ql.HisCallsign = new PointF(int.Parse(txtHiscallX.Text), int.Parse(txtHisCallY.Text));
            ql.FontSize = int.Parse(txtFontSize.Text);
            ql.DE = new PointF(int.Parse(txtDEx.Text), int.Parse(txtDEy.Text));



            Image img = Image.FromFile(QSLImage);

            lblImageSize.Text = img.Width.ToString() + "/" + img.Height.ToString();

            //try
            //{
            //    if (ql.ImagePath.Trim().Length > 0)
            //    {

            //        if (System.IO.File.Exists(ql.ImagePath))
            //        {
            //            Graphics g = Graphics.FromImage(img);

            //            Image img2 = Image.FromFile(ql.ImagePath);
            //            g.DrawImage(img2, ql.ImageLoc);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{ 
            //    //ignore if bad iage /etc 
            //}

            ImageWriter iw = new ImageWriter();

            string myfile = System.IO.Path.Combine(Properties.Settings.Default.TMPDIR, "TEST");
            if (this.checkBox1.Checked)
            {
                img = iw.writeENVImage(QSLImage, myfile, "", "HISVeryLong Named Person\r\n9000009 LONG STREET NAME OR AVE\r\nBOX #5555555555555555\r\nVERYLONG CITY, ST 99999-9999 ", "HISCALL DE YOURCALL");
            }
            else
            {
                img = iw.writeENVImage(QSLImage, myfile, "VeryLong Named Person\r\n9000009 LONG STREET NAME OR AVE\r\nBOX #5555555555555555\r\nVERYLONG CITY, ST 99999-9999 ", "HISVeryLong Named Person\r\n9000009 LONG STREET NAME OR AVE\r\nBOX #5555555555555555\r\nVERYLONG CITY, ST 99999-9999 ", "HISCALL DE YOURCALL");
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

            //panel1.BackgroundImage = ResizeImage(img, panel1.Width, panel1.Height);
            
            //panel1.Refresh();
            pictureBox1.BackgroundImage = img;

            if (this.Width < pictureBox1.Width)
            {
                this.Width = pictureBox1.Width + 10;
            }

            if (this.Height < (pictureBox1.Height + pictureBox1.Top))
            {
                this.Height = (pictureBox1.Height + pictureBox1.Top) + 10;
            }

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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            string loc = e.Location.X.ToString() + "/" + e.Location.Y.ToString();
            label7.Text = loc;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            string loc = e.Location.X.ToString() + "/" + e.Location.Y.ToString();
            label7.Text = loc;
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            string loc = e.Location.X.ToString() + "/" + e.Location.Y.ToString();
            label7.Text = loc;
            if (setText.Equals(1))
            {
                txtCallX.Text = e.Location.X.ToString();
                txtCallY.Text = e.Location.Y.ToString();
                setText = -1;
                button5.BackColor = Color.LightBlue;
            }
            if (setText.Equals(2))
            {
                txtHiscallX.Text = e.Location.X.ToString();
                txtHisCallY.Text = e.Location.Y.ToString();
                setText = -1;
                button4.BackColor = Color.LightBlue;
            }
            if (setText.Equals(3))
            {
                txtImageX.Text = e.Location.X.ToString();
                txtImageY.Text = e.Location.Y.ToString();
                setText = -1;
                button6.BackColor = Color.LightBlue;
            }
            if (setText.Equals(4))
            {
                txtDEx.Text = e.Location.X.ToString();
                txtDEy.Text = e.Location.Y.ToString();
                setText = -1;
                button8.BackColor = Color.LightBlue;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            setText = 1;
            button5.BackColor = Color.GreenYellow;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            setText = 2;
            button4.BackColor = Color.GreenYellow; 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            setText = 3;
            button6.BackColor = Color.GreenYellow;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (txtImagePath.Text.Trim().Length > 0)
            {
                fd.FileName = txtImagePath.Text.Trim();
                fd.ShowDialog();
                txtImagePath.Text = fd.FileName;
            }
            else
            {
                fd.ShowDialog();
                txtImagePath.Text = fd.FileName;
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            setText = 4;
            button8.BackColor = Color.GreenYellow;
        }
    }
}
