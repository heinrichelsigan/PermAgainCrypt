using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Gui.Controls;
using EU.CqrXs.Gui.Helper;
using EU.CqrXs.Gui.Properties;
using EU.CqrXs.Gui.Sound;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using System.Security.Cryptography;



namespace EU.CqrXs.Gui.Forms
{

    /// <summary>
    /// EncryptForm
    /// </summary>
    public partial class EncryptFormMultiControls : EncryptFormBase
    {

        protected internal CipherPipe? cPipe = null;
        protected internal string simg = "";
        protected internal ToolStripMenuItem[] menuEncodings;
        protected internal ToolStripMenuItem[] menuZips;
        protected internal ToolStripMenuItem[] mHashes;
        protected internal ToolStripMenuItem[] mCipherModes;

        #region ctor and load

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptFormMultiControls"/> class.
        /// </summary>
        /// <remarks>This constructor sets up the form and initializes its components.  It should be
        /// called when creating a new instance of the <see cref="EncryptFormMultiControls"/> form.</remarks>
        public EncryptFormMultiControls()
        {
            InitializeComponent();
            menuEncodings = new ToolStripMenuItem[] { menuEncNone, menuEncBase16, menuEncHex16, menuEncHex32, menuEncBase32, menuEncHex64, menuEncBase64, menuEncUu, menuEncXx };
            menuZips = new ToolStripMenuItem[] { zmenu7z, zmenuBZip2, zmenuGZip, zmenuZip, zmenuNone };
            mHashes = new ToolStripMenuItem[] { menuHashBCrypt, menuHashBlake2xs, menuHashCShake, menuHashDstu7564, menuHashHex, menuHashMD5, menuHashOpenBSDCrypt, menuHashRipeMD256, menuHashSCrypt, menuHashSha1, menuHashSha256, menuHashSha384, menuHashSha512, menuHashTupleHash, menuHashWhirlpool };
            // mCipherModes = new ToolStripMenuItem[] { menuCipherModeItemCBC, menuCipherModeItemCCM, menuCipherModeItemCFB, menuCipherModeItemCTS, menuCipherModeItemEAX, menuCipherModeItemECB, menuCipherModeItemGOFB };
            mCipherModes = new ToolStripMenuItem[] { menuCipherModeItemCBC, menuCipherModeItemCFB, menuCipherModeItemECB };

            tabControlWithHexDest.AsciiTextReadonly = true;
            buttonEncrypt.Click += new System.EventHandler(async (sender, e)
                => await Encrypt_Click(sender, e));
            buttonDecrypt.Click += new System.EventHandler(async (sender, e)
                => await Decrypt_Click(sender, e));
            buttonReset.Click += new System.EventHandler(async (sender, e)
                => await Reset_Click(sender, e));
            comboBoxEncoding.SelectedIndexChanged += new System.EventHandler(async (sender, e) =>
                await comboBoxEncoding_SelectedIndexChanged(sender, e));
            radioButtonListHash.SelectedIndexChanged += new EventHandler(async (sender, e)
                => await RadioButtonListHash_SelectedIndexChanged(sender, e));
            comboBoxCompression.SelectedIndexChanged += new System.EventHandler(async (sender, e)
                => await ComboBoxCompression_SelectedIndexChanged(sender, e));
            groupBoxFiles.FileAdded += GroupBoxFilesAdded;
            groupBoxFiles.FileRequired += GroupBoxFileRequired;

            menuMainEncrypt.Click += new System.EventHandler(async (sender, e)
                => await Encrypt_Click(sender, e));
            menuMainDecrypt.Click += new System.EventHandler(async (sender, e)
                => await Decrypt_Click(sender, e));
            menuMainReset.Click += new System.EventHandler(async (sender, e)
                => await Reset_Click(sender, e));
            menuAbout.Click += new System.EventHandler(async (sender, e)
                => await menuAbout_Click(sender, e));
            menuHelpHelp.Click += new System.EventHandler(async (sender, e)
                => await menuHelp_Click(sender, e));
            menuMainItemOneTwoThreeFish.Click += new System.EventHandler(async (sender, e)
                 => await menuMainItemOneTwoThreeFish_Click(sender, e));

            menuMainItemSimple.Click += menuMainFormSimple_Click;            

            foreach (ToolStripMenuItem encodingMenu in menuEncodings)
                encodingMenu.Click += new System.EventHandler(async (sender, e) => await menuEncodingKind_Click(sender, e));

            foreach (var zipMenuItem in menuZips)
                zipMenuItem.Click += new System.EventHandler(async (sender, e) => await menuCompression_Click(sender, e));

            foreach (var hashMenuItem in mHashes)
                hashMenuItem.Click += new System.EventHandler(async (sender, e) => await menuHash_Click(sender, e));

            foreach (var cipherModeItem in mCipherModes)
                cipherModeItem.Click += new System.EventHandler(async (sender, e) => await menuCipherMode_Click(sender, e));
            this.comboBoxCompression.Items.Clear();
            foreach (ZipType zipType in ZipTypeExtensions.GetZipTypes())
                this.comboBoxCompression.Items.Add(zipType.ToString());
            this.comboBoxCompression.SelectedItem = ZipType.None.ToString();

            this.comboBoxCipherModes.Items.Clear();
            foreach (var chmode in mCipherModes)
                this.comboBoxCipherModes.Items.Add(chmode.Name.ToString());
            this.comboBoxCipherModes.SelectedValue = "CFB";
            comboBoxCipherModes.SelectedIndexChanged += new System.EventHandler(async (sender, e) => await comboCipherMode_Changed(sender, e));

            this.comboBoxAlgo.Items.Clear();
            foreach (string cipher in GetCipherEnums())
                this.comboBoxAlgo.Items.Add(cipher.ToString());

            this.comboBoxEncoding.Items.Clear();
            foreach (EncodingType encodingType in EncodingTypesExtensions.GetEncodingTypes())
                this.comboBoxEncoding.Items.Add(encodingType.ToString());
            

            this.Load += new System.EventHandler(async (sender, e)
                => await EncryptFormMultiControls_LoadAsync(sender, e));
        }


