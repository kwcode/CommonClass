using ImageHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHelper_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(@"C:\Users\Administrator\Pictures\logo.png");
            ImageExif exif = ImageHelper.Exif.GetExifInfo(img);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(@"C:\Users\Administrator\Pictures\big.jpg");
            Bitmap bmp = ImageHelper.Resize.GetThumbnailImage(img, ThumbnailImageType.Zoom, 100, 100);
            bmp.Save("a.png");

            Bitmap bmp2 = ImageHelper.Resize.GetThumbnailImage(img, ThumbnailImageType.Zoom, 100, 0);
            bmp2.Save("b.png");

            Bitmap bmp3 = ImageHelper.Resize.GetThumbnailImage(img, ThumbnailImageType.Zoom, 0, 100);
            bmp3.Save("c.png");

        }
    }
}
