using System.ComponentModel;
using System.Text;

namespace EU.CqrXs.Crypt.EnDeCoding
{
    /// <summary>
    /// EncodingType Enum 
    /// TODO: base58
    /// </summary>
    [DefaultValue(EncodingType.Base64)]
    public enum EncodingType
    {
        None =      0x000,
        Base16 =    0x100,
        Hex16 =     0x101,
        Base32 =    0x200,
        Hex32 =     0x201,
        Uu =        0x400,
        Base64 =    0x401,
        Hex64 =     0x402,        
        Xx =        0x404,
        Ascii85 =   0x550
    }

    public static class EncodingTypesExtensions
    {
        public static EncodingType[] GetEncodingTypes()
        {
            List<EncodingType> list = new List<EncodingType>();
            foreach (string encName in Enum.GetNames(typeof(EncodingType)))
            {
                list.Add((EncodingType)Enum.Parse(typeof(EncodingType), encName));
            }

            return list.ToArray();
        }

        public static EncodingType GetEncodingTypeFromValue(short eValue)
        {
            eValue = (short)((eValue % 0x1000) - (eValue % 0x100));
            foreach (EncodingType eType in GetEncodingTypes())
            {
                if ((short)eType == eValue)
                    return eType;
            }
            return EncodingType.None;
        }

        public static EncodingType GetEncodingTypeFromString(string svalue)
        {
            EncodingType encodingType = EncodingType.None;
            if (!Enum.TryParse<EncodingType>(svalue, out encodingType))
                return EncodingType.Base64;
            return encodingType;
        }

        public static IDecodable GetEnCoder(this EncodingType type)
        {
            switch (type)
            {
                case EncodingType.None: return ((IDecodable)new RawString());
                case EncodingType.Hex16: return ((IDecodable)new Hex16());
                case EncodingType.Base16: return ((IDecodable)new Base16());
                case EncodingType.Hex32: return ((IDecodable)new Hex32());
                case EncodingType.Base32: return ((IDecodable)new Base32());
                case EncodingType.Uu: return ((IDecodable)new Uu());
                case EncodingType.Xx: return ((IDecodable)new Xx());
                case EncodingType.Hex64: return ((IDecodable)new Hex64());
                case EncodingType.Ascii85: return ((IDecodable)new Ascii85());
                case EncodingType.Base64:
                default: return ((IDecodable)new Base64());
            }
        }


        public static string GetEnCodingExtension(this EncodingType type)
        {
            switch (type)
            {
                case EncodingType.None: return "";
                case EncodingType.Hex16: return ".hex16";
                case EncodingType.Base16: return ".base16";
                case EncodingType.Hex32: return ".hex32";
                case EncodingType.Base32: return ".base32";
                case EncodingType.Uu: return ".uu";
                case EncodingType.Xx: return ".xx";
                case EncodingType.Hex64: return ".hex64";
                case EncodingType.Ascii85: return ".ascii85";
                case EncodingType.Base64:
                default: return ".base64";
            }
        }

        public static string EnCode(this EncodingType encodeType, byte[] inBytes)
        {
            IDecodable enc = encodeType.GetEnCoder();
            return enc.Encode(inBytes);
        }

        public static byte[] DeCode(this EncodingType encodeType, string encodedString)
        {
            IDecodable dec = encodeType.GetEnCoder();
            return dec.Decode(encodedString);
        }


        public static EncodingType GetEnum(string enCodingString)
        {
            string encodeExt = enCodingString.ToLower().Replace(".", "").Trim();
            switch (encodeExt)
            {
                case "raw":
                case "none":
                case "null":
                case "0":
                    return EncodingType.None;

                case "hex16":
                case "hex":
                case "h16":
                case "16":
                    return EncodingType.Hex16;

                case "base16":
                case "b16":
                    return EncodingType.Base16;

                case "base32":
                case "b32":
                    return EncodingType.Base32;

                case "hex32":
                case "h32":
                case "32":
                    return EncodingType.Hex32;

                case "uu":
                case "uue":
                case "uud":
                case "uuencode":
                case "uudecode":
                    return EncodingType.Uu;

                case "xx":
                case "xxe":
                case "xxd":
                case "xxencode":
                case "xxdecode":
                    return EncodingType.Xx;

                case "hex64":
                case "h64":
                case "64":
                    return EncodingType.Hex64;

                case "ascii85":
                case "a85":
                case "85":
                    return EncodingType.Ascii85;

                case "base64":
                case "b64":
                case "mime":
                default:
                    return EncodingType.Base64;
            }

        }


        public static ToolStripMenuItem CheckMenuItemForEncoding(this EncodingType encodingType, ToolStripMenuItem[] items) 
        {
            ToolStripMenuItem checkedItem = null;
            foreach (ToolStripMenuItem item in items)
            {
                item.Tag = encodingType;
                if (item.Name.Contains(encodingType.ToString()))
                {
                    item.Checked = true;
                    checkedItem = item;                    
                }
                else
                    item.Checked = false;
            }
            return checkedItem;
        }


        public static EncodingType GetEncodíngTypeFromCheckMenuItem(ToolStripMenuItem[] items)
        {
            EncodingType encodingType = EncodingType.None;
            foreach (ToolStripMenuItem item in items)
            {
                if (item.Checked)
                {
                   string enncodingName =  string.IsNullOrEmpty(item.Tag.ToString() ?? "") ? 
                        item.Name.Replace("menuEnc", "") : item.Tag.ToString();
                    encodingType = Enum.Parse<EncodingType>(enncodingName);
                    return encodingType;
                }                
            }
            return EncodingType.None; ;
        }
    }

}
