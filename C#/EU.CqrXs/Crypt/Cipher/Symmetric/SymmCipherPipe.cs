using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;

namespace EU.CqrXs.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Provides a simple crypt pipe for <see cref="SymmCipherEnum"/>
    /// </summary>
    public class SymmCipherPipe : CipherPipe
    {
        
        protected internal SymmCipherEnum[] inSymmPipe;                

        public SymmCipherEnum[] InSymmPipe
        {
            get => inSymmPipe; 
            set
            {                
                inSymmPipe = value;
                InPipe = value.ToList().ConvertAll(new Converter<SymmCipherEnum, CipherEnum>(SymmCipherToCipher)).ToArray();
            }
        }

        public SymmCipherEnum[] OutSymmPipe { get => new List<SymmCipherEnum>(InSymmPipe).Reverse<SymmCipherEnum>().ToArray(); }


        public new string PipeString
        {
            get
            {
                string pipeString = string.Empty;
                foreach (SymmCipherEnum symmCipher in inSymmPipe)
                    pipeString += symmCipher.GetSymmCipherChar();
                return pipeString;
            }
        }

        //#if DEBUG
        //        public Dictionary<SymmCipherEnum, byte[]> stageDictionary = new Dictionary<SymmCipherEnum, byte[]>();

        //        public string HexStages
        //        {
        //            get
        //            {
        //                string hexOut = string.Empty;
        //                foreach (var stage in stageDictionary)
        //                {
        //                    hexOut += stage.Key.ToString() + "\r\n" + Hex16.ToHex16(stage.Value) + "\r\n";
        //                }

        //                return hexOut;
        //            }
        //        }
        //#endif

        #region ctor SymmCipherPipe

        public SymmCipherPipe() : base()
        {
            inSymmPipe = (new List<SymmCipherEnum>()).ToArray();

            Area23Log.LogOriginMsg("SymmCipherPipe", $"Generating symmetric cipher pipe: {PipeString}, encoding = {encodeType}, zipping={zType}, hashing={kHash}");
        }

        /// <summary>
        /// SymmCipherPipe constructor with an array of <see cref="T:SymmCipherEnum[]"/> as inpipe
        /// </summary>
        /// <param name="symmCipherEnums">array of <see cref="T:SymmCipherEnum[]"/> as inpipe</param>
        public SymmCipherPipe(
            SymmCipherEnum[] symmCipherEnums, 
            uint maxpipe = 8, 
            EncodingType encType = EncodingType.Base64, 
            ZipType zpType = ZipType.None, 
            KeyHash kh = KeyHash.Hex) : 
            base(
                symmCipherEnums.ToList().ConvertAll(new Converter<SymmCipherEnum, CipherEnum>(SymmCipherToCipher)).ToArray(),
                maxpipe,
                encType,
                zpType,
                kh)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less

            int isize = Math.Min(((int)symmCipherEnums.Length), ((int)maxpipe));
            inSymmPipe = new SymmCipherEnum[isize];
            Array.Copy(symmCipherEnums, inSymmPipe, isize);
                       
            Area23Log.LogOriginMsg("SymmCipherPipe", $"Generating symmetric cipher pipe: {PipeString}, encoding = {encType}, zipping={zpType}, hashing={kh}");
        }

        /// <summary>
        /// SymmCipherPipe constructor with an array of <see cref="T:string[]"/> as inpipe
        /// </summary>
        /// <param name="symmCipherAlgos">array of <see cref="T:string[]"/> as inpipe</param>
        public SymmCipherPipe(string[] symmCipherAlgos, uint maxpipe = 8,
            EncodingType encType = EncodingType.Base64, ZipType zpType = ZipType.None, KeyHash kh = KeyHash.Hex)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less
            int maxCnt = 0; 
            List<SymmCipherEnum> symmCipherEnums = new List<SymmCipherEnum>();
            foreach (string algo in symmCipherAlgos)
            {
                if (!string.IsNullOrEmpty(algo))
                {
                    SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes;
                    if (!Enum.TryParse<SymmCipherEnum>(algo, out cipherAlgo))
                        cipherAlgo = SymmCipherEnum.Aes;
                    
                    symmCipherEnums.Add(cipherAlgo);
                    if (++maxCnt > maxpipe)
                        break;
                }
            }

            InSymmPipe = symmCipherEnums.ToArray();
            encodeType = encType;
            kHash = kh;
            zType = zpType;
            // Area23Log.LogOriginMsg("SymmCipherPipe", $"Generating symmetric cipher pipe: {PipeString}, encoding = {encType}, zipping={zpType}, hashing={kh}");
        }

        /// <summary>
        /// SymmCipherPipe ctor with array of user key bytes
        /// </summary>
        /// <param name="keyBytes">user key bytes</param>
        /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
        public SymmCipherPipe(byte[] keyBytes, uint maxpipe = 8, 
            EncodingType encType = EncodingType.Base64, ZipType zpType = ZipType.None, KeyHash kh = KeyHash.Hex)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less

            ushort scnt = 0;
            List<SymmCipherEnum> pipeList = new List<SymmCipherEnum>();
            Dictionary<byte, SymmCipherEnum> symDict = SymmCipherEnumExtensions.GetByteSymmCipherDict();

            string hexString = string.Empty;
            HashSet<byte> hashBytes = new HashSet<byte>();
            for (int bc = 0; (bc < keyBytes.Length && hashBytes.Count < maxpipe); bc++)
            { 
                byte msb = (byte)(keyBytes[bc] / 0x10);
                byte lsb = (byte)(keyBytes[bc] % 0x10);
                if (!hashBytes.Contains(msb))
                {
                    hashBytes.Add(msb);
                    pipeList.Add(symDict[msb]);
                }
                if (!hashBytes.Contains(lsb))
                {
                    hashBytes.Add(lsb);
                    pipeList.Add(symDict[lsb]);
                }
            }


            InSymmPipe = pipeList.ToArray();

            encodeType = encType;
            kHash = kh;
            zType = zpType;

            // Area23Log.LogOriginMsg("SymmCipherPipe", $"Generating symmetric cipher pipe: {PipeString}, encoding = {encType}, zipping={zpType}, hashing={kh}");
        }

        /// <summary>
        /// Constructs a <see cref="SymmCipherPipe"/> from key and hash
        /// by getting <see cref="T:byte[]">byte[] keybytes</see> with <see cref="CryptHelper.GetUserKeyBytes(string, string, int)"/>
        /// </summary>
        /// <param name="key">secret key to generate pipe</param>
        /// <param name="hash">hash value of secret key</param>
        public SymmCipherPipe(string key, string hash, 
                            EncodingType encType = EncodingType.Base64, 
                            ZipType zpType = ZipType.None, KeyHash kh = KeyHash.Hex)
            : this(CryptHelper.GetKeyBytesSimple(key, hash, 16), Constants.MAX_PIPE_LEN, encType, zpType, kh)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");            
            
            cipherKey = key;
            cipherHash = (string.IsNullOrEmpty(hash)) ? kHash.Hash(key) : hash;
        }

        /// <summary>
        /// SymmCipherPipe ctor with only key
        /// </summary>
        /// <param name="key"></param>
        public SymmCipherPipe(string key)
            : this(key, EnDeCodeHelper.KeyToHex(key))
        {
            cipherKey = key;
        }

        #endregion ctor SymmCipherPipe

        #region json

        /// <summary>
        /// ToJson 
        /// </summary>
        /// <returns>serialized string</returns>
        public new string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="json">serialized json</param>
        /// <returns><see cref="CipherPipe"/></returns>
        public new SymmCipherPipe FromJson(string json)
        {
            SymmCipherPipe pipe = JsonConvert.DeserializeObject<SymmCipherPipe>(json);
            if (pipe == null)
            {
                this.inPipe = pipe.InPipe;
                this.inSymmPipe = pipe.InSymmPipe;
                this.encodeType = pipe.EncodeType;
                this.kHash = pipe.KHash;
                this.zType = pipe.ZType;
                this.cipherKey = pipe.cipherKey;
                this.cipherHash = pipe.cipherHash;
            }
            return pipe;
        }

        #endregion json

        #region static members EncryptBytesFast DecryptBytesFast

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="hashIv">key's hash</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytesFast(byte[] inBytes, SymmCipherEnum cipherAlgo,
            string secretKey, string hashIv)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("seretkey");
            string hash = (!string.IsNullOrEmpty(hashIv)) ? hashIv : KeyHash.Hex.Hash(secretKey);

            byte[] encryptBytes = inBytes;
            
            SymmCryptParams cpParams = new SymmCryptParams(cipherAlgo, secretKey, hash);
            Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);
            encryptBytes = cryptBounceCastle.Encrypt(inBytes);

            return encryptBytes;
        }

        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/>both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="hashIv">key's hash</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytesFast(byte[] cipherBytes, SymmCipherEnum cipherAlgo,
            string secretKey, string hashIv)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("seretkey");
            string hash = (!string.IsNullOrEmpty(hashIv)) ? hashIv : KeyHash.Hex.Hash(secretKey);

            byte[] decryptBytes = cipherBytes;

            SymmCryptParams cpParams = new SymmCryptParams(cipherAlgo, secretKey, hash);
            Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);
            decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);

            return EnDeCodeHelper.GetBytesTrimNulls(decryptBytes);
        }

		#endregion static members EncryptBytesFast DecryptBytesFast

		#region multiple rounds en-de-cryption

		/// <summary>
		/// MerryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
		/// </summary>
		/// <param name="inBytes">plain <see cref="T:byte[]"/> to encrypt</param>
		/// <param name="secretKey">user secret key to use for all symmetric cipher algorithms in the pipe</param>
		/// <param name="hashIv">hash key iv relational to secret key</param>
		/// <returns>encrypted byte[]</returns>
		public override byte[] MerryGoRoundEncrpyt(byte[] inBytes, string secretKey = "", string hashIv = "")
        {
            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                throw new ArgumentNullException("secretKey");
            if (string.IsNullOrEmpty(hashIv) && string.IsNullOrEmpty(cipherHash))
                hashIv = (!string.IsNullOrEmpty(hashIv)) ? hashIv : (KHash != null) ? KHash.Hash(secretKey) : EnDeCodeHelper.KeyToHex(secretKey);
            cipherKey = secretKey;
            cipherHash = hashIv;

            byte[] encryptedBytes = new byte[inBytes.Length];
            Array.Copy(inBytes, 0, encryptedBytes, 0, inBytes.Length);
//#if DEBUG
//            stageDictionary = new Dictionary<SymmCipherEnum, byte[]>();
//            // stageDictionary.Add(SymmCipherEnum.ZenMatrix, inBytes);
//#endif
            foreach (SymmCipherEnum symmCipher in InPipe)
            {
                encryptedBytes = EncryptBytesFast(inBytes, symmCipher, secretKey, hashIv);
                inBytes = encryptedBytes;
//#if DEBUG
//                stageDictionary.Add(symmCipher, encryptedBytes);
//#endif
            }

            return encryptedBytes;
        }

        /// <summary>
        /// DecrpytRoundGoMerry against clock turn -
        /// starts merry to turn arround from right to left against clock hour cycle 
        /// </summary>
        /// <param name="cipherBytes">encrypted byte array</param>
        /// <param name="secretKey">user secret key, normally email address</param>
        /// <param name="hashIv">hash relational to secret kay</param>
        /// <returns><see cref="T:byte[]"/> plain bytes</returns>
        public override byte[] DecrpytRoundGoMerry(byte[] cipherBytes, string secretKey = "", string hashIv = "")
        {
            if (string.IsNullOrEmpty(cipherKey) && string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("secretKey");

            cipherKey = string.IsNullOrEmpty(secretKey) ? cipherKey : secretKey;

            if (string.IsNullOrEmpty(hashIv) && string.IsNullOrEmpty(cipherHash))
                hashIv = (!string.IsNullOrEmpty(hashIv)) ? hashIv : (kHash != null) ? kHash.Hash(secretKey) : EnDeCodeHelper.KeyToHex(cipherKey);

            cipherHash = string.IsNullOrEmpty(hashIv) ? cipherHash : hashIv;

            long outByteLen = (OutPipe == null || OutPipe.Length == 0) ? cipherBytes.Length : ((cipherBytes.Length * 3) + 1);
            byte[] decryptedBytes = new byte[outByteLen];
//#if DEBUG
//            stageDictionary = new Dictionary<SymmCipherEnum, byte[]>();
//            // stageDictionary.Add(SymmCipherEnum.ZenMatrix, cipherBytes);
//#endif 
            if (OutPipe == null || OutPipe.Length == 0)
                Array.Copy(cipherBytes, 0, decryptedBytes, 0, cipherBytes.Length);
            else
                foreach (SymmCipherEnum symmCipher in OutPipe)
                {
                    decryptedBytes = DecryptBytesFast(cipherBytes, symmCipher, secretKey, hashIv);
                    cipherBytes = decryptedBytes;
//#if DEBUG
//                    stageDictionary.Add(symmCipher, cipherBytes);
//#endif
                }

            return decryptedBytes;
        }


        public override byte[] EncrpytGoRounds(byte[] inBytes, string secretKey, ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                throw new ArgumentNullException("seretkey");

            cipherKey = (!string.IsNullOrEmpty(secretKey)) ? secretKey : cipherKey;
            KHash = keyHash;
            ZType = zipBefore;
            cipherHash = KHash.Hash(secretKey);
            
            // zip if requested
            byte[] zippedBytes = (zipBefore != ZipType.None) ? zipBefore.Zip(inBytes) : inBytes;
            // encrypt in a marry go round way
            return MerryGoRoundEncrpyt(zippedBytes, secretKey, cipherHash);
        }

        public override byte[] DecrpytRoundsGo(byte[] cipherBytes, string secretKey, ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                throw new ArgumentNullException("seretkey");

            cipherKey = (!string.IsNullOrEmpty(secretKey)) ? secretKey : cipherKey;
            cipherHash = keyHash.Hash(secretKey);
            ZType = unzipAfter;
            KHash = keyHash;

            // perform multi crypt pipe stages
            byte[] intermediatBytes = DecrpytRoundGoMerry(cipherBytes, secretKey, cipherHash);
            // Unzip after if necessary
            byte[] decryptedBytes = (unzipAfter != ZipType.None) ? unzipAfter.Unzip(intermediatBytes) : intermediatBytes;

            return decryptedBytes;
        }

        public byte[] Encrpyt(byte[] plainBytes, string cryptKey, EncodingType encoding = EncodingType.Base64,
            ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // construct symmetric cipher pipeline with cryptKey and pass pipeString as out param                          

            // perform multi crypt pipe stages
            byte[] encryptedBytes = this.EncrpytGoRounds(plainBytes, cryptKey, zipBefore, keyHash);
            // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
            string encoded = encoding.GetEnCoder().Encode(encryptedBytes);
            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(encoded);

            return encodedBytes;
        }

        public byte[] Decrpyt(byte[] encodedBytes, string cryptKey, EncodingType decoding = EncodingType.Base64,
            ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            string decodedString = System.Text.Encoding.UTF8.GetString(encodedBytes);

            byte[] cipherBytes = decoding.GetEnCoder().Decode(decodedString);

            // staged decryption of bytes
            byte[] unroundedMerryBytes = DecrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);
            
            return unroundedMerryBytes;
            // return unroundedMerryBytes.TrimEnd((byte)0).ToArray();
        }


        public override string EncrpytEncode(byte[] inBytes, string secretKey, 
            EncodingType encType = EncodingType.Base64,
            ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                secretKey = "";

            cipherKey = (!string.IsNullOrEmpty(secretKey)) ? secretKey : cipherKey;
            encodeType = encType;
            ZType = zipBefore;
            KHash = keyHash;
            cipherHash = (!string.IsNullOrEmpty(secretKey)) ? KHash.Hash(secretKey) : "";

            // zip if requested
            byte[] zippedBytes = (zipBefore != ZipType.None) ? zipBefore.Zip(inBytes) : inBytes;
            // now encrypt in a merry go round 
            byte[] outBytes = MerryGoRoundEncrpyt(zippedBytes, secretKey, cipherHash);
            // encode to ascii string after encryption pipe
            string cryptedEncoded = encType.EnCode(outBytes);

            return cryptedEncoded;
        }

        public override byte[] DecodeDecrpyt(string encoded, string secretKey, 
            EncodingType encType = EncodingType.Base64,
            ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                secretKey = "";

            cipherKey = (!string.IsNullOrEmpty(secretKey)) ? secretKey : cipherKey;
            cipherHash = (!string.IsNullOrEmpty(secretKey)) ? keyHash.Hash(secretKey) : "";

            encodeType = encType;
            ZType = unzipAfter;
            KHash = keyHash;

            // decode encoded ascii string to byte array
            byte[] cipherBytes = encodeType.GetEnCoder().Decode(encoded);
            // perform multi crypt pipe stages
            byte[] intermediatBytes = DecrpytRoundGoMerry(cipherBytes, secretKey, cipherHash);
            // Unzip after if necessary
            byte[] decryptedBytes = (unzipAfter != ZipType.None) ? unzipAfter.Unzip(intermediatBytes) : intermediatBytes;

            return decryptedBytes;
        }


        public override byte[] EncryptEncodeBytes(byte[] inBytes, string secretKey, string hashIV, 
            EncodingType encType = EncodingType.Base64,
            ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                secretKey = "";

            cipherKey = (!string.IsNullOrEmpty(secretKey)) ? secretKey : cipherKey;
            if (string.IsNullOrEmpty(hashIV))
                cipherHash = (!string.IsNullOrEmpty(secretKey)) ? keyHash.Hash(secretKey) : "";
            else
                cipherHash = hashIV;
            encodeType = encType;
            ZType = zipBefore;
            KHash = keyHash;

            // zip if requested
            byte[] zippedBytes = (zipBefore != ZipType.None) ? zipBefore.Zip(inBytes) : inBytes;
            // now encrypt with pipe
            byte[] outBytes = MerryGoRoundEncrpyt(zippedBytes, secretKey, cipherHash);
            // encode after encryption pipe
            if (encType == EncodingType.None)
                return outBytes;

            return System.Text.Encoding.UTF8.GetBytes(encType.EnCode(outBytes));
        }

        public override byte[] DecodeDecrpytBytes(byte[] encodedBytes, string secretKey, string hashIV, 
            EncodingType encType = EncodingType.Base64,
            ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(cipherKey))
                secretKey = "";

            cipherKey = (!string.IsNullOrEmpty(secretKey)) ? secretKey : cipherKey;
            if (string.IsNullOrEmpty(hashIV))
                cipherHash = (!string.IsNullOrEmpty(secretKey)) ? keyHash.Hash(secretKey) : "";
            else
                cipherHash = hashIV;
            cipherHash = hashIV;
            encodeType = encType;
            ZType = unzipAfter;
            KHash = keyHash;

            // Decoded encoded bytes first, if necessary
            byte[] cipherBytes = (encType != EncodingType.None) ?
                encodeType.GetEnCoder().Decode(System.Text.Encoding.UTF8.GetString(encodedBytes)) :
                encodedBytes;
            // perform multi crypt pipe stages
            byte[] intermediatBytes = DecrpytRoundGoMerry(cipherBytes, secretKey, cipherHash);
            // Unzip after all, if it's necessary
            byte[] decryptedBytes = (unzipAfter != ZipType.None) ? unzipAfter.Unzip(intermediatBytes) : intermediatBytes;

            return decryptedBytes;
        }

        /// <summary>
        /// Multi functional 
        /// <see cref="EncryptEncodeBytes(byte[], string, string, EncodingType, ZipType, KeyHash)"/>
        /// <see cref="DecodeDecrpytBytes(byte[], string, string, EncodingType, ZipType, KeyHash)"/>
        /// </summary>
        /// <param name="inBytes">incoming bytes</param>
        /// <param name="secretKey">user private key</param>
        /// <param name="hashIV">hashed secret key</param>
        /// <param name="directionDecrypt">true for decryption, false for encryption</param>
        /// <param name="encType">encoding ascii type, e.g. base64, uu, xx</param>
        /// <param name="zip">compression method to zip before or unzip after pipe processed</param>
        /// <param name="keyHash">hashing type of hashing method to hash key</param>
        /// <returns>transformed byte array</returns>
        public override byte[] CryptCodeBytes(byte[] inBytes, string secretKey, string hashIV,
            bool directionDecrypt = false, EncodingType encType = EncodingType.Base64,
            ZipType zip = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            return (!directionDecrypt) ?
                EncryptEncodeBytes(inBytes, secretKey, hashIV, encType, zip, keyHash) :
                DecodeDecrpytBytes(inBytes, secretKey, hashIV, encType, zip, keyHash);
        }

        #region static en-de-crypt members

        /// <summary>
        /// EncrpytToStringd
        /// </summary>
        /// <param name="inString">string to encrypt multiple times</param>
        /// <param name="cryptKey">Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline 
        /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe</param>
        /// <param name="pipeString">out parameter for setting hash to compare entities encryption</param>
        /// <param name="encoding"><see cref="EncodingType"/> type for encoding encrypted bytes back in plain text></param>
        /// <param name="zipBefore">Zip bytes with <see cref="ZipType"/> before passing them in encrypted stage pipeline. <see cref="ZipTypeExtensions.Zip(ZipType, byte[])"/></param>
        /// <param name="keyHash"><see cref="KeyHash"/> hashing key algorithm</param>
        /// <returns>encrypted string</returns>        
        /// <returns></returns>
        [Obsolete("use Encoding.UTF8.GetString(new SymmCipherPipe(cyptKey).EncryptEncodeBytes(Encoding.UTF8.GetBytes(inString), secretKey, hashIV, encType, zip, keyHash)) instead.", true)]
        public static new string EncrpytToString(string inString, string cryptKey, out string pipeString,
            EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // construct symmetric cipher pipeline with cryptKey and pass pipeString as out param            
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
            pipeString = symmPipe.PipeString;

            // Transform string to bytes
            byte[] inBytes = EnDeCodeHelper.GetBytesFromString(inString);
            // perform multi crypt pipe stages
            byte[] encryptedBytes = symmPipe.EncrpytGoRounds(inBytes, cryptKey, zipBefore, keyHash);
            // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
            string encrypted = encoding.GetEnCoder().Encode(encryptedBytes);

            return encrypted;
        }

        [Obsolete("use Encoding.UTF8.GetString(new SymmCipherPipe(cyptKey).EncryptEncodeBytes(Encoding.UTF8.GetBytes(inString), secretKey, hashIV, encType, zip, keyHash)) instead.", true)]
        public static new byte[] EncrpytStringToBytes(string inString, string cryptKey, out string pipeString,
			EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
			// construct symmetric cipher pipeline with cryptKey and pass pipeString as out param            
			SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
			pipeString = symmPipe.PipeString;

			// Transform string to bytes
			byte[] inBytes = EnDeCodeHelper.GetBytesFromString(inString);
			// perform multi crypt pipe stages
			byte[] encryptedBytes = symmPipe.EncrpytGoRounds(inBytes, cryptKey, zipBefore, keyHash);			

			return encryptedBytes;
		}

        [Obsolete("use Encoding.UTF8.GetString(new SymmCipherPipe(cyptKey).EncryptEncodeBytes(plainBytes, secretKey, hashIV, encType, zip, keyHash)) instead.", true)]
        public static new string EncrpytBytesToString(byte[] plainBytes, string cryptKey, out string pipeString,
            EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // construct symmetric cipher pipeline with cryptKey and pass pipeString as out param            
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
            pipeString = symmPipe.PipeString;

            // perform multi crypt pipe stages
            byte[] encryptedBytes = symmPipe.EncrpytGoRounds(plainBytes, cryptKey, zipBefore, keyHash);
            // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
            string encrypted = encoding.GetEnCoder().Encode(encryptedBytes);

            return encrypted;
        }


        /// <summary>
        /// DecrpytToString
        /// </summary>
        /// <param name="cryptedEncodedMsg">encrypted message</param>
        /// <param name="cryptKey">Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline 
        /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe</param>
        /// <param name="pipeString">out parameter for setting hash to compare entities encryption</param>
        /// <param name="decoding"><see cref="EncodingType"/> type for encoding encrypted bytes back in plain text></param>
        /// <param name="unzipAfter"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Unzip(ZipType, byte[])"/></param>
        /// <param name="keyHash"><see cref="KeyHash"/> hashing key algorithm</param>
        /// <returns>Decrypted stirng</returns>
        [Obsolete("use Encoding.UTF8.GetString(new SymmCipherPipe(cryptKey).DecodeDecrpyt(...)) instead.", true)]
        public static new string DecrpytToString(string cryptedEncodedMsg, string cryptKey, out string pipeString,
            EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
            pipeString = symmPipe.PipeString;

            // get bytes from encrypted encoded string dependent on the encoding type (uu, base64, base32,..)
            byte[] cipherBytes = decoding.GetEnCoder().Decode(cryptedEncodedMsg);
            // staged decryption of bytes
            byte[] unroundedMerryBytes = symmPipe.DecrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

            // Get string from decrypted bytes
            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); 
            // find first \0 = NULL char in string and truncate all after first \0 apperance in string
            while (decrypted[decrypted.Length - 1] == '\0')
                decrypted = decrypted.Substring(0, decrypted.Length - 1);

            return decrypted;
        }

        [Obsolete("Use byte[] new SymmCipherPipe(cryptKey).DecodeDecrpyt(...) instead.", true)]
        public static new byte[] DecrpytStringToBytes(string cryptedEncodedMsg, string cryptKey, out string pipeString,
            EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
            pipeString = symmPipe.PipeString;

            // get bytes from encrypted encoded string dependent on the encoding type (uu, base64, base32,..)
            byte[] cipherBytes = decoding.GetEnCoder().Decode(cryptedEncodedMsg);
            // staged decryption of bytes
            byte[] unroundedMerryBytes = symmPipe.DecrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

            return unroundedMerryBytes;
        }

        [Obsolete("Use byte[] Encoding.UTF8.GetString(new SymmCipherPipe(cryptKey).DecodeDecrpytBytes(...)) instead.", true)]
        public static new string DecrpytBytesToString(byte[] cipherBytes, string cryptKey, out string pipeString,
			EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
			pipeString = symmPipe.PipeString;

			// staged decryption of bytes
			byte[] unroundedMerryBytes = symmPipe.DecrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

            // Get string from decrypted bytes
            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes);
			// find first \0 = NULL char in string and truncate all after first \0 apperance in string
			while (decrypted[decrypted.Length - 1] == '\0')
				decrypted = decrypted.Substring(0, decrypted.Length - 1);
            
            return decrypted;
		}

		#endregion static en-de-crypt members

		#endregion multiple rounds en-de-cryption

	}

}
