using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace EU.CqrXs.Crypt.Cipher
{

    /// <summary>
    /// MatrixPipe means different CipherPipe's for eachj block of bytes, which are encrypted with different cipher algorithms in a marry go round way.
    /// </summary>
    public class MatrixPipe : CipherPipe
    {

        #region fields and properties

        private static readonly Lock _lock = new Lock();
        private CipherEnum[] _inPipe = new List<CipherEnum>().ToArray();

        List<CipherEnum[]> inPipes = new List<CipherEnum[]>(8); // max. 8 stages of pipe, max each 8 different pipes per block

        List<CipherEnum[]> outPipes = new List<CipherEnum[]>(8); // max. 8 stages of pipe, max each 8 different pipes per block

        /// <summary>
        /// InPipe is current encryption pipe
        /// </summary>
        public List<CipherEnum[]> InPipes { get => inPipes; set => inPipes = value; }

        public override CipherEnum[] InPipe
        {
            get => GetInPipe(0); 
            set
            {
                _inPipe = value;
                BuildInPipes(_inPipe);
                outPipes = new List<CipherEnum[]>(inPipes.Count);
                foreach (var iPipe in inPipes)
                {
                    CipherEnum[] oPipe = iPipe.Reverse<CipherEnum>().ToArray();
                    outPipes.Add(oPipe);
                }
            }
        }


        public override CipherEnum[] OutPipe { get => GetOutPipe(0); } // TODO: set

        /// <summary>
        /// OutPipe will always be generated from <see cref="InPipes"/>
        /// </summary>
        public List<CipherEnum[]> OutPipes
        {
            get
            {
                if (outPipes != null && outPipes.Count == inPipes.Count)
                    return outPipes;

                outPipes = new List<CipherEnum[]>(inPipes.Count);
                foreach (var iPipe in inPipes)
                {
                    CipherEnum[] oPipe = iPipe.Reverse<CipherEnum>().ToArray();
                    outPipes.Add(oPipe);
                }
                return outPipes;
            }
        }

        // protected internal int pipeRingBufIdx = 0;

        /// <summary>
        /// InPipe is current encryption pipe
        /// </summary>
        public virtual CipherEnum[] GetInPipe(int pipeRingBufIdx)
        {           
            List<CipherEnum> inPipeList = new List<CipherEnum>();
            lock (_lock)
            {
                inPipeList = InPipes.ElementAt(pipeRingBufIdx).ToList();                
            }

            return (inPipeList != null && inPipeList.Count > 0) ? inPipeList.ToArray() :
                    new List<CipherEnum>().ToArray();                
        }

        /// <summary>
        /// OutPipe will always be generated from <see cref="InPipe"/>
        /// </summary>
        public virtual CipherEnum[] GetOutPipe(int pipeRingBufIdx)
        {
            List<CipherEnum> outPipeList = new List<CipherEnum>();
            lock (_lock)
            {
                outPipeList = OutPipes.ElementAt(pipeRingBufIdx).ToList();
            }

            return (outPipeList != null && outPipeList.Count > 0) ? outPipeList.ToArray() :
                    new List<CipherEnum>().ToArray();
        }


        /// <summary>
        /// PipeString will always be generated on the fly from <see cref="InPipes"/>
        /// </summary>
        public override string PipeString
        {
            get
            {
                string pipeString = "";
                foreach (CipherEnum cipher in _inPipe)
                    pipeString += cipher.GetCipherChar();
                
                return pipeString;
            }
        }


        #endregion fields and properties

        #region ctor Helpers 
        protected void BuildInPipes(CipherEnum[] theInPipe)
        {
            inPipes.Clear();

            if (theInPipe == null || theInPipe.Length == 0)
            {
                outPipes.Clear();
                _inPipe = new CipherEnum[0];
                return;
            }
                

            lock (_lock)
            {
                int l = theInPipe.Length;
                for (int i = 0; i < l; i++)
                {
                    List<CipherEnum> iInPipeList = new List<CipherEnum>();
                    for (int j = 0; j < l; j++)
                    {
                        iInPipeList.Add(theInPipe[(i + j) % l]);
                    }
                    inPipes.Add(iInPipeList.ToArray());
                }
                outPipes = new List<CipherEnum[]>(inPipes.Count);
                foreach (var iPipe in inPipes)
                {
                    CipherEnum[] oPipe = iPipe.Reverse<CipherEnum>().ToArray();
                    outPipes.Add(oPipe);
                }
            }
        }

        #endregion ctor Helpers


        #region ctor CipherPipe

        /// <summary>
        /// parameterless default constructor for <see cref="CipherPipe"/>
        /// </summary>
        public MatrixPipe()
        {
            cipherKey = ""; //
            cipherHash = "";
            inPipes = new List<CipherEnum[]>(8);
            encodeType = EncodingType.Base64;
            zType = ZipType.None;
            kHash = KeyHash.Hex;
            CMode2 = CipherMode2.CFB;
        }


        /// <summary>
        /// MatrixPipe constructor with an array of <see cref="T:CipherEnum[]"/> as inpipe
        /// </summary>
        /// <param name="cipherEnums">array of <see cref="T:CipherEnum[]"/> as inpipe</param>
        /// <param name="maxpipe">size of max. pipe stages, can't be greater than 8</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <param name="zpType"><see cref="ZipType"/></param>
        /// <param name="kh"><see cref="KeyHash"/></param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        public MatrixPipe(CipherEnum[] cipherEnums, uint maxpipe = 8,
            EncodingType encType = EncodingType.Base64, ZipType zpType = ZipType.None, KeyHash kh = KeyHash.Hex,
            CipherMode2 cmode2 = CipherMode2.CFB)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = Constants.MAX_PIPE_LEN;

            int isize = Math.Min(((int)cipherEnums.Length), ((int)maxpipe));
            _inPipe = new CipherEnum[8];
            Array.Copy(cipherEnums, _inPipe, isize);
            BuildInPipes(_inPipe);

            encodeType = encType;
            zType = zpType;
            kHash = kh;
            CMode2 = cmode2;
        }

        /// <summary>
        /// MatrixPipe constructor with an array of <see cref="T:string[]"/> cipherAlgos as inpipe
        /// </summary>
        /// <param name="cipherAlgos">array of <see cref="T:string[]"/> as inpipe</param>
        /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <param name="zpType"><see cref="Zip.ZipType"/></param>
        /// <param name="kh"><see cref="KeyHash"/></param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        public MatrixPipe(string[] cipherAlgos, uint maxpipe = 8, EncodingType encType = EncodingType.Base64,
            ZipType zpType = ZipType.None, KeyHash kh = KeyHash.Hex,
            CipherMode2 cmode2 = CipherMode2.CFB)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = Constants.MAX_PIPE_LEN; // if somebody wants more, he/she/it gets less

            List<CipherEnum> cipherEnums = new List<CipherEnum>();
            int cnt = 0;
            foreach (string algo in cipherAlgos)
            {
                if (!string.IsNullOrEmpty(algo))
                {
                    CipherEnum cipherAlgo = CipherEnum.Aes;
                    if (!Enum.TryParse<CipherEnum>(algo, out cipherAlgo))
                        cipherAlgo = CipherEnum.Aes;

                    cipherEnums.Add(cipherAlgo);

                    if (++cnt > maxpipe)
                        break;
                }
            }

            _inPipe = cipherEnums.ToArray();
            BuildInPipes(_inPipe);

            encodeType = encType;
            kHash = kh;
            zType = zpType;
            CMode2 = cmode2;
        }

        /// <summary>
        /// MatrixPipe ctor with array of user key bytes
        /// </summary>
        /// <param name="keyBytes">user key bytes</param>
        /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <param name="zpType"><see cref="Zip.ZipType"/></param>
        /// <param name="kh"><see cref="KeyHash"/></param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        /// <param name="verbose"></param>
        /// <exception cref="ArgumentException"></exception>
        public MatrixPipe(byte[] keyBytes, uint maxpipe = 8,
            EncodingType encType = EncodingType.Base64, ZipType zpType = ZipType.None, KeyHash kh = KeyHash.Hex,
            CipherMode2 cmode2 = CipherMode2.CFB, bool verbose = false)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = Constants.MAX_PIPE_LEN; // if somebody wants more, he/she/it gets less

            List<CipherEnum> pipeList = new List<CipherEnum>();

            HashSet<byte> hashBytes = new HashSet<byte>();
            for (int i = 0; i < keyBytes.Length && pipeList.Count < maxpipe; i++)
            {
                byte cb = (byte)((int)((int)keyBytes[i] % 0x1d));
                // TODO: future design
                // if (hashBytes.Contains(cb)) // mit magic add to generate deterministic more on same bytes
                //     cb = (byte)((int)(cb + Math.Pow(2, i) + keyBytes.Length) % 0x1d);                
                if (!hashBytes.Contains(cb))
                {
                    hashBytes.Add(cb);
                    CipherEnum cipherEnm = CipherEnumExtensions.ByteCipherDict[cb];
                    pipeList.Add(cipherEnm);

                    if (verbose)
                        Console.Out.WriteLine("keybyts[" + i + "]=" + keyBytes[i] + " byte cb = " + (int)cb + " CipherEnum: " + cipherEnm);
                }
            }

            _inPipe = pipeList.ToArray();
            BuildInPipes(_inPipe);

            zType = zpType;
            encodeType = encType;
            kHash = kh;
            CMode2 = cmode2;

        }

        public MatrixPipe(CipherPipe pipe)
        {
            int maxpipe = 8;
            cipherKey = pipe.cipherKey;
            cipherHash = pipe.cipherHash;
            this.EncodeType = pipe.EncodeType;
            this.KHash = pipe.KHash;
            this.ZType = pipe.ZType;
            this.CMode = pipe.CMode;
            this.CMode2 = pipe.CMode2;
            _inPipe = pipe.InPipe;
            BuildInPipes(_inPipe);
        }

        /// <summary>
        /// Constructs a <see cref="MatrixPipe"/> from key and hash
        /// by getting <see cref="T:byte[]">byte[] keybytes</see> with <see cref="CryptHelper.GetUserKeyBytes(string, string, int)"/>
        /// </summary>
        /// <param name="key">secret key to generate pipe</param>
        /// <param name="hash">hash value of secret key</param>
        /// <param name="encType"></param>
        /// <param name="zpType"></param>
        /// <param name="kh"></param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        /// <param name="verbose"></param>
        public MatrixPipe(string key, string hash, EncodingType encType = EncodingType.Base64,
            ZipType zpType = ZipType.None, KeyHash kh = KeyHash.Hex,
            CipherMode2 cmode2 = CipherMode2.CFB, bool verbose = false)
            : this(CryptHelper.GetKeyBytesSimple(key, hash, 32), Constants.MAX_PIPE_LEN, encType, zpType, kh, cmode2, verbose)
        {
            cipherKey = key;
            cipherHash = hash;
        }

        /// <summary>
        /// MatrixPipe ctor with only key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="verbose"></param>
        public MatrixPipe(string key, bool verbose = false)
            : this(key, EnDeCodeHelper.KeyToHex(key), EncodingType.Base64, ZipType.None, KeyHash.Hex,
                  CipherMode2.CFB, verbose)
        {
            cipherKey = key;
        }

        #endregion ctor CipherPipe

        #region json

        /// <summary>
        /// ToJson 
        /// </summary>
        /// <returns>serialized string</returns>
        public override string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="json">serialized json</param>
        /// <returns><see cref="CipherPipe"/></returns>
        public new MatrixPipe FromJson(string json)
        {
            MatrixPipe pipe = JsonConvert.DeserializeObject<MatrixPipe>(json);
            if (pipe == null)
            {
                this.inPipes = pipe.InPipes;
                this.encodeType = pipe.EncodeType;
                this.kHash = pipe.KHash;
                this.zType = pipe.ZType;
                this.cipherKey = pipe.cipherKey;
                this.cipherHash = pipe.cipherHash;
                this.CMode2 = pipe.CMode2;
            }
            return pipe;
        }

        #endregion json


        #region multiple rounds en-de-cryption

        /// <summary>
        /// MerryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
        /// </summary>
        /// <param name="inBytes">plain <see cref="T:byte[]"/> to encrypt</param>
        /// <param name="secretKey">user secret key to use for all symmetric cipher algorithms in the pipe</param>
        /// <param name="hashIv">hash key iv relational to secret key</param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        /// <returns>encrypted byte[]</returns>
        public override byte[] MerryGoRoundEncrpyt(byte[] inBytes, string secretKey, string hashIv, CipherMode2 cmode2)
        {
            if (_inPipe == null || _inPipe.Length == 0)   // return immideate, when zero round cipher merry go round
                return inBytes;

            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                secretKey = "";
            string hash = hashIv ?? "";
            if (string.IsNullOrEmpty(hash) && !string.IsNullOrEmpty(secretKey))
                hash = (KHash != null) ? KHash.Hash(secretKey) : EnDeCodeHelper.KeyToHex(secretKey);
            cipherKey = string.IsNullOrEmpty(secretKey) ? cipherKey : secretKey;
            cipherHash = hash;
            CMode2 = cmode2;

            int suplement256 = ((inBytes.Length % 256) == 0) ? inBytes.Length : (256 - (inBytes.Length % 256));
            int finalLen = (inBytes.Length + suplement256);
            int byteQuater = finalLen / 8;
            byte[] encryptedBytes = new byte[finalLen];
            // TODO: parallel processing
            //Task[] taskArray = new Task[8];
            //for (int i = 0; i < 8; i++)
            //{
            //    taskArray[i] = Task.Factory.StartNew((object obj) =>
            //    {
            //        byte[] outEightPart = new byte[byteQuater];
            //        Array.Copy(inBytes, i * byteQuater, inEightPart, 0, byteQuater);
            //        Array.Copy(inEightPart, 0, outEightPart, 0, byteQuater);

            //        foreach (CipherEnum cipher in GetInPipe(i))
            //        {
            //            outEightPart = EncryptBytesFast(inEightPart, cipher, cipherKey, cipherHash, CMode2);
            //            inEightPart = outEightPart;
            //        }

            //        Array.Copy(outEightPart, 0, encryptedBytes, i * byteQuater, byteQuater);
            //    });
            //}
            //Task.WaitAll(taskArray);

            for (int i = 0; i < 8; i++)
            {
                byte[] inEightPart = new byte[byteQuater];
                byte[] outEightPart = new byte[byteQuater];
                if (inBytes.Length < ((i + 1) * byteQuater))
                {
                    int iBlen = ((i + 1) * byteQuater) - inBytes.Length;
                    Array.Copy(inBytes, i * byteQuater, inEightPart, 0, iBlen);
                    Array.Copy(inEightPart, 0, outEightPart, 0, iBlen);
                }
                else
                {
                    Array.Copy(inBytes, i * byteQuater, inEightPart, 0, byteQuater);
                    Array.Copy(inEightPart, 0, outEightPart, 0, byteQuater);
                }

                foreach (CipherEnum cipher in GetInPipe(i))
                {
                    outEightPart = EncryptBytesFast(inEightPart, cipher, cipherKey, cipherHash, CMode2);
                    inEightPart = outEightPart;
                }
                
                Array.Copy(outEightPart, 0, encryptedBytes, i * byteQuater, byteQuater);
            }
           
#if STAGE_DICT_PIPE_DBG
            stageDictionary.Add(cipher, encryptedBytes);
#endif

            return encryptedBytes;

        }

        /// <summary>
        /// DecrpytRoundGoMerry against clock turn -
        /// starts merry to turn arround from right to left against clock hour cycle 
        /// </summary>
        /// <param name="cipherBytes">encrypted byte array</param>
        /// <param name="secretKey">user secret key, normally email address</param>
        /// <param name="hashIv">hash relational to secret kay</param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        /// <returns><see cref="T:byte[]"/> plain bytes</returns>
        public override byte[] DecrpytRoundGoMerry(byte[] cipherBytes, string secretKey, string hashIv, CipherMode2 cmode2)
        {
            if (_inPipe == null || _inPipe.Length == 0) // when 0 rounds carusell, return immideate inBytes
                return cipherBytes;

            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                secretKey = "";
            string hash = hashIv ?? "";
            if (string.IsNullOrEmpty(hash) && !string.IsNullOrEmpty(secretKey))
                hash = (KHash != null) ? KHash.Hash(secretKey) : EnDeCodeHelper.KeyToHex(secretKey);
            cipherKey = string.IsNullOrEmpty(secretKey) ? cipherKey : secretKey;
            cipherHash = hash;
            CMode2 = cmode2;

            int suplement256 = ((cipherBytes.Length % 256) == 0) ? cipherBytes.Length : (256 - (cipherBytes.Length % 256));
            int finalLen = (cipherBytes.Length + suplement256);
            int byteQuater = finalLen / 8;
            byte[] decryptedBytes = new byte[finalLen];
            Array.Copy(cipherBytes, 0, decryptedBytes, 0, cipherBytes.Length);

            for (int i = 0; i < 8; i++)
            {
                byte[] cipherEightPart = new byte[byteQuater];
                byte[] outEightPart = new byte[byteQuater];
                Array.Copy(cipherBytes, i * byteQuater, cipherEightPart, 0, byteQuater);
                Array.Copy(cipherEightPart, 0, outEightPart, 0, byteQuater);

                foreach (CipherEnum cipher in GetOutPipe(i))
                {
                    outEightPart = DecryptBytesFast(cipherEightPart, cipher, cipherKey, cipherHash, cmode2);
                    cipherEightPart = outEightPart;
                }

                Array.Copy(outEightPart, 0, decryptedBytes, i * byteQuater, byteQuater);
            }
            
            return decryptedBytes;

        }

