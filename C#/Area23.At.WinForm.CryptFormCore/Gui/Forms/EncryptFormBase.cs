using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Crypt.Msg;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Area23.At.WinForm.CryptFormCore.Helper;
using Area23.At.WinForm.CryptFormCore.Properties;

namespace Area23.At.WinForm.CryptFormCore.Gui.Forms
{
    public class EncryptFormBase : System.Windows.Forms.Form
    {
        protected internal Cursor NormalCursor, NoDropCursor;
        protected internal System.Windows.Forms.DragDropEffects _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
        protected internal bool isDragMode = false;
        protected internal readonly Lock _Lock = new Lock();
        protected internal CipherPipe? CPipe;

        protected internal static HashSet<string> HashFiles = new HashSet<string>();
        protected internal delegate void SetLabelVisibleCallback(System.Windows.Forms.Label label, bool visible);
        protected internal delegate void SetLabelTextCallback(System.Windows.Forms.Label label, string text);
        protected internal delegate void SetLabelBackColorCallback(System.Windows.Forms.Label label, Color backColor);
        protected internal delegate void SetGroupBoxTextCallback(System.Windows.Forms.GroupBox groupBox, string headerText);
        protected internal delegate void SetPictureBoxCallback(System.Windows.Forms.PictureBox pictBox, Image image, string tagTxt, bool show);
        protected internal delegate void SetStatusLabelTextCallback(System.Windows.Forms.ToolStripStatusLabel tsLabel, string text);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #region delegates and async delegates to set GUI controls from any thread not only GUI thread

