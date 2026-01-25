using EU.CqrXs.Crypt.Cipher.Symmetric;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using Newtonsoft.Json;
using System.Text;

namespace EU.CqrXs.Crypt.Msg
{

    /// <summary>
    /// <see cref="CryptMsg{TC}"/> derived from <see cref="CMsg"/> is a generic crypt container, 
    /// where instanciated generic object from <typeparamref name="TC"/> will be serialized and encrypted.
    /// For decrypting containing generic object serialized crypted string will be decrypted and deserialized and mapped into
    /// TODO: refactoring of en-/decryption
    /// </summary>
    /// <typeparam name="TC">generic type parameter</typeparam>
    [Serializable]
    public class CryptMsg<TC> : CMsg, IMsgAble where TC : class
    {
       
        public TC TContent { get; set; }

        #region ctor

        public CryptMsg() : base()
        {
            Message = "";
            // SerializedMsg = string.Empty;
            Hash = "";
            TContent = null;
            Md5Hash = "";
            Cerializer = SerType.Json;
            CBytes = null;
        }

        public CryptMsg(string msg) : this()
        {
            Message = msg ?? string.Empty;
        }


        public CryptMsg(TC tContent) : this()
        {
            TContent = tContent;         
        }


        public CryptMsg(string msg, TC tContent) : this()
        {
            Message = msg ?? string.Empty;
            TContent = tContent;
            Md5Hash = "";
            Cerializer = SerType.Json;
            CBytes = null;
        }

        public CryptMsg(string serializedString, SerType msgArt = SerType.Json) : base()
        {
            CryptMsg<TC>? cMsg = null;
            if (string.IsNullOrEmpty(serializedString))
                throw new CException("Can not deserialize null or empty serializedString.");

            if (msgArt == SerType.Json)
            {
                cMsg = Cerializer.DeCerialize<CryptMsg<TC>>(serializedString);
                cMsg.Cerializer = SerType.Json;
            }
            else if (msgArt == SerType.Xml)
            {
                cMsg = Cerializer.DeCerialize<CryptMsg<TC>>(serializedString);
                cMsg.Cerializer = SerType.Xml;
            }

            if (cMsg == null)
                throw new CException("Can not deserialize serializedString to CryptMsg<TC>.");

            TContent = cMsg.TContent;
            Hash = cMsg.Hash;
            Message = cMsg.Message;
            CBytes = cMsg.CBytes;
            Md5Hash = cMsg.Md5Hash;
            Cerializer = cMsg.Cerializer;
        }



        public CryptMsg(CryptMsg<TC> cMsg) : this()
        {
            if (cMsg != null)
            {
                CloneCopy(cMsg, this);
            }
        }


        #endregion ctor

        public new CryptMsg<TC> CCopy(CryptMsg<TC> leftDest, CryptMsg<TC> rightSrc)
        {
            return CloneCopy(rightSrc, leftDest);
        }

        #region EnDeCrypt+DeSerialize

        /// <summary>
        /// Serialize <see cref="T:CSrvMsg{TC}"/> to Json Stting
        /// </summary>
        /// <returns>json serialized string</returns>
        public override string EncryptToJson(string serverKey, EncodingType encoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            if (Encrypt(serverKey, encoder, zipType))
            {
                Cerializer = SerType.Json;
                string serializedJson = Cerialize();
                return serializedJson;
            }
            throw new CException($"EncryptToJson(string severKey failed");
        }

        public override bool Encrypt(string serverKey, EncodingType encoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            string pipeString = "", encrypted = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {                
                if (TContent != null)
                {
                    Message = JsonConvert.SerializeObject(TContent);
                }
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey);
                pipeString = symmPipe.PipeString;

                Hash = pipeString;                
                Md5Hash = MD5Sum.HashString(string.Concat(serverKey, keyHash, pipeString, Message), "");

                encrypted = Encoding.UTF8.GetString(new SymmCipherPipe(serverKey).EncryptEncodeBytes(
                    Encoding.UTF8.GetBytes(Message), serverKey, keyHash, encoder, zipType, KeyHash.Hex));
                // encrypted = SymmCipherPipe.EncrpytToString(Message, serverKey, out pipeString, encoder, zipType);

                Message = encrypted;                
            }
            catch (Exception exCrypt)
            {
                CException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }

        public new CryptMsg<TC> DecryptFromJson(string serverKey, string serialized = "",
            EncodingType decoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            if (string.IsNullOrEmpty(serialized))
                serialized = SerializedMsg;

            CryptMsg<TC> csrvmsg = DeCerialize<CryptMsg<TC>>(serialized);
            if (csrvmsg != null && Decrypt(serverKey, decoder, zipType))
            {
                csrvmsg.Message = Message;
                csrvmsg.CBytes = CBytes;
                csrvmsg.Cerializer = Cerializer;
                csrvmsg.Md5Hash = Md5Hash;
                csrvmsg.Hash = Hash;
                csrvmsg.TContent = TContent;
                return csrvmsg;
            }
            throw new CException($"DecryptFromJson<T>(string severKey, string serialized) failed");
        }

