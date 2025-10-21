namespace Area23.At.WinForm.CryptFormCore.Gui.Forms
{
    partial class EncryptFormSimple
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EncryptFormSimple));
            menuStripEncrypt = new MenuStrip();
            toolMenuMain = new ToolStripMenuItem();
            menuFileOpen = new ToolStripMenuItem();
            menuMainSave = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            menuMainHashKey = new ToolStripMenuItem();
            menuMainHashPipe = new ToolStripMenuItem();
            menuMainSetPipe = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            menuMainEncrypt = new ToolStripMenuItem();
            menuMainDecrypt = new ToolStripMenuItem();
            menuMainRandomText = new ToolStripMenuItem();
            menuMainClear = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuFileExit = new ToolStripMenuItem();
            menuCompression = new ToolStripMenuItem();
            menu7z = new ToolStripMenuItem();
            menuBZip2 = new ToolStripMenuItem();
            menuGZip = new ToolStripMenuItem();
            menuZip = new ToolStripMenuItem();
            menuCompressionNone = new ToolStripMenuItem();
            menuEncoding = new ToolStripMenuItem();
            menuItemNone = new ToolStripMenuItem();
            menuBase16 = new ToolStripMenuItem();
            menuHex16 = new ToolStripMenuItem();
            menuBase32 = new ToolStripMenuItem();
            menuHex32 = new ToolStripMenuItem();
            menuBase64 = new ToolStripMenuItem();
            menuUu = new ToolStripMenuItem();
            menuXx = new ToolStripMenuItem();
            menuHash = new ToolStripMenuItem();
            menuHashBCrypt = new ToolStripMenuItem();
            menuHashMD5 = new ToolStripMenuItem();
            menuHashHex = new ToolStripMenuItem();
            menuHashOpenBsd = new ToolStripMenuItem();
            menuHashSha1 = new ToolStripMenuItem();
            menuHashSha256 = new ToolStripMenuItem();
            menuHashSha512 = new ToolStripMenuItem();
            menuHashSCrypt = new ToolStripMenuItem();
            menuSerialize = new ToolStripMenuItem();
            menuJson = new ToolStripMenuItem();
            menuXml = new ToolStripMenuItem();
            menuRaw = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            menuHelpHelp = new ToolStripMenuItem();
            comboBoxAlgo = new ComboBox();
            cipherEnumBindingSource2 = new BindingSource(components);
            cipherEnumBindingSource = new BindingSource(components);
            enumOptionsBindingSource = new BindingSource(components);
            textBoxKey = new TextBox();
            pictureBoxKey = new PictureBox();
            pictureBoxHash = new PictureBox();
            textBoxHash = new TextBox();
            buttonSetPipeline = new Button();
            buttonClear = new Button();
            pictureBoxFileIn = new PictureBox();
            pictureBoxAddAlgo = new PictureBox();
            textBoxPipe = new TextBox();
            labelFileIn = new Label();
            labelOutputFile = new Label();
            pictureBoxOutFile = new PictureBox();
            textBoxSrc = new TextBox();
            textBoxOut = new TextBox();
            buttonEncrypt = new Button();
            buttonDecrypt = new Button();
            cipherEnumBindingSource1 = new BindingSource(components);
            groupBoxFiles = new GroupBox();
            pictureBoxDelete = new PictureBox();
            notifyIcon1 = new NotifyIcon(components);
            comboBoxCompression = new ComboBox();
            comboBoxEncoding = new ComboBox();
            buttonRandomText = new Button();
            buttonHashPipe = new Button();
            radioButtonListHash = new Area23.At.WinForm.CryptFormCore.Gui.Controls.RadioButtonList();
            menuStripEncrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHash).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileIn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAddAlgo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutFile).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource1).BeginInit();
            groupBoxFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDelete).BeginInit();
            SuspendLayout();
            // 
            // menuStripEncrypt
            // 
            menuStripEncrypt.AllowMerge = false;
            menuStripEncrypt.BackColor = SystemColors.MenuBar;
            menuStripEncrypt.Font = new Font("Lucida Sans Unicode", 9F);
            menuStripEncrypt.Items.AddRange(new ToolStripItem[] { toolMenuMain, menuCompression, menuEncoding, menuHash, menuSerialize, menuHelp });
            menuStripEncrypt.Location = new Point(0, 0);
            menuStripEncrypt.Name = "menuStripEncrypt";
            menuStripEncrypt.Padding = new Padding(3, 2, 2, 2);
            menuStripEncrypt.Size = new Size(784, 24);
            menuStripEncrypt.TabIndex = 0;
            menuStripEncrypt.Text = "menuStripEncrypt";
            // 
            // toolMenuMain
            // 
            toolMenuMain.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuMainSave, toolStripSeparator2, menuMainHashKey, menuMainHashPipe, menuMainSetPipe, toolStripSeparator3, menuMainEncrypt, menuMainDecrypt, menuMainRandomText, menuMainClear, toolStripSeparator1, menuFileExit });
            toolMenuMain.Font = new Font("Lucida Sans Unicode", 9F);
            toolMenuMain.Name = "toolMenuMain";
            toolMenuMain.Size = new Size(46, 20);
            toolMenuMain.Text = "Main";
            // 
            // menuFileOpen
            // 
            menuFileOpen.BackColor = SystemColors.Menu;
            menuFileOpen.Name = "menuFileOpen";
            menuFileOpen.ShortcutKeys = Keys.Control | Keys.O;
            menuFileOpen.Size = new Size(152, 22);
            menuFileOpen.Text = "Open";
            menuFileOpen.Click += menuFileOpen_Click;
            // 
            // menuMainSave
            // 
            menuMainSave.BackColor = SystemColors.Menu;
            menuMainSave.Name = "menuMainSave";
            menuMainSave.ShortcutKeys = Keys.Control | Keys.S;
            menuMainSave.Size = new Size(152, 22);
            menuMainSave.Text = "Save";
            menuMainSave.Click += menuMainSave_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(149, 6);
            // 
            // menuMainHashKey
            // 
            menuMainHashKey.BackColor = SystemColors.Menu;
            menuMainHashKey.Name = "menuMainHashKey";
            menuMainHashKey.Size = new Size(152, 22);
            menuMainHashKey.Text = "Hash Key";
            menuMainHashKey.Click += Hash_Pipe_Click;
            // 
            // menuMainHashPipe
            // 
            menuMainHashPipe.BackColor = SystemColors.Menu;
            menuMainHashPipe.Name = "menuMainHashPipe";
            menuMainHashPipe.Size = new Size(152, 22);
            menuMainHashPipe.Text = "Hash Pipe";
            // 
            // menuMainSetPipe
            // 
            menuMainSetPipe.BackColor = SystemColors.Menu;
            menuMainSetPipe.Name = "menuMainSetPipe";
            menuMainSetPipe.Size = new Size(152, 22);
            menuMainSetPipe.Text = "Set Pipe";
            menuMainSetPipe.Click += SetPipeline_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(149, 6);
            // 
            // menuMainEncrypt
            // 
            menuMainEncrypt.BackColor = SystemColors.Menu;
            menuMainEncrypt.Name = "menuMainEncrypt";
            menuMainEncrypt.Size = new Size(152, 22);
            menuMainEncrypt.Text = "Encrypt";
            menuMainEncrypt.Click += Encrypt_Click;
            // 
            // menuMainDecrypt
            // 
            menuMainDecrypt.BackColor = SystemColors.Menu;
            menuMainDecrypt.Name = "menuMainDecrypt";
            menuMainDecrypt.Size = new Size(152, 22);
            menuMainDecrypt.Text = "Decrypt";
            menuMainDecrypt.Click += Decrypt_Click;
            // 
            // menuMainRandomText
            // 
            menuMainRandomText.BackColor = SystemColors.Menu;
            menuMainRandomText.Name = "menuMainRandomText";
            menuMainRandomText.Size = new Size(152, 22);
            menuMainRandomText.Text = "Random Text";
            menuMainRandomText.Click += RandomText_Click;
            // 
            // menuMainClear
            // 
            menuMainClear.BackColor = SystemColors.Menu;
            menuMainClear.Name = "menuMainClear";
            menuMainClear.Size = new Size(152, 22);
            menuMainClear.Text = "Clear";
            menuMainClear.Click += Clear_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(149, 6);
            // 
            // menuFileExit
            // 
            menuFileExit.BackColor = SystemColors.Menu;
            menuFileExit.Name = "menuFileExit";
            menuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            menuFileExit.Size = new Size(152, 22);
            menuFileExit.Text = "Exit";
            menuFileExit.Click += menuFileExit_Click;
            // 
            // menuCompression
            // 
            menuCompression.DropDownItems.AddRange(new ToolStripItem[] { menu7z, menuBZip2, menuGZip, menuZip, menuCompressionNone });
            menuCompression.Font = new Font("Lucida Sans Unicode", 9F);
            menuCompression.Name = "menuCompression";
            menuCompression.Size = new Size(94, 20);
            menuCompression.Text = "Compression";
            // 
            // menu7z
            // 
            menu7z.Enabled = false;
            menu7z.Name = "menu7z";
            menu7z.ShortcutKeys = Keys.Control | Keys.D7;
            menu7z.Size = new Size(151, 22);
            menu7z.Text = "7z";
            menu7z.Click += menuCompression_Click;
            // 
            // menuBZip2
            // 
            menuBZip2.BackColor = SystemColors.Menu;
            menuBZip2.Name = "menuBZip2";
            menuBZip2.ShortcutKeys = Keys.Control | Keys.B;
            menuBZip2.Size = new Size(151, 22);
            menuBZip2.Text = "BZip2";
            menuBZip2.Click += menuCompression_Click;
            // 
            // menuGZip
            // 
            menuGZip.BackColor = SystemColors.Menu;
            menuGZip.Name = "menuGZip";
            menuGZip.ShortcutKeys = Keys.Control | Keys.G;
            menuGZip.Size = new Size(151, 22);
            menuGZip.Text = "GZip";
            menuGZip.Click += menuCompression_Click;
            // 
            // menuZip
            // 
            menuZip.BackColor = SystemColors.Menu;
            menuZip.Name = "menuZip";
            menuZip.ShortcutKeys = Keys.Control | Keys.Z;
            menuZip.Size = new Size(151, 22);
            menuZip.Text = "Zip";
            menuZip.Click += menuCompression_Click;
            // 
            // menuCompressionNone
            // 
            menuCompressionNone.BackColor = SystemColors.Menu;
            menuCompressionNone.Checked = true;
            menuCompressionNone.CheckState = CheckState.Checked;
            menuCompressionNone.Name = "menuCompressionNone";
            menuCompressionNone.ShortcutKeys = Keys.Control | Keys.N;
            menuCompressionNone.Size = new Size(151, 22);
            menuCompressionNone.Text = "None";
            menuCompressionNone.Click += menuCompression_Click;
            // 
            // menuEncoding
            // 
            menuEncoding.DropDownItems.AddRange(new ToolStripItem[] { menuItemNone, menuBase16, menuHex16, menuBase32, menuHex32, menuBase64, menuUu, menuXx });
            menuEncoding.Font = new Font("Lucida Sans Unicode", 9F);
            menuEncoding.Name = "menuEncoding";
            menuEncoding.ShortcutKeys = Keys.Alt | Keys.E;
            menuEncoding.Size = new Size(71, 20);
            menuEncoding.Text = "Encoding";
            // 
            // menuItemNone
            // 
            menuItemNone.BackColor = SystemColors.Menu;
            menuItemNone.Name = "menuItemNone";
            menuItemNone.Size = new Size(152, 22);
            menuItemNone.Text = "None";
            menuItemNone.Click += menuEncodingKind_Click;
            // 
            // menuBase16
            // 
            menuBase16.BackColor = SystemColors.Menu;
            menuBase16.Name = "menuBase16";
            menuBase16.Size = new Size(152, 22);
            menuBase16.Text = "Base16";
            menuBase16.Click += menuEncodingKind_Click;
            // 
            // menuHex16
            // 
            menuHex16.BackColor = SystemColors.Menu;
            menuHex16.Name = "menuHex16";
            menuHex16.Size = new Size(152, 22);
            menuHex16.Text = "Hex16";
            menuHex16.Click += menuEncodingKind_Click;
            // 
            // menuBase32
            // 
            menuBase32.BackColor = SystemColors.Menu;
            menuBase32.Name = "menuBase32";
            menuBase32.Size = new Size(152, 22);
            menuBase32.Text = "Base32";
            menuBase32.Click += menuEncodingKind_Click;
            // 
            // menuHex32
            // 
            menuHex32.BackColor = SystemColors.Menu;
            menuHex32.Name = "menuHex32";
            menuHex32.Size = new Size(152, 22);
            menuHex32.Text = "Hex32";
            menuHex32.Click += menuEncodingKind_Click;
            // 
            // menuBase64
            // 
            menuBase64.BackColor = SystemColors.Menu;
            menuBase64.Checked = true;
            menuBase64.CheckState = CheckState.Checked;
            menuBase64.Name = "menuBase64";
            menuBase64.Size = new Size(152, 22);
            menuBase64.Text = "Base64 Mime";
            menuBase64.Click += menuEncodingKind_Click;
            // 
            // menuUu
            // 
            menuUu.BackColor = SystemColors.Menu;
            menuUu.Name = "menuUu";
            menuUu.Size = new Size(152, 22);
            menuUu.Text = "Uu";
            menuUu.Click += menuEncodingKind_Click;
            // 
            // menuXx
            // 
            menuXx.Name = "menuXx";
            menuXx.Size = new Size(152, 22);
            menuXx.Text = "Xx";
            menuXx.Click += menuEncodingKind_Click;
            // 
            // menuHash
            // 
            menuHash.DropDownItems.AddRange(new ToolStripItem[] { menuHashBCrypt, menuHashMD5, menuHashHex, menuHashOpenBsd, menuHashSha1, menuHashSha256, menuHashSha512, menuHashSCrypt });
            menuHash.Font = new Font("Lucida Sans Unicode", 9F);
            menuHash.Name = "menuHash";
            menuHash.Size = new Size(48, 20);
            menuHash.Text = "Hash";
            // 
            // menuHashBCrypt
            // 
            menuHashBCrypt.BackColor = SystemColors.AppWorkspace;
            menuHashBCrypt.Name = "menuHashBCrypt";
            menuHashBCrypt.Size = new Size(164, 22);
            menuHashBCrypt.Text = "B-Crypt";
            menuHashBCrypt.Click += menuHash_Click;
            // 
            // menuHashMD5
            // 
            menuHashMD5.BackColor = SystemColors.Menu;
            menuHashMD5.Name = "menuHashMD5";
            menuHashMD5.Size = new Size(164, 22);
            menuHashMD5.Tag = "";
            menuHashMD5.Text = "MD5";
            menuHashMD5.Click += menuHash_Click;
            // 
            // menuHashHex
            // 
            menuHashHex.BackColor = SystemColors.Menu;
            menuHashHex.Checked = true;
            menuHashHex.CheckState = CheckState.Checked;
            menuHashHex.Name = "menuHashHex";
            menuHashHex.Size = new Size(164, 22);
            menuHashHex.Text = "Hex";
            menuHashHex.Click += menuHash_Click;
            // 
            // menuHashOpenBsd
            // 
            menuHashOpenBsd.BackColor = SystemColors.AppWorkspace;
            menuHashOpenBsd.Name = "menuHashOpenBsd";
            menuHashOpenBsd.Size = new Size(164, 22);
            menuHashOpenBsd.Text = "OpenBsd-Crypt";
            menuHashOpenBsd.Click += menuHash_Click;
            // 
            // menuHashSha1
            // 
            menuHashSha1.BackColor = SystemColors.Menu;
            menuHashSha1.Name = "menuHashSha1";
            menuHashSha1.Size = new Size(164, 22);
            menuHashSha1.Text = "Sha1";
            menuHashSha1.Click += menuHash_Click;
            // 
            // menuHashSha256
            // 
            menuHashSha256.BackColor = SystemColors.Menu;
            menuHashSha256.Name = "menuHashSha256";
            menuHashSha256.Size = new Size(164, 22);
            menuHashSha256.Text = "Sha256";
            menuHashSha256.Click += menuHash_Click;
            // 
            // menuHashSha512
            // 
            menuHashSha512.BackColor = SystemColors.Menu;
            menuHashSha512.Name = "menuHashSha512";
            menuHashSha512.Size = new Size(164, 22);
            menuHashSha512.Text = "Sha512";
            menuHashSha512.Click += menuHash_Click;
            // 
            // menuHashSCrypt
            // 
            menuHashSCrypt.BackColor = SystemColors.AppWorkspace;
            menuHashSCrypt.Name = "menuHashSCrypt";
            menuHashSCrypt.Size = new Size(164, 22);
            menuHashSCrypt.Text = "S-Crypt";
            menuHashSCrypt.Click += menuHash_Click;
            // 
            // menuSerialize
            // 
            menuSerialize.DropDownItems.AddRange(new ToolStripItem[] { menuJson, menuXml, menuRaw });
            menuSerialize.Enabled = false;
            menuSerialize.Font = new Font("Lucida Sans Unicode", 9F);
            menuSerialize.Name = "menuSerialize";
            menuSerialize.ShortcutKeys = Keys.Alt | Keys.S;
            menuSerialize.Size = new Size(67, 20);
            menuSerialize.Text = "Serialize";
            // 
            // menuJson
            // 
            menuJson.BackColor = SystemColors.Menu;
            menuJson.Enabled = false;
            menuJson.Name = "menuJson";
            menuJson.ShortcutKeys = Keys.Control | Keys.J;
            menuJson.Size = new Size(143, 22);
            menuJson.Text = "Json";
            // 
            // menuXml
            // 
            menuXml.BackColor = SystemColors.Menu;
            menuXml.Enabled = false;
            menuXml.Name = "menuXml";
            menuXml.ShortcutKeys = Keys.Control | Keys.X;
            menuXml.Size = new Size(143, 22);
            menuXml.Text = "Xml";
            // 
            // menuRaw
            // 
            menuRaw.BackColor = SystemColors.Menu;
            menuRaw.Enabled = false;
            menuRaw.Name = "menuRaw";
            menuRaw.ShortcutKeys = Keys.Control | Keys.R;
            menuRaw.Size = new Size(143, 22);
            menuRaw.Text = "Raw";
            // 
            // menuHelp
            // 
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout, menuHelpHelp });
            menuHelp.Font = new Font("Lucida Sans Unicode", 9F);
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(24, 20);
            menuHelp.Text = "?";
            // 
            // menuAbout
            // 
            menuAbout.BackColor = SystemColors.MenuBar;
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(147, 22);
            menuAbout.Text = "About";
            menuAbout.Click += menuAbout_Click;
            // 
            // menuHelpHelp
            // 
            menuHelpHelp.BackColor = SystemColors.MenuBar;
            menuHelpHelp.Name = "menuHelpHelp";
            menuHelpHelp.ShortcutKeys = Keys.Alt | Keys.F3;
            menuHelpHelp.Size = new Size(147, 22);
            menuHelpHelp.Text = "Help";
            // 
            // comboBoxAlgo
            // 
            comboBoxAlgo.BackColor = SystemColors.ControlLight;
            comboBoxAlgo.Font = new Font("Lucida Sans Unicode", 10F);
            comboBoxAlgo.FormattingEnabled = true;
            comboBoxAlgo.Location = new Point(101, 158);
            comboBoxAlgo.Margin = new Padding(2);
            comboBoxAlgo.MaxDropDownItems = 32;
            comboBoxAlgo.Name = "comboBoxAlgo";
            comboBoxAlgo.Size = new Size(115, 24);
            comboBoxAlgo.TabIndex = 8;
            // 
            // textBoxKey
            // 
            textBoxKey.BackColor = SystemColors.ControlLight;
            textBoxKey.Font = new Font("Lucida Sans Unicode", 11F);
            textBoxKey.Location = new Point(48, 32);
            textBoxKey.Margin = new Padding(1);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new Size(591, 30);
            textBoxKey.TabIndex = 3;
            textBoxKey.Text = "ftp@ftp.cdrom.com";
            // 
            // pictureBoxKey
            // 
            pictureBoxKey.BackColor = SystemColors.ControlLight;
            pictureBoxKey.Image = Properties.Resources.key_ring;
            pictureBoxKey.Location = new Point(8, 32);
            pictureBoxKey.Margin = new Padding(1);
            pictureBoxKey.Name = "pictureBoxKey";
            pictureBoxKey.Size = new Size(28, 29);
            pictureBoxKey.TabIndex = 4;
            pictureBoxKey.TabStop = false;
            pictureBoxKey.Click += pictureBoxKey_Click;
            // 
            // pictureBoxHash
            // 
            pictureBoxHash.BackColor = SystemColors.ControlLight;
            pictureBoxHash.Image = Properties.Resources.a_hash6;
            pictureBoxHash.Location = new Point(8, 119);
            pictureBoxHash.Margin = new Padding(1);
            pictureBoxHash.Name = "pictureBoxHash";
            pictureBoxHash.Size = new Size(28, 29);
            pictureBoxHash.TabIndex = 5;
            pictureBoxHash.TabStop = false;
            pictureBoxHash.Click += Hash_Click;
            // 
            // textBoxHash
            // 
            textBoxHash.BackColor = SystemColors.InactiveCaption;
            textBoxHash.Font = new Font("Lucida Sans Unicode", 10F);
            textBoxHash.Location = new Point(48, 119);
            textBoxHash.Margin = new Padding(1);
            textBoxHash.Name = "textBoxHash";
            textBoxHash.ReadOnly = true;
            textBoxHash.Size = new Size(591, 28);
            textBoxHash.TabIndex = 6;
            // 
            // buttonSetPipeline
            // 
            buttonSetPipeline.BackColor = SystemColors.ControlLight;
            buttonSetPipeline.Font = new Font("Lucida Sans Unicode", 10F);
            buttonSetPipeline.Location = new Point(654, 32);
            buttonSetPipeline.Margin = new Padding(1);
            buttonSetPipeline.Name = "buttonSetPipeline";
            buttonSetPipeline.Size = new Size(120, 29);
            buttonSetPipeline.TabIndex = 7;
            buttonSetPipeline.Text = "Set Pipeline";
            buttonSetPipeline.UseVisualStyleBackColor = false;
            buttonSetPipeline.Click += SetPipeline_Click;
            // 
            // buttonClear
            // 
            buttonClear.BackColor = SystemColors.ControlLight;
            buttonClear.Font = new Font("Lucida Sans Unicode", 10F);
            buttonClear.Location = new Point(654, 334);
            buttonClear.Margin = new Padding(1);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(120, 29);
            buttonClear.TabIndex = 4;
            buttonClear.Text = "Clear Form";
            buttonClear.UseVisualStyleBackColor = false;
            buttonClear.Click += Clear_Click;
            // 
            // pictureBoxFileIn
            // 
            pictureBoxFileIn.Image = Properties.Resources.image_file;
            pictureBoxFileIn.InitialImage = Properties.Resources.img_success;
            pictureBoxFileIn.Location = new Point(18, 38);
            pictureBoxFileIn.Margin = new Padding(2);
            pictureBoxFileIn.Name = "pictureBoxFileIn";
            pictureBoxFileIn.Size = new Size(64, 64);
            pictureBoxFileIn.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBoxFileIn.TabIndex = 10;
            pictureBoxFileIn.TabStop = false;
            // 
            // pictureBoxAddAlgo
            // 
            pictureBoxAddAlgo.BackColor = SystemColors.ControlLight;
            pictureBoxAddAlgo.Image = Properties.Resources.AddAesArrowHover;
            pictureBoxAddAlgo.Location = new Point(220, 158);
            pictureBoxAddAlgo.Margin = new Padding(1);
            pictureBoxAddAlgo.Name = "pictureBoxAddAlgo";
            pictureBoxAddAlgo.Size = new Size(28, 27);
            pictureBoxAddAlgo.TabIndex = 11;
            pictureBoxAddAlgo.TabStop = false;
            pictureBoxAddAlgo.Click += pictureBoxAddAlgo_Click;
            // 
            // textBoxPipe
            // 
            textBoxPipe.BackColor = SystemColors.InactiveCaption;
            textBoxPipe.Font = new Font("Lucida Sans Unicode", 9F);
            textBoxPipe.Location = new Point(252, 158);
            textBoxPipe.Margin = new Padding(1);
            textBoxPipe.Name = "textBoxPipe";
            textBoxPipe.ReadOnly = true;
            textBoxPipe.Size = new Size(378, 26);
            textBoxPipe.TabIndex = 12;
            // 
            // labelFileIn
            // 
            labelFileIn.Location = new Point(18, 111);
            labelFileIn.Margin = new Padding(2, 0, 2, 0);
            labelFileIn.Name = "labelFileIn";
            labelFileIn.Size = new Size(360, 24);
            labelFileIn.TabIndex = 13;
            labelFileIn.Text = "[Input File]";
            // 
            // labelOutputFile
            // 
            labelOutputFile.Location = new Point(386, 111);
            labelOutputFile.Margin = new Padding(2, 0, 2, 0);
            labelOutputFile.Name = "labelOutputFile";
            labelOutputFile.RightToLeft = RightToLeft.Yes;
            labelOutputFile.Size = new Size(360, 24);
            labelOutputFile.TabIndex = 15;
            labelOutputFile.Text = "[Output File]";
            labelOutputFile.Visible = false;
            // 
            // pictureBoxOutFile
            // 
            pictureBoxOutFile.Image = Properties.Resources.image_file_encrypted;
            pictureBoxOutFile.Location = new Point(679, 38);
            pictureBoxOutFile.Margin = new Padding(2);
            pictureBoxOutFile.Name = "pictureBoxOutFile";
            pictureBoxOutFile.Size = new Size(58, 69);
            pictureBoxOutFile.TabIndex = 14;
            pictureBoxOutFile.TabStop = false;
            pictureBoxOutFile.Visible = false;
            pictureBoxOutFile.Click += pictureOutBoxFile_Click;
            pictureBoxOutFile.DoubleClick += pictureOutBoxFile_Click;
            // 
            // textBoxSrc
            // 
            textBoxSrc.BackColor = SystemColors.ControlLight;
            textBoxSrc.Font = new Font("Lucida Sans Unicode", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxSrc.Location = new Point(10, 372);
            textBoxSrc.Margin = new Padding(2);
            textBoxSrc.MaxLength = 524288;
            textBoxSrc.Multiline = true;
            textBoxSrc.Name = "textBoxSrc";
            textBoxSrc.ScrollBars = ScrollBars.Vertical;
            textBoxSrc.Size = new Size(376, 200);
            textBoxSrc.TabIndex = 20;
            // 
            // textBoxOut
            // 
            textBoxOut.BackColor = SystemColors.InactiveCaption;
            textBoxOut.Font = new Font("Lucida Sans Unicode", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxOut.Location = new Point(394, 372);
            textBoxOut.Margin = new Padding(2);
            textBoxOut.MaxLength = 524288;
            textBoxOut.Multiline = true;
            textBoxOut.Name = "textBoxOut";
            textBoxOut.ReadOnly = true;
            textBoxOut.ScrollBars = ScrollBars.Vertical;
            textBoxOut.Size = new Size(380, 200);
            textBoxOut.TabIndex = 21;
            // 
            // buttonEncrypt
            // 
            buttonEncrypt.BackColor = SystemColors.ControlLight;
            buttonEncrypt.Font = new Font("Lucida Sans Unicode", 10F);
            buttonEncrypt.Location = new Point(8, 334);
            buttonEncrypt.Margin = new Padding(1);
            buttonEncrypt.Name = "buttonEncrypt";
            buttonEncrypt.Size = new Size(120, 29);
            buttonEncrypt.TabIndex = 18;
            buttonEncrypt.Text = "Encrypt";
            buttonEncrypt.UseVisualStyleBackColor = false;
            buttonEncrypt.Click += Encrypt_Click;
            // 
            // buttonDecrypt
            // 
            buttonDecrypt.BackColor = SystemColors.ControlLight;
            buttonDecrypt.Font = new Font("Lucida Sans Unicode", 10F);
            buttonDecrypt.Location = new Point(394, 334);
            buttonDecrypt.Margin = new Padding(1);
            buttonDecrypt.Name = "buttonDecrypt";
            buttonDecrypt.Size = new Size(120, 29);
            buttonDecrypt.TabIndex = 19;
            buttonDecrypt.Text = "Decrypt";
            buttonDecrypt.UseVisualStyleBackColor = false;
            buttonDecrypt.Click += Decrypt_Click;
            // 
            // groupBoxFiles
            // 
            groupBoxFiles.AllowDrop = true;
            groupBoxFiles.BackColor = SystemColors.ControlDark;
            groupBoxFiles.Controls.Add(pictureBoxFileIn);
            groupBoxFiles.Controls.Add(labelFileIn);
            groupBoxFiles.Controls.Add(pictureBoxOutFile);
            groupBoxFiles.Controls.Add(labelOutputFile);
            groupBoxFiles.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBoxFiles.Location = new Point(8, 192);
            groupBoxFiles.Margin = new Padding(2);
            groupBoxFiles.Name = "groupBoxFiles";
            groupBoxFiles.Padding = new Padding(2);
            groupBoxFiles.Size = new Size(766, 136);
            groupBoxFiles.TabIndex = 17;
            groupBoxFiles.TabStop = false;
            groupBoxFiles.Text = "Files (drag files into)";
            groupBoxFiles.DragDrop += Drag_Drop;
            groupBoxFiles.DragEnter += Drag_Enter;
            groupBoxFiles.DragOver += Drag_Over;
            groupBoxFiles.DragLeave += Drag_Leave;
            groupBoxFiles.GiveFeedback += Give_FeedBack;
            // 
            // pictureBoxDelete
            // 
            pictureBoxDelete.BackColor = SystemColors.ControlLight;
            pictureBoxDelete.Image = Properties.Resources.close_delete1;
            pictureBoxDelete.Location = new Point(747, 158);
            pictureBoxDelete.Margin = new Padding(1);
            pictureBoxDelete.Name = "pictureBoxDelete";
            pictureBoxDelete.Size = new Size(27, 27);
            pictureBoxDelete.TabIndex = 21;
            pictureBoxDelete.TabStop = false;
            pictureBoxDelete.Click += pictureBoxDelete_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // comboBoxCompression
            // 
            comboBoxCompression.BackColor = SystemColors.ControlLight;
            comboBoxCompression.Font = new Font("Lucida Sans Unicode", 10F);
            comboBoxCompression.FormattingEnabled = true;
            comboBoxCompression.Items.AddRange(new object[] { "None", "BZip2", "GZip", "Zip" });
            comboBoxCompression.Location = new Point(8, 158);
            comboBoxCompression.Margin = new Padding(2);
            comboBoxCompression.MaxDropDownItems = 32;
            comboBoxCompression.Name = "comboBoxCompression";
            comboBoxCompression.Size = new Size(82, 24);
            comboBoxCompression.TabIndex = 22;
            comboBoxCompression.SelectedIndexChanged += ComboBoxCompression_SelectedIndexChanged;
            // 
            // comboBoxEncoding
            // 
            comboBoxEncoding.BackColor = SystemColors.ControlLight;
            comboBoxEncoding.Font = new Font("Lucida Sans Unicode", 10F);
            comboBoxEncoding.FormattingEnabled = true;
            comboBoxEncoding.Items.AddRange(new object[] { "None", "Base16", "Hex16", "Base32", "Hex32", "Base64", "Uu", "Xx" });
            comboBoxEncoding.Location = new Point(633, 158);
            comboBoxEncoding.Margin = new Padding(2);
            comboBoxEncoding.MaxDropDownItems = 32;
            comboBoxEncoding.Name = "comboBoxEncoding";
            comboBoxEncoding.Size = new Size(108, 24);
            comboBoxEncoding.TabIndex = 23;
            comboBoxEncoding.SelectedIndexChanged += comboBoxEncoding_SelectedIndexChanged;
            // 
            // buttonRandomText
            // 
            buttonRandomText.BackColor = SystemColors.ControlLight;
            buttonRandomText.Font = new Font("Lucida Sans Unicode", 10F);
            buttonRandomText.Location = new Point(266, 334);
            buttonRandomText.Margin = new Padding(1);
            buttonRandomText.Name = "buttonRandomText";
            buttonRandomText.Size = new Size(120, 29);
            buttonRandomText.TabIndex = 24;
            buttonRandomText.Text = "Random Text";
            buttonRandomText.UseVisualStyleBackColor = false;
            buttonRandomText.Click += RandomText_Click;
            // 
            // buttonHashPipe
            // 
            buttonHashPipe.BackColor = SystemColors.ControlLight;
            buttonHashPipe.Font = new Font("Lucida Sans Unicode", 10F);
            buttonHashPipe.Location = new Point(654, 119);
            buttonHashPipe.Margin = new Padding(1);
            buttonHashPipe.Name = "buttonHashPipe";
            buttonHashPipe.Size = new Size(120, 29);
            buttonHashPipe.TabIndex = 25;
            buttonHashPipe.Text = "Hash Pipe";
            buttonHashPipe.UseVisualStyleBackColor = false;
            buttonHashPipe.Click += Hash_Pipe_Click;
            // 
            // radioButtonListHash
            // 
            radioButtonListHash.BackColor = SystemColors.ControlDark;
            radioButtonListHash.Font = new Font("Lucida Sans Unicode", 10F);
            radioButtonListHash.FormattingEnabled = true;
            radioButtonListHash.HorizontalExtent = 1;
            radioButtonListHash.Items.AddRange(new object[] { "BCrypt", "Hex", "MD5", "OpenBSDCrypt", "SCrypt", "Sha1", "Sha256", "Sha512" });
            radioButtonListHash.Location = new Point(8, 66);
            radioButtonListHash.Margin = new Padding(1);
            radioButtonListHash.MultiColumn = true;
            radioButtonListHash.Name = "radioButtonListHash";
            radioButtonListHash.Size = new Size(766, 50);
            radioButtonListHash.Sorted = true;
            radioButtonListHash.TabIndex = 26;
            radioButtonListHash.SelectedIndexChanged += RadioButtonListHash_SelectedIndexChanged;
            // 
            // EncryptFormSimple
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(784, 581);
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
            Controls.Add(textBoxSrc);
            Controls.Add(textBoxPipe);
            Controls.Add(pictureBoxAddAlgo);
            Controls.Add(buttonClear);
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
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EncryptFormSimple";
            Opacity = 0.84D;
            Text = "EncryptFormSimple";
            FormClosed += menuFileExit_Close;
            Load += EncryptForm_Load;
            menuStripEncrypt.ResumeLayout(false);
            menuStripEncrypt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource2).EndInit();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHash).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileIn).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAddAlgo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutFile).EndInit();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource1).EndInit();
            groupBoxFiles.ResumeLayout(false);
            groupBoxFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDelete).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripEncrypt;
        private ToolStripMenuItem toolMenuMain;
        private ToolStripMenuItem menuFileOpen;
        private ToolStripMenuItem menuMainDecrypt;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem menuFileExit;
        private ToolStripMenuItem menuCompression;
        private ToolStripMenuItem menuBZip2;
        private ToolStripMenuItem menuZip;
        private ToolStripMenuItem menu7z;
        private ToolStripMenuItem menuGZip;
        private ToolStripMenuItem menuCompressionNone;
        private ToolStripMenuItem menuEncoding;
        private ToolStripMenuItem menuBase16;
        private ToolStripMenuItem menuHex16;
        private ToolStripMenuItem menuBase32;
        private ToolStripMenuItem menuHex32;
        private ToolStripMenuItem menuBase64;
        private ToolStripMenuItem menuUu;
        private ToolStripMenuItem menuSerialize;
        private ToolStripMenuItem menuJson;
        private ToolStripMenuItem menuXml;
        private ToolStripMenuItem menuRaw;
        private ToolStripMenuItem menuHash;
        private ToolStripMenuItem menuHashBCrypt;
        private ToolStripMenuItem menuHashSCrypt;
        private ToolStripMenuItem menuHashMD5;
        private ToolStripMenuItem menuHashSha1;
        private ToolStripMenuItem menuHashSha512;
        private ToolStripMenuItem menuHashOpenBsd;
        private ToolStripMenuItem menuHashSha256;
        private ToolStripMenuItem menuHashHex;
        private ComboBox comboBoxAlgo;
        private TextBox textBoxKey;
        private PictureBox pictureBoxKey;
        private PictureBox pictureBoxHash;
        private TextBox textBoxHash;
        private Button buttonSetPipeline;
        private Button buttonClear;
        private BindingSource enumOptionsBindingSource;
        private BindingSource cipherEnumBindingSource;
        private PictureBox pictureBoxFileIn;
        private PictureBox pictureBoxAddAlgo;
        private TextBox textBoxPipe;
        private Label labelFileIn;
        private Label labelOutputFile;
        private PictureBox pictureBoxOutFile;
        private TextBox textBoxSrc;
        private TextBox textBoxOut;
        private ToolStripMenuItem menuMainSave;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem menuMainEncrypt;
        private ToolStripMenuItem menuMainClear;
        private ToolStripMenuItem menuMainRandomText;
        private ToolStripMenuItem menuMainHashKey;
        private ToolStripMenuItem menuMainSetPipe;
        private ToolStripSeparator toolStripSeparator3;
        private Button buttonEncrypt;
        private Button buttonDecrypt;
        private BindingSource cipherEnumBindingSource2;
        private BindingSource cipherEnumBindingSource1;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuAbout;
        private ToolStripMenuItem menuHelpHelp;
        internal GroupBox groupBoxFiles;
        private ToolStripMenuItem menuItemNone;
        private PictureBox pictureBoxDelete;
        private NotifyIcon notifyIcon1;
        private ToolStripMenuItem menuXx;
        private ComboBox comboBoxCompression;
        private ComboBox comboBoxEncoding;
        private Button buttonRandomText;
        private Button buttonHashPipe;
        private ToolStripMenuItem menuMainHashPipe;
        private Controls.RadioButtonList radioButtonListHash;
    }
}