        /// <summary>
        /// EncryptFormMultiControls_LoadAsync - form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal async Task EncryptFormMultiControls_LoadAsync(object sender, EventArgs e)
        {
            this.labelInfoMessage.Visible = false;
            this.textBoxKey.Text = GetEmailFromRegistry();
            comboBoxEncoding.SelectedItem = EncodingType.Base64.ToString();
            radioButtonListHash.SelectedItem = KeyHash.Hex.ToString();
            
            await groupBoxFiles.pictureBoxRunningPipe.SetImageTagVisibleAsync(Resources.BlankEncrypt_640x108, "", true);
            await SetInfoMessageAsync($"{this.Name} started...", ToolTipIcon.Info, 2000);

            Hash_Click(sender, e);
        }

        #endregion ctor and load

        #region MenuCompressionEncodingZipHash

        protected internal async Task menuCompression_Click(object sender, EventArgs e) => await SetCompressionAsync((ToolStripMenuItem)sender, null);

        protected internal async Task ComboBoxCompression_SelectedIndexChanged(object sender, EventArgs e) => await SetCompressionAsync(null, comboBoxCompression.SelectedItem);

        /// <summary>
        /// SetCompression – sets compression type from menu or combobox
        /// </summary>
        /// <param name="mi">selected compression ToolStripMenuItem</param>
        /// <param name="comboItem">selected compression combobox item</param>
        protected internal async Task SetCompressionAsync(ToolStripMenuItem? mi = null, object? comboItem = null)
        {
            ZipType zipType = (mi != null) ? ZipTypeExtensions.GetZipType(mi.Name ?? "None") :
                (comboItem != null && !string.IsNullOrEmpty(comboItem.ToString())) ? ZipTypeExtensions.GetZipType(comboItem.ToString() ?? "None") :
                    ZipType.None;

            if (mi != null && mi.Checked && comboItem == null)
            {
                comboBoxCompression.SelectedItem = zipType.ToString();
                return;
            }

            foreach (var menuZ in menuZips)
                menuZ.Checked = false;

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

                    case ZipType.GZip: zmenuGZip.Checked = true; break;
                    case ZipType.Zip: zmenuZip.Checked = true; break;
                    case ZipType.Z7:
                    case ZipType.BZip2: zmenuBZip2.Checked = true; break;
                    case ZipType.None:
                    default:
                        zmenuNone.Checked = true;
                        comboBoxCompression.SelectedItem = ZipType.None.ToString();
                        break;
                }
            }
            if (cPipe != null)
            {
                cPipe.ZType = zipType;
                await groupBoxFiles.pictureBoxRunningPipe.SetImageTagVisibleAsync(cPipe?.GenerateEncryptPipeImage());
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

            foreach (var menuEncods in menuEncodings)
                menuEncods.Checked = false;

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
                var menuItemChecked = encodingType.CheckMenuItemForEncoding(menuEncodings);
                switch (encodingType)
                {
                    case EncodingType.Base16: menuEncBase16.Checked = true; break;
                    case EncodingType.Hex16: menuEncHex16.Checked = true; break;
                    case EncodingType.Base32: menuEncBase32.Checked = true; break;
                    case EncodingType.Hex32: menuEncHex32.Checked = true; break;
                    case EncodingType.Uu: menuEncUu.Checked = true; break;
                    case EncodingType.Xx: menuEncXx.Checked = true; break;
                    case EncodingType.Hex64: menuEncHex64.Checked = true; break;
                    case EncodingType.None: menuEncNone.Checked = true; break;
                    case EncodingType.Base64:
                    default: menuEncBase64.Checked = true; break;
                }
            }
            if (cPipe != null)
            {
                cPipe.EncodeType = encodingType;
                await groupBoxFiles.pictureBoxRunningPipe.SetImageTagVisibleAsync(cPipe?.GenerateEncryptPipeImage());
            }
            await SetInfoMessageAsync($"Encoding {encodingType.ToString()} set.", ToolTipIcon.Info, 1000);
        }

        /// <summary>
        /// GetEncoding - gets selected encoding type
        /// </summary>
        /// <returns></returns>
        protected internal EncodingType GetEncoding()
        {
            EncodingType encType = EncodingTypesExtensions.GetEncodíngTypeFromCheckMenuItem(menuEncodings);
            if (menuEncNone.Checked) return EncodingType.None;
            if (menuEncBase16.Checked) return EncodingType.Base16;
            if (menuEncHex16.Checked) return EncodingType.Hex16;
            if (menuEncBase32.Checked) return EncodingType.Base32;
            if (menuEncHex32.Checked) return EncodingType.Hex32;
            if (menuEncHex64.Checked) return EncodingType.Hex64;
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

            foreach (var menuHashe in mHashes)
                menuHashe.Checked = false;


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
                    case KeyHash.Sha384: menuHashSha384.Checked = true; break;
                    case KeyHash.Sha512: menuHashSha512.Checked = true; break;
                    case KeyHash.Whirlpool: menuHashWhirlpool.Checked = true; break;
                    case KeyHash.Oct: menuHashSha384.Checked = true; break;
                    case KeyHash.Blake2xs: menuHashBlake2xs.Checked = true; break;
                    case KeyHash.CShake: menuHashCShake.Checked = true; break;
                    case KeyHash.Dstu7564: menuHashDstu7564.Checked = true; break;
                    case KeyHash.RipeMD256: menuHashRipeMD256.Checked = true; break;
                    case KeyHash.TupleHash: menuHashTupleHash.Checked = true; break;
                    case KeyHash.Hex: menuHashHex.Checked = true; break;
                    default:
                        Area23Log.LogOriginMsg("EncryptForm Hash", $"RadioButtonList: {radioButtonList.SelectedItem.ToString()} => KeyHash = {aKeyHash.ToString()}.");
                        menuHashHex.Checked = true;
                        break;
                }
            }

            Hash_Click(this, new EventArgs());
            if (cPipe != null)
            {
                cPipe.KHash = aKeyHash;
                await groupBoxFiles.pictureBoxRunningPipe.SetImageTagVisibleAsync(cPipe?.GenerateEncryptPipeImage());
            }
            await SetInfoMessageAsync($"{GetHash().ToString()} hashed.", ToolTipIcon.Info, 1000);
        }

        /// <summary>
        /// GetHash - gets selected hash type
        /// </summary>
        /// <returns></returns>
        protected internal KeyHash GetHash()
        {
            if (menuHashBCrypt.Checked) return KeyHash.BCrypt;
            if (menuHashBlake2xs.Checked) return KeyHash.Blake2xs;
            if (menuHashHex.Checked) return KeyHash.Hex;
            if (menuHashMD5.Checked) return KeyHash.MD5;
            if (menuHashOpenBSDCrypt.Checked) return KeyHash.OpenBSDCrypt;
            if (menuHashSCrypt.Checked) return KeyHash.SCrypt;
            if (menuHashSha1.Checked) return KeyHash.Sha1;
            if (menuHashSha256.Checked) return KeyHash.Sha256;
            if (menuHashSha384.Checked) return KeyHash.Sha384;            
            if (menuHashSha512.Checked) return KeyHash.Sha512;
            if (menuHashWhirlpool.Checked) return KeyHash.Whirlpool;            
            if (menuHashCShake.Checked) return KeyHash.CShake;
            if (menuHashDstu7564.Checked) return KeyHash.Dstu7564;
            if (menuHashRipeMD256.Checked) return KeyHash.RipeMD256;
            if (menuHashTupleHash.Checked) return KeyHash.TupleHash;

            menuHashHex.Checked = true;
            return KeyHash.Hex;
        }

        

        protected internal async Task menuCipherMode_Click(object sender, EventArgs e)
        {
            foreach (var cipherModeItem in mCipherModes)
                cipherModeItem.Checked = false;

            if (sender is ToolStripMenuItem mi && mi.Name != null &&
                (mi.Name.StartsWith("menuCipherModeItem") || mi.Name.StartsWith("menuMode")))
            {
                mi.Checked = true;
                string cipherModeString = mi.Name.Replace("menuCipherModeItem", "").Replace("menuMode", "");
                int ix = 0;
                for (ix = 0; ix < comboBoxCipherModes.Items.Count; ix++)
                {
                    if (comboBoxCipherModes.Items[ix].ToString() == cipherModeString)
                        break;                        
                }
                comboBoxCipherModes.SelectedIndex = ix;
                CipherMode2 cmode2 = CipherModeExtensions.ParseText(cipherModeString);
                CipherMode cmode = cmode2.ToCipherMode();                
                if (cPipe != null)
                {
                    cPipe.CMode2 = cmode2;
                    await groupBoxFiles.pictureBoxRunningPipe.SetImageTagVisibleAsync(cPipe?.GenerateEncryptPipeImage());
                }
                await SetInfoMessageAsync($"CipherMode {cmode2.ToString()} set.", ToolTipIcon.Info, 2000);
            }
        }


        protected internal async Task comboCipherMode_Changed(object sender, EventArgs e)
        {
            switch (comboBoxCipherModes.SelectedText)
            {
                case "CBC": await menuCipherMode_Click(menuCipherModeItemCBC, e); break;
                case "ECB": await menuCipherMode_Click(menuCipherModeItemECB, e); break;
                case "CFB":
                default: await menuCipherMode_Click(menuCipherModeItemCFB, e); break;
            }
        }


        public CipherMode2 GetCipherMode2()
        {
            foreach (var cipherModeItem in mCipherModes)
            {
                if (cipherModeItem.Checked)
                {
                    string cipherModeString = cipherModeItem.Name.Replace("menuCipherModeItem", "");
                    CipherMode2 cmode2 = CipherModeExtensions.ParseText(cipherModeString);
                    return cmode2;
                }
            }

            menuCipherModeItemCFB.Checked = true;
            return CipherMode2.CFB;
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
            CipherEnum[] cipherAlgos = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
            if (!string.IsNullOrEmpty(comboBoxAlgo.SelectedItem.ToString()) && Enum.TryParse<CipherEnum>(comboBoxAlgo.SelectedItem.ToString(), out CipherEnum cipherEnum))
            {
                if (cipherAlgos.Length < 8)
                {
                    switch (cipherEnum)
                    {
                        case CipherEnum.BlowFish:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.blowfish, "", true);
                            break;
                        case CipherEnum.Fish2:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.TwoFish, "", true);
                            break;
                        case CipherEnum.Fish3:
                        //case CipherEnum.ThreeFish256:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.ThreeFish, "", true);
                            break;
                        case CipherEnum.Serpent:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.Serpent, "", true);
                            break;
                        case CipherEnum.XTea:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.XTea, "", true);
                            break;
                        case CipherEnum.Tea:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.Tea, "", true);
                            break;
                        case CipherEnum.Des:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.Des, "", true);
                            break;
                        case CipherEnum.Des3:
                        case CipherEnum.Des3Net:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.TripleDes, "", true);
                            break;
                        case CipherEnum.RC2:
                        case CipherEnum.RC532:
                        case CipherEnum.RC564:
                        case CipherEnum.RC6:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.RC, "", true);                            
                           break;
                        case CipherEnum.Camellia:
                        case CipherEnum.CamelliaLight:
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.Camellia, "", true);
                            break;
                        default:
                            break;
                    }
                    this.textBoxPipe.Text += cipherEnum.ToString() + ";";
                    cipherAlgos = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
                    cPipe = new CipherPipe(cipherAlgos, 8, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());
                    SetPictureBoxImage(groupBoxFiles.pictureBoxRunningPipe, cPipe.GenerateEncryptPipeImage(), "", true);
                    System.Timers.Timer setInfoMessageTimer = new System.Timers.Timer { Interval = 3000 };
                    setInfoMessageTimer.Elapsed += (s, en) =>
                    {
                        Task.Run(new System.Action(() =>
                        {
                            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, Properties.Resources.file, "", true);
                        }));
                        setInfoMessageTimer.Stop(); // Stop the timer(otherwise keeps on calling)
                    };
                    setInfoMessageTimer.Start();                    
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
            if (cPipe != null)
            {
                cPipe.InPipe = Array.Empty<CipherEnum>();

                SetPictureBoxImage(groupBoxFiles.pictureBoxRunningPipe, cPipe.GenerateEncryptPipeImage(), "", true);
            }
            else
                SetPictureBoxImage(groupBoxFiles.pictureBoxRunningPipe, Properties.Resources.BlankEncrypt_640x108, "", true);
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

            cPipe = new CipherPipe(this.textBoxHash.Text, this.textBoxKey.Text, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());
            foreach (CipherEnum cipher in cPipe.InPipe)
            {
                this.textBoxPipe.Text += cipher.ToString() + ";";
            }
            SetPictureBoxImage(groupBoxFiles.pictureBoxRunningPipe, cPipe.GenerateEncryptPipeImage());
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

            cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());
            foreach (CipherEnum cipher in cPipe.InPipe)
            {
                this.textBoxPipe.Text += cipher.ToString() + ";";
            }
            SetPictureBoxImage(groupBoxFiles.pictureBoxRunningPipe, cPipe.GenerateEncryptPipeImage());
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
                this.tabControlWithHexSrc.AsciiText = fortunes[rIdx];
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
            this.tabControlWithHexSrc.EncoderType = EncodingType.None;
            this.tabControlWithHexSrc.AsciiText = string.Empty;
            this.tabControlWithHexDest.EncoderType = EncodingType.None;
            this.tabControlWithHexDest.AsciiText = string.Empty;
            cPipe = null;
            await this.groupBoxFiles.ResetPictureBoxFilesAsync(sender, e);

            await this.SetEncodingAsync(menuEncBase64);
            await this.SetCompressionAsync(null, "None");
            await this.SetHashAsync(menuHashHex, radioButtonListHash);
            await this.statusLabelSource.SetTextAsync("");
            await this.statusLabelDestination.SetTextAsync("");
            await this.statusLabelMsg.SetTextAsync("");
            await SetInfoMessageAsync("reset", ToolTipIcon.Info, 6000);

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
            cPipe = new CipherPipe(pipeAlgos, 8, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());

            await groupBoxFiles.pictureBoxRunningPipe.SetImageTagVisibleAsync(cPipe.GenerateEncryptPipeImage());

            DateTime start = DateTime.Now;
            if (!string.IsNullOrEmpty(this.tabControlWithHexSrc.AsciiText))
            {
                this.tabControlWithHexDest.EncoderType = EncodingType.None;
                this.tabControlWithHexDest.AsciiText = "";
                Cursor.Current = new Cursor(iconSandClock.Handle);
                await SetInfoMessageAsync("Starting encryption plain text", ToolTipIcon.Info, -1);
                try
                {
                    await this.statusLabelSource.SetTextAsync($"source chars: {tabControlWithHexSrc.AsciiText.Length}");
                    if (menuEncNone.Checked && (pipeAlgos.Length > 0 || GetZip() != ZipType.None))
                        await SetEncodingAsync(menuEncBase64);

                    string encrypted = cPipe.EncrpytTextGoRounds(this.tabControlWithHexSrc.AsciiText, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());
                    this.tabControlWithHexDest.EncoderType = GetEncoding();
                    this.tabControlWithHexDest.AsciiText = encrypted;
                    await this.statusLabelDestination.SetTextAsync($"destination chars: {this.tabControlWithHexDest.AsciiText.Length}");
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
            if (!string.IsNullOrEmpty(groupBoxFiles.labelFileIn.Text) && !groupBoxFiles.labelFileIn.Text.StartsWith("["))
            {
                string fileName = FileMatches();
                if (string.IsNullOrEmpty(fileName))
                {
                    if (string.IsNullOrEmpty(this.tabControlWithHexSrc.AsciiText))
                    {
                        await SetInfoMessageAsync("No file found to encrypt", ToolTipIcon.Warning, 6000);
                        await this.statusLabelSource.SetTextAsync("No file found to encrypt");
                    }
                    return;
                }

                if (this.warnOnDoubleZippingToolStripMenuItem.Checked && Path.GetExtension(fileName).IsCompressedFile() && GetZip() != ZipType.None)
                {
                    DialogResult dresult = MessageBox.Show(this, "Zip an already compressed file twice?", "Double zip warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dresult == DialogResult.No)
                        return;
                }

                await SetInfoMessageAsync("Starting encryption for file " + groupBoxFiles.labelFileIn.Text, ToolTipIcon.Info, -1);

                Cursor.Current = new Cursor(iconSandClock.Handle);
                try
                {
                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(fileName);

                    byte[] encodedBytes = cPipe.EncryptEncodeBytes(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());
                    string miniPipe = string.IsNullOrEmpty(cPipe.PipeString) ? "" : "." + cPipe.PipeString;
                    string outFilePath = (fileName + GetHash().GetExtension() + GetZip().GetZipTypeExtension() + miniPipe + GetEncoding().GetEnCodingExtension());

                    Cursor.Current = new Cursor(iconSandClock.Handle);
                    await this.statusLabelMsg.SetTextAsync("encryption time: " + DateTime.Now.Subtract(start).ToString());
                    await SetInfoMessageAsync("Starting verificaton", ToolTipIcon.Info, -1);

                    bool saved = (menuFileSettingsItemAutomaticallySaveToTemp.Checked) ?
                                SaveBytesNoDialog(encodedBytes, ref outFilePath) :
                                SaveBytesDialog(encodedBytes, ref outFilePath);
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
                            await this.PlaySoundFromResourcesAsync("sound_hammer");
                            await groupBoxFiles.pictureBoxFileOut.SetImageTagVisibleAsync(Properties.Resources.file_encrypted_broken, "{" + outFilePath + "}", true);
                        }
                        else
                        {
                            await SetInfoMessageAsync("Encryption verified", ToolTipIcon.Info, -1);
                            // await this.PlaySoundFromResourcesAsync("sound_laser");
                            await groupBoxFiles.pictureBoxFileOut.SetImageTagVisibleAsync(outFilePath.GetImageThumbnailFromFile(), "{" + outFilePath + "}", true);
                        }

                        await groupBoxFiles.labelOutputFile.SetTextVisibleAsync(outFileName, true);


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

            await this.statusLabelMsg.SetTextAsync("total time: " + DateTime.Now.Subtract(start).ToString());

        }

        public string FileMatches()
        {
            foreach (string file in HashFiles)
            {
                if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file) &&
                    groupBoxFiles.labelFileIn != null && Path.GetFileName(file) == groupBoxFiles.labelFileIn.Text &&
                        groupBoxFiles.pictureBoxFileIn.Tag != null && groupBoxFiles.pictureBoxFileIn.Tag.ToString() == file)
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

            Icon iconSandClock = new Icon(Properties.Resources.icon_sandclock, new Size(60, 60));

            CipherEnum[] pipeAlgos = CipherEnumExtensions.ParsePipeText(this.textBoxPipe.Text);
            cPipe = new CipherPipe(pipeAlgos, 8, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());
            // SetPictureBoxImage(groupBoxFiles.pictureBoxRunningPipe, cPipe.GenerateDecryptPipeImage());
            await this.groupBoxFiles.pictureBoxRunningPipe.SetImageTagVisibleAsync(cPipe.GenerateDecryptPipeImage());

            if (!string.IsNullOrEmpty(this.tabControlWithHexSrc.AsciiText))
            {
                this.tabControlWithHexDest.EncoderType = EncodingType.None;
                this.tabControlWithHexDest.AsciiText = "";
                Cursor.Current = new Cursor(iconSandClock.Handle);
                await SetInfoMessageAsync("Starting decryption of cipher text", ToolTipIcon.Info, -1);

                try
                {
                    await this.statusLabelSource.SetTextAsync($"source chars: {tabControlWithHexSrc.AsciiText.Length}");
                    if (menuEncNone.Checked && (pipeAlgos.Length > 0 || GetZip() != ZipType.None))
                        await SetEncodingAsync(menuEncBase64);

                    string decrypted = cPipe.DecryptTextRoundsGo(this.tabControlWithHexSrc.AsciiText, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());
                    this.tabControlWithHexDest.EncoderType = EncodingType.None;
                    this.tabControlWithHexDest.AsciiText = decrypted;

                    await SetInfoMessageAsync("Decryption finished", ToolTipIcon.Info, 6000);
                    await this.statusLabelDestination.SetTextAsync($"destination chars: {this.tabControlWithHexDest.AsciiText.Length}");
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
            if (!string.IsNullOrEmpty(groupBoxFiles.labelFileIn.Text) && !groupBoxFiles.labelFileIn.Text.StartsWith("["))
            {
                string fileName = FileMatches();
                if (string.IsNullOrEmpty(fileName))
                {
                    await SetInfoMessageAsync("No file found to decrypt", ToolTipIcon.Warning, 6000);
                    await this.statusLabelSource.SetTextAsync("No file found to decrypt");
                    return;
                }

                Cursor.Current = new Cursor(iconSandClock.Handle);
                await SetInfoMessageAsync("Starting decryption file " + groupBoxFiles.labelFileIn.Text, ToolTipIcon.Info, -1);

                try
                {
                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(fileName);
                    byte[] outBytes = cPipe.DecodeDecrpytBytes(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash(), GetCipherMode2());

                    string outFileDecrypt = fileName.StripCiphersInFileName();

                    bool saved = (menuFileSettingsItemAutomaticallySaveToTemp.Checked) ?
                                SaveBytesNoDialog(outBytes, ref outFileDecrypt) :
                                SaveBytesDialog(outBytes, ref outFileDecrypt);
                    if (saved)
                    {
                        HashFiles.Add(outFileDecrypt);
                        await groupBoxFiles.pictureBoxFileOut.SetImageTagVisibleAsync(outFileDecrypt.GetImageThumbnailFromFile(), outFileDecrypt, true);
                        await groupBoxFiles.labelOutputFile.SetTextVisibleAsync(Path.GetFileName(outFileDecrypt), true);
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

            await this.statusLabelMsg.SetTextAsync("Time: " + DateTime.Now.Subtract(start).ToString());
        }

        #endregion EncryptDecrypt_Click        

        //TODO: DragNDrop moved to GroupBoxFiles
        #region DragNDrop

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

        #endregion DragNDrop

        #region file loading and saving ops

        public virtual void GroupBoxFilesAdded(object sender, Area23EventArgs<string> fileToAddArgs)
        {
            string fileToAdd = "";
            if (fileToAddArgs != null && ((fileToAdd = fileToAddArgs.GenericTData.ToString()) != null))
            {
                FileAddedAction(fileToAdd);
            }
        }

        public virtual void GroupBoxFileRequired(object sender, EventArgs e)
        {
            menuFileOpen_Click(sender, e);
        }

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
                this.tabControlWithHexSrc.EncoderType = EncodingType.None;
                this.tabControlWithHexSrc.AsciiText = string.Empty;
                this.tabControlWithHexDest.EncoderType = EncodingType.None;
                this.tabControlWithHexDest.AsciiText = string.Empty;

                SetGBoxText(this.groupBoxFiles, "Files Group Box");

                groupBoxFiles.pictureBoxFileIn.Image = fileName.GetImageThumbnailFromFile();
                groupBoxFiles.pictureBoxFileIn.Tag = fileName;
                groupBoxFiles.labelFileIn.Text = Path.GetFileName(fileName);

                _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
                isDragMode = false;

                Task.Run(() => IPlayable.PlaySoundFromResource("sound_arrow"));

                HashFiles = new HashSet<string>();
                HashFiles.Add(fileName);

                if (fi.Length > 1048576)
                    SetStatusLabelText(this.statusLabelSource, $"FileSize: {(fi.Length / 1048576)} MB");
                else if (fi.Length > 2048)
                    SetStatusLabelText(this.statusLabelSource, $"FileSize: {(fi.Length / 1024)} kb");
                else SetStatusLabelText(this.statusLabelSource, $"FileSize: {fi.Length} bytes");

                if (menuItemCreatePipeSettingsFromFileName.Checked)
                {
                    cPipe = GetCPipeFromFileName(fileName);
                    if (cPipe != null)
                    {
                        foreach (var miHash in mHashes)
                        {
                            if (miHash.Name.Replace("menuHash", "").Equals(cPipe.KHash.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                                miHash.Text.Equals(cPipe.KHash.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                SetHashAsync((ToolStripMenuItem)miHash, radioButtonListHash).ConfigureAwait(false);

                                break;
                            }
                        }
                        foreach (var miEnc in menuEncodings)
                        {
                            if (miEnc.Name.Replace("menuEnc", "").Equals(cPipe.EncodeType.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                                miEnc.Text.Equals(cPipe.EncodeType.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                SetEncodingAsync((ToolStripMenuItem)miEnc, null).ConfigureAwait(true);
                                break;
                            }
                        }
                        foreach (var miZip in menuZips)
                        {
                            if (miZip.Name.Replace("zmenu", "").Equals(cPipe.ZType.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                               miZip.Text.Equals(cPipe.ZType.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                SetCompressionAsync((ToolStripMenuItem)miZip, null).ConfigureAwait(true);
                                break;
                            }
                        }

                        this.textBoxPipe.Text = "";
                        foreach (CipherEnum cipher in cPipe.InPipe)
                        {
                            this.textBoxPipe.Text += cipher.ToString() + ";";
                        }
                        SetPictureBoxImage(groupBoxFiles.pictureBoxRunningPipe, cPipe.GenerateEncryptPipeImage());
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
        /// Shows OneTwoThreeFish Demo form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns><see cref="T:Task"</returns>
        protected internal virtual async Task menuMainItemOneTwoThreeFish_Click(object sender, EventArgs e)
        {
            OneTwoThreeFish oneTwoThreeFish = new OneTwoThreeFish();
            this.Hide();
            Program.formComplex.Hide();
            DialogResult result = await oneTwoThreeFish.ShowDialogAsync();
            if (result == DialogResult.Cancel || result == DialogResult.No || result == DialogResult.Abort ||
                result == DialogResult.Ignore)
            {
                try { oneTwoThreeFish.Close(); } catch { }
            }
        }

        internal void menuMainFormSimple_Click(object sender, EventArgs e)
        {
            if (Program.formSimple == null || Program.formSimple.Disposing)
                Program.formSimple = new EncryptFormSimple();
            try
            {
                Program.formSimple.Show();
            } 
            catch (Exception exShow)
            {
                Program.formSimple = new EncryptFormSimple();
                Program.formSimple.Show();
            }
            this.Hide();
            Program.formComplex.Hide();
            Program.formSimple.Focus();
        }

        /// <summary>
        /// menuFileOpen_Click opens a file dialog to select a file to encrypt/decrypt
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        internal void menuFileOpen_Click(object sender, EventArgs e)
        {
            // this.groupBoxFiles.pictureBoxRunningPipe.Image = Resources.BlankEncrypt_640x108;
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
        /// SaveBytesNoDialog saves byte array to file in temporary directory
        /// </summary>
        /// <param name="fileBytes">byte array to save</param>
        /// <param name="outFilePath">ref will be returned; calculated outFilePath</param>
        /// <returns>true if saved, false if not saved</returns>
        internal bool SaveBytesNoDialog(byte[] fileBytes, ref string outFilePath)
        {
            bool written = false;
            int writeCnt = 0;
            outFilePath = outFilePath ?? string.Empty;

            if (fileBytes != null && fileBytes.Length > 0)
            {
                while (!written && writeCnt < 5)
                {
                    switch (writeCnt++)
                    {
                        case 0:
                            outFilePath = Path.Combine(Area23Log.TempDir, Path.GetFileName(outFilePath));
                            break;
                        case 1:
                            outFilePath = Path.Combine(Area23Log.TempDir, "D" + Path.GetFileName(outFilePath));
                            break;
                        default:
                            RandomName rname = new RandomName();
                            outFilePath = Path.Combine(Area23Log.TempDir, rname.GetNewString(), Path.GetExtension(outFilePath));
                            break;
                    }

                    try
                    {
                        File.WriteAllBytes(outFilePath, fileBytes);
                        written = true;
                    }
                    catch (Exception ex)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in SaveBytesDialog for file: \"{outFilePath}\".\n", ex);
                    }
                }
                if (!written && writeCnt > 2)
                    return false;
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
            return false;
        }

        /// <summary>
        /// menuMainSave_Click - saves a file
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        internal void menuMainSave_Click(object sender, EventArgs e)
        {
            // this.pictureBoxRunningPipe.Visible = false;
            if (groupBoxFiles.pictureBoxFileOut.Visible || groupBoxFiles.labelOutputFile.Visible)
            {
                byte[] fileBytes = new byte[0];
                string fileName = "";

                foreach (string filePath in HashFiles)
                {
                    if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                    {
                        if (Path.GetFileName(filePath) == groupBoxFiles.labelOutputFile.Text)
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
                    groupBoxFiles.pictureBoxFileOut.Visible = false;
                    groupBoxFiles.labelOutputFile.Visible = false;
                }


            }
        }

        #endregion OpenSave    

        #region Media Methods

        protected internal void SetInfoMessage(string message, ToolTipIcon toolIcon = ToolTipIcon.Info, int duration = 4000)
        {
            SetLabelTextVisible(this.labelInfoMessage, message, true);
            SetStatusLabelText(this.statusLabelMsg, message);
            string toolHeader = toolIcon.ToString();
            switch (toolIcon)
            {
                case ToolTipIcon.Error:
                    toolHeader = "Error";
                    SetLabelBackColor(labelInfoMessage, ColorTranslator.FromHtml("#bab510"));
                    IPlayable.PlaySoundFromResource("sound_error");
                    break;
                case ToolTipIcon.Warning:
                    SetLabelBackColor(labelInfoMessage, Color.LightYellow);
                    toolHeader = "Warning";
                    IPlayable.PlaySoundFromResource("sound_warning");
                    break;
                case ToolTipIcon.Info:
                default:
                    SetLabelBackColor(labelInfoMessage, SystemColors.Info);
                    toolHeader = "Info";
                    IPlayable.PlaySoundFromResource("sound_info");
                    break;
            }

            if (duration > 0)
            {
                System.Timers.Timer setInfoMessageTimer = new System.Timers.Timer { Interval = duration };
                setInfoMessageTimer.Elapsed += (s, en) =>
                {
                    Task.Run(new System.Action(() =>
                    {
                        SetLabelBackColor(labelInfoMessage, SystemColors.Info);
                        SetLabelTextVisible(labelInfoMessage, "", false);
                    }));
                    setInfoMessageTimer.Stop(); // Stop the timer(otherwise keeps on calling)
                };
                setInfoMessageTimer.Start();
            }
        }

        protected internal async Task SetInfoMessageAsync(string message, ToolTipIcon toolIcon = ToolTipIcon.Info, int duration = 4000)
        {
            await labelInfoMessage.SetTextVisibleAsync(message);
            string toolHeader = toolIcon.ToString();
            switch (toolIcon)
            {
                case ToolTipIcon.Error:
                    toolHeader = "Error";
                    await labelInfoMessage.SetBackColorAsync(ColorTranslator.FromHtml("#bab510"));
                    await this.PlaySoundFromResourcesAsync("sound_error");
                    break;
                case ToolTipIcon.Warning:
                    await labelInfoMessage.SetBackColorAsync(Color.LightYellow);
                    toolHeader = "Warning";
                    await this.PlaySoundFromResourcesAsync("sound_warning");
                    break;
                case ToolTipIcon.Info:
                default:
                    await labelInfoMessage.SetBackColorAsync(SystemColors.Info);
                    toolHeader = "Info";
                    if ((++Program.ProgramCount % 23) == 17)
                        await this.PlaySoundFromResourcesAsync("sound_killer_state");
                    else
                        await this.PlaySoundFromResourcesAsync("sound_info");
                    break;
            }

            if (duration > 0)
            {
                System.Timers.Timer setInfoMessageTimer = new System.Timers.Timer { Interval = duration };
                setInfoMessageTimer.Elapsed += (s, en) =>
                {
                    Task.Run(new System.Action(async () =>
                    {
                        await labelInfoMessage.SetBackColorAsync(SystemColors.Info);
                        await labelInfoMessage.SetTextVisibleAsync("", false);
                    }));
                    setInfoMessageTimer.Stop(); // Stop the timer(otherwise keeps on calling)
                };
                setInfoMessageTimer.Start();
            }

        }

        protected internal void LoadImage_Click(object sender, EventArgs e)
        {
            Util.RandomImage rimg = new Util.RandomImage();
            Image picImg = rimg.SaveFileName.GetImageThumbnailFromFile();
            SetPictureBoxImage(groupBoxFiles.pictureBoxFileIn, picImg, rimg.SaveFileName, true);
            SetLabelTextVisible(groupBoxFiles.labelFileIn, Path.GetFileName(rimg.SaveFileName), true);
            HashFiles.Add(rimg.SaveFileName);
        }

        #endregion Media Methods


        
    }

}
