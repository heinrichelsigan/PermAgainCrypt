using System.Windows.Forms;

namespace EU.CqrXs.Gui.Forms
{

    partial class OneTwoThreeFish
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneTwoThreeFish));
            menuStripEncrypt = new MenuStrip();
            toolMenuMain = new ToolStripMenuItem();
            menuFileOpen = new ToolStripMenuItem();
            menuMainSave = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            menuMainSetPipe = new ToolStripMenuItem();
            menuMainHashKey = new ToolStripMenuItem();
            menuMainHashPipe = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            menuMainEncrypt = new ToolStripMenuItem();
            menuMainDecrypt = new ToolStripMenuItem();
            menuMainDownloadImage = new ToolStripMenuItem();
            menuMainRandomText = new ToolStripMenuItem();
            menuMainReset = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuFileExit = new ToolStripMenuItem();
            menuCompression = new ToolStripMenuItem();
            zmenu7z = new ToolStripMenuItem();
            zmenuBZip2 = new ToolStripMenuItem();
            zmenuGZip = new ToolStripMenuItem();
            zmenuZip = new ToolStripMenuItem();
            zmenuNone = new ToolStripMenuItem();
            menuEncoding = new ToolStripMenuItem();
            menuEncNone = new ToolStripMenuItem();
            menuEncBase16 = new ToolStripMenuItem();
            menuEncHex16 = new ToolStripMenuItem();
            menuEncBase32 = new ToolStripMenuItem();
            menuEncHex32 = new ToolStripMenuItem();
            menuEncBase64 = new ToolStripMenuItem();
            menuEncUu = new ToolStripMenuItem();
            menuEncXx = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            menuOptionsItemsWarnings = new ToolStripMenuItem();
            warnOnEmptyPipeToolStripMenuItem = new ToolStripMenuItem();
            warnOnDoubleZippingToolStripMenuItem = new ToolStripMenuItem();
            verifyEncryptionToolStripMenuItem = new ToolStripMenuItem();
            sha512ToolStripMenuItem = new ToolStripMenuItem();
            bytesOfFileToolStripMenuItem = new ToolStripMenuItem();
            menuOptionsMenuFileSettings = new ToolStripMenuItem();
            menuItemCreatePipeSettingsFromFileName = new ToolStripMenuItem();
            menuFileSettingsItemAutomaticallySaveToTemp = new ToolStripMenuItem();
            menuSerialize = new ToolStripMenuItem();
            menuJson = new ToolStripMenuItem();
            menuXml = new ToolStripMenuItem();
            menuRaw = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            menuHelpHelp = new ToolStripMenuItem();
            menuHelpUrlFetch = new ToolStripMenuItem();
            menuOptionsMenuWindowsCharHexDecOctBin = new ToolStripMenuItem();
            menuOptionsMenuWindowsitemAbout = new ToolStripMenuItem();
            enumOptionsBindingSource = new BindingSource(components);
            textBoxKey = new TextBox();
            pictureBoxKey = new PictureBox();
            pictureBoxHash = new PictureBox();
            textBoxHash3 = new TextBox();
            buttonSetPipeline = new Button();
            buttonReset = new Button();
            textBoxPipe = new TextBox();
            buttonEncrypt = new Button();
            buttonDecrypt = new Button();
            pictureBoxDelete = new PictureBox();
            comboBoxCompression = new ComboBox();
            comboBoxEncoding = new ComboBox();
            buttonRandomText = new Button();
            buttonHashPipe = new Button();
            labelInfoMessage = new Label();
            statusStrip = new StatusStrip();
            statusLabelSource = new ToolStripStatusLabel();
            statusLabelMsg = new ToolStripStatusLabel();
            statusLabelDestination = new ToolStripStatusLabel();
            groupBoxFiles = new EU.CqrXs.Gui.Controls.GroupBoxFiles();
            panelPipe = new Panel();
            panel1 = new Panel();
            tabControlWithHexSrc = new EU.CqrXs.Gui.Controls.TabControlWithHex();
            tabControlWithHexDest = new EU.CqrXs.Gui.Controls.TabControlWithHex();
            pictureBox1 = new PictureBox();
            textBoxHash1 = new TextBox();
            textBoxHash2 = new TextBox();
            pictureBox2 = new PictureBox();
            textBoxHashHash1 = new TextBox();
            textBoxHashHash2 = new TextBox();
            textBoxHashHash3 = new TextBox();
            menuStripEncrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHash).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDelete).BeginInit();
            statusStrip.SuspendLayout();
            panelPipe.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // menuStripEncrypt
            // 
            menuStripEncrypt.AllowMerge = false;
            menuStripEncrypt.BackColor = SystemColors.MenuBar;
            menuStripEncrypt.Font = new Font("Lucida Sans Typewriter", 9F);
            menuStripEncrypt.Items.AddRange(new ToolStripItem[] { toolMenuMain, menuCompression, menuEncoding, optionsToolStripMenuItem, menuSerialize, menuHelp });
            menuStripEncrypt.Location = new Point(0, 0);
            menuStripEncrypt.Name = "menuStripEncrypt";
            menuStripEncrypt.Padding = new Padding(3, 2, 2, 2);
            menuStripEncrypt.Size = new Size(1008, 24);
            menuStripEncrypt.TabIndex = 0;
            menuStripEncrypt.Text = "menuStripEncrypt";
            // 
            // toolMenuMain
            // 
            toolMenuMain.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuMainSave, toolStripSeparator2, menuMainSetPipe, menuMainHashKey, menuMainHashPipe, toolStripSeparator3, menuMainEncrypt, menuMainDecrypt, menuMainDownloadImage, menuMainRandomText, menuMainReset, toolStripSeparator1, menuFileExit });
            toolMenuMain.Font = new Font("Lucida Sans Typewriter", 10F);
            toolMenuMain.Name = "toolMenuMain";
            toolMenuMain.Size = new Size(51, 20);
            toolMenuMain.Text = "Main";
            // 
            // menuFileOpen
            // 
            menuFileOpen.BackColor = SystemColors.Menu;
            menuFileOpen.Name = "menuFileOpen";
            menuFileOpen.ShortcutKeys = Keys.Control | Keys.O;
            menuFileOpen.Size = new Size(180, 22);
            menuFileOpen.Text = "Open";
            menuFileOpen.Click += menuFileOpen_Click;
            // 
            // menuMainSave
            // 
            menuMainSave.BackColor = SystemColors.Menu;
            menuMainSave.Name = "menuMainSave";
            menuMainSave.ShortcutKeys = Keys.Control | Keys.S;
            menuMainSave.Size = new Size(180, 22);
            menuMainSave.Text = "Save";
            menuMainSave.Click += menuMainSave_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            // 
            // menuMainSetPipe
            // 
            menuMainSetPipe.BackColor = SystemColors.Menu;
            menuMainSetPipe.Name = "menuMainSetPipe";
            menuMainSetPipe.Size = new Size(180, 22);
            menuMainSetPipe.Text = "Set Pipe";
            menuMainSetPipe.Click += SetPipeline_Click;
            // 
            // menuMainHashKey
            // 
            menuMainHashKey.BackColor = SystemColors.Menu;
            menuMainHashKey.Name = "menuMainHashKey";
            menuMainHashKey.Size = new Size(180, 22);
            menuMainHashKey.Text = "Hash Key";
            menuMainHashKey.Click += Hash_Click;
            // 
            // menuMainHashPipe
            // 
            menuMainHashPipe.BackColor = SystemColors.Menu;
            menuMainHashPipe.Name = "menuMainHashPipe";
            menuMainHashPipe.Size = new Size(180, 22);
            menuMainHashPipe.Text = "Hash Pipe";
            menuMainHashPipe.Click += Hash_Pipe_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(177, 6);
            // 
            // menuMainEncrypt
            // 
            menuMainEncrypt.BackColor = SystemColors.Menu;
            menuMainEncrypt.Name = "menuMainEncrypt";
            menuMainEncrypt.Size = new Size(180, 22);
            menuMainEncrypt.Text = "Encrypt";
            // 
            // menuMainDecrypt
            // 
            menuMainDecrypt.BackColor = SystemColors.Menu;
            menuMainDecrypt.Name = "menuMainDecrypt";
            menuMainDecrypt.Size = new Size(180, 22);
            menuMainDecrypt.Text = "Decrypt";
            // 
            // menuMainDownloadImage
            // 
            menuMainDownloadImage.BackColor = SystemColors.Menu;
            menuMainDownloadImage.Name = "menuMainDownloadImage";
            menuMainDownloadImage.Size = new Size(180, 22);
            menuMainDownloadImage.Text = "Ramdom Image";
            menuMainDownloadImage.Click += LoadImage_Click;
            // 
            // menuMainRandomText
            // 
            menuMainRandomText.BackColor = SystemColors.Menu;
            menuMainRandomText.Name = "menuMainRandomText";
            menuMainRandomText.Size = new Size(180, 22);
            menuMainRandomText.Text = "Random Text";
            menuMainRandomText.Click += RandomText_Click;
            // 
            // menuMainReset
            // 
            menuMainReset.BackColor = SystemColors.Menu;
            menuMainReset.Name = "menuMainReset";
            menuMainReset.Size = new Size(180, 22);
            menuMainReset.Text = "Reset";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // menuFileExit
            // 
            menuFileExit.BackColor = SystemColors.Menu;
            menuFileExit.Name = "menuFileExit";
            menuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            menuFileExit.Size = new Size(180, 22);
            menuFileExit.Text = "Exit";
            menuFileExit.Click += menuFileExit_Click;
            // 
            // menuCompression
            // 
            menuCompression.DropDownItems.AddRange(new ToolStripItem[] { zmenu7z, zmenuBZip2, zmenuGZip, zmenuZip, zmenuNone });
            menuCompression.Font = new Font("Lucida Sans Typewriter", 10F);
            menuCompression.Name = "menuCompression";
            menuCompression.Size = new Size(107, 20);
            menuCompression.Text = "Compression";
            // 
            // zmenu7z
            // 
            zmenu7z.Enabled = false;
            zmenu7z.Name = "zmenu7z";
            zmenu7z.ShortcutKeys = Keys.Control | Keys.D7;
            zmenu7z.Size = new Size(169, 22);
            zmenu7z.Text = "7z";
            // 
            // zmenuBZip2
            // 
            zmenuBZip2.BackColor = SystemColors.Menu;
            zmenuBZip2.Enabled = false;
            zmenuBZip2.Name = "zmenuBZip2";
            zmenuBZip2.ShortcutKeys = Keys.Control | Keys.B;
            zmenuBZip2.Size = new Size(169, 22);
            zmenuBZip2.Text = "BZip2";
            // 
            // zmenuGZip
            // 
            zmenuGZip.BackColor = SystemColors.Menu;
            zmenuGZip.Name = "zmenuGZip";
            zmenuGZip.ShortcutKeys = Keys.Control | Keys.G;
            zmenuGZip.Size = new Size(169, 22);
            zmenuGZip.Text = "GZip";
            // 
            // zmenuZip
            // 
            zmenuZip.BackColor = SystemColors.Menu;
            zmenuZip.Name = "zmenuZip";
            zmenuZip.ShortcutKeys = Keys.Control | Keys.Z;
            zmenuZip.Size = new Size(169, 22);
            zmenuZip.Text = "Zip";
            // 
            // zmenuNone
            // 
            zmenuNone.BackColor = SystemColors.Menu;
            zmenuNone.Checked = true;
            zmenuNone.CheckState = CheckState.Checked;
            zmenuNone.Name = "zmenuNone";
            zmenuNone.ShortcutKeys = Keys.Control | Keys.N;
            zmenuNone.Size = new Size(169, 22);
            zmenuNone.Text = "None";
            // 
            // menuEncoding
            // 
            menuEncoding.DropDownItems.AddRange(new ToolStripItem[] { menuEncNone, menuEncBase16, menuEncHex16, menuEncBase32, menuEncHex32, menuEncBase64, menuEncUu, menuEncXx });
            menuEncoding.Font = new Font("Lucida Sans Typewriter", 10F);
            menuEncoding.Name = "menuEncoding";
            menuEncoding.ShortcutKeys = Keys.Alt | Keys.E;
            menuEncoding.Size = new Size(83, 20);
            menuEncoding.Text = "Encoding";
            // 
            // menuEncNone
            // 
            menuEncNone.BackColor = SystemColors.Menu;
            menuEncNone.Name = "menuEncNone";
            menuEncNone.Size = new Size(122, 22);
            menuEncNone.Text = "None";
            menuEncNone.ToolTipText = "no encoding, let it be binary as it is";
            // 
            // menuEncBase16
            // 
            menuEncBase16.BackColor = SystemColors.Menu;
            menuEncBase16.Name = "menuEncBase16";
            menuEncBase16.Size = new Size(122, 22);
            menuEncBase16.Text = "Base16";
            menuEncBase16.ToolTipText = "base16 en-/decoding";
            // 
            // menuEncHex16
            // 
            menuEncHex16.BackColor = SystemColors.Menu;
            menuEncHex16.Name = "menuEncHex16";
            menuEncHex16.Size = new Size(122, 22);
            menuEncHex16.Text = "Hex16";
            menuEncHex16.ToolTipText = "hexadecimal half byte encoding";
            // 
            // menuEncBase32
            // 
            menuEncBase32.BackColor = SystemColors.Menu;
            menuEncBase32.Name = "menuEncBase32";
            menuEncBase32.Size = new Size(122, 22);
            menuEncBase32.Text = "Base32";
            menuEncBase32.ToolTipText = "base32 en-/decoding";
            // 
            // menuEncHex32
            // 
            menuEncHex32.BackColor = SystemColors.Menu;
            menuEncHex32.Name = "menuEncHex32";
            menuEncHex32.Size = new Size(122, 22);
            menuEncHex32.Text = "Hex32";
            // 
            // menuEncBase64
            // 
            menuEncBase64.BackColor = SystemColors.Menu;
            menuEncBase64.Checked = true;
            menuEncBase64.CheckState = CheckState.Checked;
            menuEncBase64.Name = "menuEncBase64";
            menuEncBase64.Size = new Size(122, 22);
            menuEncBase64.Text = "Base64";
            menuEncBase64.ToolTipText = "base64 mime en-/decoding";
            // 
            // menuEncUu
            // 
            menuEncUu.BackColor = SystemColors.Menu;
            menuEncUu.Name = "menuEncUu";
            menuEncUu.Size = new Size(122, 22);
            menuEncUu.Text = "Uu";
            menuEncUu.ToolTipText = "unix 2 unix en-/decoding, see uuencode, uudecode";
            // 
            // menuEncXx
            // 
            menuEncXx.Name = "menuEncXx";
            menuEncXx.Size = new Size(122, 22);
            menuEncXx.Text = "Xx";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuOptionsItemsWarnings, verifyEncryptionToolStripMenuItem, menuOptionsMenuFileSettings });
            optionsToolStripMenuItem.Font = new Font("Lucida Sans Typewriter", 10F);
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(75, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // menuOptionsItemsWarnings
            // 
            menuOptionsItemsWarnings.BackColor = SystemColors.ControlLight;
            menuOptionsItemsWarnings.DropDownItems.AddRange(new ToolStripItem[] { warnOnEmptyPipeToolStripMenuItem, warnOnDoubleZippingToolStripMenuItem });
            menuOptionsItemsWarnings.Name = "menuOptionsItemsWarnings";
            menuOptionsItemsWarnings.Size = new Size(210, 22);
            menuOptionsItemsWarnings.Text = "Warnings";
            // 
            // warnOnEmptyPipeToolStripMenuItem
            // 
            warnOnEmptyPipeToolStripMenuItem.BackColor = SystemColors.ControlLight;
            warnOnEmptyPipeToolStripMenuItem.CheckOnClick = true;
            warnOnEmptyPipeToolStripMenuItem.Name = "warnOnEmptyPipeToolStripMenuItem";
            warnOnEmptyPipeToolStripMenuItem.Size = new Size(250, 22);
            warnOnEmptyPipeToolStripMenuItem.Text = "Warn on empty pipe";
            warnOnEmptyPipeToolStripMenuItem.ToolTipText = "Warn on en-/decrypting when cipher pipe is empty";
            // 
            // warnOnDoubleZippingToolStripMenuItem
            // 
            warnOnDoubleZippingToolStripMenuItem.BackColor = SystemColors.ControlLight;
            warnOnDoubleZippingToolStripMenuItem.Checked = true;
            warnOnDoubleZippingToolStripMenuItem.CheckOnClick = true;
            warnOnDoubleZippingToolStripMenuItem.CheckState = CheckState.Checked;
            warnOnDoubleZippingToolStripMenuItem.Name = "warnOnDoubleZippingToolStripMenuItem";
            warnOnDoubleZippingToolStripMenuItem.Size = new Size(250, 22);
            warnOnDoubleZippingToolStripMenuItem.Text = "Warn on double zipping";
            warnOnDoubleZippingToolStripMenuItem.ToolTipText = "Warn, when zipping an already zipped or strong compressed file";
            // 
            // verifyEncryptionToolStripMenuItem
            // 
            verifyEncryptionToolStripMenuItem.BackColor = SystemColors.ControlLight;
            verifyEncryptionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { sha512ToolStripMenuItem, bytesOfFileToolStripMenuItem });
            verifyEncryptionToolStripMenuItem.Name = "verifyEncryptionToolStripMenuItem";
            verifyEncryptionToolStripMenuItem.Size = new Size(210, 22);
            verifyEncryptionToolStripMenuItem.Text = "Verify Encryption";
            // 
            // sha512ToolStripMenuItem
            // 
            sha512ToolStripMenuItem.BackColor = SystemColors.ControlLight;
            sha512ToolStripMenuItem.Checked = true;
            sha512ToolStripMenuItem.CheckOnClick = true;
            sha512ToolStripMenuItem.CheckState = CheckState.Checked;
            sha512ToolStripMenuItem.Name = "sha512ToolStripMenuItem";
            sha512ToolStripMenuItem.Size = new Size(210, 22);
            sha512ToolStripMenuItem.Text = "sha512 hash";
            // 
            // bytesOfFileToolStripMenuItem
            // 
            bytesOfFileToolStripMenuItem.BackColor = SystemColors.ControlLight;
            bytesOfFileToolStripMenuItem.CheckOnClick = true;
            bytesOfFileToolStripMenuItem.Name = "bytesOfFileToolStripMenuItem";
            bytesOfFileToolStripMenuItem.Size = new Size(210, 22);
            bytesOfFileToolStripMenuItem.Text = "1/8 bytes of file";
            // 
            // menuOptionsMenuFileSettings
            // 
            menuOptionsMenuFileSettings.BackColor = SystemColors.ControlLight;
            menuOptionsMenuFileSettings.DropDownItems.AddRange(new ToolStripItem[] { menuItemCreatePipeSettingsFromFileName, menuFileSettingsItemAutomaticallySaveToTemp });
            menuOptionsMenuFileSettings.Name = "menuOptionsMenuFileSettings";
            menuOptionsMenuFileSettings.Size = new Size(210, 22);
            menuOptionsMenuFileSettings.Text = "File Settings";
            // 
            // menuItemCreatePipeSettingsFromFileName
            // 
            menuItemCreatePipeSettingsFromFileName.BackColor = SystemColors.ControlLight;
            menuItemCreatePipeSettingsFromFileName.CheckOnClick = true;
            menuItemCreatePipeSettingsFromFileName.Name = "menuItemCreatePipeSettingsFromFileName";
            menuItemCreatePipeSettingsFromFileName.Size = new Size(346, 22);
            menuItemCreatePipeSettingsFromFileName.Text = "Create Pipe Settings from FileName";
            menuItemCreatePipeSettingsFromFileName.ToolTipText = "Creates Cipher Pipe, hash, encode and zip settings in Form from opened  fileName";
            // 
            // menuFileSettingsItemAutomaticallySaveToTemp
            // 
            menuFileSettingsItemAutomaticallySaveToTemp.BackColor = SystemColors.ControlLight;
            menuFileSettingsItemAutomaticallySaveToTemp.Checked = true;
            menuFileSettingsItemAutomaticallySaveToTemp.CheckOnClick = true;
            menuFileSettingsItemAutomaticallySaveToTemp.CheckState = CheckState.Checked;
            menuFileSettingsItemAutomaticallySaveToTemp.Name = "menuFileSettingsItemAutomaticallySaveToTemp";
            menuFileSettingsItemAutomaticallySaveToTemp.Size = new Size(346, 22);
            menuFileSettingsItemAutomaticallySaveToTemp.Text = "Automatically Save to Temp";
            menuFileSettingsItemAutomaticallySaveToTemp.ToolTipText = "Don't show a save file dialog, when processimg files";
            // 
            // menuSerialize
            // 
            menuSerialize.DropDownItems.AddRange(new ToolStripItem[] { menuJson, menuXml, menuRaw, toolStripMenuItem1 });
            menuSerialize.Enabled = false;
            menuSerialize.Font = new Font("Lucida Sans Typewriter", 10F);
            menuSerialize.Name = "menuSerialize";
            menuSerialize.ShortcutKeys = Keys.Alt | Keys.S;
            menuSerialize.Size = new Size(91, 20);
            menuSerialize.Text = "Serialize";
            // 
            // menuJson
            // 
            menuJson.BackColor = SystemColors.Menu;
            menuJson.Enabled = false;
            menuJson.Name = "menuJson";
            menuJson.ShortcutKeys = Keys.Control | Keys.J;
            menuJson.Size = new Size(180, 22);
            menuJson.Text = "Json";
            // 
            // menuXml
            // 
            menuXml.BackColor = SystemColors.Menu;
            menuXml.Enabled = false;
            menuXml.Name = "menuXml";
            menuXml.ShortcutKeys = Keys.Control | Keys.X;
            menuXml.Size = new Size(180, 22);
            menuXml.Text = "Xml";
            // 
            // menuRaw
            // 
            menuRaw.BackColor = SystemColors.Menu;
            menuRaw.Enabled = false;
            menuRaw.Name = "menuRaw";
            menuRaw.ShortcutKeys = Keys.Control | Keys.R;
            menuRaw.Size = new Size(180, 22);
            menuRaw.Text = "Raw";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(180, 22);
            toolStripMenuItem1.Text = "?";
            // 
            // menuHelp
            // 
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout, menuHelpHelp, menuHelpUrlFetch });
            menuHelp.Font = new Font("Lucida Sans Typewriter", 10F);
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(27, 20);
            menuHelp.Text = "?";
            // 
            // menuAbout
            // 
            menuAbout.BackColor = SystemColors.MenuBar;
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(180, 22);
            menuAbout.Text = "About";
            // 
            // menuHelpHelp
            // 
            menuHelpHelp.BackColor = SystemColors.MenuBar;
            menuHelpHelp.Name = "menuHelpHelp";
            menuHelpHelp.ShortcutKeys = Keys.Alt | Keys.F3;
            menuHelpHelp.Size = new Size(180, 22);
            menuHelpHelp.Text = "Help";
            // 
            // menuHelpUrlFetch
            // 
            menuHelpUrlFetch.BackColor = SystemColors.MenuBar;
            menuHelpUrlFetch.Name = "menuHelpUrlFetch";
            menuHelpUrlFetch.Size = new Size(180, 22);
            menuHelpUrlFetch.Text = "Url Fetch";
            menuHelpUrlFetch.Click += menuHelpUrlFetch_Click;
            // 
            // menuOptionsMenuWindowsCharHexDecOctBin
            // 
            menuOptionsMenuWindowsCharHexDecOctBin.Name = "menuOptionsMenuWindowsCharHexDecOctBin";
            menuOptionsMenuWindowsCharHexDecOctBin.Size = new Size(32, 19);
            // 
            // menuOptionsMenuWindowsitemAbout
            // 
            menuOptionsMenuWindowsitemAbout.Name = "menuOptionsMenuWindowsitemAbout";
            menuOptionsMenuWindowsitemAbout.Size = new Size(32, 19);
            // 
            // textBoxKey
            // 
            textBoxKey.BackColor = SystemColors.ControlLightLight;
            textBoxKey.Font = new Font("Lucida Sans Typewriter", 10F);
            textBoxKey.Location = new Point(48, 26);
            textBoxKey.Margin = new Padding(1);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new Size(688, 23);
            textBoxKey.TabIndex = 4;
            textBoxKey.Text = "ftp@ftp.cdrom.com";
            textBoxKey.TextChanged += textBoxKey_TextChanged;
            // 
            // pictureBoxKey
            // 
            pictureBoxKey.BackColor = SystemColors.Control;
            pictureBoxKey.Image = Properties.Resources.key_ring;
            pictureBoxKey.Location = new Point(8, 23);
            pictureBoxKey.Margin = new Padding(1);
            pictureBoxKey.Name = "pictureBoxKey";
            pictureBoxKey.Size = new Size(30, 30);
            pictureBoxKey.TabIndex = 3;
            pictureBoxKey.TabStop = false;
            pictureBoxKey.Click += pictureBoxKey_Click;
            // 
            // pictureBoxHash
            // 
            pictureBoxHash.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxHash.BackColor = SystemColors.Control;
            pictureBoxHash.BackgroundImage = Properties.Resources.ThreeFish;
            pictureBoxHash.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxHash.Location = new Point(6, 124);
            pictureBoxHash.Margin = new Padding(1);
            pictureBoxHash.Name = "pictureBoxHash";
            pictureBoxHash.Size = new Size(36, 36);
            pictureBoxHash.TabIndex = 8;
            pictureBoxHash.TabStop = false;
            pictureBoxHash.Click += Hash_Click;
            // 
            // textBoxHash3
            // 
            textBoxHash3.BackColor = SystemColors.Control;
            textBoxHash3.Font = new Font("Lucida Sans Typewriter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxHash3.Location = new Point(48, 130);
            textBoxHash3.Margin = new Padding(1);
            textBoxHash3.Name = "textBoxHash3";
            textBoxHash3.ReadOnly = true;
            textBoxHash3.Size = new Size(468, 22);
            textBoxHash3.TabIndex = 9;
            // 
            // buttonSetPipeline
            // 
            buttonSetPipeline.BackColor = SystemColors.Control;
            buttonSetPipeline.Font = new Font("Lucida Sans Typewriter", 10F);
            buttonSetPipeline.Location = new Point(751, 24);
            buttonSetPipeline.Margin = new Padding(1);
            buttonSetPipeline.Name = "buttonSetPipeline";
            buttonSetPipeline.Size = new Size(120, 27);
            buttonSetPipeline.TabIndex = 5;
            buttonSetPipeline.Text = "Set Pipeline";
            buttonSetPipeline.UseVisualStyleBackColor = false;
            buttonSetPipeline.Click += SetPipeline_Click;
            // 
            // buttonReset
            // 
            buttonReset.BackColor = SystemColors.Control;
            buttonReset.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonReset.Location = new Point(874, 4);
            buttonReset.Margin = new Padding(1);
            buttonReset.Name = "buttonReset";
            buttonReset.Size = new Size(120, 27);
            buttonReset.TabIndex = 25;
            buttonReset.Text = "Reset Form";
            buttonReset.UseVisualStyleBackColor = false;
            // 
            // textBoxPipe
            // 
            textBoxPipe.BackColor = SystemColors.ControlLightLight;
            textBoxPipe.Font = new Font("Lucida Sans Typewriter", 10F);
            textBoxPipe.Location = new Point(132, 5);
            textBoxPipe.Margin = new Padding(1);
            textBoxPipe.MaxLength = 8192;
            textBoxPipe.Name = "textBoxPipe";
            textBoxPipe.ReadOnly = true;
            textBoxPipe.Size = new Size(700, 23);
            textBoxPipe.TabIndex = 14;
            // 
            // buttonEncrypt
            // 
            buttonEncrypt.BackColor = SystemColors.Control;
            buttonEncrypt.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonEncrypt.Location = new Point(4, 4);
            buttonEncrypt.Margin = new Padding(1);
            buttonEncrypt.Name = "buttonEncrypt";
            buttonEncrypt.Size = new Size(120, 27);
            buttonEncrypt.TabIndex = 21;
            buttonEncrypt.Text = "Encrypt";
            buttonEncrypt.UseVisualStyleBackColor = false;
            // 
            // buttonDecrypt
            // 
            buttonDecrypt.BackColor = SystemColors.Control;
            buttonDecrypt.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonDecrypt.Location = new Point(135, 4);
            buttonDecrypt.Margin = new Padding(1);
            buttonDecrypt.Name = "buttonDecrypt";
            buttonDecrypt.Size = new Size(120, 27);
            buttonDecrypt.TabIndex = 22;
            buttonDecrypt.Text = "Decrypt";
            buttonDecrypt.UseVisualStyleBackColor = false;
            // 
            // pictureBoxDelete
            // 
            pictureBoxDelete.BackColor = SystemColors.Control;
            pictureBoxDelete.Image = Properties.Resources.image_delete;
            pictureBoxDelete.Location = new Point(837, 2);
            pictureBoxDelete.Margin = new Padding(1);
            pictureBoxDelete.Name = "pictureBoxDelete";
            pictureBoxDelete.Size = new Size(27, 27);
            pictureBoxDelete.TabIndex = 15;
            pictureBoxDelete.TabStop = false;
            pictureBoxDelete.Click += pictureBoxDelete_Click;
            // 
            // comboBoxCompression
            // 
            comboBoxCompression.BackColor = SystemColors.Control;
            comboBoxCompression.Font = new Font("Lucida Sans Typewriter", 10F);
            comboBoxCompression.FormattingEnabled = true;
            comboBoxCompression.Items.AddRange(new object[] { "None", "GZip", "Zip" });
            comboBoxCompression.Location = new Point(6, 4);
            comboBoxCompression.Margin = new Padding(1);
            comboBoxCompression.MaxDropDownItems = 32;
            comboBoxCompression.Name = "comboBoxCompression";
            comboBoxCompression.Size = new Size(118, 23);
            comboBoxCompression.TabIndex = 11;
            // 
            // comboBoxEncoding
            // 
            comboBoxEncoding.BackColor = SystemColors.Control;
            comboBoxEncoding.DropDownWidth = 144;
            comboBoxEncoding.Font = new Font("Lucida Sans Typewriter", 10F);
            comboBoxEncoding.FormattingEnabled = true;
            comboBoxEncoding.Items.AddRange(new object[] { "None", "Base16", "Hex16", "Base32", "Hex32", "Base64", "Uu", "Xx" });
            comboBoxEncoding.Location = new Point(868, 4);
            comboBoxEncoding.Margin = new Padding(1);
            comboBoxEncoding.MaxDropDownItems = 32;
            comboBoxEncoding.Name = "comboBoxEncoding";
            comboBoxEncoding.Size = new Size(126, 23);
            comboBoxEncoding.TabIndex = 16;
            // 
            // buttonRandomText
            // 
            buttonRandomText.BackColor = SystemColors.Control;
            buttonRandomText.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonRandomText.Location = new Point(372, 4);
            buttonRandomText.Margin = new Padding(1);
            buttonRandomText.Name = "buttonRandomText";
            buttonRandomText.Size = new Size(120, 27);
            buttonRandomText.TabIndex = 23;
            buttonRandomText.Text = "Random Text";
            buttonRandomText.UseVisualStyleBackColor = false;
            buttonRandomText.Click += RandomText_Click;
            // 
            // buttonHashPipe
            // 
            buttonHashPipe.BackColor = SystemColors.Control;
            buttonHashPipe.Font = new Font("Lucida Sans Typewriter", 10F);
            buttonHashPipe.Location = new Point(876, 24);
            buttonHashPipe.Margin = new Padding(1);
            buttonHashPipe.Name = "buttonHashPipe";
            buttonHashPipe.Size = new Size(120, 27);
            buttonHashPipe.TabIndex = 6;
            buttonHashPipe.Text = "Hash Pipe";
            buttonHashPipe.UseVisualStyleBackColor = false;
            buttonHashPipe.Click += Hash_Pipe_Click;
            // 
            // labelInfoMessage
            // 
            labelInfoMessage.BackColor = SystemColors.Info;
            labelInfoMessage.Font = new Font("Lucida Fax", 9.25F);
            labelInfoMessage.ForeColor = SystemColors.InfoText;
            labelInfoMessage.Location = new Point(516, 4);
            labelInfoMessage.Margin = new Padding(1);
            labelInfoMessage.Name = "labelInfoMessage";
            labelInfoMessage.Size = new Size(343, 27);
            labelInfoMessage.TabIndex = 24;
            labelInfoMessage.Text = "Info Message";
            labelInfoMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // statusStrip
            // 
            statusStrip.Font = new Font("Lucida Sans Typewriter", 9F);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabelSource, statusLabelMsg, statusLabelDestination });
            statusStrip.Location = new Point(0, 707);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1008, 22);
            statusStrip.TabIndex = 44;
            statusStrip.Text = "statusStrip";
            // 
            // statusLabelSource
            // 
            statusLabelSource.AutoSize = false;
            statusLabelSource.Font = new Font("Lucida Sans Typewriter", 9F);
            statusLabelSource.Name = "statusLabelSource";
            statusLabelSource.Size = new Size(216, 17);
            statusLabelSource.Text = "statusLabelSource";
            // 
            // statusLabelMsg
            // 
            statusLabelMsg.AutoSize = false;
            statusLabelMsg.Font = new Font("Lucida Sans Typewriter", 9F);
            statusLabelMsg.Name = "statusLabelMsg";
            statusLabelMsg.Size = new Size(520, 17);
            statusLabelMsg.Text = "statusLabelMsg";
            // 
            // statusLabelDestination
            // 
            statusLabelDestination.AutoSize = false;
            statusLabelDestination.Font = new Font("Lucida Sans Typewriter", 9F);
            statusLabelDestination.Name = "statusLabelDestination";
            statusLabelDestination.Size = new Size(216, 17);
            statusLabelDestination.Text = "statusLabelDestination";
            // 
            // groupBoxFiles
            // 
            groupBoxFiles.AllowDrop = true;
            groupBoxFiles.BackColor = SystemColors.Control;
            groupBoxFiles.Font = new Font("Lucida Sans Typewriter", 8F);
            groupBoxFiles.Location = new Point(4, 198);
            groupBoxFiles.Margin = new Padding(1);
            groupBoxFiles.Name = "groupBoxFiles";
            groupBoxFiles.Padding = new Padding(1);
            groupBoxFiles.Size = new Size(996, 156);
            groupBoxFiles.TabIndex = 18;
            groupBoxFiles.TabStop = false;
            groupBoxFiles.Text = "groupBoxFiles";
            // 
            // panelPipe
            // 
            panelPipe.BackColor = SystemColors.GradientActiveCaption;
            panelPipe.BorderStyle = BorderStyle.Fixed3D;
            panelPipe.Controls.Add(textBoxPipe);
            panelPipe.Controls.Add(pictureBoxDelete);
            panelPipe.Controls.Add(comboBoxCompression);
            panelPipe.Controls.Add(comboBoxEncoding);
            panelPipe.Location = new Point(0, 160);
            panelPipe.Margin = new Padding(2);
            panelPipe.Name = "panelPipe";
            panelPipe.Padding = new Padding(1);
            panelPipe.Size = new Size(1008, 36);
            panelPipe.TabIndex = 10;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(buttonDecrypt);
            panel1.Controls.Add(buttonReset);
            panel1.Controls.Add(buttonEncrypt);
            panel1.Controls.Add(buttonRandomText);
            panel1.Controls.Add(labelInfoMessage);
            panel1.Location = new Point(0, 356);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(1);
            panel1.Size = new Size(1008, 39);
            panel1.TabIndex = 20;
            // 
            // tabControlWithHexSrc
            // 
            tabControlWithHexSrc.Font = new Font("Lucida Sans Typewriter", 9F);
            tabControlWithHexSrc.ItemSize = new Size(72, 19);
            tabControlWithHexSrc.Location = new Point(4, 400);
            tabControlWithHexSrc.Margin = new Padding(1);
            tabControlWithHexSrc.Name = "tabControlWithHexSrc";
            tabControlWithHexSrc.Padding = new Point(1, 1);
            tabControlWithHexSrc.SelectedIndex = 0;
            tabControlWithHexSrc.Size = new Size(490, 306);
            tabControlWithHexSrc.TabIndex = 40;
            // 
            // tabControlWithHexDest
            // 
            tabControlWithHexDest.Font = new Font("Lucida Sans Typewriter", 9F);
            tabControlWithHexDest.ItemSize = new Size(72, 19);
            tabControlWithHexDest.Location = new Point(518, 400);
            tabControlWithHexDest.Margin = new Padding(1);
            tabControlWithHexDest.Name = "tabControlWithHexDest";
            tabControlWithHexDest.Padding = new Point(1, 1);
            tabControlWithHexDest.SelectedIndex = 0;
            tabControlWithHexDest.Size = new Size(478, 306);
            tabControlWithHexDest.TabIndex = 46;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = SystemColors.Control;
            pictureBox1.BackgroundImage = Properties.Resources.blowfish;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(6, 53);
            pictureBox1.Margin = new Padding(1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(36, 36);
            pictureBox1.TabIndex = 47;
            pictureBox1.TabStop = false;
            // 
            // textBoxHash1
            // 
            textBoxHash1.BackColor = SystemColors.Control;
            textBoxHash1.Font = new Font("Lucida Sans Typewriter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxHash1.Location = new Point(48, 60);
            textBoxHash1.Margin = new Padding(1);
            textBoxHash1.Name = "textBoxHash1";
            textBoxHash1.ReadOnly = true;
            textBoxHash1.Size = new Size(468, 22);
            textBoxHash1.TabIndex = 48;
            // 
            // textBoxHash2
            // 
            textBoxHash2.BackColor = SystemColors.Control;
            textBoxHash2.Font = new Font("Lucida Sans Typewriter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxHash2.Location = new Point(48, 96);
            textBoxHash2.Margin = new Padding(1);
            textBoxHash2.Name = "textBoxHash2";
            textBoxHash2.ReadOnly = true;
            textBoxHash2.Size = new Size(468, 22);
            textBoxHash2.TabIndex = 50;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox2.BackColor = SystemColors.Control;
            pictureBox2.BackgroundImage = Properties.Resources.TwoFish;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(6, 90);
            pictureBox2.Margin = new Padding(1);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(36, 36);
            pictureBox2.TabIndex = 49;
            pictureBox2.TabStop = false;
            // 
            // textBoxHashHash1
            // 
            textBoxHashHash1.BackColor = SystemColors.Control;
            textBoxHashHash1.Font = new Font("Lucida Sans Typewriter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxHashHash1.Location = new Point(518, 60);
            textBoxHashHash1.Margin = new Padding(1);
            textBoxHashHash1.Name = "textBoxHashHash1";
            textBoxHashHash1.ReadOnly = true;
            textBoxHashHash1.Size = new Size(478, 22);
            textBoxHashHash1.TabIndex = 51;
            // 
            // textBoxHashHash2
            // 
            textBoxHashHash2.BackColor = SystemColors.Control;
            textBoxHashHash2.Font = new Font("Lucida Sans Typewriter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxHashHash2.Location = new Point(518, 96);
            textBoxHashHash2.Margin = new Padding(1);
            textBoxHashHash2.Name = "textBoxHashHash2";
            textBoxHashHash2.ReadOnly = true;
            textBoxHashHash2.Size = new Size(478, 22);
            textBoxHashHash2.TabIndex = 52;
            // 
            // textBoxHashHash3
            // 
            textBoxHashHash3.BackColor = SystemColors.Control;
            textBoxHashHash3.Font = new Font("Lucida Sans Typewriter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxHashHash3.Location = new Point(518, 130);
            textBoxHashHash3.Margin = new Padding(1);
            textBoxHashHash3.Name = "textBoxHashHash3";
            textBoxHashHash3.ReadOnly = true;
            textBoxHashHash3.Size = new Size(478, 22);
            textBoxHashHash3.TabIndex = 53;
            // 
            // OneTwoThreeFish
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1008, 729);
            Controls.Add(textBoxHashHash3);
            Controls.Add(textBoxHashHash2);
            Controls.Add(textBoxHashHash1);
            Controls.Add(textBoxHash2);
            Controls.Add(pictureBox2);
            Controls.Add(textBoxHash1);
            Controls.Add(pictureBox1);
            Controls.Add(tabControlWithHexDest);
            Controls.Add(tabControlWithHexSrc);
            Controls.Add(panel1);
            Controls.Add(panelPipe);
            Controls.Add(groupBoxFiles);
            Controls.Add(statusStrip);
            Controls.Add(buttonHashPipe);
            Controls.Add(buttonSetPipeline);
            Controls.Add(textBoxHash3);
            Controls.Add(pictureBoxHash);
            Controls.Add(pictureBoxKey);
            Controls.Add(textBoxKey);
            Controls.Add(menuStripEncrypt);
            Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStripEncrypt;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MaximumSize = new Size(1024, 768);
            MinimizeBox = false;
            Name = "OneTwoThreeFish";
            Opacity = 0.96D;
            Text = "OneTwoThreeFish";
            FormClosed += menuFileExit_Close;
            Load += OneTwoThreeFish_Load;
            menuStripEncrypt.ResumeLayout(false);
            menuStripEncrypt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHash).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDelete).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            panelPipe.ResumeLayout(false);
            panelPipe.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private BindingSource enumOptionsBindingSource;
        protected internal PictureBox pictureBoxKey;
        protected internal PictureBox pictureBoxHash;
        protected internal TextBox textBoxHash3;
        protected internal Button buttonSetPipeline;
        protected internal ComboBox comboBoxCompression;
        protected internal Button buttonHashPipe;
        protected internal MenuStrip menuStripEncrypt;
        protected internal ToolStripMenuItem toolMenuMain;
        protected internal ToolStripMenuItem menuFileOpen;
        protected internal ToolStripMenuItem menuMainDecrypt;
        protected internal ToolStripSeparator toolStripSeparator1;
        protected internal ToolStripMenuItem menuFileExit;
        protected internal ToolStripMenuItem menuCompression;
        internal  ToolStripMenuItem zmenuBZip2;
        internal  ToolStripMenuItem zmenuZip;
        internal  ToolStripMenuItem zmenu7z;
        internal  ToolStripMenuItem zmenuGZip;
        internal  ToolStripMenuItem zmenuNone;
        internal  ToolStripMenuItem menuEncoding;
        internal  ToolStripMenuItem menuEncBase16;
        internal  ToolStripMenuItem menuEncHex16;
        internal  ToolStripMenuItem menuEncBase32;
        internal  ToolStripMenuItem menuEncHex32;
        protected internal Button buttonReset;
        protected internal TextBox textBoxPipe;
        protected internal ToolStripMenuItem menuMainSave;
        protected internal ToolStripSeparator toolStripSeparator2;
        protected internal ToolStripMenuItem menuMainEncrypt;
        protected internal ToolStripMenuItem menuMainReset;
        protected internal ToolStripMenuItem menuMainRandomText;
        protected internal ToolStripMenuItem menuMainHashKey;
        protected internal ToolStripMenuItem menuMainSetPipe;
        protected internal ToolStripSeparator toolStripSeparator3;
        protected internal Button buttonEncrypt;
        protected internal Button buttonDecrypt;
        internal  ToolStripMenuItem menuEncNone;
        protected internal PictureBox pictureBoxDelete;
        protected internal ComboBox comboBoxEncoding;
        protected internal Button buttonRandomText;
        internal  ToolStripMenuItem menuMainHashPipe;
        internal  ToolStripMenuItem menuEncBase64;
        internal  ToolStripMenuItem menuEncUu;
        internal  ToolStripMenuItem menuEncXx;
        internal  ToolStripMenuItem menuSerialize;
        internal  ToolStripMenuItem menuJson;
        internal  ToolStripMenuItem menuXml;
        internal  ToolStripMenuItem menuRaw;
        internal  ToolStripMenuItem menuHelp;
        internal  ToolStripMenuItem menuAbout;
        internal  ToolStripMenuItem menuHelpHelp;
        private Label labelInfoMessage;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabelSource;
        private ToolStripStatusLabel statusLabelMsg;
        private ToolStripStatusLabel statusLabelDestination;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem verifyEncryptionToolStripMenuItem;
        private ToolStripMenuItem sha512ToolStripMenuItem;
        private ToolStripMenuItem bytesOfFileToolStripMenuItem;
        internal TextBox textBoxKey;
        private ToolStripMenuItem menuOptionsItemsWarnings;
        private ToolStripMenuItem warnOnEmptyPipeToolStripMenuItem;
        private ToolStripMenuItem warnOnDoubleZippingToolStripMenuItem;
        private ToolStripMenuItem menuOptionsMenuFileSettings;
        private ToolStripMenuItem menuItemCreatePipeSettingsFromFileName;
        private ToolStripMenuItem menuFileSettingsItemAutomaticallySaveToTemp;
        private Controls.GroupBoxFiles groupBoxFiles;
        private Panel panelPipe;
        private Panel panel1;
        internal ToolStripMenuItem menuHelpCharHexDecOctBin;
        private Controls.TabControlWithHex tabControlWithHexSrc;
        private Controls.TabControlWithHex tabControlWithHexDest;
        internal ToolStripMenuItem menuOptionsMenuWindowsCharHexDecOctBin;
        internal ToolStripMenuItem menuOptionsMenuWindowsitemAbout;
        protected internal ToolStripMenuItem menuMainDownloadImage;
        internal ToolStripMenuItem menuHelpUrlFetch;
        private ToolStripMenuItem toolStripMenuItem1;
        protected internal PictureBox pictureBox1;
        protected internal TextBox textBoxHash1;
        protected internal TextBox textBoxHash2;
        protected internal PictureBox pictureBox2;
        protected internal TextBox textBoxHashHash1;
        protected internal TextBox textBoxHashHash2;
        protected internal TextBox textBoxHashHash3;
    }


}