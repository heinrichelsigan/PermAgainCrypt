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

        #region async invoke gui extensions

        /// <summary>
        /// SetBackColorAsync extension delegate to set <see cref="Color">Backcolor</see> for <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">extension method for this label</param>
        /// <param name="backColor"><see cref="Color">backColor</see></param>
        /// <returns>void Task for async method</returns>
        public static async Task SetBackColorAsync(this Label label, Color backColor)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    try
                    {
                        await label.InvokeAsync(() =>
                        {
                            if (label != null)
                                label.BackColor = backColor;
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        string labelName = (label != null && !string.IsNullOrEmpty(label.Name)) ? label.Name : "Label";
                        if (label != null && label.Parent != null && !string.IsNullOrEmpty(label.Parent.Name))
                            labelName = label.Parent.Name;
                        Area23Log.LogOriginMsgEx(labelName, $"Exception in delegate SetLabelBackColor Color: \"{backColor.ToString()}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (label != null)
                        label.BackColor = backColor;
                }
            }
        }

        /// <summary>
        /// SetTextVisibleAsync extension method delegate to set a text to <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">the label</param>       
        /// <param name="text"><see cref="string" /></param>
        /// <param name="visible"><see cref="bool"/>, default to true</param>
        /// <returns>void Task for async method</returns>
        public static async Task SetTextVisibleAsync(this Label label, string text, bool visible = true)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    try
                    {
                        await label.InvokeAsync(() =>
                        {
                            if (label != null && (!visible || text != null))
                            {
                                label.Text = text ?? "";
                                label.Visible = visible;
                            }
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        string nameLabel = (label != null && !string.IsNullOrEmpty(label.Name)) ? label.Name : "Label";
                        if (label != null && label.Parent != null && !string.IsNullOrEmpty(label.Parent.Name))
                            nameLabel = label.Parent.Name;
                        Area23Log.LogOriginMsgEx(nameLabel, $"Exception in delegate SetLabelTextVisibleAsync visible={visible}; Text: \"{text}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (label != null && (!visible || text != null))
                    {
                        label.Text = text ?? "";
                        label.Visible = visible;
                    }
                }
            }
        }


        /// <summary>
        /// SetImageTagVisibleAsync extension method to set an <see cref="Image"/> in <see cref="PictureBox"/> across threads
        /// </summary>
        /// <param name="pictBox">the PictureBox</param>
        /// <param name="image">the Image</param>
        /// <param name="tagText">image tag</param>
        /// <param name="visible">true, if visible, false if invisible</param>
        /// <returns>void Task for async method</returns>
        public static async Task SetImageTagVisibleAsync(this PictureBox pictBox, System.Drawing.Image image, string tagText = "", bool visible = true)
        {
            if (pictBox != null && image != null)
            {
                if (pictBox.InvokeRequired)
                {
                    try
                    {
                        await pictBox.InvokeAsync(() =>
                        {
                            if (pictBox != null && image != null && tagText != null)
                            {
                                pictBox.Image = image;
                                pictBox.Tag = tagText;
                                pictBox.Visible = visible;
                            }
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        string picName = (pictBox != null && !string.IsNullOrEmpty(pictBox.Name)) ? pictBox.Name : "PictureBox";
                        Area23Log.LogOriginMsgEx(picName, $"Exception in delegate SetPictureBoxImage image: \"{image}\", tag: \"{tagText}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (pictBox != null && image != null && tagText != null)
                    {
                        pictBox.Image = image;
                        pictBox.Tag = tagText;
                        pictBox.Visible = visible;
                    }
                }
            }
        }

        public static async Task SetBitmapTagVisibleAsync(this PictureBox pictBox, Bitmap bmp, string tagText, bool visible = true)
            => await SetImageTagVisibleAsync(pictBox, (System.Drawing.Image)bmp, tagText, visible);


        /// <summary>
        /// SetTextAsync extension method delegate to set a <see cref="string">string text</see>/ to <see cref="GroupBox">this</see> across threads
        /// </summary>
        /// <param name="text">text header for GroupBox</param>
        /// <returns>void Task for async method</returns>
        public static async Task SetTextAsync(this System.Windows.Forms.GroupBox groupBox, string text)
        {
            string textToSet = (!string.IsNullOrEmpty(text)) ? text : string.Empty;
            if (groupBox != null)
            {
                if (groupBox.InvokeRequired)
                {
                    try
                    {
                        await groupBox.InvokeAsync(() =>
                        {
                            if (groupBox != null && textToSet != null)
                                groupBox.Text = textToSet;
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        string gBoxName = (groupBox != null && !string.IsNullOrEmpty(groupBox.Name)) ? groupBox.Name : "GroupBox";
                        Area23Log.LogOriginMsgEx(gBoxName, $"Exception in delegate SetGBoxText text: \"{textToSet}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (groupBox != null && textToSet != null)
                        groupBox.Text = textToSet;
                }
            }
        }


        /// <summary>
        /// SetTextAsync extension method for System.Windows.Forms.ToolStripStatusLabel to set text in a thread safe manner
        /// </summary>
        /// <param name="tsLabel">ToolStripStatusLabel</param>
        /// <param name="text">text to set</param>
        /// <returns></returns>
        public static async Task SetTextAsync(this System.Windows.Forms.ToolStripStatusLabel tsLabel, string text)
        {
            if (tsLabel != null)
            {
                ToolStrip? tsParent = tsLabel.GetCurrentParent();
                if (tsParent != null && tsParent.InvokeRequired)
                {
                    try
                    {
                        await tsParent.InvokeAsync(() =>
                        {
                            if (tsLabel != null && text != null)
                                tsLabel.Text = text;
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        string tsLabelName = (tsLabel != null && !string.IsNullOrEmpty(tsLabel.Name)) ? tsLabel.Name : "ToolStripStatusLabel";
                        Area23Log.LogOriginMsgEx(tsLabelName, $"Exception in delegate SetStatusLabelTextCallback Text: \"{text}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (tsLabel != null && text != null)
                        tsLabel.Text = text;
                }
            }
        }

        #endregion async invoke gui extensions

    }
}
