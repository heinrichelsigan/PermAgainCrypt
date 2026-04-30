using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Zip;

namespace EU.CqrXs.Web.BlazorMicrosoftIdentity.Data
{
    public class CipherPipeParameters
    {
        public string Key { get; set; } = string.Empty;

        public string Hash { get; set; } = string.Empty;

        public byte[] KeyBytes { get; set; } = Array.Empty<byte>();

        public KeyHash KeyHashing { get; set; } = KeyHash.Hex;

        public EncodingType Encoding { get; set; } = EncodingType.Base64;

        public ZipType Compression { get; set; } = ZipType.None;

        public CipherMode2 SymmetricCipherMode { get; set; } = CipherMode2.CFB;

        
        private List<CipherEnum> _ciphers = new List<CipherEnum>();
        public CipherEnum[] Ciphers { get => _ciphers.ToArray(); set => _ciphers = value.ToList(); }


    }
}
