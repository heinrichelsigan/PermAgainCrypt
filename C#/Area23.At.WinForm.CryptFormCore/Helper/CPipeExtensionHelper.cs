using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Zip;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area23.At.WinForm.CryptFormCore.Helper
{
    internal static class CPipeExtensionHelper
    {


        internal static string GetFileNameAndCipherPipe(this string fileName, out CipherPipe? cipherPipe)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            cipherPipe = null;
            string origFileName = fileName;
            KeyHash kHash = KeyHash.Hex;

            EncodingType eType = EncodingType.None;
            foreach (EncodingType encTyp in EncodingTypesExtensions.GetEncodingTypes())
            {
                if (fileName.EndsWith("." + encTyp.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    eType = encTyp;
                    origFileName = fileName.Replace("." + encTyp.ToString(), "").Replace("." + encTyp.ToString().ToLower(), "");
                    break;
                }
            }

            bool cipherAfterZip = false;    
            ZipType zipTyp = ZipType.None;
            foreach (ZipType zType in ZipTypeExtensions.GetZipTypes())
            {
                if (zType != ZipType.None)
                {
                    if (origFileName.EndsWith(zType.GetZipTypeExtension(), StringComparison.CurrentCultureIgnoreCase))
                        zipTyp = zType;
                    else if (origFileName.Contains(zType.GetZipTypeExtension() + ".", StringComparison.CurrentCultureIgnoreCase))
                    {
                        zipTyp = zType;
                        cipherAfterZip = true;
                    }
                }
                if (zipTyp != ZipType.None)
                {
                    origFileName = origFileName.Replace(zipTyp.GetZipTypeExtension(), "").Replace(zipTyp.GetZipTypeExtension().ToLower(), "");
                    break;
                }                    
            }


            foreach (KeyHash kh in KeyHash_Extensions.GetHashTypes())
            {
                if (origFileName.Contains("." + kh.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    kHash = kh;
                    origFileName = origFileName.Replace("." + kh.ToString(), "").Replace("." + kh.ToString().ToLower(), "");

                    break;
                }
            }

            List<CipherEnum> cipherEnums = new List<CipherEnum>();
            if (cipherAfterZip)
            {
                string pipeRestString = origFileName.Substring(origFileName.LastIndexOf("."));
                foreach (char ch in pipeRestString)
                {
                    foreach (CipherEnum cipher in CipherEnumExtensions.GetCipherTypes())
                    {
                        if (cipher.GetCipherChar() == ch)
                            cipherEnums.Add(cipher);
                    }
                }

                if (cipherEnums.Count > 0)
                {
                    CipherPipe cPipe = new CipherPipe(cipherEnums.ToArray(), 8, eType, zipTyp, kHash);
                    if (origFileName.Contains("." + cPipe.PipeString))
                    {
                        cipherPipe = cPipe;
                        origFileName.Replace("." + cPipe.PipeString, "");
                    }
                }
            }


            if (cipherPipe == null) 
                cipherPipe = new CipherPipe(cipherEnums.ToArray(), 8, eType, zipTyp, kHash);


            return origFileName;                       
        }


    }
}
