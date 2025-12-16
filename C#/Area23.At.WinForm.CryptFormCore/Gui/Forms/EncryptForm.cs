using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Area23.At.WinForm.CryptFormCore.Gui.Controls;
using Area23.At.WinForm.CryptFormCore.Helper;
using Area23.At.WinForm.CryptFormCore.Properties;


namespace Area23.At.WinForm.CryptFormCore.Gui.Forms
{

    /// <summary>
    /// EncryptForm
    /// </summary>
    public partial class EncryptForm : EncryptFormBase
    {

        #region ctor and load

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptForm"/> class.
        /// </summary>
        /// <remarks>This constructor sets up the form and initializes its components.  It should be
        /// called when creating a new instance of the <see cref="EncryptForm"/> form.</remarks>
        public EncryptForm()
        {
            InitializeComponent();

            buttonEncrypt.Click += new System.EventHandler(async (sender, e) => await Encrypt_Click(sender, e));
            buttonDecrypt.Click += new System.EventHandler(async (sender, e) => await Decrypt_Click(sender, e));
            buttonReset.Click += new System.EventHandler(async (sender, e) => await Reset_Click(sender, e));
            comboBoxEncoding.SelectedIndexChanged += new System.EventHandler(async (sender, e) => await comboBoxEncoding_SelectedIndexChanged(sender, e));
            radioButtonListHash.SelectedIndexChanged += new EventHandler(async (sender, e) => await RadioButtonListHash_SelectedIndexChanged(sender, e));

            menuMainEncrypt.Click += new System.EventHandler(async (sender, e) => await Encrypt_Click(sender, e));
            menuMainDecrypt.Click += new System.EventHandler(async (sender, e) => await Decrypt_Click(sender, e));
            menuMainReset.Click += new System.EventHandler(async (sender, e) => await Reset_Click(sender, e));
            menuAbout.Click += new System.EventHandler(async (sender, e) => await menuAbout_Click(sender, e));
            menuHelpHelp.Click += new System.EventHandler(async (sender, e) => await menuHelp_Click(sender, e));

            ToolStripMenuItem[] menuEncodings = new ToolStripMenuItem[] { menuEncNone, menuEncBase16, menuEncHex16, menuEncHex32, menuEncBase32, menuEncBase64, menuEncUu, menuEncXx };
            foreach (ToolStripMenuItem encodingMenu in menuEncodings)
            {
                encodingMenu.Click += new System.EventHandler(async (sender, e) => await menuEncodingKind_Click(sender, e));
            }

            ToolStripItem[] menuHashes = new ToolStripItem[] { menuHashAscon256, menuHashBlake2xs, menuHashBCrypt, menuHashCShake, menuHashDstu7564, menuHashMD5, menuHashHex, menuHashOpenBSDCrypt, menuHashRipeMD256, menuHashSha1, menuHashSha256, menuHashSha512, menuHashSCrypt, menuHashWhirlpool, menuHashXoodyak };
            foreach (ToolStripMenuItem hashMenu in menuHashes)
            {
                hashMenu.Click += new System.EventHandler(async (sender, e) => await menuHash_Click(sender, e));
            }

        }