        public override bool Decrypt(string serverKey, EncodingType decoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            string  pipeString = "", decrypted = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {                
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, keyHash);
                pipeString = symmPipe.PipeString;

                decrypted = Encoding.UTF8.GetString(symmPipe.DecodeDecrpyt(
                    Message, serverKey, decoder, zipType, KeyHash.Hex));
                // decrypted = SymmCipherPipe.DecrpytToString(Message, serverKey, out pipeString, decoder, zipType);

                if (!Hash.Equals(pipeString))
                {
                    string errMsg = $"CSrvMsg.Hash={Hash} doesn't match pipeString={pipeString}";
                    Area23Log.Log(errMsg);
                    // throw new CException(errMsg);
                }
                    
                string md5Hash = MD5Sum.HashString(string.Concat(serverKey, keyHash, pipeString, decrypted), "");
                if (!md5Hash.Equals(Md5Hash))
                {
                    string md5ErrMsg = $"CSrvMsg.Md5Hash={Md5Hash} doesn't match md5Hash={md5Hash}.";
                    Area23Log.Log(md5ErrMsg);
                    // throw new CException(md5ErrMsg);
                }

                Message = decrypted;
                TContent = JsonConvert.DeserializeObject<TC>(decrypted);                
                
            }
            catch (Exception exCrypt)
            {
                CException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }


        #endregion EnDeCrypt+DeSerialize

        #region members

        public override string Cerialize() => Cerializer.Cerialize<CryptMsg<TC>>(this);

        public CryptMsg<TC>? DeCerialize(string jsonText) => DeCerialize<CryptMsg<TC>>(jsonText);

        #endregion members


        #region static members 

        #region static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        /// <summary>
        /// Serialize <see cref="CryptMsg{TC}"/> to Json Stting
        /// </summary>
        /// <returns>json serialized string</returns>
        public static string ToJsonEncrypt(string serverKey, CryptMsg<TC> cSrvMsg,
            EncodingType encoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            if (EncryptSrvMsg(serverKey, ref cSrvMsg, encoder, zipType))
            {
                string serializedJson = cSrvMsg.Cerialize();
                return serializedJson;
            }
            throw new CException($"EncryptToJson(string severKey, CryptMsg<TC> cSrvMsg) failed");
        }

        public static bool EncryptSrvMsg(string serverKey, ref CryptMsg<TC> cSrvMsg,
            EncodingType encoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            string encrypted = "", pipeString = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {
                if (cSrvMsg.TContent != null)
                {
                    cSrvMsg.Message = JsonConvert.SerializeObject(cSrvMsg.TContent);
                }
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey);
                pipeString = symmPipe.PipeString;

                cSrvMsg.Hash = pipeString;
                cSrvMsg.Md5Hash = MD5Sum.HashString(string.Concat(serverKey, keyHash, pipeString, cSrvMsg.Message), "");                

                encrypted = Encoding.UTF8.GetString(new SymmCipherPipe(serverKey).EncryptEncodeBytes(
                    Encoding.UTF8.GetBytes(cSrvMsg.Message), serverKey, keyHash, encoder, zipType, KeyHash.Hex));
                // encrypted = SymmCipherPipe.EncrpytToString(cSrvMsg.Message, serverKey, out pipeString, encoder, zipType);
                cSrvMsg.Message = encrypted;
            }
            catch (Exception exCrypt)
            {
                CException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }

        public static CryptMsg<TC> FromJsonDecrypt(string serverKey, string serialized,
             EncodingType decoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            if (string.IsNullOrEmpty(serialized))
                throw new CException("static CryptMsg<TC> FromJsonDecrypt(string serverKey, string serialized): serialized is null or empty.");

            CryptMsg<TC> cMsg = JsonConvert.DeserializeObject<CryptMsg<TC>>(serialized);
            CryptMsg<TC> ccMsg = DecryptSrvMsg(serverKey, ref cMsg, decoder, zipType);

            if (ccMsg != null)
            {            
                return ccMsg;
            }
           
            throw new CException($"DecryptFromJson<T>(string severKey, string serialized) failed");
        }

        public static CryptMsg<TC> DecryptSrvMsg(string serverKey, ref CryptMsg<TC> cSrvMsg, 
            EncodingType decoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            string pipeString = "", decrypted = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, keyHash);
                pipeString = symmPipe.PipeString;

                decrypted = Encoding.UTF8.GetString(symmPipe.DecodeDecrpyt(
                    cSrvMsg.Message, serverKey, decoder, zipType, KeyHash.Hex));
                // decrypted = SymmCipherPipe.DecrpytToString(cSrvMsg.Message, serverKey, out pipeString, decoder, zipType);

                if (!cSrvMsg.Hash.Equals(pipeString))
                {
                    string errMsg = $"cSrvMsg.Hash={cSrvMsg.Hash} doesn't match pipeString={pipeString}";
                    Area23Log.Log(errMsg);
                    // throw new CException(errMsg);
                    ;
                }
                string md5Hash = MD5Sum.HashString(string.Concat(serverKey, cSrvMsg.Hash, pipeString, decrypted), "");
                if (!md5Hash.Equals(cSrvMsg.Md5Hash))
                {
                    string md5ErrExcMsg = $"CSrvMsg-Md5Hash={cSrvMsg.Md5Hash} doesn't match md5Hash={md5Hash}";
                    Area23Log.Log(md5ErrExcMsg);
                    // throw new CException(md5ErrExcMsg);
                    ;
                }

                cSrvMsg.Message = decrypted; 
                cSrvMsg.TContent = JsonConvert.DeserializeObject<TC>(decrypted);                
            }
            catch (Exception exCrypt)
            {
                CException.SetLastException(exCrypt);
                throw;
            }

            return cSrvMsg;
        }

        #endregion static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        public new static CryptMsg<TC>? CloneCopy(CryptMsg<TC> source, CryptMsg<TC> destination)
        {
            if (source == null)
                return null;
            if (destination == null)
            {
                destination = new CryptMsg<TC>(source);
                return destination;
            }

            destination.Hash = source.Hash;
            destination.Message = source.Message;
            destination.Cerializer = source.Cerializer;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;      
            destination.TContent = source.TContent;

            return destination;
        }

        #endregion static members 

    }

}
