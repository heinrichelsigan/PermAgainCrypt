using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Util;
using System.Globalization;

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

                    string drawString = this.CiffrePipe.ZType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(Color.DarkOrange);
                    float x = offset + 1.0F;
                    float y = 77.5F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

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
 

                    offset += w;
                }

                offset = startset;
                for (int i = 0; (i < CiffrePipe.InPipe.Length); i++)
                {
                    w = 60;
                    Color color = (i < 5) ? ColorTranslator.FromHtml("#0000dd") : ColorTranslator.FromHtml("#0000bb");
                    string drawString = CiffrePipe.InPipe[i].ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(color);
                    float x = offset + 2.0F;
                    float y =  ((i % 4) * 18.0F);
                    switch (i)
                    {
                        case 1: y = 78F; break;
                        case 2: y = 1F; break;
                        case 3: y = 76F;  break;
                        default:
                            y = ((i % 4) * 18.0F); break;
                    }
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }
                startset = offset;

                if (this.CiffrePipe.EncodeType != Framework.Core.Crypt.EnDeCoding.EncodingType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.encoding_right_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 16, w, 64));
                    offset = startset;
                    string drawString = this.CiffrePipe.EncodeType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(Color.DarkOrange);
                    float x = offset + 1.0F;
                    float y = 2.5F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
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
                    ximage = new Bitmap(Properties.Resources.decoding_right_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 16, w, 64));
                    
                    string drawString = this.CiffrePipe.EncodeType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(Color.DarkOrange);
                    float x = offset + 0.2F;
                    float y = 77.5F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                        
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

                    offset += w;
                }

                offset = startset;
                for (int i = 0; (i < CiffrePipe.OutPipe.Length); i++)
                {
                    w = 60;
                    int r = 7 - i;

                    Color color = (i < 4) ?  ColorTranslator.FromHtml("#110099") : ColorTranslator.FromHtml("#0000cc");
                    string drawString = CiffrePipe.OutPipe[i].ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(color);
                    float x = offset + 1.0F;
                    float y = ((i % 4) * 18.0F); 
                    switch (i)
                    {
                        case 5: y = 72F; break;
                        case 6: y = 1F; break;
                        case 7: y = 75F; break;
                        default:
                            y = ((i % 4) * 18.0F); break;
                    }
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.NoWrap;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }

                if (CiffrePipe.ZType != Framework.Core.Zip.ZipType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.compress_right_end_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 16, w, 64));

                    string drawString = this.CiffrePipe.ZType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(Color.DarkOrange);
                    float x = offset + 2.4F;
                    float y = 1.5F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);                    
                    
                    offset += w;
                }

            }

            return mergeimg;
        }

    }
}
