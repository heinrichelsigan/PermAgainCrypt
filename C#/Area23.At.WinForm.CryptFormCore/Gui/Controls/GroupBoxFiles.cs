using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Util;
using Area23.At.WinForm.CryptFormCore.Helper;
using Area23.At.WinForm.CryptFormCore.Properties;
using System.ComponentModel;

namespace Area23.At.WinForm.CryptFormCore.Gui.Controls
{
    public partial class GroupBoxFiles : GroupBox
    {

        internal Cursor NormalCursor, NoDropCursor;
        internal Icon iconFileWork;
        internal System.Windows.Forms.DragDropEffects _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
        internal bool isDragMode = false;
        private readonly Lock _Lock = new Lock();
        internal CipherPipe? CPipe;
        internal static HashSet<string> HashFiles = new HashSet<string>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventHandler<Area23EventArgs<string>>? FileAdded { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventHandler<EventArgs>? FileRequired { get; set; }

        public GroupBoxFiles()
        {
            InitializeComponent();
            NormalCursor = DefaultCursor;
            iconFileWork = new Icon(Properties.Resources.icon_file_working, new Size(32, 32));
            NoDropCursor = new Cursor(iconFileWork.Handle);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        #region thread safe delegates

        internal delegate void SetGroupBoxTextCallback(string headerText);
        internal delegate void SetLabelVisibleTextCallback(System.Windows.Forms.Label label, bool visible, string text);
        internal delegate void SetPictureBoxCallback(System.Windows.Forms.PictureBox pictBox, Image image, string tagTxt, bool show);


        /// <summary>
        /// SetGBoxText delegate to set a text to <see cref="GroupBox"/> across threads
        /// </summary>
        /// <param name="text">text header for GroupBox</param>
        internal virtual void SetGBoxText(string text)
        {
            string textToSet = (!string.IsNullOrEmpty(text)) ? text : string.Empty;
            if (this != null)
            {
                if (this.InvokeRequired)
                {
                    SetGroupBoxTextCallback setGroupBoxText = delegate (string hText)
                    {
                        if (this != null && hText != null)
                            this.Text = hText;
                    };
                    try
                    {
                        Invoke(setGroupBoxText, new object[] { textToSet });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetGBoxText text: \"{textToSet}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (this != null && textToSet != null)
                        this.Text = textToSet;
                }
            }
        }

        /// <summary>
        /// SetGBoxTextAsync delegate to set a <see cref="string">string text</see>/ to <see cref="GroupBox">this</see> across threads
        /// </summary>
        /// <param name="text">text header for GroupBox</param>
        /// <returns>void Task for async method</returns>
        internal virtual async Task SetGBoxTextAsync(string text)
        {
            string textToSet = (!string.IsNullOrEmpty(text)) ? text : string.Empty;
            if (this != null)
            {
                if (this.InvokeRequired)
                {
                    try
                    {
                        await InvokeAsync(() =>
                        {
                            if (this != null && textToSet != null)
                                this.Text = textToSet;
                        });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetGBoxText text: \"{textToSet}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (this != null && textToSet != null)
                        this.Text = textToSet;
                }
            }
        }


        /// <summary>
        /// SetLabelText delegate to set a text to <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="visible"><see cref="bool" /></param>
        /// <param name="text"><see cref="string" /></param>
        internal virtual void SetLabelVisibleText(Label label, bool visible, string text)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    SetLabelVisibleTextCallback setLabelVisibleText = delegate (Label lbl, bool vsble, string labelText) 
                    {
                        if (lbl != null && (!vsble || labelText != null))
                        {
                            lbl.Text = labelText ?? "";
                            lbl.Visible = vsble;
                        }
                    };
                    try
                    {
                        Invoke(setLabelVisibleText, new object[] { label, visible, text });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetLabelTextVisible visible={visible}; Text: \"{text}\".\n", exDelegate);
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
        /// SetLabelVisibleTextAsync delegate to set a text to <see cref="Label"/> across threads
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="visible"><see cref="bool"/></param>
        /// <param name="text"><see cref="string" /></param>
        /// <returns>void Task for async method</returns>
        internal virtual async Task SetLabelVisibleTextAsync(Label label, bool visible, string text)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    try
                    {
                        await InvokeAsync(() =>
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
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetLabelTextVisibleAsync visible={visible}; Text: \"{text}\".\n", exDelegate);
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
        /// SetPictureBoxImage delegate to set an <see cref="Image"/> in <see cref="PictureBox"/> across threads
        /// </summary>
        /// <param name="pictBox">the PictureBox</param>
        /// <param name="image">the Image</param>
        /// <param name="visible">true, if visible, false if invisible</param>
        internal virtual void SetPictureBoxImage(PictureBox pictBox, Image image, string tagText = "", bool visible = true)
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
        internal virtual async Task SetPictureBoxImageAsync(PictureBox pictBox, Image image, string tagText = "", bool visible = true)
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

        #endregion thread safe delegates


        protected internal void PictureBoxFileInOut_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pb && pb != null && pb.Visible)
            {
                
                string filePath = (pb.Tag != null && !string.IsNullOrEmpty(pb.Tag?.ToString())) ? pb.Tag?.ToString() : "";
                if (string.IsNullOrEmpty(filePath) || pb.Name.Equals("pictureBoxFileIn", StringComparison.OrdinalIgnoreCase))
                {
                    EventHandler<EventArgs> handler = FileRequired;
                    handler?.Invoke(this, e);
                    return; 
                }
                if ((filePath.StartsWith("{") && filePath.EndsWith("}")) ||
                    (filePath.StartsWith("[") && !filePath.EndsWith("]")))
                {
                    filePath = filePath.Replace("{", "").Replace("[", "").Replace("}", "").Replace("]", "");
                }
                        
                if (File.Exists(filePath))
                {
                    string fPath = filePath.ToLower();
                    if (fPath.EndsWith(".base16", StringComparison.OrdinalIgnoreCase) ||
                        fPath.EndsWith(".hex16", StringComparison.CurrentCultureIgnoreCase) ||
                        fPath.EndsWith(".base32", StringComparison.OrdinalIgnoreCase) ||
                        fPath.EndsWith(".hex32", StringComparison.CurrentCultureIgnoreCase) ||
                        fPath.Contains(".uu", StringComparison.CurrentCultureIgnoreCase) ||
                        fPath.Contains(".xx", StringComparison.CurrentCultureIgnoreCase) ||
                        fPath.Contains(".base64", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ProcessCmd.Execute("notepad", filePath);
                        return;
                    }
                    
                    ProcessCmd.Execute("explorer", filePath);
                    return;
                }
                else
                {
                    pb.Image = Resources.image_file;
                }
            }
        }

        internal void ResetPictureBoxFiles(object sender, EventArgs e)
        {
            System.Timers.Timer resetPictureBoxFileTimer = new System.Timers.Timer { Interval = 2225 };
            resetPictureBoxFileTimer.Elapsed += (s, en) =>
            {
                Task.Run(new System.Action(() =>
                {
                    SetPictureBoxImage(this.pictureBoxFileIn, Area23.At.WinForm.CryptFormCore.Properties.Resources.file, "", true);
                    SetPictureBoxImage(this.pictureBoxFileOut, Area23.At.WinForm.CryptFormCore.Properties.Resources.file, "", false);
                }));
                resetPictureBoxFileTimer.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            resetPictureBoxFileTimer.Start();
        }



        #region DragNDrop

        /// <summary>
        /// Drag_Enter - drag enter event for file drop
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DragEventArgs e</param>
        internal virtual void Drag_Enter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            string[] files = new string[1];

            if (e != null && e.Data != null)
            {
                if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[])))
                {
                    if (((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null) && files.Length > 0)
                    {
                        DragEnterOver(files, DragNDropState.DragEnter, e);
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
        }


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


        /// <summary>
        /// DragEnterOver - handles drag enter and drag over events
        /// </summary>
        /// <param name="files"></param>
        /// <param name="dragNDropState"></param>
        /// <param name="e">DragEventArgs e</param>
        public virtual void DragEnterOver(string[] files, DragNDropState dragNDropState, System.Windows.Forms.DragEventArgs e)
        {
            lock (_Lock)
            {
                if (dragNDropState == DragNDropState.DragEnter)
                    e.Effect = DragDropEffects.Copy;
                if (dragNDropState != DragNDropState.DragLeave)
                    isDragMode = true;

                _dragDropEffect = e.Effect;
                if (e.Effect != System.Windows.Forms.DragDropEffects.None)
                {
                    string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";
                    textSet += dragNDropState.ToString() + ": " + _dragDropEffect;
                    SetGBoxText(textSet);
                }

                if (NormalCursor == null || NoDropCursor == null)
                {
                    iconFileWork = new Icon(Properties.Resources.icon_file_working, new Size(32, 32));

                    NormalCursor = DefaultCursor; // new Cursor(iconFileWarn.Handle);                    
                    NoDropCursor = new Cursor(iconFileWork.Handle);
                }

                Cursor.Current = (isDragMode) ? NormalCursor : NoDropCursor;
            }
        }

        /// <summary>
        /// Drag_Leave - drag leave event for file drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal virtual void Drag_Leave(object sender, EventArgs e)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            isDragMode = false;
            Cursor.Current = DefaultCursor;
            _dragDropEffect = DragDropEffects.None;
            SetGBoxText("Files Group Box");
        }

        /// <summary>
        /// Drag_Drop - drag drop event for file drop
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DragEventArgs e</param>
        internal virtual void Drag_Drop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) ||
                e.Data.GetDataPresent(typeof(string[]))))
            {
                if ((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null)
                {
                    if (HashFiles == null || HashFiles.Count == 0)
                        HashFiles = new HashSet<string>(files);
                    else
                        foreach (string file in files)
                        {
                            if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                                if (!HashFiles.Contains(file))
                                    HashFiles.Add(file);
                        }

                    Drop_Files(files);
                }

            }
            return;
        }

        /// <summary>
        /// Drop_Files - handles dropped files
        /// </summary>
        /// <param name="files"></param>
        internal virtual void Drop_Files(string[] files)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            string ext = null;
            if (isDragMode && files != null && files.Length > 0 && FileAdded != null)
            {
                foreach (string file in files)
                {
                    if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                    {
                        EventHandler<Area23EventArgs<string>> handler = FileAdded;
                        Area23EventArgs<string> area23EventArgs = new Area23EventArgs<string>(file);
                        handler?.Invoke(this, area23EventArgs);

                        ext = Path.GetExtension(file).Replace(".", "");
                        _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
                        isDragMode = false;
                        break;
                    }
                }

            }

            Cursor.Current = DefaultCursor;
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

        #endregion DragNDrop

    }
}
