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
            System.Drawing.Bitmap mergeimg = new Bitmap(672, 64), ximage;
            string bmpName = "";
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                int addInt = 0, w = 64, offset = 0;
                if (CiffrePipe.ZType != Framework.Core.Zip.ZipType.None)
                {
                    addInt = 1;
                    ximage = new Bitmap(Properties.Resources.compress_right_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(0, 0, w, 64));
                    offset = 64;
                }
                
                for (int i = addInt; (i < CiffrePipe.InPipe.Length); i++)
                {
                    w = 60;
                    char ch = CiffrePipe.InPipe[i].GetCipherChar();
                    bmpName = $"arrow_right_{i}";
                    object obj = Properties.Resources.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
                    ximage = new Bitmap(((System.Drawing.Bitmap)(obj)), new Size(64, 64));
                                                            
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 0, w, 64));
                    offset += w;
                }

                if (this.CiffrePipe.EncodeType != Framework.Core.Crypt.EnDeCoding.EncodingType.None)
                {
                    w = 87;
                    ximage = new Bitmap(Properties.Resources.encoding_right_0, new Size(87, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 0, w, 64));
                    offset += w;
                }

            }

            return mergeimg;
        }


    }
}
