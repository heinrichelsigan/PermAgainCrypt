using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using System.Drawing;
using System.Globalization;
using System.Reflection.Metadata;

namespace Area23.At.WinForm.CryptFormCore.Helper
{

    /// <summary>
    /// BitmapPipelineGnerator - abstraction for generating images for en-/decryption pipelines
    /// </summary>
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



        /// <summary>
        /// GenerateEncryptPipeImage - generates image for symmetric cipher encryption pipeline
        /// </summary>
        /// <returns><see cref="Image">the image</see></returns>
        public Image GenerateEncryptPipeImage()
        {
            System.Drawing.Bitmap mergeimg = new Bitmap(Properties.Resources.BlankEncrypt_640x96, new Size(640, 108)), ximage;
            System.Drawing.Bitmap? gifStartImage = new Bitmap(Properties.Resources.BlankEncrypt_640x96, new Size(640, 108));
            List<Bitmap> bitmaps = new List<Bitmap>();

            string bmpName = "";
            int w = 64, offset = 0, startset = 0;
            if (CiffrePipe.ZType != Framework.Core.Zip.ZipType.None)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.block_arrow_right_compress, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(0, 20, w, 64));

                    string drawString = this.CiffrePipe.ZType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#df0fef"));
                    float x = offset + 1.0F;
                    float y = 82.5F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                    
                    offset += w;
                    startset += w;
                }
                gifStartImage = new Bitmap(mergeimg, 640, 108);
                bitmaps.Add(gifStartImage);
            }

            startset = offset;

            for (int i = 0; (i < CiffrePipe.InPipe.Length); i++)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;
                    char ch = CiffrePipe.InPipe[i].GetCipherChar();
                    bmpName = $"arrow_right-{i}";
                    object obj = Properties.Resources.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
                    ximage = new Bitmap(((System.Drawing.Bitmap)(obj)));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    offset += w;
                }                
                if (gifStartImage == null)
                    gifStartImage = new Bitmap(mergeimg, 640, 108);
                bitmaps.Add(new Bitmap(mergeimg, 640, 108));
            }


            offset = startset;

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                for (int i = 0; (i < CiffrePipe.InPipe.Length); i++)
                {

                    Color color = (i < 5) ? ColorTranslator.FromHtml("#0000ee") : ColorTranslator.FromHtml("#0000dd");
                    string drawString = CiffrePipe.InPipe[i].ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(color);
                    float x = offset + 1.0F;
                    float y = 2F + ((i % 4) * 23.0F);
                    switch (i)
                    {
                        case 1: y = 84F;    break;
                        case 2: y = 1F;     break;
                        case 3: y = 86F;    break;
                        case 4: y = 2F;     break;
                        case 5: 
                        case 6:
                        case 7: y = 1F + ((i % 4) * 23.0F);
                            drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold); break;
                        default: y = 1F + ((i % 4) * 23.0F); break;
                    }
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }
            }
            bitmaps.Add(new Bitmap(mergeimg, 640, 108));


            if (this.CiffrePipe.EncodeType != Framework.Core.Crypt.EnDeCoding.EncodingType.None)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.encoding_right_end_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));                    
                    string drawString = this.CiffrePipe.EncodeType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#bf0fef"));
                    float x = offset + 1.0F;
                    float y = 4.0F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                }
                bitmaps.Add(new Bitmap(mergeimg, 640, 108));
            }

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, 125);
            GifEncoder gifAnimEncoder = new GifEncoder(bitmaps.ToArray(), 1, ts);
            Image animGif = gifAnimEncoder.AnimImage;
            // animGif.Save("H:\\tmp\\" + DateTime.Now.ToString("yyyy-MM-DD_hhmmss") + ".gif");
            

            return animGif;
        }


        /// <summary>
        /// GenerateDecryptPipeImage generates an image for decrypt symmetric cipher pipeline 
        /// </summary>
        /// <returns><see cref="Image">the image</see></returns>
        public Image GenerateDecryptPipeImage()
        {
            System.Drawing.Bitmap mergeimg = new Bitmap(640, 108), ximage;
            string bmpName = "";
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                int w = 64, offset = 0, startset = 0;
                if (this.CiffrePipe.EncodeType != Framework.Core.Crypt.EnDeCoding.EncodingType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resources.encoding_right_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));
                    
                    string drawString = this.CiffrePipe.EncodeType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#fa0ade"));
                    float x = offset + 1F;
                    float y = 86F;
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
                    bmpName = $"arrow_right-{r}";
                    object obj = Properties.Resources.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
                    ximage = new Bitmap(((System.Drawing.Bitmap)(obj)), new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    offset += w;
                }

                offset = startset;
                for (int i = 0; (i < CiffrePipe.OutPipe.Length); i++)
                {
                    w = 60;
                    int r = 7 - i;

                    Color color = (i < 4) ? ColorTranslator.FromHtml("#2200aa") : ColorTranslator.FromHtml("#0000dd");
                    string drawString = CiffrePipe.OutPipe[i].ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(color);
                    float x = offset + 2.0F;
                    float y = 1.5F + ((i % 4) * 23.0F);
                    switch (i)
                    {
                        case 5: y = 84F; break;
                        case 6: y = 4F; break;
                        case 7: y = 86F; break;
                        default:
                            y = 1.5F + ((i % 4) * 23.0F); break;
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
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    string drawString = this.CiffrePipe.ZType.GetUnzipString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#fa0ade"));
                    float x = offset + 2.4F;
                    float y = 3.8F;
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
