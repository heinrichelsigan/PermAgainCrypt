using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;

namespace Area23.At.Framework.Core.Crypt.Msg
{

    /// <summary>
    /// CryptMsg
    /// </summary>
    /// <typeparam name="TC"></typeparam>
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
            MsgType = SerType.Json;
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
            MsgType = SerType.Json;
            CBytes = null;
        }

        public CryptMsg(string serializedString, SerType msgArt = SerType.Json) : base()
        {
            CryptMsg<TC>? cMsg = null;
            if (string.IsNullOrEmpty(serializedString))
                throw new CException("Can not deserialize null or empty serializedString.");

            if (msgArt == SerType.Json)
            {
                cMsg = FromJson<CryptMsg<TC>>(serializedString);
                cMsg.MsgType = SerType.Json;
            }
            else if (msgArt == SerType.Xml)
            {
                cMsg = FromXml<CryptMsg<TC>>(serializedString);
                cMsg.MsgType = SerType.Xml;
            }

            if (cMsg == null)
                throw new CException("Can not deserialize serializedString to CryptMsg<TC>.");

            TContent = cMsg.TContent;
            Hash = cMsg.Hash;
            Message = cMsg.Message;
            CBytes = cMsg.CBytes;
            Md5Hash = cMsg.Md5Hash;
            MsgType = cMsg.MsgType;
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
        /// Serialize <see cref="CSrvMsg{TC}"/> to Json Stting
        /// </summary>
        /// <returns>json serialized string</returns>
        public override string EncryptToJson(string serverKey, EncodingType encoder = EncodingType.Base64, Zip.ZipType zipType = Zip.ZipType.None)
        {
            if (Encrypt(serverKey, encoder, zipType))
            {
                string serializedJson = ToJson();
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
                pipeString = new SymmCipherPipe(serverKey, keyHash).PipeString;
                Hash = pipeString;
                Md5Hash = MD5Sum.HashString(string.Concat(serverKey, keyHash, pipeString, Message), "");

                encrypted = SymmCipherPipe.EncrpytToString(Message, serverKey, out pipeString, encoder, zipType);

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

            CryptMsg<TC> csrvmsg = FromJson<CryptMsg<TC>>(serialized);
            if (csrvmsg != null && Decrypt(serverKey, decoder, zipType))
            {
                csrvmsg.Message = Message;
                csrvmsg.CBytes = CBytes;
                csrvmsg.MsgType = MsgType;
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
                pipeString = new SymmCipherPipe(serverKey, keyHash).PipeString;

                decrypted = SymmCipherPipe.DecrpytToString(Message, serverKey, out pipeString, decoder, zipType);

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

        public override T? FromJson<T>(string jsonText) where T : default
        {
            CryptMsg<TC> cMsg = JsonConvert.DeserializeObject<CryptMsg<TC>>(jsonText);
            try
            {
                if (this is T t && cMsg is T && cMsg != null)
                {
                    TContent = cMsg.TContent;
                    Hash = cMsg.Hash;
                    Md5Hash = cMsg.Md5Hash;
                    Message = cMsg.Message;
                    MsgType = SerType.Json;

                    return t;
                }
            }
            catch (Exception exJson)
            {
                Area23Log.LogOriginMsgEx("CryptMsg", "FromJson", exJson);
            }
            
            return base.FromJson<T>(jsonText);
        }

        public override string ToXml() => Utils.SerializeToXml(this);

        public override T FromXml<T>(string xmlText)
        {
            CryptMsg<TC> cMsg = Utils.DeserializeFromXml<CryptMsg<TC>>(xmlText);
            try
            {
                if (this is T t && cMsg is T && cMsg != null)
                {
                    TContent = cMsg.TContent;
                    Hash = cMsg.Hash;
                    Md5Hash = cMsg.Md5Hash;
                    Message = cMsg.Message;
                    MsgType = SerType.Xml;

                    return t;
                }
            }
            catch (Exception exJson)
            {
                Area23Log.LogOriginMsgEx("CryptMsg", "FromXml", exJson);
            }

            return base.FromXml<T>(xmlText);
        }

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
                string serializedJson = cSrvMsg.ToJson();
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
                pipeString = new SymmCipherPipe(serverKey, keyHash).PipeString;
                cSrvMsg.Hash = pipeString;
                cSrvMsg.Md5Hash = MD5Sum.HashString(string.Concat(serverKey, keyHash, pipeString, cSrvMsg.Message), "");

                encrypted = SymmCipherPipe.EncrpytToString(cSrvMsg.Message, serverKey, out pipeString, encoder, zipType);
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
                pipeString = new SymmCipherPipe(serverKey, keyHash).PipeString;

                decrypted = SymmCipherPipe.DecrpytToString(cSrvMsg.Message, serverKey, out pipeString, decoder, zipType);

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
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;      
            destination.TContent = source.TContent;

            return destination;
        }

        #endregion static members 

    }

}
