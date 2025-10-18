using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.CryptFormCore.Helper
{
    /// <summary>
    /// ExtensionMethods class provides serveral internal extension methods 
    /// in form of first argument => this type variable
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// GetImageThumbnailFromFile gets thumbnail image from file based on file extension
        /// </summary>
        /// <param name="fileName">full file path</param>
        /// <returns>thumbnail image for pictures, otherwise icon image</returns>
        /// <exception cref="FileNotFoundException">thrown, when file doesn't exist on filepath</exception>
        internal static Image GetImageThumbnailFromFile(this string filepath)
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
                    if (ext.Length > 6)
                        return Properties.Resources.image_file_encrypted;
                    break;
            }

            return Properties.Resources.image_file;
        }

    }
}
