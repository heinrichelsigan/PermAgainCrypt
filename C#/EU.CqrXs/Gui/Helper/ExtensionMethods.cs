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
