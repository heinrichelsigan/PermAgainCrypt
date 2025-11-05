using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Area23.At.WinForm.CryptFormCore.Gui.Controls;
using Area23.At.WinForm.CryptFormCore.Helper;
using Area23.At.WinForm.CryptFormCore.Properties;
using System.Media;
using System.Security.Policy;

namespace Area23.At.WinForm.CryptFormCore.Gui.Forms
{

    /// <summary>
    /// EncryptFormSimple
    /// </summary>
    public partial class EncryptFormSimple : EncryptFormBase
    {

        #region ctor and load

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptFormSimple"/> class.
        /// </summary>
        /// <remarks>This constructor sets up the form and initializes its components.  It should be
        /// called when creating a new instance of the <see cref="EncryptFormSimple"/> form.</remarks>
        public EncryptFormSimple()
        {
            InitializeComponent();
        }

        /// <summary>
        /// EncryptForm_Load - form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void EncryptForm_Load(object sender, EventArgs e)
        {
            this.comboBoxAlgo.Items.Clear();
            foreach (string cipher in GetCipherEnums())
            {
                //if (cipher.StartsWith(CipherEnum.ZenMatrix2.ToString(), StringComparison.OrdinalIgnoreCase))
                //    continue;
                //else
                    this.comboBoxAlgo.Items.Add(cipher);
            }

            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            this.pictureBoxRunningPipe.Visible = true;
            this.textBoxKey.Text = GetEmailFromRegistry();

            comboBoxCompression.SelectedItem = ZipType.None.ToString();
            comboBoxEncoding.SelectedItem = EncodingType.Base64.ToString();
            radioButtonListHash.SelectedItem = KeyHash.Hex.ToString();
            Hash_Click(sender, e);
        }

        #endregion ctor and load

        #region MenuCompressionEncodingZipHash

        protected internal void menuCompression_Click(object sender, EventArgs e) => SetCompression((ToolStripMenuItem)sender, null);

        protected internal void ComboBoxCompression_SelectedIndexChanged(object sender, EventArgs e) => SetCompression(null, comboBoxCompression.SelectedItem);

