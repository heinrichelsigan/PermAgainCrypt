using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Area23.At.WinForm.CryptFormCore.Helper;
using Area23.At.WinForm.CryptFormCore.Properties;
using System.Media;
using System.Net.NetworkInformation;

namespace Area23.At.WinForm.CryptFormCore.Gui.Forms
{
    public partial class EncryptForm : EncryptFormBase
    {

        
        public EncryptForm()
        {
            InitializeComponent();
        }


        internal void EncryptForm_Load(object sender, EventArgs e)
        {
            this.comboBoxAlgo.Items.Clear();
            foreach (string cipher in GetCipherEnums())
                this.comboBoxAlgo.Items.Add(cipher);
            
            this.textBoxKey.Text = GetEmailFromRegistry();
        }


        #region MenuCompressionEncodingZipHash

        protected internal void menuCompression_Click(object sender, EventArgs e) => SetCompression((ToolStripMenuItem)sender);

        protected internal void SetCompression(ToolStripMenuItem mi)
        {
            if (!mi.Checked)
            {
                menu7z.Checked = false;
                menuBZip2.Checked = false;
                menuGZip.Checked = false;
                menuZip.Checked = false;
                menuCompressionNone.Checked = false;

                if (mi != null && mi.Name != null &&
                    (mi.Name.StartsWith("menu") && (mi.Name.EndsWith("7z") || mi.Name.EndsWith("BZip2") || mi.Name.EndsWith("GZip") || mi.Name.EndsWith("Zip") || mi.Name.EndsWith("None"))))
                {
                    mi.Checked = true;
                }
                ZipType zipType = ZipTypeExtensions.GetZipType(mi.Name);
                notifyIcon1.ShowBalloonTip(1000, "Info", $"{zipType.ToString()} set.", ToolTipIcon.Info);
            }
        }

        protected ZipType GetZip()
        {
            if (menu7z.Checked) return ZipType.Z7;
            if (menuBZip2.Checked) return ZipType.BZip2;
            if (menuGZip.Checked) return ZipType.GZip;
            if (menuZip.Checked) return ZipType.Zip;
            // if (menuCompressionNone.Checked) return ZipType.None;
            menuCompressionNone.Checked = true;
            return ZipType.None;
        }

        protected internal void menuEncodingKind_Click(object sender, EventArgs e) => SetEncoding((ToolStripMenuItem)sender);

        protected void SetEncoding(ToolStripMenuItem mi)
        {
            menuItemNone.Checked = false;
            menuBase16.Checked = false;
            menuHex16.Checked = false;
            menuBase32.Checked = false;
            menuHex32.Checked = false;
            menuBase64.Checked = false;
            menuUu.Checked = false;
            menuXx.Checked = false;

            if (mi != null && mi.Name != null &&
                (mi.Name.StartsWith("menuBase") || mi.Name.StartsWith("menuHex") || mi.Name.StartsWith("menuUu") || mi.Name.StartsWith("menuXx")))
            {
                mi.Checked = true;
            }

        }

        protected EncodingType GetEncoding()
        {
            if (menuItemNone.Checked) return EncodingType.None;
            if (menuBase16.Checked) return EncodingType.Base16;
            if (menuHex16.Checked) return EncodingType.Hex16;
            if (menuBase32.Checked) return EncodingType.Base32;
            if (menuHex32.Checked) return EncodingType.Hex32;
            if (menuUu.Checked) return EncodingType.Uu;
            if (menuXx.Checked) return EncodingType.Xx;
            menuBase64.Checked = true;
            return EncodingType.Base64;

        }

        protected internal void menuHash_Click(object sender, EventArgs e) => SetHash((ToolStripMenuItem)sender);

        protected void SetHash(ToolStripMenuItem mi)
        {

            KeyHash[] keyHashes = KeyHash_Extensions.GetHashTypes();
            menuHashBCrypt.Checked = false;
            menuHashHex.Checked = false;
            menuHashMD5.Checked = false;
            menuHashOpenBsd.Checked = false;
            menuHashSCrypt.Checked = false;
            menuHashSha1.Checked = false;
            menuHashSha256.Checked = false;
            menuHashSha512.Checked = false;


            if (mi != null && mi.Name != null && mi.Name.StartsWith("menuHash"))
            {
                mi.Checked = true;
            }

            Hash_Click(this, new EventArgs());

        }

        protected internal KeyHash GetHash()
        {
            if (menuHashBCrypt.Checked) return KeyHash.BCrypt;
            if (menuHashHex.Checked) return KeyHash.Hex;
            if (menuHashMD5.Checked) return KeyHash.MD5;
            if (menuHashOpenBsd.Checked) return KeyHash.OpenBSDCrypt;
            if (menuHashSCrypt.Checked) return KeyHash.SCrypt;
            if (menuHashSha1.Checked) return KeyHash.Sha1;
            if (menuHashSha256.Checked) return KeyHash.Sha256;
            if (menuHashSha512.Checked) return KeyHash.Sha512;

            menuHashHex.Checked = true;
            return KeyHash.Hex;
        }

