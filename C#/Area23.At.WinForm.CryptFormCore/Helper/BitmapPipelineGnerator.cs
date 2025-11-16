using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Pipelines;
using System.Resources;
using System.Text;

namespace Area23.At.WinForm.CryptFormCore.Helper
{
    internal class BitmapPipelineGnerator
    {
        public CipherPipe CiffrePipe { get; private set; }

        public BitmapPipelineGnerator()
        {
            CiffrePipe = new CipherPipe(Constants.AUTHOR_EMAIL);
        }

        public BitmapPipelineGnerator(CipherPipe pipe)
        {
            if (pipe == null) 
                throw new ArgumentException(nameof(pipe));
            CiffrePipe = pipe;
        }


        public Image GenerateEncryptPipeImage()
        {
            System.Drawing.Bitmap mergeimg = new Bitmap(640, 96), ximage;
            string bmpName = "";
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                int w = 64, offset = 0, startset = 0;
                if (CiffrePipe.ZType != Framework.Core.Zip.ZipType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.compress_right_start_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(0, 16, w, 64));
                    offset += w;
                    startset += w;
                }
                

                for (int i = 0; (i < CiffrePipe.InPipe.Length); i++)
                {
                    w = 60;
                    char ch = CiffrePipe.InPipe[i].GetCipherChar();
                    bmpName = $"arrow_right_{i}";
                    object obj = Properties.Resources.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
                    ximage = new Bitmap(((System.Drawing.Bitmap)(obj)), new Size(64, 64));                                                            
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 16, w, 64));
                    
                    //string drawString = CiffrePipe.InPipe[i].ToString();
                    //Font drawFont = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
                    //SolidBrush drawBrush = new SolidBrush(Color.BlueViolet);
                    //float x = offset + 6.0F;
                    //float y = 5.0F + (i % 4) * 15.0F;
                    //StringFormat drawFormat = new StringFormat();
                    //drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    //g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }

                offset = startset;
                for (int i = 0; (i < CiffrePipe.InPipe.Length); i++)
                {
                    w = 60;                    

                    string drawString = CiffrePipe.InPipe[i].ToString();
                    Font drawFont = new Font("Lucida Sans Unicode", 12, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(Color.LightSkyBlue);
                    float x = offset + 1.0F;
                    float y = 1.0F + (i % 4) * 18.0F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }


                if (this.CiffrePipe.EncodeType != Framework.Core.Crypt.EnDeCoding.EncodingType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.encoding_right_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 16, w, 64));
                    offset += w;
                }

            }

            return mergeimg;
        }

        public Image GenerateDecryptPipeImage()
        {
            System.Drawing.Bitmap mergeimg = new Bitmap(640, 96), ximage;
            string bmpName = "";
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                int w = 64, offset = 0, startset = 0;
                if (this.CiffrePipe.EncodeType != Framework.Core.Crypt.EnDeCoding.EncodingType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.encoding_right_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 16, w, 64));
                    offset += w;
                    startset += w;
                }

                for (int i = 0; (i < CiffrePipe.OutPipe.Length); i++)
                {
                    w = 60;
                    int r = 7 - i;
                    char ch = CiffrePipe.OutPipe[i].GetCipherChar();
                    bmpName = $"arrow_right_{r}";
                    object obj = Properties.Resources.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
                    ximage = new Bitmap(((System.Drawing.Bitmap)(obj)), new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 16, w, 64));

                    //string drawString = CiffrePipe.OutPipe[i].ToString();
                    //Font drawFont = new Font("Lucida Sans Unicode", 12, FontStyle.Bold);
                    //SolidBrush drawBrush = new SolidBrush(Color.DeepSkyBlue);
                    //float x = offset + 6.0F;
                    //float y = 8.0F + (i % 4) * 12.0F;
                    //StringFormat drawFormat = new StringFormat();
                    //drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    //g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }

                offset = startset;
                for (int i = 0; (i < CiffrePipe.OutPipe.Length); i++)
                {
                    w = 60;
                    int r = 7 - i;
                    
                    string drawString = CiffrePipe.OutPipe[i].ToString();
                    Font drawFont = new Font("Lucida Sans Unicode", 12, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#0000ee"));
                    float x = offset + 1.0F;
                    float y = 1.0F + (i % 4) * 18.0F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }

                if (CiffrePipe.ZType != Framework.Core.Zip.ZipType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.compress_right_end_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 16, w, 64));
                    offset += w;
                }

            }

            return mergeimg;
        }

    }
}
