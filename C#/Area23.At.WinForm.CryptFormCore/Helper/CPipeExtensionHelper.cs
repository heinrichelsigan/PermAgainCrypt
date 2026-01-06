using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Zip;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace Area23.At.WinForm.CryptFormCore.Helper
{
    internal static class CPipeExtensionHelper
    {

        
        internal static bool IsNormalFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;

            string ext = Path.GetExtension(fileName);
            if (ext.Contains("uu", StringComparison.CurrentCultureIgnoreCase) ||
                ext.Contains("xx", StringComparison.CurrentCultureIgnoreCase) ||
                ext.Contains("base", StringComparison.CurrentCultureIgnoreCase) ||
                ext.Contains("hex", StringComparison.CurrentCultureIgnoreCase) ||
                ext.Contains("gz", StringComparison.CurrentCultureIgnoreCase) ||
                ext.Contains("bz", StringComparison.CurrentCultureIgnoreCase))
                    return false;

            if (ext.EndsWith("png", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("jpg", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("jpg", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("gif", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("tif", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("bmp", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("exif", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("ico", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("docx", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("doc", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("xlsx", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("xls", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("pptx", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("ppt", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("vsmx", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("vstx", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("txt", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("cs", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("c", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("html", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("htm", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("xhtml", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("cshtml", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("xml", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("mp3", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("mp4", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("mpeg", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("wav", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("mpg", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("wmv", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("exe", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("dll", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("pdb", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("json", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("bat", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("com", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("ps", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("sh", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("pdf", StringComparison.CurrentCultureIgnoreCase) ||
                ext.EndsWith("rtf", StringComparison.CurrentCultureIgnoreCase))
                return true;

            return false;  
        }


        internal static string GetFileNameAndCipherPipe(this string fileName, out CipherPipe? cipherPipe)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            cipherPipe = null;            
            string origFileName = fileName;

            if (IsNormalFile(fileName))
                return origFileName;

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
                        origFileName = origFileName.Replace("." + cPipe.PipeString, "");
                    }
                }
            }


            if (cipherPipe == null) 
                cipherPipe = new CipherPipe(cipherEnums.ToArray(), 8, eType, zipTyp, kHash);


            return origFileName;                       
        }


        internal static string StripCiphersInFileName(this string fileName)
        {
            // Count dots
            int dotCnt = 0, dotIdx = -1;
            string fname = fileName, origFileName = fileName;
            do
            {
                if ((dotIdx = fname.IndexOf(".")) >= 0)
                {
                    dotCnt++;
                    fname = fname.Substring(dotIdx + 1);
                }

            } while (dotIdx >= 0);

            ZipType zipTyp = ZipType.None;
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
            
            List<CipherEnum> cipherEnums = new List<CipherEnum>();
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
                    origFileName = origFileName.Replace("." + cPipe.PipeString, "");
                }
            }
            
            foreach (ZipType zType in ZipTypeExtensions.GetZipTypes())
            {
                if (zType != ZipType.None)
                {
                    if (origFileName.EndsWith(zType.GetZipTypeExtension(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        zipTyp = zType;
                        origFileName = origFileName.Replace(zipTyp.GetZipTypeExtension(), "");
                        break;
                    }
                    if (origFileName.EndsWith(zipTyp.GetZipTypeExtension().ToLower())) 
                    {
                        zipTyp = zType;
                        origFileName = origFileName.Replace(zipTyp.GetZipTypeExtension().ToLower(), "");
                        break;
                    }
                    if (origFileName.Contains("." + zType.GetZipTypeExtension(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        zipTyp = zType;
                        int idx = origFileName.IndexOf("." + zipTyp.GetZipTypeExtension(), StringComparison.CurrentCultureIgnoreCase);
                        string first = origFileName.Substring(0, idx);
                        string rest = origFileName.Substring(idx + zipTyp.GetZipTypeExtension().Length + 1);
                        origFileName = first + rest;
                        break;
                    }
                        
                }               
            }


            foreach (KeyHash kh in KeyHash_Extensions.GetHashTypes())
            {
                if (origFileName.EndsWith("." + kh.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    kHash = kh;
                    origFileName = origFileName.Replace("." + kh.ToString(), "").Replace("." + kh.ToString().ToLower(), "");

                    break;
                }
            }

            return origFileName;
        }

    }
}
