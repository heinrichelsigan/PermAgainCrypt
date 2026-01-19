using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using EU.CqrXs.Net.WebHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.CqrXs.Gui.Helper
{

    /// <summary>
    /// ExtensionMethods class provides serveral internal extension methods 
    /// in form of first argument => this type variable
    /// </summary>
    public static class ExtensionMethods
    {

        /// <summary>
        /// GetImageThumbnailFromFile gets thumbnail image from file based on file extension
        /// </summary>
        /// <param name="fileName">full file path</param>
        /// <returns>thumbnail image for pictures, otherwise icon image</returns>
        /// <exception cref="FileNotFoundException">thrown, when file doesn't exist on filepath</exception>
        public static Image GetImageThumbnailFromFile(this string filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException($"file {filepath} doesn't exist");

            string ext = Path.GetExtension(filepath).Replace(".", "");
            switch (ext)
            {
                case "doc":
                case "docm":
                case "docx":
                case "dot":
                case "dotm":
                case "dotx":
                case "rtf":
                case "odm":
                    return Properties.Resources.img_word;
                case "xl":
                case "xls":
                case "xlsx":
                case "xla":
                case "xlb":
                case "xlc":
                case "xld":
                case "xlk":
                case "xll":
                case "xlm":
                case "xlsb":
                case "xlsm":
                case "xlt":
                case "xltm":
                case "xltx":
                case "xlv":
                case "xlw":
                case "odx":
                case "csv":
                    return Properties.Resources.img_excel;
                case "ppt":
                case "pptx":
                case "odp":
                    return Properties.Resources.img_powerpoint;
                case "vsd":
                case "vsw":
                case "vsx":
                case "vtx":
                case "vsdx":
                case "vds":
                case "vdx":
                case "vsto":
                case "vss":
                case "vst":
                    return Properties.Resources.img_visio;
                case "pdf":
                    return Properties.Resources.image_pdf;
                case "gif":
                case "jpg":
                case "png":
                case "bmp":
                case "tif":
                case "exif":
                    Image image = Image.FromFile(filepath);
                    return image.GetThumbnailImage(84, 84, () => false, IntPtr.Zero);
                case "gz":
                case "tar":
                case "tar.gz":
                case "tgz":
                case "bz":
                case "bz2":
                case "tar.bz":
                case "tar.bz2":
                case "tbz":
                case "7z":
                case "7zip":
                case "zip":
                case "rar":
                case "jar":
                    return Properties.Resources.image_zip;
                case "uue":
                case "uu":
                case "base32":
                case "base64":
                case "xx":
                case "hex16":
                case "hex32":
                    return Properties.Resources.image_file_encrypted;
                default:
                    if (ext.Length > 4)
                        return Properties.Resources.image_file_encrypted;
                    break;
            }

            return Properties.Resources.image_file;
        }

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

        public static bool IsCompressedFile(this string fileExtension)
        {
            switch (fileExtension.Replace(".", "").ToLower())
            {
                case "gz":
                case "tar":
                case "tar.gz":
                case "tgz":
                case "bz":
                case "bz2":
                case "tar.bz":
                case "tar.bz2":
                case "tbz":
                case "7z":
                case "7zip":
                case "zip":
                case "rar":
                case "jar":
                case "mp4":
                case "mp3":
                case "arj":
                case "z":
                case "exe":
                case "dll":
                    return true;
            }
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

            ZipType[] zipTypes = new ZipType[] { ZipType.GZip, ZipType.BZip2, ZipType.Zip, ZipType.None };
            foreach (ZipType zType in zipTypes)
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
                    if (origFileName.Contains(zType.GetZipTypeExtension(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        zipTyp = zType;
                        int idx = origFileName.IndexOf(zipTyp.GetZipTypeExtension(), StringComparison.CurrentCultureIgnoreCase);
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

        /// <summary>
        /// Downloads an absolute referemces image from toplevel domain via 4 known search engines
        /// </summary>
        /// <param name="topLevelDomain">top level domain to search, default: eu</param>
        public static string[] DownloadImage(string topLevelDomain = ".eu")
        {
            int imgUrlIdx = -1;
            string urlImage = "";
            List<string> imgs = WebClientRequest.LatestAtImages(topLevelDomain), fileList = new List<string>();
            string fName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.Area23DateTimeWithMillis() + ".img");

            foreach (string anImg in imgs)
            {
                try
                {
                    if (anImg.Contains("<img", StringComparison.InvariantCultureIgnoreCase) &&
                        anImg.Contains("src", StringComparison.InvariantCultureIgnoreCase) &&
                        anImg.Contains("=") &&
                        (imgUrlIdx = anImg.IndexOf("src")) > -1)
                    {
                        urlImage = anImg.Substring(imgUrlIdx + 1);
                        if ((imgUrlIdx = urlImage.IndexOf("=")) > -1)
                            urlImage = urlImage.Substring(imgUrlIdx + 1);
                        urlImage = urlImage.Trim("\"'".ToCharArray());

                        if (urlImage.Contains('>'))
                        {
                            urlImage = (urlImage.Contains("/>") ?
                                        urlImage.Substring(0, urlImage.IndexOf("/") - 1) :
                                        urlImage.Substring(0, urlImage.IndexOf(">") - 1));
                        }

                        bool fileAdded = false;
                        Uri uri = new Uri(urlImage);
                        if (uri.IsWellFormedOriginalString() || uri.ToString().Contains("://"))
                        {
                            FileInfo fi = WebClientRequest.DownloadBytes(uri.ToString(), fName, System.Text.Encoding.UTF8);
                            if (fi.Exists && uri.ToString().Contains('/') && uri.ToString().Contains("."))
                            {
                                string fileRest = "", localPath = uri.LocalPath.ToString();
                                if ((imgUrlIdx = uri.ToString().LastIndexOf("/")) > -1)
                                {
                                    fileRest = uri.ToString().Substring(imgUrlIdx + 1);
                                    if (localPath.Contains('.') && localPath.Length > 3)
                                        fileRest = localPath;
                                    if (fileRest.Contains('.') && fileRest.Length > 3)
                                    {
                                        fi.CopyTo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileRest));
                                        fileList.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileRest));
                                        fileAdded = true;
                                        Thread.Sleep(40);
                                        fi.Delete();
                                    }
                                }
                            }
                            if (!fileAdded)
                                fileList.Add(fi.FullName);
                            fileAdded = false;
                        }

                    }
                }
                catch (Exception urlExc)
                {
                    Area23Log.LogOriginMsgEx(typeof(ExtensionMethods).Name, $"{urlExc.GetType()}:", urlExc);
                }
            }

            return fileList.ToArray();
        }

    }
}
