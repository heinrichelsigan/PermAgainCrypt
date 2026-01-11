using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Util;
using EU.CqrXs.Gui.Helper;
using EU.CqrXs.Gui.Properties;
using EU.CqrXs.Gui.Sound;
using System.ComponentModel;

namespace EU.CqrXs.Gui.Controls
{
    
    /// <summary>
    /// GroupBoxFiles - handles drag and drop events and show cipherpipe image
    /// </summary>
    public partial class GroupBoxFiles : GroupBox, IPlayable
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
            //this.DragDrop += new DragEventHandler(async (sender, e) => await Drag_Drop(sender, e));
            //this.DragEnter += new DragEventHandler(async (sender, e) => await Drag_Enter(sender, e));
            //this.DragOver += new DragEventHandler(async (sender, e) => await Drag_Over(sender, e));
            //this.DragLeave += new EventHandler(async (sender, e) => await Drag_Leave(sender, e));
            //this.GiveFeedback += new GiveFeedbackEventHandler(async (sender, e) => await Give_FeedBack(sender, e));
            NormalCursor = DefaultCursor;
            iconFileWork = new Icon(Properties.Resources.icon_file_working, new Size(32, 32));
            NoDropCursor = new Cursor(iconFileWork.Handle);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        /// <summary>
        /// PictureBoxFileInOut_Click opens the file with standard windows program associated
        /// in case of ascii encodings, opens file with notepad 
        /// </summary>
        /// <param name="sender"><see cref="object">sender</see></param>
        /// <param name="e"><see cref="EventArgs">e</see></param>
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
                    Task.Run(() => IPlayable.PlaySoundFromResource("sound_click"));

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

        /// <summary>
        /// ResetPictureBoxFiles resets input and output file <see cref="PictureBox"/> to default images
        /// </summary>
        /// <param name="sender"><see cref="object">sender</see></param>
        /// <param name="e"><see cref="EventArgs">e</see></param>
        internal async Task ResetPictureBoxFilesAsync(object sender, EventArgs e)
        {
            await this.PlaySoundFromResourcesAsync("sound_volatage");            
            System.Timers.Timer resetPictureBoxFileTimer = new System.Timers.Timer { Interval = 1125 };
            resetPictureBoxFileTimer.Elapsed += (s, en) =>
            {
                Task.Run(new System.Action(async () =>
                {
                    // SetLabelVisibleText(labelOutputFile, false, "");
                    // SetLabelVisibleText(labelFileIn, true, "[no file selected]");

                    await labelOutputFile.SetTextVisibleAsync("", false);
                    await labelFileIn.SetTextVisibleAsync("[no file selected]", true);
                    await pictureBoxRunningPipe.SetBitmapTagVisibleAsync(Properties.Resources.Blank_640x96, "DeCryptPipeLine", true);
                    await pictureBoxFileIn.SetBitmapTagVisibleAsync(Properties.Resources.file, "", true);
                    await pictureBoxFileOut.SetBitmapTagVisibleAsync(Properties.Resources.file, "", false);
                    // SetPictureBoxImage(this.pictureBoxRunningPipe, Properties.Resources.BlankEncrypt_640x96, "DeCryptPipeLine", true);
                    // SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.file, "", true);
                    // SetPictureBoxImage(this.pictureBoxFileOut, Properties.Resources.file, "", false);
                }));
                resetPictureBoxFileTimer.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            resetPictureBoxFileTimer.Start();
        }


        #region DragNDrop

        /// <summary>
        /// Drag_Enter - drag enter event for file drop
        /// </summary>
        /// <param name="sender"><see cref="object">sender</see></param>
        /// <param name="e"><see cref="DragEventArgs">e</see></param>
        internal virtual async Task Drag_Enter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            string[] files = new string[1];

            if (e != null && e.Data != null)
            {
                if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[])))
                {
                    if (((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null) && files.Length > 0)
                    {
                        await DragEnterOver(files, DragNDropState.DragEnter, e);
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
        }

        /// <summary>
        /// Drag_Over standard implementation on <see cref="Control.DragOver"/>
        /// </summary>
        /// <param name="sender"><see cref="object">sender</see></param>
        /// <param name="e"><see cref="DragEventArgs">e</see></param>
        internal virtual async Task Drag_Over(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[]))))
            {
                if (((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null) && files.Length > 0)
                {
                    await DragEnterOver(files, DragNDropState.DragOver, e);
                }
            }
        }

        /// <summary>
        /// DragEnterOver - handles drag enter and drag over events
        /// </summary>
        /// <param name="files"></param>
        /// <param name="dragNDropState"></param>
        /// <param name="e">DragEventArgs e</param>
        public virtual async Task DragEnterOver(string[] files, DragNDropState dragNDropState, System.Windows.Forms.DragEventArgs e)
        {
            lock (_Lock)
            {
                if (dragNDropState == DragNDropState.DragEnter)
                    e.Effect = DragDropEffects.Copy;
                if (dragNDropState != DragNDropState.DragLeave)
                    isDragMode = true;

                _dragDropEffect = e.Effect;
            }

            if (e.Effect != System.Windows.Forms.DragDropEffects.None)
            {
                string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";
                textSet += dragNDropState.ToString() + ": " + _dragDropEffect;
                await this.SetTextAsync(textSet);
            }

            lock (_Lock)
            {
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
        /// <param name="sender"><see cref="object">sender</see></param>
        /// <param name="e"><see cref="EventArgs">e</see></param>
        internal virtual async Task Drag_Leave(object sender, EventArgs e)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            isDragMode = false;
            Cursor.Current = DefaultCursor;
            _dragDropEffect = DragDropEffects.None;
            await this.SetTextAsync("Files Group Box");
        }

        /// <summary>
        /// Drag_Drop - drag drop event for file drop
        /// </summary>
        /// <param name="sender"><see cref="object">sender</see></param>
        /// <param name="e"><see cref="DragEventArgs">e</see></param>
        internal virtual async Task Drag_Drop(object sender, System.Windows.Forms.DragEventArgs e)
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

                    await Drop_Files(files);
                }

            }
            return;
        }

        /// <summary>
        /// Drop_Files - handles dropped files
        /// TODO: <see cref="Drop_Files(string[])"/> handles only first file, 
        /// but should handle all
        /// </summary>
        /// <param name="files">array of files to drop</param>
        internal virtual async Task Drop_Files(string[] files)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            string ext = null;
            if (isDragMode && files != null && files.Length > 0 && FileAdded != null)
            {
                foreach (string file in files)
                {
                    if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                    {
                        await this.PlaySoundFromResourcesAsync("sound_breakpoint");
                        // Task.Run(() => IPlayable.PlaySoundFromResource("sound_breakpoint\"));
                        EventHandler <Area23EventArgs<string>> handler = FileAdded;
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

        /// <summary>
        /// Give_FeedBack - standard implementation of <see cref="Control.GiveFeedback"/>
        /// </summary>
        /// <param name="sender"><see cref="object">sender</see></param>
        /// <param name="e"><see cref="GiveFeedbackEventArgs">e</see></param>
        internal virtual async Task Give_FeedBack(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
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