        #endregion MenuCompressionEncodingZipHash

        #region ButtonPictureBoxClickEvents

        protected internal void pictureBoxKey_Click(object sender, EventArgs e)
        {
            this.textBoxKey.Text = GetEmailFromRegistry();
        }

        protected internal void pictureBoxAddAlgo_Click(object sender, EventArgs e)
        {
            CipherEnum[] cipherAlgors = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
            if (!string.IsNullOrEmpty(comboBoxAlgo.SelectedText) && Enum.TryParse<CipherEnum>(comboBoxAlgo.SelectedText, out CipherEnum cipherEnum))
            {
                if (cipherAlgors.Length <= 8)                
                {
                    switch (cipherEnum)
                    {
                        case CipherEnum.BlowFish:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.blowfish, true);
                            break;
                        case CipherEnum.Fish2:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.TwoFish, true);
                            break;
                        case CipherEnum.Fish3:
                        case CipherEnum.ThreeFish256:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.ThreeFish, true);
                            break;
                        case CipherEnum.Serpent:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.Serpent, true);
                            break;
                        case CipherEnum.XTea:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.XTea, true);
                            break;
                        case CipherEnum.Tea:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.Tea, true);
                            break;
                        case CipherEnum.Des:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.Des, true);
                            break;
                        case CipherEnum.Des3:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.TripleDes, true);
                            break;
                        default:
                            break;
                    }                    
                    this.textBoxPipe.Text += cipherEnum.ToString() + ";";
                    resetPictureBoxFiles(sender, e);
                }
                else
                {
                    notifyIcon1.Text = "Max 8 algorithms in pipe reached!";
                    notifyIcon1.Icon = Properties.Resources.icon_warning;
                    notifyIcon1.BalloonTipTitle = "Warning";
                    notifyIcon1.ShowBalloonTip(3600, "Warning", "Max 8 algorithms in pipe reached!", ToolTipIcon.Warning);
                }
            }
        }

        protected internal void pictureBoxDelete_Click(object sender, EventArgs e)
        {
            this.textBoxPipe.Text = "";
        }

        protected internal void Clear_Click(object sender, EventArgs e)
        {
            this.textBoxHash.Text = string.Empty;
            this.textBoxKey.Text = string.Empty;
            this.textBoxPipe.Text = string.Empty;
            this.textBoxSrc.Text = string.Empty;
            this.textBoxOut.Text = string.Empty;
            this.labelOutputFile.Text = string.Empty;
            this.labelOutputFile.Visible = false;
            this.pictureBoxOutFile.Tag = null;
            this.pictureBoxOutFile.Visible = false;
            this.labelFileIn.Text = "[no file selected]";
            this.pictureBoxFileIn.Image = Area23.At.WinForm.CryptFormCore.Properties.Resources.file;
        }
        protected internal void Hash_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                this.textBoxHash.Text = GetHash().Hash(this.textBoxKey.Text);
            }
        }

        protected internal void SetPipeline_Click(object sender, EventArgs e)
        {
            this.textBoxPipe.Text = string.Empty;
            CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
            foreach (CipherEnum cipher in cPipe.InPipe)
            {
                this.textBoxPipe.Text += cipher.ToString() + ";";
            }
        }

        #endregion ButtonPictureBoxClickEvents


        #region EncryptDecrypt_Click

        /// <summary>
        /// Encrypt_Click - encrypts text or file with given key, hash, zip and encoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected internal void Encrypt_Click(object sender, EventArgs e)
        {            
            if (!string.IsNullOrEmpty(this.textBoxHash.Text) && !string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                Icon iconSandClock = new Icon(Properties.Resources.icon_sandclock, new Size(120, 120));
                CipherEnum[] pipeAlgos = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
                CipherPipe cPipe = new CipherPipe(pipeAlgos);

                if (!string.IsNullOrEmpty(this.textBoxSrc.Text))
                {
                    this.textBoxOut.Text = "";                    
                    Cursor.Current = new Cursor(iconSandClock.Handle);
                    try
                    {
                        if (menuItemNone.Checked)
                            SetEncoding(menuBase64);

                        string encrypted = cPipe.EncrpytTextGoRounds(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                        this.textBoxOut.Text = encrypted;
                        Cursor.Current = DefaultCursor;
                    }
                    catch (Exception ex)
                    {
                        Area23Log.LogOriginMsgEx("EncryptForm", "Decrypt_Click", ex);
                    }
                }
                if (!string.IsNullOrEmpty(this.labelFileIn.Text) && !labelFileIn.Text.StartsWith("["))
                {
                    foreach (string file in HashFiles)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                        {
                            if (Path.GetFileName(file) == labelFileIn.Text)
                            {
                                Cursor.Current = new Cursor(iconSandClock.Handle);
                                // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);                                
                                byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                                byte[] outBytes = cPipe.EncrpytFileBytesGoRounds(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                                string outFilePath = (file + GetZip().GetZipTypeExtension() + "." + cPipe.PipeString + "." + GetHash());
                                SaveBytesDialog(outBytes, ref outFilePath);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Image = outFilePath.GetImageThumbnailFromFile();
                                string outFileName = Path.GetFileName(outFilePath);
                                labelOutputFile.Text = outFileName;
                                labelOutputFile.Visible = true;
                                HashFiles.Add(outFilePath);

                                Cursor.Current = DefaultCursor;
                                break;
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Decrypt_Click - decrypts text or file with given key, hash, zip and encoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected internal void Decrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxHash.Text) && !string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                Icon iconSandClock = new Icon(Properties.Resources.icon_sandclock, new Size(120, 120));

                CipherEnum[] pipeAlgos = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
                CipherPipe cPipe = new CipherPipe(pipeAlgos);

                if (!string.IsNullOrEmpty(this.textBoxSrc.Text))
                {
                    this.textBoxOut.Text = "";                    
                    Cursor.Current = new Cursor(iconSandClock.Handle);

                    this.textBoxOut.Text = "";                    
                    try
                    {
                        if (menuItemNone.Checked)
                            SetEncoding(menuBase64);

                        // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                        string decrypted = cPipe.DecryptTextRoundsGo(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                        this.textBoxOut.Text = decrypted;
                        Cursor.Current = DefaultCursor;
                    } 
                    catch (Exception ex)
                    {
                        Area23Log.LogOriginMsgEx("EncryptForm", "Decrypt_Click", ex);
                    }
                }
                if (!string.IsNullOrEmpty(this.labelFileIn.Text) && !labelFileIn.Text.StartsWith("["))
                {
                    foreach (string file in HashFiles)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                        {
                            if (Path.GetFileName(file) == labelFileIn.Text)
                            {
                                Cursor.Current = new Cursor(iconSandClock.Handle);
                                // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                                byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                                byte[] outBytes = cPipe.DecryptFileBytesRoundsGo(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                                string outFileDecrypt = file.Replace(GetZip().GetZipTypeExtension() + "." + cPipe.PipeString + "." + GetHash(), "");
                                SaveBytesDialog(outBytes, ref outFileDecrypt);
                                HashFiles.Add(outFileDecrypt);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Image = outFileDecrypt.GetImageThumbnailFromFile();
                                pictureBoxOutFile.Tag = outFileDecrypt;
                                labelOutputFile.Text = Path.GetFileName(outFileDecrypt);
                                labelOutputFile.Visible = true;

                                Cursor.Current = DefaultCursor;
                                break;
                            }
                        }
                    }
                }
            }
        }      

        #endregion EncryptDecrypt_Click


        #region DragNDrop

        internal void Drag_Enter(object sender, System.Windows.Forms.DragEventArgs e)
        {
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
     
        public override void DragEnterOver(string[] files, DragNDropState dragNDropState, System.Windows.Forms.DragEventArgs e)
        {
            lock (_Lock)
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

                if (dragNDropState == DragNDropState.DragEnter)
                    e.Effect = DragDropEffects.Copy;
                if (dragNDropState != DragNDropState.DragLeave)
                    isDragMode = true;

                _dragDropEffect = e.Effect;
                if (e.Effect != System.Windows.Forms.DragDropEffects.None)
                {
                    string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";
                    textSet += dragNDropState.ToString() + ": " + _dragDropEffect;
                    SetGBoxText(this.groupBoxFiles, textSet);
                }

                if (NormalCursor == null || NoDropCursor == null)
                {
                    Icon iconFileWork = new Icon(Properties.Resources.icon_file_working, new Size(32, 32));
                    Icon iconFileWarn = new Icon(Properties.Resources.icon_file_warning, new Size(32, 32));
                    NormalCursor = new Cursor(iconFileWork.Handle);
                    NoDropCursor = new Cursor(iconFileWarn.Handle);
                }

                Cursor.Current = (isDragMode) ? NormalCursor : NoDropCursor;
            }
        }

        internal void Drag_Leave(object sender, EventArgs e)
        {
            isDragMode = false;
            Cursor.Current = DefaultCursor;
            _dragDropEffect = DragDropEffects.None;
            SetGBoxText(this.groupBoxFiles, "Files Group Box");
        }

        internal void Drag_Drop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) ||
                e.Data.GetDataPresent(typeof(string[]))))
            {
                if ((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null)
                    Drop_Files(files);
            }
            return;
        }

        internal void Drop_Files(string[] files)
        {
            if (isDragMode && files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                    {
                        lock (_Lock)
                        {
                            this.textBoxSrc.Text = string.Empty;
                            this.textBoxOut.Text = string.Empty;
                            pictureBoxFileIn.Image = file.GetImageThumbnailFromFile();                            
                            this.labelFileIn.Text = Path.GetFileName(file);
                            Task.Run(() => PlaySoundFromResource("sound_arrow"));
                            // HashFiles = new HashSet<string>();
                            _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
                            isDragMode = false;
                            SetGBoxText(this.groupBoxFiles, "Files Group Box");
                            break;
                        }
                    }
                }

            }

            Cursor.Current = DefaultCursor;
        }

        #endregion DragNDrop


        #region OpenSave

        /// <summary>
        /// menuFileOpen_Click opens a file dialog to select a file to encrypt/decrypt
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        internal void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open File";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true; ;
            dialog.RestoreDirectory = true;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName) && System.IO.File.Exists(dialog.FileName))
            {
                pictureBoxFileIn.Image = dialog.FileName.GetImageThumbnailFromFile();
                this.labelFileIn.Text = Path.GetFileName(dialog.FileName);
                HashFiles = new HashSet<string>();
                HashFiles.Add(dialog.FileName);
            }
        }


        /// <summary>
        /// SaveBytesDialog saves byte array to file with save file dialog 
        /// </summary>
        /// <param name="fileBytes">byte array to save</param>
        /// <param name="outFilePath">ref will be returned; calculated outFilePath</param>
        /// <returns>true if saved, false if not saved</returns>
        internal bool SaveBytesDialog(byte[] fileBytes, ref string outFilePath)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            outFilePath = outFilePath ?? string.Empty;
            if (fileBytes != null && fileBytes.Length > 0)
            {
                dialog.Title = "Save File";
                dialog.CheckPathExists = true;
                dialog.RestoreDirectory = true;
                dialog.SupportMultiDottedExtensions = true;
                dialog.AddExtension = true;
                dialog.FileName = Path.GetFileName(outFilePath);
                dialog.DefaultExt = Path.GetExtension(outFilePath);
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
                {
                    outFilePath = dialog.FileName;
                    try
                    {
                        File.WriteAllBytes(outFilePath, fileBytes);
                    }
                    catch (Exception ex)
                    {
                        Area23Log.LogOriginMsg(this.Name, $"Exception in SaveBytesDialog for file: \"{outFilePath}\".\n{ex}");
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        internal void menuMainSave_Click(object sender, EventArgs e)
        {
            if (this.pictureBoxOutFile.Visible || labelOutputFile.Visible)
            {
                byte[] fileBytes = new byte[0];
                string fileName = "";

                foreach (string filePath in HashFiles)
                {
                    if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                    {
                        if (Path.GetFileName(filePath) == labelOutputFile.Text)
                        {
                            fileName = filePath;
                            fileBytes = System.IO.File.ReadAllBytes(filePath);
                            break;
                        }
                    }
                }

                if (SaveBytesDialog(fileBytes, ref fileName))
                {
                    if (HashFiles.Contains(fileName))
                        HashFiles.Remove(fileName);
                    this.pictureBoxOutFile.Visible = false;
                    this.labelOutputFile.Visible = false;
                }


            }
        }

        #endregion OpenSave
       

        #region Media Methods

        protected internal virtual void pictureOutBoxFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pictureBoxOutFile.Tag.ToString()) && pictureBoxOutFile.Visible &&
                File.Exists(pictureBoxOutFile.Tag.ToString()))
            {
                ProcessCmd.Execute("explorer", pictureBoxOutFile.Tag.ToString());
            }
        }

        protected internal virtual void resetPictureBoxFiles(object sender, EventArgs e)
        {
            System.Timers.Timer resetPictureBoxFileTimer = new System.Timers.Timer { Interval = 2225 };
            resetPictureBoxFileTimer.Elapsed += (s, en) =>
            {
                Task.Run(new System.Action(() =>
                {
                    SetPictureBoxImage(this.pictureBoxFileIn, Area23.At.WinForm.CryptFormCore.Properties.Resources.file, true);
                    SetPictureBoxImage(this.pictureBoxOutFile, Area23.At.WinForm.CryptFormCore.Properties.Resources.file, false);
                }));
                resetPictureBoxFileTimer.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            resetPictureBoxFileTimer.Start();
        }

        #endregion Media Methods

    }
}