        /// <summary>
        /// SetLabelVisible delegate to set a text to <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="visible">bool visible</param>
        protected internal virtual void SetLabelVisible(Label label, bool visible)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    SetLabelVisibleCallback setLabelVisible = delegate (Label lbl, bool isVisible)
                    {
                        if (lbl != null)
                            lbl.Visible = isVisible;
                    };
                    try
                    {
                        Invoke(setLabelVisible, new object[] { label, visible });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetLabelVisible visible: \"{visible}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (this != null && label != null)
                        label.Visible = visible;
                }
            }
        }

        protected internal virtual async Task SetLabelVisibleAsync(Label label, bool visible)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {                    
                    try
                    {
                        await InvokeAsync(() =>
                        {
                            if (this != null && label != null)
                                label.Visible = visible;
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetLabelVisible visible: \"{visible}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (this != null && label != null)
                        label.Visible = visible;
                }
            }
        }

        /// <summary>
        /// SetLabelText delegate to set a text to <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="text"><see cref="string">text</see>/param>
        protected internal virtual void SetLabelText(Label label, string text)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    SetLabelTextCallback setLabelText = delegate (Label lbl, string labelText)
                    {
                        if (lbl != null && labelText != null)
                        {
                            lbl.Text = labelText;
                            lbl.Visible = true;
                        }
                    };
                    try
                    {
                        Invoke(setLabelText, new object[] { label, text });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetLabelText Text: \"{text}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (text != null && label != null)
                    {
                        label.Text = text;
                        label.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// SetLabelTextAsync delegate to set a text to <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="text"><see cref="string">text</see>/param>
        /// <returns>void Task for async method</returns>
        protected internal virtual async Task SetLabelTextAsync(Label label, string text)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    try
                    {
                        await InvokeAsync(() =>
                        {
                            if (label != null && text != null)
                            {
                                label.Text = text;
                                label.Visible = true;
                            }
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetLabelText Text: \"{text}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (text != null && label != null)
                    {
                        label.Text = text;
                        label.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// SetLabelBackColor delegate to set <see cref="Color">Backcolor</see> for <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="backColor"><see cref="Color">backColor</see>/param>
        protected internal virtual void SetLabelBackColor(Label label, Color backColor)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    SetLabelBackColorCallback setLabelBackColor = delegate (Label lbl, Color bgColor)
                    {
                        if (lbl != null)
                            lbl.BackColor = bgColor;
                    };
                    try
                    {
                        Invoke(setLabelBackColor, new object[] { label, backColor });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetLabelBackColor Color: \"{backColor.ToString()}\".\n", exDelegate);
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
        /// SetLabelBackColorAsync delegate to set <see cref="Color">Backcolor</see> for <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="backColor"><see cref="Color">backColor</see>/param>
        /// <returns>void Task for async method</returns>
        protected internal virtual async Task SetLabelBackColorAsync(Label label, Color backColor)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    try
                    {
                        await InvokeAsync(() =>
                        {
                            if (label != null)
                                label.BackColor = backColor;
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetLabelBackColor Color: \"{backColor.ToString()}\".\n", exDelegate);
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
        /// SetGBoxText delegate to set a text to <see cref="GroupBox"/> across threads
        /// </summary>
        /// <param name="text">text header for GroupBox</param>
        protected internal virtual void SetGBoxText(GroupBox groupBox, string text)
        {
            string textToSet = (!string.IsNullOrEmpty(text)) ? text : string.Empty;
            if (groupBox != null)
            {
                if (groupBox.InvokeRequired)
                {
                    SetGroupBoxTextCallback setGroupBoxText = delegate (GroupBox gBox, string hText)
                    {
                        if (gBox != null && hText != null)
                            gBox.Text = hText;
                    };
                    try
                    {
                        Invoke(setGroupBoxText, new object[] { groupBox, textToSet });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetGBoxText text: \"{textToSet}\".\n", exDelegate);
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
        /// SetGBoxTextAsync delegate to set a text to <see cref="GroupBox"/> across threads
        /// </summary>
        /// <param name="text">text header for GroupBox</param>
        /// <returns>void Task for async method</returns>
        protected internal virtual async Task SetGBoxTextAsync(GroupBox groupBox, string text)
        {
            string textToSet = (!string.IsNullOrEmpty(text)) ? text : string.Empty;
            if (groupBox != null)
            {
                if (groupBox.InvokeRequired)
                {                    
                    try
                    {
                        await InvokeAsync(() =>
                        {
                            if (groupBox != null && textToSet != null)
                                groupBox.Text = textToSet;
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetGBoxText text: \"{textToSet}\".\n", exDelegate);
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
        /// SetPictureBoxImage delegate to set an <see cref="Image"/> in <see cref="PictureBox"/> across threads
        /// </summary>
        /// <param name="pictBox">the PictureBox</param>
        /// <param name="image">the Image</param>
        /// <param name="visible">true, if visible, false if invisible</param>
        protected internal virtual void SetPictureBoxImage(PictureBox pictBox, Image image, string tagText = "", bool visible = true)
        {
            if (pictBox != null && image != null)
            {
                if (pictBox.InvokeRequired)
                {
                    SetPictureBoxCallback setPictureBoxDelegate = delegate (PictureBox pBox, Image img, string tagTxt, bool showing)
                    {
                        if (pBox != null && img != null && tagTxt != null)
                        {
                            pBox.Image = img;
                            pBox.Tag = tagTxt;
                            pBox.Visible = showing;
                        }
                    };
                    try
                    {
                        Invoke(setPictureBoxDelegate, new object[] { pictBox, image, tagText, visible });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetPictureBoxImage image: \"{image}\", tag: \"{tagText}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (pictBox != null && tagText != null && image != null)
                    {
                        pictBox.Image = image;
                        pictBox.Tag = tagText;
                        pictBox.Visible = visible;
                    }
                }
            }
        }

        /// <summary>
        /// SetPictureBoxImageAsync delegate to set an <see cref="Image"/> in <see cref="PictureBox"/> across threads
        /// </summary>
        /// <param name="pictBox">the PictureBox</param>
        /// <param name="image">the Image</param>
        /// <param name="visible">true, if visible, false if invisible</param>
        /// <returns>void Task for async method</returns>
        protected internal virtual async Task SetPictureBoxImageAsync(PictureBox pictBox, Image image, string tagText = "", bool visible = true)
        {
            if (pictBox != null && image != null)
            {
                if (pictBox.InvokeRequired)
                {
                    try
                    {
                        await InvokeAsync(() =>
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
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetPictureBoxImage image: \"{image}\", tag: \"{tagText}\".\n", exDelegate);
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
        
        protected internal virtual void SetPictureBox(PictureBox pictBox, Bitmap bmp, string tagText, bool visible = true) => SetPictureBoxImage(pictBox, (Image)bmp, tagText, visible);

        protected internal virtual async Task SetPictureBoxAsync(PictureBox pictBox, Bitmap bmp, string tagText, bool visible = true) => await SetPictureBoxImageAsync(pictBox, (Image)bmp, tagText, visible);


        protected internal virtual void SetStatusLabelText(System.Windows.Forms.ToolStripStatusLabel tsLabel, string text)
        {            
            if (tsLabel != null)
            {
                ToolStrip? tsParent = tsLabel.GetCurrentParent();
                if (tsParent != null && tsParent.InvokeRequired)
                {
                    SetStatusLabelTextCallback setStatusLabelTextCallback = delegate (ToolStripStatusLabel tlbl, string msg)
                    {
                        if (tlbl != null)
                            tlbl.Text = msg;
                    };
                    try
                    {
                        Invoke(setStatusLabelTextCallback, new object[] { tsLabel, text });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetStatusLabelTextCallback Text: \"{text}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (this != null && tsLabel != null)
                        tsLabel.Text = text;
                }
            }
        }

        protected internal virtual async Task SetStatusLabelTextAsync(System.Windows.Forms.ToolStripStatusLabel tsLabel, string text)
        {
            if (tsLabel != null)
            {
                ToolStrip? tsParent = tsLabel.GetCurrentParent();
                if (tsParent != null && tsParent.InvokeRequired)
                {
                    try
                    {
                        await InvokeAsync(() =>
                        {
                            if (tsLabel != null && text != null)
                                tsLabel.Text = text;
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetStatusLabelTextCallback Text: \"{text}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (tsLabel != null && text != null)
                        tsLabel.Text = text;
                }
            }
        }

        #endregion delegates and async delegates to set GUI controls from any thread not only GUI thread


        #region getter methods
        

        /// <summary>
        /// GetCipherEnums gets all cipher algos for the cipher pipeline
        /// </summary>
        /// <param name="sorted">sort list with all cipher algos, default true</param>
        /// <returns><see cref="T:string[]"/></returns>
        protected internal virtual string[] GetCipherEnums(bool sorted = true)
        {
            List<string> cipherEnums = new List<string>();
            foreach (object item in Enum.GetValues(typeof(Area23.At.Framework.Core.Crypt.Cipher.CipherEnum)))
                cipherEnums.Add(item.ToString());
            if (sorted)
                cipherEnums.Sort();
            return cipherEnums.ToArray();
        }

        /// <summary>
        /// GetEmailFromRegistry reads user email address from registry database
        /// </summary>
        /// <returns>user email adddress or anonymous ftp.cdrom.com</returns>
        public string GetEmailFromRegistry() => RegistryAccessor.GetEmailFromRegistry();

        #endregion getter methods

        #region DragNDrop

        internal virtual void Drag_Over(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[]))))
            {
                if (((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null) && files.Length > 0)
                {
                    DragEnterOver(files, DragNDropState.DragOver, e);
                }
            }
        }

        internal virtual void Give_FeedBack(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
            if (e != null)
            {
                // Sets the custom cursor based upon the effect.
                e.UseDefaultCursors = false;
                _dragDropEffect = e.Effect;
                NormalCursor = new Cursor(Properties.Resources.icon_file_warning.Handle);
                NoDropCursor = new Cursor(Properties.Resources.icon_file_working.Handle);
                Cursor.Current = (isDragMode) ? NormalCursor : NoDropCursor;
                // HOTFIX: no drop cursor
                // Cursor.Current = (!firstLeavedDropTarget) ? MyNormalCursor : MyNoDropCursor;
            }
        }

        public virtual void DragEnterOver(string[] files, DragNDropState dragNDropState, System.Windows.Forms.DragEventArgs e)
        {
        }

        #endregion DragNDrop


        #region AboutHelpExitClose

        protected internal virtual async Task menuAbout_Click(object sender, EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            await aboutDialog.ShowDialogAsync();  
        }


        protected internal virtual async Task menuHelp_Click(object sender, EventArgs e)
        {
            // System.Windows.Forms.Help.ShowHelp(this, Resources.HelpUrl);
            System.Windows.Forms.Help.ShowHelp(this, Resources.HelpUrl, HelpNavigator.TableOfContents, "cqrxs.eu");
        }

        protected internal virtual void menuFileExit_Click(object sender, EventArgs e)
        {
            try
            {
                Program.ReleaseCloseDisposeMutex();
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("BaseChatForm", "menuFileExit_Click", ex);
            }
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("BaseChatForm", "menuFileExit_Click", ex);
            }

            Application.ExitThread();
            Dispose();
            Application.Exit();
            Environment.Exit(0);
        }

        protected internal virtual void menuFileExit_Close(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
            Environment.Exit(0);
        }

        #endregion AboutHelpExitClose


        #region verify output file

        public CipherPipe? GetCPipeFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            string origName = fileName.GetFileNameAndCipherPipe(out CipherPipe? fPipe);
            if (fPipe != null && 
                (!string.IsNullOrEmpty(fPipe.PipeString) || fPipe.ZType != ZipType.None || fPipe.EncodeType != EncodingType.None || fPipe.KHash != KeyHash.Hex))
            {
                CPipe = fPipe;
                return CPipe;
            }

            return null;
        }

        public async Task<bool> VerifyEncryptedFileBytesAsync(string inFilePath, string outFilePath, string key, string hash, CipherPipe cPipe)
        {
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(outFilePath);
            // string encodingOutFile = await System.IO.File.ReadAllTextAsync(outFilePath);
            byte[] outBytes = cPipe.DecodeDecrpytBytes(fileBytes, key, hash, cPipe.EncodeType, cPipe.ZType, cPipe.KHash);
            // string outFileDecrypt = Path.GetFileName(outFilePath).Replace(cPipe.ZType.GetZipTypeExtension(), "").Replace("." + cPipe.PipeString, "").Replace(cPipe.EncodeType.GetEnCodingExtension(), "");
            byte[] inBytes = await File.ReadAllBytesAsync(inFilePath);

            bool success = await Task.Run(() => CompareBytes(inBytes, outBytes));
            return success;
        }

        public async Task<bool> VerifyEncryptedFileShaAsync(string inFilePath, string outFilePath, string key, string hash, CipherPipe cPipe)
        {
            // string encodingOutFile = await System.IO.File.ReadAllTextAsync(outFilePath);
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(outFilePath);
            byte[] outBytes = cPipe.DecodeDecrpytBytes(fileBytes, key, hash, cPipe.EncodeType, cPipe.ZType, cPipe.KHash);
            // string outFileDecrypt = Path.GetFileName(outFilePath).Replace(cPipe.ZType.GetZipTypeExtension(), "").Replace("." + cPipe.PipeString, "").Replace(cPipe.EncodeType.GetEnCodingExtension(), "");
            byte[] inBytes = await File.ReadAllBytesAsync(inFilePath);

            bool shaSuccess = CompareSha512HashSum(inBytes, outBytes);
            return shaSuccess;
        }

        public bool CompareSha512HashSum(byte[] inBytes, byte[] outBytes)
        {
            string shaHashIn = Sha512Sum.Hash(inBytes);
            string shaHashOut = Sha512Sum.Hash(outBytes);
            return (shaHashIn.Equals(shaHashOut));
        }

        public bool CompareBytes(byte[] inBytes, byte[] outBytes)
        {
            if (inBytes != null && outBytes != null && inBytes.Length > 0 && outBytes.Length > 0)
            {
                if (Math.Abs(inBytes.LongLength - outBytes.LongLength) < 16)
                {
                    int divisor = (inBytes.LongLength > 50000000) ? 40 : 8;
                    long q = (long)(inBytes.LongLength / divisor);
                    for (long l = 0; l < (q); l++)
                    {
                        if (inBytes[l] != outBytes[l])
                            return false;
                    }
                    if (inBytes.LongLength > 32)
                    {
                        for (long k = inBytes.LongLength - q; k < (inBytes.LongLength - 16); k++)
                        {
                            if (inBytes[k] != outBytes[k])
                                return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        
        #endregion verify output file

        #region Media Methods

        /// <summary>
        /// PlaySoundFromResource - plays a sound embedded in application ressource file
        /// </summary>
        /// <param name="soundName">unique qualified name for sound</param>
        protected static bool PlaySoundFromResource(string soundName)
        {
            bool played = false;
            if (true)
            {
                UnmanagedMemoryStream stream = (UnmanagedMemoryStream)Resources.ResourceManager.GetStream(soundName);


                if (stream != null)
                {
                    try
                    {
                        // Construct the sound player
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(stream);
                        player.Play();
                        played = true;
                        stream.Close();
                    }
                    catch (Exception exSound)
                    {
                        Area23Log.LogOriginMsgEx("EncryptForm", $"PlaySoundFromResource(string soundName = {soundName})", exSound);
                        played = false;
                    }
                    //fixed (byte* bufferPtr = &bytes[0])
                    //{
                    //    System.IO.UnmanagedMemoryStream ums = new UnmanagedMemoryStream(bufferPtr, bytes.Length);
                    //    SoundPlayer player = new SoundPlayer(ums);                        
                    //    player.Play();
                    //}
                }
            }

            return played;
        }

        protected virtual async Task<bool> PlaySoundFromResourcesAsync(string soundName)
        {
            return await Task.Run(() => PlaySoundFromResource(soundName));
        }

        #endregion Media Methods

    }
}
