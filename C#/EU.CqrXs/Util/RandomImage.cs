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

        string[] bmpNames = new string[]
        {
            "filesymbol", "file_encrypted", "file_encrypted2", "file_encrypted_broken", "file_pdf", "file_zip", "file_powerpoint", "file_word", "file_excel"
        };

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

            int ix = ((int)rand.NextInt64(bmpNames.Length) % bmpNames.Length);
            Bitmap bmpx = (System.Drawing.Bitmap)Properties.Resource.ResourceManager.GetObject(bmpNames[ix], Properties.Resource.Culture);

            Bitmap mergeImage = new Bitmap(bmpx);

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeImage))
            {
                Color color = (ix < 7) ? ColorTranslator.FromHtml("#0000dd") 
                    : ColorTranslator.FromHtml("#efefef");
                string drawString = simg.Substring(0, 5);
                Font drawFont = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                SolidBrush drawBrush = new SolidBrush(color);
                float x = (ix < 7) ? 14F : 15F;
                float y = 5F;
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
