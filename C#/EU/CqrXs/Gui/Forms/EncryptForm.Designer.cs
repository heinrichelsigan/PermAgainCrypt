using System.Windows.Forms;

namespace EU.CqrXs.Gui.Forms
{

    partial class EncryptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EncryptForm));
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
            menuHash = new ToolStripMenuItem();
            menuHashOct = new ToolStripMenuItem();
            menuHashBlake2xs = new ToolStripMenuItem();
            menuHashBCrypt = new ToolStripMenuItem();
            menuHashCShake = new ToolStripMenuItem();
            menuHashDstu7564 = new ToolStripMenuItem();
            menuHashMD5 = new ToolStripMenuItem();
            menuHashHex = new ToolStripMenuItem();
            menuHashOpenBSDCrypt = new ToolStripMenuItem();
            menuHashRipeMD256 = new ToolStripMenuItem();
            menuHashSha1 = new ToolStripMenuItem();
            menuHashSha256 = new ToolStripMenuItem();
            menuHashSha512 = new ToolStripMenuItem();
            menuHashSCrypt = new ToolStripMenuItem();
            menuHashWhirlpool = new ToolStripMenuItem();
            menuHashTupleHash = new ToolStripMenuItem();
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
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            menuHelpHelp = new ToolStripMenuItem();
            comboBoxAlgo = new ComboBox();
            enumOptionsBindingSource = new BindingSource(components);
            textBoxKey = new TextBox();
            pictureBoxKey = new PictureBox();
            pictureBoxHash = new PictureBox();
            textBoxHash = new TextBox();
            buttonSetPipeline = new Button();
            buttonReset = new Button();
            pictureBoxFileIn = new PictureBox();
            pictureBoxAddAlgo = new PictureBox();
            textBoxPipe = new TextBox();
            labelFileIn = new Label();
            pictureBoxOutFile = new PictureBox();
            textBoxOut = new TextBox();
            buttonEncrypt = new Button();
            buttonDecrypt = new Button();
            groupBoxFiles = new GroupBox();
            panelOutLabel = new Panel();
            labelOutputFile = new Label();
            pictureBoxRunningPipe = new PictureBox();
            pictureBoxDelete = new PictureBox();
            comboBoxCompression = new ComboBox();
            comboBoxEncoding = new ComboBox();
            buttonRandomText = new Button();
            buttonHashPipe = new Button();
            radioButtonListHash = new EU.CqrXs.Gui.Controls.RadioButtonList();
            labelInfoMessage = new Label();
            statusStrip = new StatusStrip();
            statusLabelSource = new ToolStripStatusLabel();
            statusLabelMsg = new ToolStripStatusLabel();
            statusLabelDestination = new ToolStripStatusLabel();
            progressBar = new ProgressBar();
            tabControlSrc = new TabControl();
            tabPageAscii = new TabPage();
            textBoxSrc = new TextBox();
            tabPageHex = new TabPage();
            textBoxSrcHex = new TextBox();
            menuStripEncrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHash).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileIn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAddAlgo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutFile).BeginInit();
            groupBoxFiles.SuspendLayout();
            panelOutLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRunningPipe).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDelete).BeginInit();
            statusStrip.SuspendLayout();
            tabControlSrc.SuspendLayout();
            tabPageAscii.SuspendLayout();
            tabPageHex.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripEncrypt
            // 
            menuStripEncrypt.AllowMerge = false;
            menuStripEncrypt.BackColor = SystemColors.MenuBar;
            menuStripEncrypt.Font = new Font("Lucida Sans Typewriter", 9F);
            menuStripEncrypt.Items.AddRange(new ToolStripItem[] { toolMenuMain, menuCompression, menuEncoding, menuHash, optionsToolStripMenuItem, menuSerialize, menuHelp });
            menuStripEncrypt.Location = new Point(0, 0);
            menuStripEncrypt.Name = "menuStripEncrypt";
            menuStripEncrypt.Padding = new Padding(3, 2, 2, 2);
            menuStripEncrypt.Size = new Size(1008, 24);
            menuStripEncrypt.TabIndex = 0;
            menuStripEncrypt.Text = "menuStripEncrypt";
            // 
            // toolMenuMain
            // 
            toolMenuMain.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuMainSave, toolStripSeparator2, menuMainSetPipe, menuMainHashKey, menuMainHashPipe, toolStripSeparator3, menuMainEncrypt, menuMainDecrypt, menuMainRandomText, menuMainReset, toolStripSeparator1, menuFileExit });
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
            menuFileOpen.Size = new Size(162, 22);
            menuFileOpen.Text = "Open";
            menuFileOpen.Click += menuFileOpen_Click;
            // 
            // menuMainSave
            // 
            menuMainSave.BackColor = SystemColors.Menu;
            menuMainSave.Name = "menuMainSave";
            menuMainSave.ShortcutKeys = Keys.Control | Keys.S;
            menuMainSave.Size = new Size(162, 22);
            menuMainSave.Text = "Save";
            menuMainSave.Click += menuMainSave_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(159, 6);
            // 
            // menuMainSetPipe
            // 
            menuMainSetPipe.BackColor = SystemColors.Menu;
            menuMainSetPipe.Name = "menuMainSetPipe";
            menuMainSetPipe.Size = new Size(162, 22);
            menuMainSetPipe.Text = "Set Pipe";
            menuMainSetPipe.Click += SetPipeline_Click;
            // 
            // menuMainHashKey
            // 
            menuMainHashKey.BackColor = SystemColors.Menu;
            menuMainHashKey.Name = "menuMainHashKey";
            menuMainHashKey.Size = new Size(162, 22);
            menuMainHashKey.Text = "Hash Key";
            menuMainHashKey.Click += Hash_Click;
            // 
            // menuMainHashPipe
            // 
            menuMainHashPipe.BackColor = SystemColors.Menu;
            menuMainHashPipe.Name = "menuMainHashPipe";
            menuMainHashPipe.Size = new Size(162, 22);
            menuMainHashPipe.Text = "Hash Pipe";
            menuMainHashPipe.Click += Hash_Pipe_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(159, 6);
            // 
            // menuMainEncrypt
            // 
            menuMainEncrypt.BackColor = SystemColors.Menu;
            menuMainEncrypt.Name = "menuMainEncrypt";
            menuMainEncrypt.Size = new Size(162, 22);
            menuMainEncrypt.Text = "Encrypt";
            // 
            // menuMainDecrypt
            // 
            menuMainDecrypt.BackColor = SystemColors.Menu;
            menuMainDecrypt.Name = "menuMainDecrypt";
            menuMainDecrypt.Size = new Size(162, 22);
            menuMainDecrypt.Text = "Decrypt";
            // 
            // menuMainRandomText
            // 
            menuMainRandomText.BackColor = SystemColors.Menu;
            menuMainRandomText.Name = "menuMainRandomText";
            menuMainRandomText.Size = new Size(162, 22);
            menuMainRandomText.Text = "Random Text";
            menuMainRandomText.Click += RandomText_Click;
            // 
            // menuMainReset
            // 
            menuMainReset.BackColor = SystemColors.Menu;
            menuMainReset.Name = "menuMainReset";
            menuMainReset.Size = new Size(162, 22);
            menuMainReset.Text = "Reset";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(159, 6);
            // 
            // menuFileExit
            // 
            menuFileExit.BackColor = SystemColors.Menu;
            menuFileExit.Name = "menuFileExit";
            menuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            menuFileExit.Size = new Size(162, 22);
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
            zmenu7z.Click += menuCompression_Click;
            // 
            // zmenuBZip2
            // 
            zmenuBZip2.BackColor = SystemColors.Menu;
            zmenuBZip2.Name = "zmenuBZip2";
            zmenuBZip2.ShortcutKeys = Keys.Control | Keys.B;
            zmenuBZip2.Size = new Size(169, 22);
            zmenuBZip2.Text = "BZip2";
            zmenuBZip2.Click += menuCompression_Click;
            // 
            // zmenuGZip
            // 
            zmenuGZip.BackColor = SystemColors.Menu;
            zmenuGZip.Name = "zmenuGZip";
            zmenuGZip.ShortcutKeys = Keys.Control | Keys.G;
            zmenuGZip.Size = new Size(169, 22);
            zmenuGZip.Text = "GZip";
            zmenuGZip.Click += menuCompression_Click;
            // 
            // zmenuZip
            // 
            zmenuZip.BackColor = SystemColors.Menu;
            zmenuZip.Name = "zmenuZip";
            zmenuZip.ShortcutKeys = Keys.Control | Keys.Z;
            zmenuZip.Size = new Size(169, 22);
            zmenuZip.Text = "Zip";
            zmenuZip.Click += menuCompression_Click;
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
            zmenuNone.Click += menuCompression_Click;
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
            // menuHash
            // 
            menuHash.DropDownItems.AddRange(new ToolStripItem[] { menuHashOct, menuHashBlake2xs, menuHashBCrypt, menuHashCShake, menuHashDstu7564, menuHashMD5, menuHashHex, menuHashOpenBSDCrypt, menuHashRipeMD256, menuHashSha1, menuHashSha256, menuHashSha512, menuHashSCrypt, menuHashWhirlpool, menuHashTupleHash });
            menuHash.Font = new Font("Lucida Sans Typewriter", 10F);
            menuHash.Name = "menuHash";
            menuHash.Size = new Size(51, 20);
            menuHash.Text = "Hash";
            // 
            // menuHashOct
            // 
            menuHashOct.BackColor = SystemColors.ControlLight;
            menuHashOct.Name = "menuHashOct";
            menuHashOct.Size = new Size(170, 22);
            menuHashOct.Text = "Oct";
            // 
            // menuHashBlake2xs
            // 
            menuHashBlake2xs.BackColor = SystemColors.ControlLight;
            menuHashBlake2xs.Name = "menuHashBlake2xs";
            menuHashBlake2xs.Size = new Size(170, 22);
            menuHashBlake2xs.Text = "Blake2xs";
            // 
            // menuHashBCrypt
            // 
            menuHashBCrypt.BackColor = SystemColors.ControlLight;
            menuHashBCrypt.Name = "menuHashBCrypt";
            menuHashBCrypt.Size = new Size(170, 22);
            menuHashBCrypt.Text = "B-Crypt";
            // 
            // menuHashCShake
            // 
            menuHashCShake.BackColor = SystemColors.ControlLight;
            menuHashCShake.Name = "menuHashCShake";
            menuHashCShake.Size = new Size(170, 22);
            menuHashCShake.Text = "CShake";
            // 
            // menuHashDstu7564
            // 
            menuHashDstu7564.BackColor = SystemColors.ControlLight;
            menuHashDstu7564.Name = "menuHashDstu7564";
            menuHashDstu7564.Size = new Size(170, 22);
            menuHashDstu7564.Text = "Dstu7564";
            // 
            // menuHashMD5
            // 
            menuHashMD5.BackColor = SystemColors.Menu;
            menuHashMD5.Name = "menuHashMD5";
            menuHashMD5.Size = new Size(170, 22);
            menuHashMD5.Tag = "";
            menuHashMD5.Text = "MD5";
            // 
            // menuHashHex
            // 
            menuHashHex.BackColor = SystemColors.Menu;
            menuHashHex.Checked = true;
            menuHashHex.CheckState = CheckState.Checked;
            menuHashHex.Name = "menuHashHex";
            menuHashHex.Size = new Size(170, 22);
            menuHashHex.Text = "Hex";
            // 
            // menuHashOpenBSDCrypt
            // 
            menuHashOpenBSDCrypt.BackColor = SystemColors.ControlLight;
            menuHashOpenBSDCrypt.Name = "menuHashOpenBSDCrypt";
            menuHashOpenBSDCrypt.Size = new Size(170, 22);
            menuHashOpenBSDCrypt.Text = "OpenBSDCrypt";
            // 
            // menuHashRipeMD256
            // 
            menuHashRipeMD256.BackColor = SystemColors.ControlLight;
            menuHashRipeMD256.Name = "menuHashRipeMD256";
            menuHashRipeMD256.Size = new Size(170, 22);
            menuHashRipeMD256.Text = "RipeMD256";
            // 
            // menuHashSha1
            // 
            menuHashSha1.BackColor = SystemColors.Menu;
            menuHashSha1.MergeAction = MergeAction.Insert;
            menuHashSha1.Name = "menuHashSha1";
            menuHashSha1.Size = new Size(170, 22);
            menuHashSha1.Text = "Sha1";
            // 
            // menuHashSha256
            // 
            menuHashSha256.BackColor = SystemColors.Menu;
            menuHashSha256.Name = "menuHashSha256";
            menuHashSha256.Size = new Size(170, 22);
            menuHashSha256.Text = "Sha256";
            // 
            // menuHashSha512
            // 
            menuHashSha512.BackColor = SystemColors.Menu;
            menuHashSha512.Name = "menuHashSha512";
            menuHashSha512.Size = new Size(170, 22);
            menuHashSha512.Text = "Sha512";
            // 
            // menuHashSCrypt
            // 
            menuHashSCrypt.BackColor = SystemColors.ControlLight;
            menuHashSCrypt.Name = "menuHashSCrypt";
            menuHashSCrypt.Size = new Size(170, 22);
            menuHashSCrypt.Text = "S-Crypt";
            // 
            // menuHashWhirlpool
            // 
            menuHashWhirlpool.BackColor = SystemColors.ControlLight;
            menuHashWhirlpool.Name = "menuHashWhirlpool";
            menuHashWhirlpool.Size = new Size(170, 22);
            menuHashWhirlpool.Text = "Whirlpool";
            // 
            // menuHashTupleHash
            // 
            menuHashTupleHash.BackColor = SystemColors.ControlLight;
            menuHashTupleHash.Name = "menuHashTupleHash";
            menuHashTupleHash.Size = new Size(170, 22);
            menuHashTupleHash.Text = "TupleHash";
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
            menuSerialize.DropDownItems.AddRange(new ToolStripItem[] { menuJson, menuXml, menuRaw });
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
            menuJson.Size = new Size(161, 22);
            menuJson.Text = "Json";
            // 
            // menuXml
            // 
            menuXml.BackColor = SystemColors.Menu;
            menuXml.Enabled = false;
            menuXml.Name = "menuXml";
            menuXml.ShortcutKeys = Keys.Control | Keys.X;
            menuXml.Size = new Size(161, 22);
            menuXml.Text = "Xml";
            // 
            // menuRaw
            // 
            menuRaw.BackColor = SystemColors.Menu;
            menuRaw.Enabled = false;
            menuRaw.Name = "menuRaw";
            menuRaw.ShortcutKeys = Keys.Control | Keys.R;
            menuRaw.Size = new Size(161, 22);
            menuRaw.Text = "Raw";
            // 
            // menuHelp
            // 
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout, menuHelpHelp });
            menuHelp.Font = new Font("Lucida Sans Typewriter", 10F);
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(27, 20);
            menuHelp.Text = "?";
            // 
            // menuAbout
            // 
            menuAbout.BackColor = SystemColors.MenuBar;
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(161, 22);
            menuAbout.Text = "About";
            // 
            // menuHelpHelp
            // 
            menuHelpHelp.BackColor = SystemColors.MenuBar;
            menuHelpHelp.Name = "menuHelpHelp";
            menuHelpHelp.ShortcutKeys = Keys.Alt | Keys.F3;
            menuHelpHelp.Size = new Size(161, 22);
            menuHelpHelp.Text = "Help";
            // 
            // comboBoxAlgo
            // 
            comboBoxAlgo.BackColor = SystemColors.Control;
            comboBoxAlgo.DropDownWidth = 160;
            comboBoxAlgo.Font = new Font("Lucida Sans Typewriter", 10F);
            comboBoxAlgo.FormattingEnabled = true;
            comboBoxAlgo.Location = new Point(106, 168);
            comboBoxAlgo.Margin = new Padding(1);
            comboBoxAlgo.MaxDropDownItems = 32;
            comboBoxAlgo.Name = "comboBoxAlgo";
            comboBoxAlgo.Size = new Size(120, 23);
            comboBoxAlgo.TabIndex = 11;
            // 
            // textBoxKey
            // 
            textBoxKey.BackColor = SystemColors.ControlLightLight;
            textBoxKey.Font = new Font("Lucida Sans Typewriter", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxKey.Location = new Point(48, 32);
            textBoxKey.Margin = new Padding(1);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new Size(810, 25);
            textBoxKey.TabIndex = 4;
            textBoxKey.Text = "ftp@ftp.cdrom.com";
            textBoxKey.TextChanged += textBoxKey_TextChanged;
            // 
            // pictureBoxKey
            // 
            pictureBoxKey.BackColor = SystemColors.Control;
            pictureBoxKey.Image = Properties.Resources.key_ring;
            pictureBoxKey.Location = new Point(8, 32);
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
            pictureBoxHash.Image = Properties.Resources.a_hash1;
            pictureBoxHash.Location = new Point(8, 128);
            pictureBoxHash.Margin = new Padding(1);
            pictureBoxHash.Name = "pictureBoxHash";
            pictureBoxHash.Size = new Size(32, 30);
            pictureBoxHash.TabIndex = 7;
            pictureBoxHash.TabStop = false;
            pictureBoxHash.Click += Hash_Click;
            // 
            // textBoxHash
            // 
            textBoxHash.BackColor = SystemColors.Control;
            textBoxHash.Font = new Font("Lucida Sans Typewriter", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxHash.Location = new Point(48, 128);
            textBoxHash.Margin = new Padding(1);
            textBoxHash.Name = "textBoxHash";
            textBoxHash.ReadOnly = true;
            textBoxHash.Size = new Size(823, 23);
            textBoxHash.TabIndex = 8;
            // 
            // buttonSetPipeline
            // 
            buttonSetPipeline.BackColor = SystemColors.Control;
            buttonSetPipeline.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonSetPipeline.Location = new Point(876, 32);
            buttonSetPipeline.Margin = new Padding(1);
            buttonSetPipeline.Name = "buttonSetPipeline";
            buttonSetPipeline.Size = new Size(120, 30);
            buttonSetPipeline.TabIndex = 5;
            buttonSetPipeline.Text = "Set Pipeline";
            buttonSetPipeline.UseVisualStyleBackColor = false;
            buttonSetPipeline.Click += SetPipeline_Click;
            // 
            // buttonReset
            // 
            buttonReset.BackColor = SystemColors.Control;
            buttonReset.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonReset.Location = new Point(876, 348);
            buttonReset.Margin = new Padding(1);
            buttonReset.Name = "buttonReset";
            buttonReset.Size = new Size(120, 27);
            buttonReset.TabIndex = 36;
            buttonReset.Text = "Reset Form";
            buttonReset.UseVisualStyleBackColor = false;
            // 
            // pictureBoxFileIn
            // 
            pictureBoxFileIn.Image = Properties.Resources.image_file;
            pictureBoxFileIn.InitialImage = Properties.Resources.img_success;
            pictureBoxFileIn.Location = new Point(12, 30);
            pictureBoxFileIn.Margin = new Padding(2);
            pictureBoxFileIn.Name = "pictureBoxFileIn";
            pictureBoxFileIn.Size = new Size(64, 64);
            pictureBoxFileIn.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBoxFileIn.TabIndex = 22;
            pictureBoxFileIn.TabStop = false;
            // 
            // pictureBoxAddAlgo
            // 
            pictureBoxAddAlgo.BackColor = SystemColors.ControlLight;
            pictureBoxAddAlgo.Image = Properties.Resources.AddAesArrowHover;
            pictureBoxAddAlgo.Location = new Point(230, 168);
            pictureBoxAddAlgo.Margin = new Padding(1);
            pictureBoxAddAlgo.Name = "pictureBoxAddAlgo";
            pictureBoxAddAlgo.Size = new Size(32, 27);
            pictureBoxAddAlgo.TabIndex = 12;
            pictureBoxAddAlgo.TabStop = false;
            pictureBoxAddAlgo.Click += pictureBoxAddAlgo_Click;
            // 
            // textBoxPipe
            // 
            textBoxPipe.BackColor = SystemColors.ControlLightLight;
            textBoxPipe.Font = new Font("Lucida Sans Typewriter", 10F);
            textBoxPipe.Location = new Point(264, 169);
            textBoxPipe.Margin = new Padding(1);
            textBoxPipe.MaxLength = 8192;
            textBoxPipe.Name = "textBoxPipe";
            textBoxPipe.ReadOnly = true;
            textBoxPipe.Size = new Size(578, 23);
            textBoxPipe.TabIndex = 13;
            // 
            // labelFileIn
            // 
            labelFileIn.Font = new Font("Lucida Sans Typewriter", 8.75F);
            labelFileIn.Location = new Point(12, 115);
            labelFileIn.Margin = new Padding(1, 0, 1, 0);
            labelFileIn.Name = "labelFileIn";
            labelFileIn.Size = new Size(483, 28);
            labelFileIn.TabIndex = 21;
            labelFileIn.Text = "[Input File]";
            // 
            // pictureBoxOutFile
            // 
            pictureBoxOutFile.Image = Properties.Resources.image_file_encrypted;
            pictureBoxOutFile.Location = new Point(915, 30);
            pictureBoxOutFile.Margin = new Padding(1);
            pictureBoxOutFile.Name = "pictureBoxOutFile";
            pictureBoxOutFile.Size = new Size(68, 68);
            pictureBoxOutFile.TabIndex = 25;
            pictureBoxOutFile.TabStop = false;
            pictureBoxOutFile.Visible = false;
            pictureBoxOutFile.Click += pictureOutBoxFile_Click;
            pictureBoxOutFile.DoubleClick += pictureOutBoxFile_Click;
            // 
            // textBoxOut
            // 
            textBoxOut.BackColor = SystemColors.Control;
            textBoxOut.BorderStyle = BorderStyle.FixedSingle;
            textBoxOut.Font = new Font("Lucida Console", 8F);
            textBoxOut.Location = new Point(516, 381);
            textBoxOut.Margin = new Padding(2);
            textBoxOut.MaxLength = 1048576;
            textBoxOut.Multiline = true;
            textBoxOut.Name = "textBoxOut";
            textBoxOut.ReadOnly = true;
            textBoxOut.ScrollBars = ScrollBars.Vertical;
            textBoxOut.Size = new Size(480, 292);
            textBoxOut.TabIndex = 43;
            // 
            // buttonEncrypt
            // 
            buttonEncrypt.BackColor = SystemColors.Control;
            buttonEncrypt.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonEncrypt.Location = new Point(8, 348);
            buttonEncrypt.Margin = new Padding(1);
            buttonEncrypt.Name = "buttonEncrypt";
            buttonEncrypt.Size = new Size(120, 27);
            buttonEncrypt.TabIndex = 33;
            buttonEncrypt.Text = "Encrypt";
            buttonEncrypt.UseVisualStyleBackColor = false;
            // 
            // buttonDecrypt
            // 
            buttonDecrypt.BackColor = SystemColors.Control;
            buttonDecrypt.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonDecrypt.Location = new Point(142, 348);
            buttonDecrypt.Margin = new Padding(1);
            buttonDecrypt.Name = "buttonDecrypt";
            buttonDecrypt.Size = new Size(120, 27);
            buttonDecrypt.TabIndex = 34;
            buttonDecrypt.Text = "Decrypt";
            buttonDecrypt.UseVisualStyleBackColor = false;
            // 
            // groupBoxFiles
            // 
            groupBoxFiles.AllowDrop = true;
            groupBoxFiles.BackColor = SystemColors.Control;
            groupBoxFiles.Controls.Add(panelOutLabel);
            groupBoxFiles.Controls.Add(pictureBoxRunningPipe);
            groupBoxFiles.Controls.Add(pictureBoxFileIn);
            groupBoxFiles.Controls.Add(labelFileIn);
            groupBoxFiles.Controls.Add(pictureBoxOutFile);
            groupBoxFiles.Font = new Font("Lucida Sans Typewriter", 8F);
            groupBoxFiles.Location = new Point(8, 198);
            groupBoxFiles.Margin = new Padding(2);
            groupBoxFiles.Name = "groupBoxFiles";
            groupBoxFiles.Padding = new Padding(2);
            groupBoxFiles.Size = new Size(988, 145);
            groupBoxFiles.TabIndex = 20;
            groupBoxFiles.TabStop = false;
            groupBoxFiles.Text = "Files (drag files into)";
            groupBoxFiles.DragDrop += Drag_Drop;
            groupBoxFiles.DragEnter += Drag_Enter;
            groupBoxFiles.DragOver += Drag_Over;
            groupBoxFiles.DragLeave += Drag_Leave;
            groupBoxFiles.GiveFeedback += Give_FeedBack;
            // 
            // panelOutLabel
            // 
            panelOutLabel.Controls.Add(labelOutputFile);
            panelOutLabel.Location = new Point(508, 120);
            panelOutLabel.Name = "panelOutLabel";
            panelOutLabel.RightToLeft = RightToLeft.Yes;
            panelOutLabel.Size = new Size(477, 26);
            panelOutLabel.TabIndex = 26;
            // 
            // labelOutputFile
            // 
            labelOutputFile.AutoSize = true;
            labelOutputFile.Dock = DockStyle.Right;
            labelOutputFile.Font = new Font("Lucida Sans Typewriter", 8.75F);
            labelOutputFile.Location = new Point(379, 0);
            labelOutputFile.Margin = new Padding(1, 0, 1, 0);
            labelOutputFile.Name = "labelOutputFile";
            labelOutputFile.RightToLeft = RightToLeft.Yes;
            labelOutputFile.Size = new Size(98, 13);
            labelOutputFile.TabIndex = 25;
            labelOutputFile.Text = "[Output File]";
            labelOutputFile.Visible = false;
            // 
            // pictureBoxRunningPipe
            // 
            pictureBoxRunningPipe.Image = Properties.Resources.BlankEncrypt_640x108;
            pictureBoxRunningPipe.Location = new Point(180, 6);
            pictureBoxRunningPipe.Margin = new Padding(1);
            pictureBoxRunningPipe.Name = "pictureBoxRunningPipe";
            pictureBoxRunningPipe.Size = new Size(640, 108);
            pictureBoxRunningPipe.TabIndex = 23;
            pictureBoxRunningPipe.TabStop = false;
            // 
            // pictureBoxDelete
            // 
            pictureBoxDelete.BackColor = SystemColors.Control;
            pictureBoxDelete.Image = Properties.Resources.image_delete;
            pictureBoxDelete.Location = new Point(844, 169);
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
            comboBoxCompression.Items.AddRange(new object[] { "None", "BZip2", "GZip", "Zip" });
            comboBoxCompression.Location = new Point(8, 170);
            comboBoxCompression.Margin = new Padding(1);
            comboBoxCompression.MaxDropDownItems = 32;
            comboBoxCompression.Name = "comboBoxCompression";
            comboBoxCompression.Size = new Size(96, 23);
            comboBoxCompression.TabIndex = 10;
            comboBoxCompression.SelectedIndexChanged += ComboBoxCompression_SelectedIndexChanged;
            // 
            // comboBoxEncoding
            // 
            comboBoxEncoding.BackColor = SystemColors.Control;
            comboBoxEncoding.DropDownWidth = 144;
            comboBoxEncoding.Font = new Font("Lucida Sans Typewriter", 10F);
            comboBoxEncoding.FormattingEnabled = true;
            comboBoxEncoding.Items.AddRange(new object[] { "None", "Base16", "Hex16", "Base32", "Hex32", "Base64", "Uu", "Xx" });
            comboBoxEncoding.Location = new Point(876, 170);
            comboBoxEncoding.Margin = new Padding(1);
            comboBoxEncoding.MaxDropDownItems = 32;
            comboBoxEncoding.Name = "comboBoxEncoding";
            comboBoxEncoding.Size = new Size(120, 23);
            comboBoxEncoding.TabIndex = 14;
            // 
            // buttonRandomText
            // 
            buttonRandomText.BackColor = SystemColors.Control;
            buttonRandomText.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonRandomText.Location = new Point(368, 348);
            buttonRandomText.Margin = new Padding(1);
            buttonRandomText.Name = "buttonRandomText";
            buttonRandomText.Size = new Size(120, 27);
            buttonRandomText.TabIndex = 35;
            buttonRandomText.Text = "Random Text";
            buttonRandomText.UseVisualStyleBackColor = false;
            buttonRandomText.Click += RandomText_Click;
            // 
            // buttonHashPipe
            // 
            buttonHashPipe.BackColor = SystemColors.Control;
            buttonHashPipe.Font = new Font("Lucida Sans Typewriter", 9.75F);
            buttonHashPipe.Location = new Point(876, 128);
            buttonHashPipe.Margin = new Padding(1);
            buttonHashPipe.Name = "buttonHashPipe";
            buttonHashPipe.Size = new Size(120, 27);
            buttonHashPipe.TabIndex = 9;
            buttonHashPipe.Text = "Hash Pipe";
            buttonHashPipe.UseVisualStyleBackColor = false;
            buttonHashPipe.Click += Hash_Pipe_Click;
            // 
            // radioButtonListHash
            // 
            radioButtonListHash.BackColor = SystemColors.Control;
            radioButtonListHash.Font = new Font("Lucida Sans Typewriter", 9F);
            radioButtonListHash.FormattingEnabled = true;
            radioButtonListHash.HorizontalExtent = 1;
            radioButtonListHash.Items.AddRange(new object[] { "BCrypt", "Blake2xs", "CShake", "Dstu7564", "Hex", "MD5", "Oct", "OpenBSDCrypt", "RipeMD256", "SCrypt", "Sha1", "Sha256", "Sha512", "TupleHash", "Whirlpool" });
            radioButtonListHash.Location = new Point(8, 73);
            radioButtonListHash.Margin = new Padding(2);
            radioButtonListHash.MultiColumn = true;
            radioButtonListHash.Name = "radioButtonListHash";
            radioButtonListHash.Size = new Size(988, 38);
            radioButtonListHash.Sorted = true;
            radioButtonListHash.TabIndex = 6;
            // 
            // labelInfoMessage
            // 
            labelInfoMessage.BackColor = SystemColors.Info;
            labelInfoMessage.Font = new Font("Lucida Fax", 9.25F);
            labelInfoMessage.ForeColor = SystemColors.InfoText;
            labelInfoMessage.Location = new Point(516, 348);
            labelInfoMessage.Margin = new Padding(1);
            labelInfoMessage.Name = "labelInfoMessage";
            labelInfoMessage.Size = new Size(356, 27);
            labelInfoMessage.TabIndex = 37;
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
            // progressBar
            // 
            progressBar.Location = new Point(8, 678);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(989, 26);
            progressBar.TabIndex = 45;
            // 
            // tabControlSrc
            // 
            tabControlSrc.Controls.Add(tabPageAscii);
            tabControlSrc.Controls.Add(tabPageHex);
            tabControlSrc.Location = new Point(12, 382);
            tabControlSrc.Margin = new Padding(1);
            tabControlSrc.Name = "tabControlSrc";
            tabControlSrc.SelectedIndex = 0;
            tabControlSrc.Size = new Size(480, 284);
            tabControlSrc.TabIndex = 40;
            tabControlSrc.SelectedIndexChanged += tabControlSrc_SelectedIndexChanged;
            // 
            // tabPageAscii
            // 
            tabPageAscii.Controls.Add(textBoxSrc);
            tabPageAscii.Location = new Point(4, 25);
            tabPageAscii.Margin = new Padding(1);
            tabPageAscii.Name = "tabPageAscii";
            tabPageAscii.Padding = new Padding(1);
            tabPageAscii.Size = new Size(472, 255);
            tabPageAscii.TabIndex = 41;
            tabPageAscii.Text = "Ascii Text";
            tabPageAscii.UseVisualStyleBackColor = true;
            // 
            // textBoxSrc
            // 
            textBoxSrc.BackColor = SystemColors.ControlLight;
            textBoxSrc.Dock = DockStyle.Fill;
            textBoxSrc.Font = new Font("Lucida Console", 8F);
            textBoxSrc.Location = new Point(1, 1);
            textBoxSrc.Margin = new Padding(1);
            textBoxSrc.MaxLength = 1048576;
            textBoxSrc.Multiline = true;
            textBoxSrc.Name = "textBoxSrc";
            textBoxSrc.ScrollBars = ScrollBars.Vertical;
            textBoxSrc.Size = new Size(470, 253);
            textBoxSrc.TabIndex = 42;
            // 
            // tabPageHex
            // 
            tabPageHex.Controls.Add(textBoxSrcHex);
            tabPageHex.Location = new Point(4, 24);
            tabPageHex.Margin = new Padding(1);
            tabPageHex.Name = "tabPageHex";
            tabPageHex.Padding = new Padding(1);
            tabPageHex.Size = new Size(472, 256);
            tabPageHex.TabIndex = 43;
            tabPageHex.Text = "Hex View";
            tabPageHex.UseVisualStyleBackColor = true;
            // 
            // textBoxSrcHex
            // 
            textBoxSrcHex.BackColor = SystemColors.Control;
            textBoxSrcHex.BorderStyle = BorderStyle.FixedSingle;
            textBoxSrcHex.Dock = DockStyle.Fill;
            textBoxSrcHex.Font = new Font("Lucida Console", 9F);
            textBoxSrcHex.Location = new Point(1, 1);
            textBoxSrcHex.Margin = new Padding(1);
            textBoxSrcHex.MaxLength = 1048576;
            textBoxSrcHex.Multiline = true;
            textBoxSrcHex.Name = "textBoxSrcHex";
            textBoxSrcHex.ReadOnly = true;
            textBoxSrcHex.ScrollBars = ScrollBars.Vertical;
            textBoxSrcHex.Size = new Size(470, 254);
            textBoxSrcHex.TabIndex = 44;
            // 
            // EncryptForm
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1008, 729);
            Controls.Add(tabControlSrc);
            Controls.Add(progressBar);
            Controls.Add(statusStrip);
            Controls.Add(labelInfoMessage);
            Controls.Add(radioButtonListHash);
            Controls.Add(buttonHashPipe);
            Controls.Add(buttonRandomText);
            Controls.Add(comboBoxEncoding);
            Controls.Add(comboBoxCompression);
            Controls.Add(pictureBoxDelete);
            Controls.Add(groupBoxFiles);
            Controls.Add(buttonDecrypt);
            Controls.Add(buttonEncrypt);
            Controls.Add(textBoxOut);
            Controls.Add(textBoxPipe);
            Controls.Add(pictureBoxAddAlgo);
            Controls.Add(buttonReset);
            Controls.Add(buttonSetPipeline);
            Controls.Add(textBoxHash);
            Controls.Add(pictureBoxHash);
            Controls.Add(pictureBoxKey);
            Controls.Add(textBoxKey);
            Controls.Add(comboBoxAlgo);
            Controls.Add(menuStripEncrypt);
            Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStripEncrypt;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EncryptForm";
            Opacity = 0.92D;
            Text = "EncryptForm";
            FormClosed += menuFileExit_Close;
            Load += EncryptForm_Load;
            menuStripEncrypt.ResumeLayout(false);
            menuStripEncrypt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHash).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileIn).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAddAlgo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutFile).EndInit();
            groupBoxFiles.ResumeLayout(false);
            groupBoxFiles.PerformLayout();
            panelOutLabel.ResumeLayout(false);
            panelOutLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRunningPipe).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDelete).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            tabControlSrc.ResumeLayout(false);
            tabPageAscii.ResumeLayout(false);
            tabPageAscii.PerformLayout();
            tabPageHex.ResumeLayout(false);
            tabPageHex.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private BindingSource enumOptionsBindingSource;
        internal GroupBox groupBoxFiles;
        private PictureBox pictureBoxRunningPipe;
        protected internal PictureBox pictureBoxKey;
        protected internal PictureBox pictureBoxHash;
        protected internal TextBox textBoxHash;
        protected internal Button buttonSetPipeline;
        protected internal ComboBox comboBoxCompression;
        protected internal Button buttonHashPipe;
        protected internal Controls.RadioButtonList radioButtonListHash;
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
        protected internal ComboBox comboBoxAlgo;
        protected internal Button buttonReset;
        protected internal PictureBox pictureBoxFileIn;
        protected internal PictureBox pictureBoxAddAlgo;
        protected internal TextBox textBoxPipe;
        protected internal Label labelFileIn;
        protected internal PictureBox pictureBoxOutFile;
        protected internal TextBox textBoxOut;
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
        internal  ToolStripMenuItem menuHash;
        internal  ToolStripMenuItem menuHashBCrypt;
        internal  ToolStripMenuItem menuEncXx;
        internal  ToolStripMenuItem menuSerialize;
        internal  ToolStripMenuItem menuJson;
        internal  ToolStripMenuItem menuXml;
        internal  ToolStripMenuItem menuRaw;
        internal ToolStripMenuItem menuHashSCrypt;
        internal ToolStripMenuItem menuHashMD5;
        internal  ToolStripMenuItem menuHashSha1;
        internal  ToolStripMenuItem menuHashSha512;
        internal  ToolStripMenuItem menuHashOpenBSDCrypt;
        internal  ToolStripMenuItem menuHashSha256;
        internal  ToolStripMenuItem menuHashHex;
        internal  ToolStripMenuItem menuHelp;
        internal  ToolStripMenuItem menuAbout;
        internal  ToolStripMenuItem menuHelpHelp;
        internal  ToolStripMenuItem menuHashOct;
        internal  ToolStripMenuItem menuHashRipeMD256;
        internal  ToolStripMenuItem menuHashWhirlpool;
        internal  ToolStripMenuItem menuHashBlake2xs;
        internal  ToolStripMenuItem menuHashDstu7564;
        internal  ToolStripMenuItem menuHashCShake;
        internal  ToolStripMenuItem menuHashTupleHash;
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
        private ProgressBar progressBar;
        private ToolStripMenuItem menuOptionsItemsWarnings;
        private ToolStripMenuItem warnOnEmptyPipeToolStripMenuItem;
        private ToolStripMenuItem warnOnDoubleZippingToolStripMenuItem;
        private ToolStripMenuItem menuOptionsMenuFileSettings;
        private ToolStripMenuItem menuItemCreatePipeSettingsFromFileName;
        private ToolStripMenuItem menuFileSettingsItemAutomaticallySaveToTemp;
        private Panel panelOutLabel;
        protected internal Label labelOutputFile;
        private TabControl tabControlSrc;
        private TabPage tabPageAscii;
        protected internal TextBox textBoxSrc;
        private TabPage tabPageHex;
        protected internal TextBox textBoxSrcHex;
    }


}