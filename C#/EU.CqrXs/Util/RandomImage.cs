using EU.CqrXs.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace EU.CqrXs.Util
{
    public class RandomImage
    {
        public Bitmap RandomBitmap { get; private set; }
        public byte[] RandomBytes { get; private set; }

        public string SaveFileName { get; private set; }

        public RandomImage()
        {
            GetNewImage();
        }


        public string GetNewImage()
        {
            string simg = "";
            Random rand = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000);
            if (string.IsNullOrEmpty(simg) || File.Exists(SaveFileName))
                simg = rand.GetHexString(5, true) + ".png";

            Bitmap mergeImage = new Bitmap(Properties.Resource.filesymbol);

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeImage))
            {
                Color color = ColorTranslator.FromHtml("#0000dd");
                string drawString = simg.Substring(0, 5);
                Font drawFont = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                SolidBrush drawBrush = new SolidBrush(color);
                float x = 10.5F;
                float y = 4.0F;
                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            }

            this.RandomBitmap = mergeImage;
            this.SaveFileName = Path.Combine(Area23Log.TempDir, simg);

            mergeImage.Save(this.SaveFileName, ImageFormat.Png);

            this.RandomBytes = File.ReadAllBytes(this.SaveFileName);

            return SaveFileName;
        }

    }

    public class RandomName
    {        
        public string RandomString { get; private set; }

        public RandomName()
        {
            GetNewString();
        }


        public string GetNewString()
        {
            string rnstr = "";
            Random rand = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000);
            rnstr = rand.GetHexString(8, true);

            if (rnstr.Equals(RandomString))
                rnstr = GetNewString();
            else
                RandomString = rnstr;

           return RandomString;
        }

    }
}