        /// <summary>
        /// EncryptForm_Load - form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void EncryptForm_Load(object sender, EventArgs e)
        {
            this.labelInfoMessage.Visible = false;
            this.textBoxKey.Text = GetEmailFromRegistry();

            this.comboBoxCompression.Items.Clear();
            foreach (ZipType zipType in ZipTypeExtensions.GetZipTypes())
                this.comboBoxCompression.Items.Add(zipType.ToString());
            this.comboBoxCompression.SelectedItem = ZipType.None.ToString();

            this.comboBoxAlgo.Items.Clear();
            foreach (string cipher in GetCipherEnums())
                this.comboBoxAlgo.Items.Add(cipher.ToString());

            this.comboBoxEncoding.Items.Clear();
            foreach (EncodingType encodingType in EncodingTypesExtensions.GetEncodingTypes())
                this.comboBoxEncoding.Items.Add(encodingType.ToString());
            comboBoxEncoding.SelectedItem = EncodingType.Base64.ToString();

            this.pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            this.pictureBoxRunningPipe.Visible = true;
            SetStatusLabelText(this.statusLabelMsg, $"{this.Name} started...");

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

            zmenu7z.Checked = false;
            zmenuBZip2.Checked = false;
            zmenuGZip.Checked = false;
            zmenuZip.Checked = false;
            zmenuNone.Checked = false;

            if (mi != null && mi.Name != null &&
                (mi.Name.StartsWith("zmenu") && (mi.Name.EndsWith("7z") || mi.Name.EndsWith("BZip2") || mi.Name.EndsWith("Gzip") || mi.Name.EndsWith("Zip") || mi.Name.EndsWith("None"))))
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
                    case ZipType.BZip2: zmenuBZip2.Checked = true; break;
                    case ZipType.GZip: zmenuGZip.Checked = true; break;
                    case ZipType.Zip: zmenuZip.Checked = true; break;
                    case ZipType.Z7:
                    case ZipType.None:
                    default:
                        zmenuNone.Checked = true;
                        comboBoxCompression.SelectedItem = ZipType.None.ToString();
                        break;
                }
            }
            SetInfoMessage($"ZipType {zipType.ToString()} set.", ToolTipIcon.Info, 1000);
        }

        /// <summary>
        /// GetZip - gets selected compression type
        /// </summary>
        /// <returns></returns>
        protected internal ZipType GetZip()
        {
            if (zmenu7z.Checked) return ZipType.Z7;
            if (zmenuBZip2.Checked) return ZipType.BZip2;
            if (zmenuGZip.Checked) return ZipType.GZip;
            if (zmenuZip.Checked) return ZipType.Zip;
            // if (zmenuEncNone.Checked) return ZipType.None;
            zmenuNone.Checked = true;
            comboBoxCompression.SelectedItem = ZipType.None.ToString();
            return ZipType.None;
        }

        protected internal async Task menuEncodingKind_Click(object sender, EventArgs e) => await SetEncodingAsync((ToolStripMenuItem)sender, null);

        protected internal async Task comboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e) => await SetEncodingAsync(null, comboBoxEncoding.SelectedItem);

        /// <summary>
        /// SetEncoding - sets encoding type from menu or combobox
        /// </summary>
        /// <param name="mi">encoding ToolStripMenuItem</param>
        /// <param name="comboItem">selected encoding combobox item</param>
        protected internal async Task SetEncodingAsync(ToolStripMenuItem? mi = null, object? comboItem = null)
        {
            EncodingType encodingType = (mi != null) ? EncodingTypesExtensions.GetEncodingTypeFromString(mi.Name.Replace("menuEnc", "")) :
                (comboItem != null && !string.IsNullOrEmpty(comboItem.ToString())) ? EncodingTypesExtensions.GetEncodingTypeFromString(comboItem.ToString() ?? "None") :
                EncodingType.None;

            if (mi != null && mi.Checked && comboItem == null)
            {
                comboBoxEncoding.SelectedItem = encodingType.ToString();
                return;
            }

            menuEncNone.Checked = false;
            menuEncBase16.Checked = false;
            menuEncHex16.Checked = false;
            menuEncBase32.Checked = false;
            menuEncHex32.Checked = false;
            menuEncBase64.Checked = false;
            menuEncUu.Checked = false;
            menuEncXx.Checked = false;

            if (mi != null && mi.Name != null &&
                (mi.Name.StartsWith("menuEncBase") || mi.Name.StartsWith("menuEncHex") || mi.Name.StartsWith("menuEncUu") ||
                    mi.Name.StartsWith("menuEncNone") || mi.Name.StartsWith("menuEncXx")))
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
                    case EncodingType.Base16: menuEncBase16.Checked = true; break;
                    case EncodingType.Hex16: menuEncHex16.Checked = true; break;
                    case EncodingType.Base32: menuEncBase32.Checked = true; break;
                    case EncodingType.Hex32: menuEncHex32.Checked = true; break;
                    case EncodingType.Uu: menuEncUu.Checked = true; break;
                    case EncodingType.Xx: menuEncXx.Checked = true; break;
                    case EncodingType.None: menuEncNone.Checked = true; break;
                    case EncodingType.Base64:
                    default: menuEncBase64.Checked = true; break;
                }
            }
            await SetInfoMessageAsync($"Encoding {encodingType.ToString()} set.", ToolTipIcon.Info, 1000);
        }

        /// <summary>
        /// GetEncoding - gets selected encoding type
        /// </summary>
        /// <returns></returns>
        protected internal EncodingType GetEncoding()
        {
            if (menuEncNone.Checked) return EncodingType.None;
            if (menuEncBase16.Checked) return EncodingType.Base16;
            if (menuEncHex16.Checked) return EncodingType.Hex16;
            if (menuEncBase32.Checked) return EncodingType.Base32;
            if (menuEncHex32.Checked) return EncodingType.Hex32;
            if (menuEncUu.Checked) return EncodingType.Uu;
            if (menuEncXx.Checked) return EncodingType.Xx;
            menuEncBase64.Checked = true;
            comboBoxEncoding.SelectedItem = EncodingType.Base64.ToString();
            return EncodingType.Base64;

        }

        protected internal async Task menuHash_Click(object sender, EventArgs e) => await SetHashAsync((ToolStripMenuItem)sender, (RadioButtonList)radioButtonListHash);

        protected internal async Task RadioButtonListHash_SelectedIndexChanged(object sender, EventArgs e) => await SetHashAsync(null, (RadioButtonList)sender);

        /// <summary>
        /// SetHash – sets hash type from menu or radiobuttonlist
        /// </summary>
        /// <param name="mi">hash ToolStripMenuItem selected</param>
        /// <param name="radioButtonList">hash radioButtonList</param>
        protected internal async Task SetHashAsync(ToolStripMenuItem? mi, RadioButtonList? radioButtonList)
        {
            KeyHash[] keyHashes = KeyHash_Extensions.GetHashTypes();
            KeyHash aKeyHash = KeyHash.Hex;

            menuHashBCrypt.Checked = false;
            menuHashHex.Checked = false;
            menuHashMD5.Checked = false;
            menuHashOpenBSDCrypt.Checked = false;
            menuHashSCrypt.Checked = false;
            menuHashSha1.Checked = false;
            menuHashSha256.Checked = false;
            menuHashSha512.Checked = false;
            menuHashWhirlpool.Checked = false;
            menuHashAscon256.Checked = false;
            menuHashBlake2xs.Checked = false;
            menuHashCShake.Checked = false;
            menuHashDstu7564.Checked = false;
            menuHashRipeMD256.Checked = false;
            menuHashXoodyak.Checked = false;

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
                    Area23Log.LogOriginEx("EncryptForm Hash", exRadio);
                }
            }


            if (radioButtonList != null && radioButtonList.SelectedItem != null)
            {
                aKeyHash = (hashPattern.StartsWith("Xoo") || hashPattern.StartsWith("Zodi")) ?
                            KeyHash_Extensions.GetKeyHashFromString(hashPattern) :
                            KeyHash_Extensions.GetKeyHashFromString(radioButtonList.SelectedItem.ToString());
                switch (aKeyHash)
                {
                    case KeyHash.BCrypt: menuHashBCrypt.Checked = true; break;
                    case KeyHash.MD5: menuHashMD5.Checked = true; break;
                    case KeyHash.OpenBSDCrypt: menuHashOpenBSDCrypt.Checked = true; break;
                    case KeyHash.SCrypt: menuHashSCrypt.Checked = true; break;
                    case KeyHash.Sha1: menuHashSha1.Checked = true; break;
                    case KeyHash.Sha256: menuHashSha256.Checked = true; break;
                    case KeyHash.Sha512: menuHashSha512.Checked = true; break;
                    case KeyHash.Whirlpool: menuHashWhirlpool.Checked = true; break;
                    case KeyHash.Ascon256: menuHashAscon256.Checked = true; break;
                    case KeyHash.Blake2xs: menuHashBlake2xs.Checked = true; break;
                    case KeyHash.CShake: menuHashCShake.Checked = true; break;
                    case KeyHash.Dstu7564: menuHashDstu7564.Checked = true; break;
                    case KeyHash.RipeMD256: menuHashRipeMD256.Checked = true; break;
                    case KeyHash.Xoodyak: menuHashXoodyak.Checked = true; break;
                    case KeyHash.Hex: menuHashHex.Checked = true; break;
                    default:
                        Area23Log.LogOriginMsg("EncryptForm Hash", $"RadioButtonList: {radioButtonList.SelectedItem.ToString()} => KeyHash = {aKeyHash.ToString()}.");
                        menuHashHex.Checked = true;
                        break;
                }
            }

            Hash_Click(this, new EventArgs());
            await SetInfoMessageAsync($"{GetHash().ToString()} hashed.", ToolTipIcon.Info, 1000);
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
            if (menuHashOpenBSDCrypt.Checked) return KeyHash.OpenBSDCrypt;
            if (menuHashSCrypt.Checked) return KeyHash.SCrypt;
            if (menuHashSha1.Checked) return KeyHash.Sha1;
            if (menuHashSha256.Checked) return KeyHash.Sha256;
            if (menuHashSha512.Checked) return KeyHash.Sha512;
            if (menuHashWhirlpool.Checked) return KeyHash.Whirlpool;
            if (menuHashAscon256.Checked) return KeyHash.Ascon256;
            if (menuHashBlake2xs.Checked) return KeyHash.Blake2xs;
            if (menuHashCShake.Checked) return KeyHash.CShake;
            if (menuHashDstu7564.Checked) return KeyHash.Dstu7564;
            if (menuHashRipeMD256.Checked) return KeyHash.RipeMD256;
            if (menuHashXoodyak.Checked) return KeyHash.Xoodyak;

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
        /// Event fired, when text in textbox key changed, when leaving cursor and stopped editing
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected internal void textBoxKey_TextChanged(object sender, EventArgs e)
        {
            Hash_Click(sender, e);
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
                if (cipherAlgors.Length < 8)
                {
                    switch (cipherEnum)
                    {
                        case CipherEnum.BlowFish:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.blowfish, "", true);
                            break;
                        case CipherEnum.Fish2:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.TwoFish, "", true);
                            break;
                        case CipherEnum.Fish3:
                        case CipherEnum.ThreeFish256:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.ThreeFish, "", true);
                            break;
                        case CipherEnum.Serpent:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.Serpent, "", true);
                            break;
                        case CipherEnum.XTea:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.XTea, "", true);
                            break;
                        case CipherEnum.Tea:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.Tea, "", true);
                            break;
                        case CipherEnum.Des:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.Des, "", true);
                            break;
                        case CipherEnum.Des3:
                            SetPictureBoxImage(this.pictureBoxFileIn, Properties.Resources.TripleDes, "", true);
                            break;
                        default:
                            break;
                    }
                    this.textBoxPipe.Text += cipherEnum.ToString() + ";";
                    resetPictureBoxFiles(sender, e);
                }
                else
                {
                    SetInfoMessage("Max 8 algorithms in pipe reached!", ToolTipIcon.Warning, 2000);
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
                SetInfoMessage("Key is empty!", ToolTipIcon.Warning, 2000);
                return;
            }

            this.textBoxPipe.Text = string.Empty;
            if (string.IsNullOrEmpty(this.textBoxHash.Text))
                Hash_Click(sender, e);

            CipherPipe cPipe = new CipherPipe(this.textBoxHash.Text, this.textBoxKey.Text, GetEncoding(), GetZip(), GetHash());
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
                SetInfoMessage("Key is empty!", ToolTipIcon.Warning, 2000);
                return;
            }

            this.textBoxPipe.Text = string.Empty;
            if (string.IsNullOrEmpty(this.textBoxHash.Text))
                Hash_Click(sender, e);

            CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
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
        protected internal async Task Reset_Click(object sender, EventArgs e)
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
            await this.SetEncodingAsync(menuEncBase64);
            this.SetCompression(null, "None");
            await this.SetHashAsync(menuHashHex, radioButtonListHash);
            this.labelFileIn.Text = "[no file selected]";
            this.pictureBoxFileIn.Tag = null;
            this.pictureBoxFileIn.Image = Properties.Resources.image_file;
            this.pictureBoxRunningPipe.Image = Properties.Resources.CryptPipe1;
        }

        #endregion ButtonPictureBoxClickEvents

        #region EncryptDecrypt_Click

        /// <summary>
        /// Encrypt_Click - encrypts text or file with given key, hash, zip and encoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected internal async Task Encrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                SetInfoMessage("Key is empty!", ToolTipIcon.Warning, 2000);
                return;
            }
            if (string.IsNullOrEmpty(this.textBoxHash.Text))
                Hash_Click(sender, e);

            Icon iconSandClock = new Icon(Properties.Resources.icon_sandclock, new Size(60, 60));
            if (string.IsNullOrEmpty(this.textBoxPipe.Text) && this.warnOnEmptyPipeToolStripMenuItem.Checked)
            {
                string warnMsg = $"No encryption pipe is set, do you want to {GetEncoding()} encode only?";
                if (GetEncoding() == EncodingType.None && GetZip() == ZipType.None)
                    warnMsg = "Neither pipe, nor zip, nor encoding is set, encrypt will transform nothing.";

                DialogResult result = MessageBox.Show(this, warnMsg, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
            }
            CipherEnum[] pipeAlgos = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
            CipherPipe cPipe = new CipherPipe(pipeAlgos, 8, GetEncoding(), GetZip(), GetHash());

            BitmapPipelineGnerator bGen = new BitmapPipelineGnerator(cPipe);
            SetPictureBoxImage(pictureBoxRunningPipe, bGen.GenerateEncryptPipeImage());

            DateTime start = DateTime.Now;
            if (!string.IsNullOrEmpty(this.textBoxSrc.Text))
            {
                this.textBoxOut.Text = "";
                Cursor.Current = new Cursor(iconSandClock.Handle);
                await SetInfoMessageAsync("Starting encryption plain text", ToolTipIcon.Info, -1);
                try
                {
                    await SetStatusLabelTextAsync(this.statusLabelSource, $"source chars: {textBoxSrc.Text.Length}");
                    if (menuEncNone.Checked && (pipeAlgos.Length > 0 || GetZip() != ZipType.None))
                        await SetEncodingAsync(menuEncBase64);

                    string encrypted = cPipe.EncrpytTextGoRounds(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                    this.textBoxOut.Text = encrypted;
                    await SetStatusLabelTextAsync(this.statusLabelDestination, $"destination chars: {this.textBoxOut.Text.Length}");
                    await SetInfoMessageAsync("Encryption finished", ToolTipIcon.Info, 5000);
                }
                catch (Exception ex)
                {
                    Area23Log.LogOriginMsgEx("EncryptForm", "Decrypt_Click", ex);
                    SetInfoMessage(ex.GetType().Name + ": " + ex.Message.ToString(), ToolTipIcon.Error, 4000);
                }
                finally
                {
                    Cursor.Current = DefaultCursor;
                }
            }
            if (!string.IsNullOrEmpty(this.labelFileIn.Text) && !labelFileIn.Text.StartsWith("["))
            {
                string fileName = FileMatches();
                if (string.IsNullOrEmpty(fileName))
                {
                    if (string.IsNullOrEmpty(this.textBoxSrc.Text))
                    {
                        await SetInfoMessageAsync("No file found to encrypt", ToolTipIcon.Warning, 6000);
                        await SetStatusLabelTextAsync(this.statusLabelSource, "No file found to encrypt");
                    }
                    return;
                }

                if (this.warnOnDoubleZippingToolStripMenuItem.Checked && Path.GetExtension(fileName).IsCompressedFile() && GetZip() != ZipType.None)
                {
                    DialogResult dresult = MessageBox.Show(this, "Zip an already compressed file twice?", "Double zip warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dresult == DialogResult.No)
                        return;
                }

                await SetInfoMessageAsync("Starting encryption for file " + labelFileIn.Text, ToolTipIcon.Info, -1);

                Cursor.Current = new Cursor(iconSandClock.Handle);
                try
                {
                    // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(fileName);

                    byte[] encodedBytes = cPipe.EncryptEncodeBytes(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                    string miniPipe = string.IsNullOrEmpty(cPipe.PipeString) ? "" : "." + cPipe.PipeString;
                    string outFilePath = (fileName + GetHash().GetExtension() + GetZip().GetZipTypeExtension() + miniPipe + GetEncoding().GetEnCodingExtension());

                    Cursor.Current = new Cursor(iconSandClock.Handle);
                    await SetStatusLabelTextAsync(this.statusLabelMsg, "encryption time: " + DateTime.Now.Subtract(start).ToString());
                    await SetInfoMessageAsync("Starting verificaton", ToolTipIcon.Info, -1);

                    bool saved = SaveBytesDialog(encodedBytes, ref outFilePath);
                    if (saved)
                    {
                        string outFileName = Path.GetFileName(outFilePath);
                        bool isVerified = true;
                        if (sha512ToolStripMenuItem.Checked)
                            isVerified = await VerifyEncryptedFileShaAsync(fileName, outFilePath, this.textBoxKey.Text, this.textBoxHash.Text, cPipe);
                        if (bytesOfFileToolStripMenuItem.Checked)
                            isVerified = await VerifyEncryptedFileBytesAsync(fileName, outFilePath, this.textBoxKey.Text, this.textBoxHash.Text, cPipe);

                        if (!isVerified)
                        {
                            await SetInfoMessageAsync("Encryption couldn't be verified", ToolTipIcon.Warning, -1);
                            await PlaySoundFromResourcesAsync("sound_hammer");
                            await SetPictureBoxImageAsync(pictureBoxOutFile, Properties.Resources.file_encrypted_broken, "{" + outFilePath + "}", true);
                        }
                        else
                        {
                            await SetInfoMessageAsync("Encryption verified", ToolTipIcon.Info, -1);
                            await PlaySoundFromResourcesAsync("sound_laser");
                            await SetPictureBoxImageAsync(pictureBoxOutFile, outFilePath.GetImageThumbnailFromFile(), "{" + outFilePath + "}", true);
                        }

                        await SetLabelTextAsync(labelOutputFile, outFileName);
                        HashFiles.Add(outFilePath);

                        Cursor.Current = DefaultCursor;
                    }
                    else
                    {
                        await SetInfoMessageAsync("Saving file canceled by user", ToolTipIcon.Warning, 3000);
                    }
                }
                catch (Exception ex)
                {
                    await SetInfoMessageAsync(ex.GetType().Name + ": " + ex.Message.ToString(), ToolTipIcon.Error, 5000);
                }

                Cursor.Current = DefaultCursor;
            }

            await SetStatusLabelTextAsync(this.statusLabelMsg, "total time: " + DateTime.Now.Subtract(start).ToString());

        }

        public string FileMatches()
        {
            foreach (string file in HashFiles)
            {
                if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file) &&
                    labelFileIn != null && Path.GetFileName(file) == labelFileIn.Text &&
                        pictureBoxFileIn.Tag != null && pictureBoxFileIn.Tag.ToString() == file)
                    return file;
            }
            return string.Empty;
        }

        /// <summary>
        /// Decrypt_Click - decrypts text or file with given key, hash, zip and encoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected internal async Task Decrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                await SetInfoMessageAsync("Key is empty!", ToolTipIcon.Warning, -1);
                return;
            }
            if (string.IsNullOrEmpty(this.textBoxHash.Text))
                Hash_Click(sender, e);

            DateTime start = DateTime.Now;

            this.pictureBoxRunningPipe.Image = Properties.Resources.PipeLineDecrypt;
            Icon iconSandClock = new Icon(Properties.Resources.icon_sandclock, new Size(60, 60));

            CipherEnum[] pipeAlgos = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
            CipherPipe cPipe = new CipherPipe(pipeAlgos, 8, GetEncoding(), GetZip(), GetHash());

            BitmapPipelineGnerator bGen = new BitmapPipelineGnerator(cPipe);
            SetPictureBoxImage(pictureBoxRunningPipe, bGen.GenerateDecryptPipeImage());

            if (!string.IsNullOrEmpty(this.textBoxSrc.Text))
            {
                this.textBoxOut.Text = "";
                Cursor.Current = new Cursor(iconSandClock.Handle);
                await SetInfoMessageAsync("Starting decryption of cipher text", ToolTipIcon.Info, -1);

                try
                {
                    await SetStatusLabelTextAsync(this.statusLabelSource, $"source chars: {textBoxSrc.Text.Length}");
                    if (menuEncNone.Checked && (pipeAlgos.Length > 0 || GetZip() != ZipType.None))
                        await SetEncodingAsync(menuEncBase64);

                    // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                    string decrypted = cPipe.DecryptTextRoundsGo(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                    this.textBoxOut.Text = decrypted;
                    await SetInfoMessageAsync("Decryption finished", ToolTipIcon.Info, 6000);
                    await SetStatusLabelTextAsync(this.statusLabelDestination, $"destination chars: {this.textBoxOut.Text.Length}");
                }
                catch (Exception ex)
                {
                    Area23Log.LogOriginMsgEx("EncryptForm", "Decrypt_Click", ex);
                    SetInfoMessage(ex.GetType().Name + ": " + ex.Message.ToString(), ToolTipIcon.Error, 4000);
                }
                finally
                {
                    Cursor.Current = DefaultCursor;
                }
            }
            if (!string.IsNullOrEmpty(this.labelFileIn.Text) && !labelFileIn.Text.StartsWith("["))
            {
                string fileName = FileMatches();
                if (string.IsNullOrEmpty(fileName))
                {
                    await SetInfoMessageAsync("No file found to decrypt", ToolTipIcon.Warning, 6000);
                    await SetStatusLabelTextAsync(this.statusLabelSource, "No file found to decrypt");
                    return;
                }

                Cursor.Current = new Cursor(iconSandClock.Handle);
                await SetInfoMessageAsync("Starting decryption file " + labelFileIn.Text, ToolTipIcon.Info, -1);

                try
                {
                    // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(fileName);
                    
                    byte[] outBytes = cPipe.DecodeDecrpytBytes(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                    string miniPipe = (string.IsNullOrEmpty(cPipe.PipeString)) ? "" : "." + cPipe.PipeString;
                    string outFileDecrypt = (fileName.Contains(GetHash().GetExtension())) ? fileName.Replace(GetHash().GetExtension(), "") : fileName;
                    outFileDecrypt = outFileDecrypt.Replace(GetZip().GetZipTypeExtension() + miniPipe + GetEncoding().GetEnCodingExtension(), "");
                    
                    bool saved = SaveBytesDialog(outBytes, ref outFileDecrypt);
                    if (saved)
                    {
                        HashFiles.Add(outFileDecrypt);
                        await SetPictureBoxImageAsync(pictureBoxOutFile, outFileDecrypt.GetImageThumbnailFromFile(), outFileDecrypt, true);
                        await SetLabelTextAsync(labelOutputFile, Path.GetFileName(outFileDecrypt));
                        await SetInfoMessageAsync("file decrypted", ToolTipIcon.Info, -1);
                    }
                    else
                        await SetInfoMessageAsync("Saving file canceled by user", ToolTipIcon.Warning, 6000);
                }
                catch (Exception ex)
                {
                    await SetInfoMessageAsync(ex.GetType().Name + ": " + ex.Message.ToString(), ToolTipIcon.Error, 8000);
                }
                finally
                {
                    Cursor.Current = DefaultCursor;
                }
            }

            await SetStatusLabelTextAsync(this.statusLabelMsg, "Time: " + DateTime.Now.Subtract(start).ToString());
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
        internal void Drop_Files(string[] files)
        {
            pictureBoxRunningPipe.Image = Resources.CryptPipe1;
            string ext = null;
            if (isDragMode && files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                    {
                        FileAddedAction(file);
                        ext = Path.GetExtension(file).Replace(".", "");
                        _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
                        isDragMode = false;
                        break;
                    }
                }

            }

            Cursor.Current = DefaultCursor;
        }

        #endregion DragNDrop

        #region file loading and saving ops

        /// <summary>
        /// FileAddedAction is called, when a file was opened 
        /// either by Menu Main -> Open
        /// or dragged into the file groupbox of the WinForm
        /// </summary>
        /// <param name="fileName"></param>
        internal void FileAddedAction(string fileName)
        {            
            FileInfo fi = new FileInfo(fileName);
            if (fi.Exists && fi.Length > 0)
            {
                this.textBoxSrc.Text = string.Empty;
                this.textBoxOut.Text = string.Empty;
                SetGBoxText(this.groupBoxFiles, "Files Group Box");

                pictureBoxFileIn.Image = fileName.GetImageThumbnailFromFile();
                pictureBoxFileIn.Tag = fileName;
                labelFileIn.Text = Path.GetFileName(fileName);

                _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
                isDragMode = false;

                Task.Run(() => PlaySoundFromResource("sound_arrow"));

                HashFiles = new HashSet<string>();
                HashFiles.Add(fileName);

                if (fi.Length > 1048576)
                    SetStatusLabelText(this.statusLabelSource, $"FileSize: {(fi.Length / 1048576)} MB");
                else if (fi.Length > 2048)
                    SetStatusLabelText(this.statusLabelSource, $"FileSize: {(fi.Length / 1024)} kb");
                else SetStatusLabelText(this.statusLabelSource, $"FileSize: {fi.Length} bytes");

                if (menuItemCreatePipeSettingsFromFileName.Checked)
                {
                    var cpip = GetCPipeFromFileName(fileName);
                    if (cpip != null)
                    {
                        ToolStripItem[] menuHashes = new ToolStripItem[] { menuHashAscon256, menuHashBlake2xs, menuHashBCrypt, menuHashCShake, menuHashDstu7564, menuHashMD5, menuHashHex, menuHashOpenBSDCrypt, menuHashRipeMD256, menuHashSha1, menuHashSha256, menuHashSha512, menuHashSCrypt, menuHashWhirlpool, menuHashXoodyak };
                        ToolStripMenuItem[] menuZips = new ToolStripMenuItem[] { zmenu7z, zmenuBZip2, zmenuGZip, zmenuZip, zmenuNone };
                        ToolStripMenuItem[] menuEncodings = new ToolStripMenuItem[] { menuEncNone, menuEncBase16, menuEncHex16, menuEncHex32, menuEncBase32, menuEncBase64, menuEncUu, menuEncXx };

                        foreach (var miHash in menuHashes)
                        {
                            if (miHash.Name.Replace("menuHash", "").Equals(cpip.KHash.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                                miHash.Text.Equals(cpip.KHash.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                SetHashAsync((ToolStripMenuItem)miHash, radioButtonListHash).ConfigureAwait(false);

                                break;
                            }
                        }
                        foreach (var miEnc in menuEncodings)
                        {
                            if (miEnc.Name.Replace("menuEnc", "").Equals(cpip.EncodeType.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                                miEnc.Text.Equals(cpip.EncodeType.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                SetEncodingAsync((ToolStripMenuItem)miEnc, null).ConfigureAwait(true);
                                break;
                            }
                        }
                        foreach (var miZip in menuZips)
                        {
                            if (miZip.Name.Replace("zmenu", "").Equals(cpip.ZType.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                               miZip.Text.Equals(cpip.ZType.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                SetCompression((ToolStripMenuItem)miZip, null);
                                break;
                            }
                        }

                        this.textBoxPipe.Text = "";
                        foreach (CipherEnum cipher in cpip.InPipe)
                        {
                            this.textBoxPipe.Text += cipher.ToString() + ";";
                        }
                    }
                }

            }
        }

        #endregion file loading and saving ops

        #region HelpOpenSave

        protected internal override async Task menuHelp_Click(object sender, EventArgs e)
        {
            await base.menuHelp_Click(sender, e);
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
                FileAddedAction(dialog.FileName);
            }
            else
            {
                SetInfoMessage("Opening file canceled.", ToolTipIcon.Warning, 2000);
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
                {
                    outFilePath = dialog.FileName;
                    try
                    {
                        File.WriteAllBytes(outFilePath, fileBytes);
                    }
                    catch (Exception ex)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in SaveBytesDialog for file: \"{outFilePath}\".\n", ex);
                        return false;
                    }
                    FileInfo fi = new FileInfo(outFilePath);
                    if (fi.Exists && fi.Length > 0)
                    {
                        if (fi.Length > 1048576)
                            SetStatusLabelText(this.statusLabelDestination, $"FileSize: {(fi.Length / 1048576)} MB");
                        else if (fi.Length > 2048)
                            SetStatusLabelText(this.statusLabelDestination, $"FileSize: {(fi.Length / 1024)} kb");
                        else SetStatusLabelText(this.statusLabelDestination, $"FileSize: {fi.Length} bytes");
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

        protected internal void SetInfoMessage(string message, ToolTipIcon toolIcon = ToolTipIcon.Info, int duration = 4000)
        {
            SetLabelText(labelInfoMessage, message);
            SetStatusLabelText(this.statusLabelMsg, message);
            string toolHeader = toolIcon.ToString();
            switch (toolIcon)
            {
                case ToolTipIcon.Error:
                    toolHeader = "Error";
                    SetLabelBackColor(labelInfoMessage, ColorTranslator.FromHtml("#bab510"));
                    PlaySoundFromResource("sound_error");
                    break;
                case ToolTipIcon.Warning:
                    SetLabelBackColor(labelInfoMessage, Color.LightYellow);
                    toolHeader = "Warning";
                    PlaySoundFromResource("sound_warning");
                    break;
                case ToolTipIcon.Info:
                default:
                    SetLabelBackColor(labelInfoMessage, SystemColors.Info);
                    toolHeader = "Info";
                    PlaySoundFromResource("sound_info");
                    break;
            }
            SetLabelVisible(this.labelInfoMessage, true);

            if (duration > 0)
            {
                System.Timers.Timer setInfoMessageTimer = new System.Timers.Timer { Interval = duration };
                setInfoMessageTimer.Elapsed += (s, en) =>
                {
                    Task.Run(new System.Action(() =>
                    {
                        SetLabelText(labelInfoMessage, "");
                        SetLabelBackColor(labelInfoMessage, SystemColors.Info);
                        SetLabelVisible(labelInfoMessage, false);
                    }));
                    setInfoMessageTimer.Stop(); // Stop the timer(otherwise keeps on calling)
                };
                setInfoMessageTimer.Start();
            }
        }

        protected internal async Task SetInfoMessageAsync(string message, ToolTipIcon toolIcon = ToolTipIcon.Info, int duration = 4000)
        {
            await SetLabelTextAsync(labelInfoMessage, message);
            string toolHeader = toolIcon.ToString();
            switch (toolIcon)
            {
                case ToolTipIcon.Error:
                    toolHeader = "Error";
                    await SetLabelBackColorAsync(labelInfoMessage, ColorTranslator.FromHtml("#bab510"));
                    await PlaySoundFromResourcesAsync("sound_error");
                    break;
                case ToolTipIcon.Warning:
                    await SetLabelBackColorAsync(labelInfoMessage, Color.LightYellow);
                    toolHeader = "Warning";
                    await PlaySoundFromResourcesAsync("sound_warning");
                    break;
                case ToolTipIcon.Info:
                default:
                    await SetLabelBackColorAsync(labelInfoMessage, SystemColors.Info);
                    toolHeader = "Info";
                    await PlaySoundFromResourcesAsync("sound_info");
                    break;
            }

            if (duration > 0)
            {
                System.Timers.Timer setInfoMessageTimer = new System.Timers.Timer { Interval = duration };
                setInfoMessageTimer.Elapsed += (s, en) =>
                {
                    Task.Run(new System.Action(async () =>
                    {
                        await SetLabelBackColorAsync(labelInfoMessage, SystemColors.Info);
                        await SetLabelVisibleAsync(labelInfoMessage, false);
                    }));
                    setInfoMessageTimer.Stop(); // Stop the timer(otherwise keeps on calling)
                };
                setInfoMessageTimer.Start();
            }

        }

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
                    SetPictureBoxImage(this.pictureBoxFileIn, Area23.At.WinForm.CryptFormCore.Properties.Resources.file, "", true);
                    SetPictureBoxImage(this.pictureBoxOutFile, Area23.At.WinForm.CryptFormCore.Properties.Resources.file, "", false);
                }));
                resetPictureBoxFileTimer.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            resetPictureBoxFileTimer.Start();
        }

        #endregion Media Methods

    }
}