#endregion multiple rounds en-de-cryption



        #region graphics bmp creation
        /*
        /// <summary>
        /// GenerateEncryptPipeImage - generates image for symmetric cipher encryption pipeline
        /// </summary>
        /// <returns><see cref="Image">the image</see></returns>
        public virtual Image GenerateEncryptPipeImage()
        {
            System.Drawing.Bitmap mergeimg = new Bitmap(Properties.Resource.BlankEncrypt_640x108, new Size(640, 108)), ximage;
            System.Drawing.Bitmap? gifStartImage = new Bitmap(Properties.Resource.BlankEncrypt_640x96, new Size(640, 108));
            List<Bitmap> bitmaps = new List<Bitmap>();

            string bmpName = "";
            int w = 64, offset = 0, startset = 0;
            if (this.ZType != EU.CqrXs.Zip.ZipType.None)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;

                    ximage = new Bitmap(Properties.Resource.block_arrow_right_zip, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(0, 20, w, 64));

                    string drawString = this.ZType.ToString();
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

            for (int i = 0; (i < this.InPipe.Length); i++)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;
                    char ch = this.InPipe[i].GetCipherChar();
                    bmpName = $"arrow_right-{i}";
                    if (i < 2)
                        bmpName = (i == 0) ? "arrow_right-c" : "arrow_right-e";
                    object obj = Properties.Resource.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
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
                for (int i = 0; (i < this.InPipe.Length); i++)
                {

                    Color color = (i < 5) ? ColorTranslator.FromHtml("#0000ee") : ColorTranslator.FromHtml("#0000dd");
                    string drawString = this.InPipe[i].ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(color);
                    float x = offset;
                    float y = 2F + ((i % 4) * 23.0F);
                    switch (i)
                    {
                        case 0: y = 1F; break;
                        case 1: x = offset - 1.0F; y = 84F; break;
                        case 2: x = offset - 1.5F; y = 1F;  break;
                        case 3: x = offset - 2.0F; y = 86F; break;
                        case 4: x = offset - 2.5F; y = 2F;  break;
                        case 5: x = offset - 3.0F; y = 84F; break;
                        case 6: x = offset - 3.5F; y = 2F;  break;
                        case 7: x = offset - 4.0F; y = 76F;
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
            gifStartImage = new Bitmap(mergeimg, 640, 108);

            if (this.EncodeType != EU.CqrXs.Crypt.EnDeCoding.EncodingType.None)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resource.encoding_right_end_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));
                    string drawString = this.EncodeType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#bf0fef"));
                    float x = offset + 1.0F;
                    float y = 4.0F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                }
                bitmaps.Add(new Bitmap(mergeimg, 640, 108));
                gifStartImage = new Bitmap(mergeimg, 640, 108);
            }

            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, 125);
            //GifEncoder gifAnimEncoder = new GifEncoder(bitmaps.ToArray(), 1, ts);
            //Bitmap animGif = new Bitmap(gifAnimEncoder._memoryStream, false);
            //return animGif;
            // animGif.Save("H:\\tmp\\" + DateTime.Now.ToString("yyyy-MM-DD_hhmmss") + ".gif");
            // gifAnimEncoder.Dispose();
            return gifStartImage;

        }


        /// <summary>
        /// GenerateDecryptPipeImage generates an image for decrypt symmetric cipher pipeline 
        /// </summary>
        /// <returns><see cref="Image">the image</see></returns>
        public virtual Image GenerateDecryptPipeImage()
        {
            System.Drawing.Bitmap mergeimg = new Bitmap(640, 108), ximage;
            string bmpName = "";
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                int w = 64, offset = 0, startset = 0;
                if (this.EncodeType != EU.CqrXs.Crypt.EnDeCoding.EncodingType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resource.encoding_right_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    string drawString = this.EncodeType.ToString();
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

                for (int i = 0; (i < this.OutPipe.Length); i++)
                {
                    w = 60;
                    int r = 7 - i;
                    char ch = this.OutPipe[i].GetCipherChar();
                    bmpName = $"arrow_right-{r}";
                    if (i >= 6)
                        bmpName = (i == 6) ? "arrow_right-e" : "arrow_right-c";
                    object obj = Properties.Resource.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
                    ximage = new Bitmap(((System.Drawing.Bitmap)(obj)), new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    offset += w;
                }

                offset = startset;
                for (int i = 0; (i < this.OutPipe.Length); i++)
                {
                    w = 60;
                    int r = 7 - i;

                    Color color = (i < 4) ? ColorTranslator.FromHtml("#2200aa") : ColorTranslator.FromHtml("#0000dd");
                    string drawString = this.OutPipe[i].ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(color);
                    float x = offset + 2.0F;
                    float y = 1.5F + ((i % 4) * 23.0F);
                    switch (i)
                    {
                        case 0: y = 1F; break;
                        case 1: x = offset - 1.0F; y = 84F; break;
                        case 2: x = offset - 1.5F; y = 1F; break;
                        case 3: x = offset - 2.0F; y = 86F; break;
                        case 4: x = offset - 2.5F; y = 2F; break;
                        case 5: x = offset - 3.0F; y = 84F; break;
                        case 6: x = offset - 3.5F; y = 2F; break;
                        case 7:
                            x = offset - 4.0F; y = 76F;
                            drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold); break;
                        default: y = 1F + ((i % 4) * 23.0F); break;
                    }
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.NoWrap;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }

                if (this.ZType != EU.CqrXs.Zip.ZipType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resource.compress_right_end_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    string drawString = this.ZType.GetUnzipString();
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
        */
        #endregion graphics bmp creation

        //public static CipherEnum SymmCipherToCipher(SymmCipherEnum sCipher)
        //{
        //    return sCipher.ToCipherEnum();
        //}

    }

}