        /// <summary>
        /// SetCompression – sets compression type from menu or combobox
        /// </summary>
        /// <param name="mi">selected compression ToolStripMenuItem</param>
        /// <param name="comboItem">selected compression combobox item</param>
        protected internal void SetCompression(ToolStripMenuItem? mi = null, object? comboItem = null)
        {
            ZipType zipType = (mi != null) ? ZipTypeExtensions.GetZipType(mi.Name ?? "None") :
                (comboItem != null && !string.IsNullOrEmpty(comboItem.ToString())) ? ZipTypeExtensions.GetZipType(comboItem.ToString() ?? "None") :
                    ZipType.None;

            if (mi != null && mi.Checked && comboItem == null)
            {
                comboBoxCompression.SelectedItem = zipType.ToString();
                return;
            }

            menu7z.Checked = false;
            menuBZip2.Checked = false;
            menuGZip.Checked = false;
            menuZip.Checked = false;
            menuCompressionNone.Checked = false;

            if (mi != null && mi.Name != null &&
                (mi.Name.StartsWith("menu") && (mi.Name.EndsWith("7z") || mi.Name.EndsWith("BZip2") || mi.Name.EndsWith("Gzip") || mi.Name.EndsWith("Zip") || mi.Name.EndsWith("None"))))
            {
                mi.Checked = true;
                for (int i = 0; i < comboBoxCompression.Items.Count; i++)
                {
                    if (comboBoxCompression.Items[i] != null && comboBoxCompression.Items[i].ToString() == zipType.ToString())
                    {
                        comboBoxCompression.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (mi == null && comboItem != null && !string.IsNullOrEmpty(comboItem.ToString()))
            {
                zipType = ZipTypeExtensions.GetZipType(comboItem.ToString() ?? "None");
                switch (zipType)
                {
                    case ZipType.BZip2: menuBZip2.Checked = true; break;
                    case ZipType.GZip: menuGZip.Checked = true; break;
                    case ZipType.Zip: menuZip.Checked = true; break;
                    case ZipType.Z7:
                    case ZipType.None:
                    default:
                        menuCompressionNone.Checked = true;
                        comboBoxCompression.SelectedItem = ZipType.None.ToString();
                        break;
                }
            }
            notifyIcon1.ShowBalloonTip(1250, "Info", $"ZipType {zipType.ToString()} set.", ToolTipIcon.Info);
            notifyIcon1.Visible = true;
        }

        /// <summary>
        /// GetZip - gets selected compression type
        /// </summary>
        /// <returns></returns>
        protected internal ZipType GetZip()
        {
            if (menu7z.Checked) return ZipType.Z7;
            if (menuBZip2.Checked) return ZipType.BZip2;
            if (menuGZip.Checked) return ZipType.GZip;
            if (menuZip.Checked) return ZipType.Zip;
            // if (menuCompressionNone.Checked) return ZipType.None;
            menuCompressionNone.Checked = true;
            comboBoxCompression.SelectedItem = ZipType.None.ToString();
            return ZipType.None;
        }

        protected internal void menuEncodingKind_Click(object sender, EventArgs e) => SetEncoding((ToolStripMenuItem)sender, null);

        protected internal void comboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e) => SetEncoding(null, comboBoxEncoding.SelectedItem);

        /// <summary>
        /// SetEncoding - sets encoding type from menu or combobox
        /// </summary>
        /// <param name="mi">encoding ToolStripMenuItem</param>
        /// <param name="comboItem">selected encoding combobox item</param>
        protected internal void SetEncoding(ToolStripMenuItem? mi = null, object? comboItem = null)
        {
            EncodingType encodingType = (mi != null) ? EncodingTypesExtensions.GetEncodingTypeFromString(mi.Name.Replace("menu", "")) :
                (comboItem != null && !string.IsNullOrEmpty(comboItem.ToString())) ? EncodingTypesExtensions.GetEncodingTypeFromString(comboItem.ToString() ?? "None") :
                EncodingType.None;

            if (mi != null && mi.Checked && comboItem == null)
            {
                comboBoxEncoding.SelectedItem = encodingType.ToString();
                return;
            }

            menuItemNone.Checked = false;
            menuBase16.Checked = false;
            menuHex16.Checked = false;
            menuBase32.Checked = false;
            menuHex32.Checked = false;
            menuBase64.Checked = false;
            menuUu.Checked = false;
            menuXx.Checked = false;

            if (mi != null && mi.Name != null &&
                (mi.Name.StartsWith("menuBase") || mi.Name.StartsWith("menuHex") || mi.Name.StartsWith("menuUu") ||
                    mi.Name.StartsWith("menuItemNone") || mi.Name.StartsWith("menuXx")))
            {
                mi.Checked = true;
                for (int i = 0; i < comboBoxEncoding.Items.Count; i++)
                {
                    if (comboBoxEncoding.Items[i] != null && comboBoxEncoding.Items[i].ToString() == encodingType.ToString())
                    {
                        comboBoxEncoding.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (mi == null && comboItem != null && !string.IsNullOrEmpty(comboItem.ToString()))
            {
                encodingType = EncodingTypesExtensions.GetEncodingTypeFromString(comboItem.ToString() ?? "None");
                switch (encodingType)
                {
                    case EncodingType.Base16: menuBase16.Checked = true; break;
                    case EncodingType.Hex16: menuHex16.Checked = true; break;
                    case EncodingType.Base32: menuBase32.Checked = true; break;
                    case EncodingType.Hex32: menuHex32.Checked = true; break;
                    case EncodingType.Uu: menuUu.Checked = true; break;
                    case EncodingType.Xx: menuXx.Checked = true; break;
                    case EncodingType.None: menuItemNone.Checked = true; break;
                    case EncodingType.Base64:
                    default: menuBase64.Checked = true; break;
                }
            }
            notifyIcon1.Text = $"Encoding {encodingType.ToString()} set.";
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(1000, "Info", $"Encoding {encodingType.ToString()} set.", ToolTipIcon.Info);
        }

        /// <summary>
        /// GetEncoding - gets selected encoding type
        /// </summary>
        /// <returns></returns>
        protected internal EncodingType GetEncoding()
        {
            if (menuItemNone.Checked) return EncodingType.None;
            if (menuBase16.Checked) return EncodingType.Base16;
            if (menuHex16.Checked) return EncodingType.Hex16;
            if (menuBase32.Checked) return EncodingType.Base32;
            if (menuHex32.Checked) return EncodingType.Hex32;
            if (menuUu.Checked) return EncodingType.Uu;
            if (menuXx.Checked) return EncodingType.Xx;
            menuBase64.Checked = true;
            comboBoxEncoding.SelectedItem = EncodingType.Base64.ToString();
            return EncodingType.Base64;

        }

        protected internal void menuHash_Click(object sender, EventArgs e) => SetHash((ToolStripMenuItem)sender, null);

        protected internal void RadioButtonListHash_SelectedIndexChanged(object sender, EventArgs e) => SetHash(null, (RadioButtonList)sender);

        /// <summary>
        /// SetHash – sets hash type from menu or radiobuttonlist
        /// </summary>
        /// <param name="mi">hash ToolStripMenuItem selected</param>
        /// <param name="radioButtonList">hash radioButtonList</param>
        protected internal void SetHash(ToolStripMenuItem? mi, RadioButtonList? radioButtonList)
        {
            KeyHash[] keyHashes = KeyHash_Extensions.GetHashTypes();
            KeyHash aKeyHash = KeyHash.Hex;

            menuHashBCrypt.Checked = false;
            menuHashHex.Checked = false;
            menuHashMD5.Checked = false;
            menuHashOpenBsd.Checked = false;
            menuHashSCrypt.Checked = false;
            menuHashSha1.Checked = false;
            menuHashSha256.Checked = false;
            menuHashSha512.Checked = false;

            string hashPattern = "Hex";
            if (mi != null && mi.Name != null && mi.Name.StartsWith("menuHash"))
            {
                mi.Checked = true;
                hashPattern = mi.Name.Replace("menuHash", "");
                if (hashPattern.Equals("OpenBSD", StringComparison.CurrentCultureIgnoreCase))
                    hashPattern = "OpenBSDCrypt";
                try
                {
                    if (radioButtonList != null)
                        radioButtonList.SelectedItem = hashPattern;
                }
                catch (Exception exRadio)
                {
                    Area23Log.LogOriginEx("EncryptFormSimple Hash", exRadio);
                }
            }

            if (radioButtonList != null && radioButtonList.SelectedItem != null)
            {
                aKeyHash = KeyHash_Extensions.GetKeyHashFromString(radioButtonList.SelectedItem.ToString());
                switch (aKeyHash)
                {
                    case KeyHash.BCrypt: menuHashBCrypt.Checked = true; break;
                    case KeyHash.MD5: menuHashMD5.Checked = true; break;
                    case KeyHash.OpenBSDCrypt: menuHashOpenBsd.Checked = true; break;
                    case KeyHash.SCrypt: menuHashSCrypt.Checked = true; break;
                    case KeyHash.Sha1: menuHashSha1.Checked = true; break;
                    case KeyHash.Sha256: menuHashSha256.Checked = true; break;
                    case KeyHash.Sha512: menuHashSha512.Checked = true; break;
                    case KeyHash.Hex:
                    default: menuHashHex.Checked = true; break;
                }
            }

            Hash_Click(this, new EventArgs());
            notifyIcon1.Text = $"{GetHash().ToString()} hashed.";
            notifyIcon1.ShowBalloonTip(1000, "Info", $"{GetHash().ToString()} hashed.", ToolTipIcon.Info);

        }

        /// <summary>
        /// GetHash - gets selected hash type
        /// </summary>
        /// <returns></returns>
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

        #region Key_Click Hash_Click SetPipeline_Click Hash_Pipe_Click

        /// <summary>
        /// pictureBoxKey_Click - fills key textbox with email from registry or standard fallback
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected internal void pictureBoxKey_Click(object sender, EventArgs e)
        {
            this.textBoxKey.Text = GetEmailFromRegistry();
        }

        /// <summary>
        /// Hash_Click - generates hash from key
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected internal void Hash_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                this.textBoxHash.Text = GetHash().Hash(this.textBoxKey.Text);
            }
        }

        /// <summary>
        /// pictureBoxAddAlgo_Click - adds selected algorithm to pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
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
                    notifyIcon1.ShowBalloonTip(3600, "Warning", "Max 8 algorithms in pipe reached!", ToolTipIcon.Warning);
                    notifyIcon1.Visible = true;
                }
            }
        }

