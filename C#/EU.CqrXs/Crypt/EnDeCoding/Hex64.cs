using EU.CqrXs.Util;

namespace EU.CqrXs.Crypt.EnDeCoding
{
    /// <summary>
    /// Base64 mime standard encoding
    /// </summary>
    public class Hex64 : IDecodable
    {

        public const string VALID_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_=";
        static string invalidChars = "";

        #region common interface, interfaces for static members appear in C# 7.3 or later
        
        public IDecodable Decodable => this;

        public static HashSet<char>? ValidCharList { get; private set; } = new HashSet<char>(VALID_CHARS.ToCharArray());        

        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public string Encode(byte[] inBytes) => Hex64.ToHex64(inBytes);        

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public byte[] Decode(string encodedString) => Hex64.FromHex64(encodedString);

        public bool IsValid(string encodedStr) => Hex64.IsValidHex64(encodedStr, out _);

        public bool IsValidShowError(string encodedString, out string error) => Hex64.IsValidHex64(encodedString, out error);               

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        public static string ToHex64(byte[] inBytes)
        {
            string os = Convert.ToBase64String(
                inBytes,
                0,  
                inBytes.Length,
                Base64FormattingOptions.InsertLineBreaks
                // Base64FormattingOptions.None                
            );
            return os.Replace('+', '-').Replace('/', '_');
        }

        public static byte[] FromHex64(string inString)
        {
            bool valid = true;
            string error = "", parsedString = "";


            foreach (char ch in parsedString)
            {
                if (!ValidCharList.Contains(ch))
                {
                    error += ch;
                    valid = false;
                }
            }
            byte[] outBytes = new byte[0];

            parsedString = (string.IsNullOrEmpty(error)) ? 
                inString.Replace('-', '+').Replace('_', '/') : 
                inString.Trim(error.ToCharArray()).Replace('-', '+').Replace('_', '/'); 
            try
            {
                outBytes = Convert.FromBase64String(inString.Replace('-', '+').Replace('_', '/'));
            } 
            catch(Exception ex)
            {
                Area23Log.LogOriginMsg($"Base64.FromBase64", "need to trim error chars \"{error}\", " +
                    $"because of Exception {ex.GetType().Name} with message: {ex.Message}", 2);
                outBytes = Convert.FromBase64String(parsedString);
            }
            return outBytes;
        }

       
        public static bool IsValidHex64(string inString, out string error)
        {
            bool valid = true;
            error = "";
            foreach (char ch in inString)
            {
                if (!ValidCharList.Contains(ch))
                {
                    error += ch;
                    valid = false;
                }
            }
            return valid;
        }

    }

}