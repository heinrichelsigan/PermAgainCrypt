using Area23.At.Framework.Core.Crypt;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Area23.At.WinForm.CryptFormCore.Helper;
using Area23.At.WinForm.CryptFormCore.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.CryptFormCore.Gui.Forms
{
    public class EncryptFormBase : System.Windows.Forms.Form
    {
        protected internal Cursor NormalCursor, NoDropCursor;
        protected internal System.Windows.Forms.DragDropEffects _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
        protected internal bool isDragMode = false;
        protected internal readonly Lock _Lock = new Lock();

        protected internal static HashSet<string> HashFiles = new HashSet<string>();
        protected internal delegate void SetGroupBoxTextCallback(System.Windows.Forms.GroupBox groupBox, string headerText);
        protected internal delegate void SetPictureBoxCallback(System.Windows.Forms.PictureBox pictBox, Image image, bool show);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// SetGBoxText delegate to set a text to <see cref="GroupBox"/> across threads
        /// </summary>
        /// <param name="text">text header for GroupBox</param>
        protected internal virtual void SetGBoxText(GroupBox groupBox, string text)
        {
            string textToSet = (!string.IsNullOrEmpty(text)) ? text : string.Empty;
            if (InvokeRequired)
            {
                SetGroupBoxTextCallback setGroupBoxText = delegate (GroupBox gBox, string hText)
                {
                    if (gBox != null && gBox.Name != null && !string.IsNullOrEmpty(hText))
                        gBox.Text = hText;
                };
                try
                {
                    Invoke(setGroupBoxText, new object[] { groupBox, textToSet });
                }
                catch (System.Exception exDelegate)
                {
                    Area23Log.LogOriginMsg(this.Name, $"Exception in delegate SetGBoxText text: \"{textToSet}\".\n");
                }
            }
            else
            {
                if (this != null && this.Name != null && textToSet != null)
                    groupBox.Text = textToSet;
            }
        }

        /// <summary>
        /// SetPictureBoxImage delegate to set an <see cref="Image"/> in <see cref="PictureBox"/> across threads
        /// </summary>
        /// <param name="pictBox">the PictureBox</param>
        /// <param name="image">the Image</param>
        /// <param name="visible">true, if visible, false if invisible</param>
        protected internal virtual void SetPictureBoxImage(PictureBox pictBox, Image image, bool visible = true)
        {
            if (pictBox != null && image != null)
            {
                if (InvokeRequired)
                {
                    SetPictureBoxCallback setPictureBoxDelegate = delegate (PictureBox pBox, Image img, bool showing)
                    {
                        if (pBox != null && img != null)
                        {
                            pBox.Image = img;
                            pBox.Visible = showing;
                        }

                    };
                    try
                    {
                        Invoke(setPictureBoxDelegate, new object[] { pictBox, image, visible });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsg(this.Name, $"Exception in delegate SetPictureBoxImage image: \"{image}\".\n");
                    }
                }
                else
                {
                    if (this != null && this.Name != null && image != null)
                    {
                        pictBox.Image = image;
                        pictBox.Visible = visible;
                    }
                }
            }
        }


        /// <summary>
        /// GetCipherEnums gets all cipher algos for the cipher pipeline
        /// </summary>
        /// <param name="sorted">sort list with all cipher algos, default true</param>
        /// <returns><see cref="List{string}"/></returns>
        protected internal virtual List<string> GetCipherEnums(bool sorted = true)
        {
            List<string> cipherEnums = new List<string>();
            foreach (object item in Enum.GetValues(typeof(Area23.At.Framework.Core.Crypt.Cipher.CipherEnum)))
                cipherEnums.Add(item.ToString());
            if (sorted)
                cipherEnums.Sort();
            return cipherEnums;
        }

        /// <summary>
        /// GetEmailFromRegistry reads user email address from registry database
        /// </summary>
        /// <returns>user email adddress or anonymous ftp.cdrom.com</returns>
        public string GetEmailFromRegistry()
        {
            string userEmail = "anonymous@ftp.cdrom.com";
            try
            {
                userEmail = (string)RegistryAccessor.GetRegistryEntry(Microsoft.Win32.RegistryHive.CurrentUser,
                    "Software\\Microsoft\\OneDrive\\Accounts\\Personal", "UserEmail");
                if (userEmail.Contains('@') && userEmail.Contains('.'))
                    return userEmail;
            }
            catch (Exception exUserEmail)
            {
                CException.SetLastException(exUserEmail);
            }
            try
            {
                userEmail = (string)RegistryAccessor.GetRegistryEntry(Microsoft.Win32.RegistryHive.CurrentUser,
                    "Software\\Microsoft\\VSCommon\\ConnectedUser\\IdeUserV4\\Cache", "EmailAddress");
                if (userEmail.Contains("@") && userEmail.Contains("."))
                    return userEmail;
            }
            catch (Exception exEmailAddress)
            {
                CException.SetLastException(exEmailAddress);
            }
            userEmail = "anonymous@ftp.cdrom.com";
            return userEmail;
        }


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

        protected internal virtual void menuAbout_Click(object sender, EventArgs e)
        {
            TransparentDialog transparentDialog = new TransparentDialog();
            transparentDialog.ShowDialog(this);
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
                        SoundPlayer player = new SoundPlayer(stream);
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