        /// <summary>
        /// pictureBoxDelete_Click - clears pipeline textbox
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void pictureBoxDelete_Click(object sender, EventArgs e)
        {
            this.textBoxPipe.Text = "";
        }

        /// <summary>
        /// Hash_Pipe_Click - creates pipeline from hash
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected internal void Hash_Pipe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                notifyIcon1.ShowBalloonTip(2000, "Warning", "Key is empty!", ToolTipIcon.Warning);
                notifyIcon1.Visible = true;
                return;
            }

            this.textBoxPipe.Text = string.Empty;
            if (string.IsNullOrEmpty(this.textBoxHash.Text))
                Hash_Click(sender, e);

            CipherPipe cPipe = new CipherPipe(this.textBoxHash.Text, this.textBoxKey.Text);
            foreach (CipherEnum cipher in cPipe.InPipe)
            {
                this.textBoxPipe.Text += cipher.ToString() + ";";
            }
        }

        /// <summary>
        /// SetPipeline_Click - creates pipeline from key
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected internal void SetPipeline_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                notifyIcon1.ShowBalloonTip(2000, "Warning", "Key is empty!", ToolTipIcon.Warning);
                notifyIcon1.Visible = true;
                return;
            }

            this.textBoxPipe.Text = string.Empty;
            if (string.IsNullOrEmpty(this.textBoxHash.Text))
                Hash_Click(sender, e);

            CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
            foreach (CipherEnum cipher in cPipe.InPipe)
            {
                this.textBoxPipe.Text += cipher.ToString() + ";";
            }
        }

        /// <summary>
        /// RandomText_Click - fills source textbox with random fortune text
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected internal void RandomText_Click(object sender, EventArgs e)
        {
            string[] fortunes = ResReader.GetFortunes();
            if (fortunes.Length > 0)
            {
                Random rand = new Random(DateTime.Now.Millisecond + DateTime.Now.Second);
                int rIdx = rand.Next(0, fortunes.Length - 1);
                this.textBoxSrc.Text = fortunes[rIdx];
            }
        }

        /// <summary>
        /// Reset_Click - resets all fields to default
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected internal void Reset_Click(object sender, EventArgs e)
        {
            this.textBoxHash.Text = string.Empty;
            this.textBoxKey.Text = string.Empty;
            this.textBoxPipe.Text = string.Empty;
            this.textBoxSrc.Text = string.Empty;
            this.textBoxOut.Text = string.Empty;
            this.labelOutputFile.Text = string.Empty;
            this.labelOutputFile.Visible = false;
            this.pictureBoxOutFile.Image = Properties.Resources.image_file;
            this.pictureBoxOutFile.Tag = null;
            this.pictureBoxOutFile.Visible = false;
            this.SetEncoding(null, "Base64");
            this.SetCompression(null, "None");
            this.SetHash(menuHashHex, radioButtonListHash);
            this.labelFileIn.Text = "[no file selected]";
            this.pictureBoxFileIn.Tag = null;
            this.pictureBoxFileIn.Image = Properties.Resources.image_file;
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
            if (string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                notifyIcon1.Text = "Key is empty!";
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(3000, "Warning", "Key is empty!", ToolTipIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(this.textBoxHash.Text))
                Hash_Click(sender, e);

            if (!string.IsNullOrEmpty(this.textBoxHash.Text))
            {
                this.pictureBoxRunningPipe.Image = Properties.Resources.CryptPipe;
                Icon iconSandClock = new Icon(Properties.Resources.icon_sandclock, new Size(60, 60));
                CipherEnum[] pipeAlgos = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
                CipherPipe cPipe = new CipherPipe(pipeAlgos);

                if (!string.IsNullOrEmpty(this.textBoxSrc.Text))
                {
                    // this.pictureBoxRunningPipe.Visible = true;
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
                    // this.pictureBoxRunningPipe.Visible = true;
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
                                string outFilePath = (file + GetZip().GetZipTypeExtension() + "." + cPipe.PipeString + GetEncoding().GetEnCodingExtension());
                                SaveBytesDialog(outBytes, ref outFilePath);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Tag = "{" + outFilePath + "}";
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
            if (string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                notifyIcon1.ShowBalloonTip(3000, "Warning", "Key is empty!", ToolTipIcon.Warning);
                notifyIcon1.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(this.textBoxHash.Text))
                Hash_Click(sender, e);

            if (!string.IsNullOrEmpty(this.textBoxHash.Text))
            {
                this.pictureBoxRunningPipe.Image = Properties.Resources.PipeDecrypt;
                Icon iconSandClock = new Icon(Properties.Resources.icon_sandclock, new Size(60, 60));

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
                            if (Path.GetFileName(file) == labelFileIn.Text &&  
                                pictureBoxFileIn.Tag != null && pictureBoxFileIn.Tag.ToString() == file)
                            {
                                Cursor.Current = new Cursor(iconSandClock.Handle);
                                // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                                byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                                byte[] outBytes = cPipe.DecryptFileBytesRoundsGo(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                                string outFileDecrypt = file.Replace(GetZip().GetZipTypeExtension() + "." + cPipe.PipeString + GetEncoding().GetEnCodingExtension(), "");
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

        /// <summary>
        /// Drag_Enter - drag enter event for file drop
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DragEventArgs e</param>
        internal void Drag_Enter(object sender, System.Windows.Forms.DragEventArgs e)
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

        /// <summary>
        /// DragEnterOver - handles drag enter and drag over events
        /// </summary>
        /// <param name="files"></param>
        /// <param name="dragNDropState"></param>
        /// <param name="e">DragEventArgs e</param>
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

        /// <summary>
        /// Drag_Leave - drag leave event for file drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void Drag_Leave(object sender, EventArgs e)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            isDragMode = false;
            Cursor.Current = DefaultCursor;
            _dragDropEffect = DragDropEffects.None;
            SetGBoxText(this.groupBoxFiles, "Files Group Box");
        }

        /// <summary>
        /// Drag_Drop - drag drop event for file drop
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DragEventArgs e</param>
        internal void Drag_Drop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) ||
                e.Data.GetDataPresent(typeof(string[]))))
            {
                if ((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null)
                    Drop_Files(files);
            }
            return;
        }

        /// <summary>
        /// Drop_Files - handles dropped files
        /// </summary>
        /// <param name="files"></param>
        internal void Drop_Files(string[] files)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            string ext = null;
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
                            // byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                            // string mimeSig = MimeSignature.GetMimeType(fileBytes, Path.GetFileName(fileName));
                            ext = Path.GetExtension(file).Replace(".", "");
                            pictureBoxFileIn.Image = file.GetImageThumbnailFromFile();
                            pictureBoxFileIn.Tag = file;
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

        #region HelpOpenSave

        protected internal override void menuHelp_Click(object sender, EventArgs e)
        {
            base.menuHelp_Click(sender, e);
        }

        /// <summary>
        /// menuFileOpen_Click opens a file dialog to select a file to encrypt/decrypt
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        internal void menuFileOpen_Click(object sender, EventArgs e)
        {
            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open File";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true; ;
            dialog.RestoreDirectory = true;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName) && System.IO.File.Exists(dialog.FileName))
            {
                pictureBoxFileIn.Image = dialog.FileName.GetImageThumbnailFromFile();
                pictureBoxFileIn.Tag = dialog.FileName;
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
            // this.pictureBoxRunningPipe.Visible = false;
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
            // this.pictureBoxRunningPipe.Visible = false;
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


        private void pictureOutBoxFile_Click(object sender, EventArgs e)
        {
            if (pictureBoxOutFile != null && pictureBoxOutFile.Visible)
            {
                string filePath = pictureBoxOutFile.Tag.ToString() ?? "";
                if (!string.IsNullOrEmpty(filePath) && 
                    !filePath.StartsWith("{") && !filePath.EndsWith("}") &&
                        File.Exists(filePath))
                {
                    ProcessCmd.Execute("explorer", pictureBoxOutFile.Tag.ToString());
                }
            }
        }

        protected void resetPictureBoxFiles(object sender, EventArgs e)
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
