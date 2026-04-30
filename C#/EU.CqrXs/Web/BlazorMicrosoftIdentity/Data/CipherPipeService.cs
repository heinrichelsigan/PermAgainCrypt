using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Zip;
using System.Security.Cryptography;

namespace EU.CqrXs.Web.BlazorMicrosoftIdentity.Data
{
    public class CipherPipeService
    {
        private static readonly CipherMode2 CipherModus = CipherMode2.CFB;

        public CipherPipe GetCipherPipe(string key, string hash, 
            KeyHash keyHash, EncodingType encodeType, ZipType zipType, CipherMode2 cmode2)
        {
            CipherPipe cpipe = new CipherPipe(key, hash, encodeType, zipType, keyHash, cmode2);
            return cpipe;
        }

        public Task<CipherPipe> GetCipherPipeAsync(CipherPipeParameters cPipeParams)
        // KeyHash keyHash, EncodingType encodeType, ZipType zipType, CipherMode2 cmode)
        {
            CipherPipe cpipe = new CipherPipe(cPipeParams.Ciphers, 8,
                cPipeParams.Encoding, cPipeParams.Compression, cPipeParams.KeyHashing,
                cPipeParams.SymmetricCipherMode);
            
            return Task.FromResult(cpipe);
        }
    }
}
