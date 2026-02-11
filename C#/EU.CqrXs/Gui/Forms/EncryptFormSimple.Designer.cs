using System.Windows.Forms;

namespace EU.CqrXs.Gui.Forms
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
            menuMainSetPipe = new ToolStripMenuItem();
            menuMainHashPipe = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            menuMainEncrypt = new ToolStripMenuItem();
            menuMainDecrypt = new ToolStripMenuItem();
            menuMainDownloadImage = new ToolStripMenuItem();
            menuMainRandomText = new ToolStripMenuItem();
            menuMainReset = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuMainComplex = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            menuFileExit = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            cipherModeToolStripMenuItem = new ToolStripMenuItem();
            menuCipherModeItemCBC = new ToolStripMenuItem();
            menuCipherModeItemCCM = new ToolStripMenuItem();
            menuCipherModeItemCFB = new ToolStripMenuItem();
            menuCipherModeItemCTS = new ToolStripMenuItem();
            menuCipherModeItemEAX = new ToolStripMenuItem();
            menuCipherModeItemECB = new ToolStripMenuItem();
            menuCipherModeItemGOFB = new ToolStripMenuItem();
            verifyEncryptionToolStripMenuItem = new ToolStripMenuItem();
            sha512ToolStripMenuItem = new ToolStripMenuItem();
            bytesOfFileToolStripMenuItem = new ToolStripMenuItem();
            menuOptionsMenuFileSettings = new ToolStripMenuItem();
            warnOnEmptyPipeToolStripMenuItem = new ToolStripMenuItem();
            menuItemCreatePipeSettingsFromFileName = new ToolStripMenuItem();
            menuFileSettingsItemAutomaticallySaveToTemp = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            menuHelpHelp = new ToolStripMenuItem();
            menuHelpCharHexDecOctBin = new ToolStripMenuItem();
            menuHelpUrlFetch = new ToolStripMenuItem();
            menuOptionsMenuWindowsCharHexDecOctBin = new ToolStripMenuItem();
            menuOptionsMenuWindowsitemAbout = new ToolStripMenuItem();
            enumOptionsBindingSource = new BindingSource(components);
            textBoxKey = new TextBox();
            pictureBoxKey = new PictureBox();
            buttonSetPipeline = new Button();
            buttonReset = new Button();
            textBoxPipe = new TextBox();
            buttonEncrypt = new Button();
            buttonDecrypt = new Button();
            pictureBoxDelete = new PictureBox();
            buttonRandomText = new Button();
            buttonHashPipe = new Button();
            labelInfoMessage = new Label();
            statusStrip = new StatusStrip();
            statusLabelSource = new ToolStripStatusLabel();
            statusLabelMsg = new ToolStripStatusLabel();
            statusLabelDestination = new ToolStripStatusLabel();
            groupBoxFiles = new EU.CqrXs.Gui.Controls.GroupBoxFiles();
            panel1 = new Panel();
            tabControlWithHexSrc = new EU.CqrXs.Gui.Controls.TabControlWithHex();
            tabControlWithHexDest = new EU.CqrXs.Gui.Controls.TabControlWithHex();
            labelGZip = new Label();
            labelBase64 = new Label();
            menuStripEncrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDelete).BeginInit();
            statusStrip.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripEncrypt
            // 
            menuStripEncrypt.AllowMerge = false;
            menuStripEncrypt.BackColor = SystemColors.MenuBar;
            menuStripEncrypt.Font = new Font("Lucida Sans Typewriter", 9F);
            menuStripEncrypt.Items.AddRange(new ToolStripItem[] { toolMenuMain, optionsToolStripMenuItem, menuHelp });
            menuStripEncrypt.Location = new Point(0, 0);
            menuStripEncrypt.Name = "menuStripEncrypt";
            menuStripEncrypt.Padding = new Padding(3, 2, 2, 2);
            menuStripEncrypt.Size = new Size(1004, 24);
            menuStripEncrypt.TabIndex = 0;
            menuStripEncrypt.Text = "menuStripEncrypt";
            // 
            // toolMenuMain
            // 
            toolMenuMain.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuMainSave, toolStripSeparator2, menuMainSetPipe, menuMainHashPipe, toolStripSeparator3, menuMainEncrypt, menuMainDecrypt, menuMainDownloadImage, menuMainRandomText, menuMainReset, toolStripSeparator1, menuMainComplex, toolStripSeparator4, menuFileExit });
            toolMenuMain.Font = new Font("Lucida Sans Typewriter", 10F);
            toolMenuMain.ForeColor = SystemColors.MenuText;
            toolMenuMain.Name = "toolMenuMain";
            toolMenuMain.Size = new Size(51, 20);
            toolMenuMain.Text = "Main";
            // 
            // menuFileOpen
            // 
            menuFileOpen.BackColor = SystemColors.Menu;
            menuFileOpen.ForeColor = SystemColors.MenuText;
            menuFileOpen.Name = "menuFileOpen";
            menuFileOpen.ShortcutKeys = Keys.Control | Keys.O;
            menuFileOpen.Size = new Size(170, 22);
            menuFileOpen.Text = "Open";
            menuFileOpen.Click += menuFileOpen_Click;
            // 
            // menuMainSave
            // 
            menuMainSave.BackColor = SystemColors.Menu;
            menuMainSave.ForeColor = SystemColors.MenuText;
            menuMainSave.Name = "menuMainSave";
            menuMainSave.ShortcutKeys = Keys.Control | Keys.S;
            menuMainSave.Size = new Size(170, 22);
            menuMainSave.Text = "Save";
            menuMainSave.Click += menuMainSave_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.BackColor = SystemColors.Menu;
            toolStripSeparator2.ForeColor = SystemColors.MenuText;
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(167, 6);
            // 
            // menuMainSetPipe
            // 
            menuMainSetPipe.BackColor = SystemColors.Menu;
            menuMainSetPipe.ForeColor = SystemColors.MenuText;
            menuMainSetPipe.Name = "menuMainSetPipe";
            menuMainSetPipe.Size = new Size(170, 22);
            menuMainSetPipe.Text = "Set Pipe";
            menuMainSetPipe.Click += SetPipeline_Click;    
            // 
            // menuMainHashPipe
            // 
            menuMainHashPipe.BackColor = SystemColors.Menu;
            menuMainHashPipe.ForeColor = SystemColors.MenuText;
            menuMainHashPipe.Name = "menuMainHashPipe";
            menuMainHashPipe.Size = new Size(170, 22);
            menuMainHashPipe.Text = "Hash Pipe";
            menuMainHashPipe.Click += Hash_Pipe_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.BackColor = SystemColors.Menu;
            toolStripSeparator3.ForeColor = SystemColors.MenuHighlight;
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(167, 6);
            // 
            // menuMainEncrypt
            // 
            menuMainEncrypt.BackColor = SystemColors.Menu;
            menuMainEncrypt.ForeColor = SystemColors.MenuText;
            menuMainEncrypt.Name = "menuMainEncrypt";
            menuMainEncrypt.Size = new Size(170, 22);
            menuMainEncrypt.Text = "Encrypt";
            // 
            // menuMainDecrypt
            // 
            menuMainDecrypt.BackColor = SystemColors.Menu;
            menuMainDecrypt.ForeColor = SystemColors.MenuText;
            menuMainDecrypt.Name = "menuMainDecrypt";
            menuMainDecrypt.Size = new Size(170, 22);
            menuMainDecrypt.Text = "Decrypt";
            // 
            // menuMainDownloadImage
            // 
            menuMainDownloadImage.BackColor = SystemColors.Menu;
            menuMainDownloadImage.ForeColor = SystemColors.MenuText;
            menuMainDownloadImage.Name = "menuMainDownloadImage";
            menuMainDownloadImage.Size = new Size(170, 22);
            menuMainDownloadImage.Text = "Ramdom Image";
            menuMainDownloadImage.Click += LoadImage_Click;
            // 
            // menuMainRandomText
            // 
            menuMainRandomText.BackColor = SystemColors.Menu;
            menuMainRandomText.ForeColor = SystemColors.MenuText;
            menuMainRandomText.Name = "menuMainRandomText";
            menuMainRandomText.Size = new Size(170, 22);
            menuMainRandomText.Text = "Random Text";
            menuMainRandomText.Click += RandomText_Click;
            // 
            // menuMainReset
            // 
            menuMainReset.BackColor = SystemColors.Menu;
            menuMainReset.ForeColor = SystemColors.MenuText;
            menuMainReset.Name = "menuMainReset";
            menuMainReset.Size = new Size(170, 22);
            menuMainReset.Text = "Reset";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.BackColor = SystemColors.Menu;
            toolStripSeparator1.ForeColor = SystemColors.MenuHighlight;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(167, 6);
            // 
            // menuMainComplex
            // 
            menuMainComplex.BackColor = SystemColors.Menu;
            menuMainComplex.ForeColor = SystemColors.MenuText;
            menuMainComplex.ImageAlign = ContentAlignment.MiddleLeft;
            menuMainComplex.Name = "menuMainComplex";
            menuMainComplex.Size = new Size(170, 22);
            menuMainComplex.Text = "Complex Mode";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.BackColor = SystemColors.Menu;
            toolStripSeparator4.ForeColor = SystemColors.MenuText;
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(167, 6);
            // 
            // menuFileExit
            // 
            menuFileExit.BackColor = SystemColors.Menu;
            menuFileExit.ForeColor = SystemColors.MenuText;
            menuFileExit.Name = "menuFileExit";
            menuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            menuFileExit.Size = new Size(170, 22);
            menuFileExit.Text = "Exit";
            menuFileExit.Click += menuFileExit_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.BackColor = SystemColors.MenuBar;
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cipherModeToolStripMenuItem, verifyEncryptionToolStripMenuItem, menuOptionsMenuFileSettings });
            optionsToolStripMenuItem.Font = new Font("Lucida Sans Typewriter", 10F);
            optionsToolStripMenuItem.ForeColor = SystemColors.MenuText;
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(75, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // cipherModeToolStripMenuItem
            // 
            cipherModeToolStripMenuItem.BackColor = SystemColors.Menu;
            cipherModeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuCipherModeItemCBC, menuCipherModeItemCCM, menuCipherModeItemCFB, menuCipherModeItemCTS, menuCipherModeItemEAX, menuCipherModeItemECB, menuCipherModeItemGOFB });
            cipherModeToolStripMenuItem.Font = new Font("Lucida Sans Typewriter", 10F);
            cipherModeToolStripMenuItem.ForeColor = SystemColors.MenuText;
            cipherModeToolStripMenuItem.Name = "cipherModeToolStripMenuItem";
            cipherModeToolStripMenuItem.Size = new Size(210, 22);
            cipherModeToolStripMenuItem.Text = "CipherMode";
            // 
            // menuCipherModeItemCBC
            // 
            menuCipherModeItemCBC.BackColor = SystemColors.Menu;
            menuCipherModeItemCBC.ForeColor = SystemColors.MenuText;
            menuCipherModeItemCBC.Name = "menuCipherModeItemCBC";
            menuCipherModeItemCBC.Size = new Size(106, 22);
            menuCipherModeItemCBC.Text = "CBC";
            // 
            // menuCipherModeItemCCM
            // 
            menuCipherModeItemCCM.BackColor = SystemColors.Menu;
            menuCipherModeItemCCM.Enabled = false;
            menuCipherModeItemCCM.ForeColor = SystemColors.MenuText;
            menuCipherModeItemCCM.Name = "menuCipherModeItemCCM";
            menuCipherModeItemCCM.Size = new Size(106, 22);
            menuCipherModeItemCCM.Text = "CCM";
            // 
            // menuCipherModeItemCFB
            // 
            menuCipherModeItemCFB.BackColor = SystemColors.Menu;
            menuCipherModeItemCFB.ForeColor = SystemColors.MenuText;
            menuCipherModeItemCFB.CheckState = CheckState.Checked;
            menuCipherModeItemCFB.Name = "menuCipherModeItemCFB";
            menuCipherModeItemCFB.Size = new Size(106, 22);
            menuCipherModeItemCFB.Text = "CFB";
            menuCipherModeItemCFB.Checked = true;
            // 
            // menuCipherModeItemCTS
            // 
            menuCipherModeItemCTS.BackColor = SystemColors.Menu;
            menuCipherModeItemCTS.Enabled = false;
            menuCipherModeItemCTS.ForeColor = SystemColors.MenuText;
            menuCipherModeItemCTS.Name = "menuCipherModeItemCTS";
            menuCipherModeItemCTS.Size = new Size(106, 22);
            menuCipherModeItemCTS.Text = "CTS";
            // 
            // menuCipherModeItemEAX
            // 
            menuCipherModeItemEAX.BackColor = SystemColors.Menu;
            menuCipherModeItemEAX.Enabled = false;
            menuCipherModeItemEAX.ForeColor = SystemColors.MenuText;
            menuCipherModeItemEAX.Name = "menuCipherModeItemEAX";
            menuCipherModeItemEAX.Size = new Size(106, 22);
            menuCipherModeItemEAX.Text = "EAX";
            // 
            // menuCipherModeItemECB
            // 
            menuCipherModeItemECB.BackColor = SystemColors.Menu;
            menuCipherModeItemECB.Checked = false;
            menuCipherModeItemECB.CheckState = CheckState.Unchecked;
            menuCipherModeItemECB.ForeColor = SystemColors.MenuText;
            menuCipherModeItemECB.Name = "menuCipherModeItemECB";
            menuCipherModeItemECB.Size = new Size(106, 22);
            menuCipherModeItemECB.Text = "ECB";
            // 
            // menuCipherModeItemGOFB
            // 
            menuCipherModeItemGOFB.BackColor = SystemColors.Menu;
            menuCipherModeItemGOFB.Enabled = false;
            menuCipherModeItemGOFB.ForeColor = SystemColors.MenuText;
            menuCipherModeItemGOFB.Name = "menuCipherModeItemGOFB";
            menuCipherModeItemGOFB.Size = new Size(106, 22);
            menuCipherModeItemGOFB.Text = "GOFB";
            // 
            // verifyEncryptionToolStripMenuItem
            // 
            verifyEncryptionToolStripMenuItem.BackColor = SystemColors.Menu;
            verifyEncryptionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { sha512ToolStripMenuItem, bytesOfFileToolStripMenuItem });
            verifyEncryptionToolStripMenuItem.ForeColor = SystemColors.MenuText;
            verifyEncryptionToolStripMenuItem.Name = "verifyEncryptionToolStripMenuItem";
            verifyEncryptionToolStripMenuItem.Size = new Size(210, 22);
            verifyEncryptionToolStripMenuItem.Text = "Verify Encryption";
            // 
            // sha512ToolStripMenuItem
            // 
            sha512ToolStripMenuItem.BackColor = SystemColors.Menu;
            sha512ToolStripMenuItem.Checked = true;
            sha512ToolStripMenuItem.CheckOnClick = true;
            sha512ToolStripMenuItem.CheckState = CheckState.Checked;
            sha512ToolStripMenuItem.ForeColor = SystemColors.MenuText;
            sha512ToolStripMenuItem.Name = "sha512ToolStripMenuItem";
            sha512ToolStripMenuItem.Size = new Size(210, 22);
            sha512ToolStripMenuItem.Text = "sha512 hash";
            // 
            // bytesOfFileToolStripMenuItem
            // 
            bytesOfFileToolStripMenuItem.BackColor = SystemColors.Menu;
            bytesOfFileToolStripMenuItem.CheckOnClick = true;
            bytesOfFileToolStripMenuItem.ForeColor = SystemColors.MenuText;
            bytesOfFileToolStripMenuItem.Name = "bytesOfFileToolStripMenuItem";
            bytesOfFileToolStripMenuItem.Size = new Size(210, 22);
            bytesOfFileToolStripMenuItem.Text = "1/8 bytes of file";
            // 
            // menuOptionsMenuFileSettings
            // 
            menuOptionsMenuFileSettings.BackColor = SystemColors.Menu;
            menuOptionsMenuFileSettings.DropDownItems.AddRange(new ToolStripItem[] { warnOnEmptyPipeToolStripMenuItem, menuItemCreatePipeSettingsFromFileName, menuFileSettingsItemAutomaticallySaveToTemp });
            menuOptionsMenuFileSettings.ForeColor = SystemColors.MenuText;
            menuOptionsMenuFileSettings.Name = "menuOptionsMenuFileSettings";
            menuOptionsMenuFileSettings.Size = new Size(210, 22);
            menuOptionsMenuFileSettings.Text = "Settings";
            // 
            // warnOnEmptyPipeToolStripMenuItem
            // 
            warnOnEmptyPipeToolStripMenuItem.BackColor = SystemColors.Menu;
            warnOnEmptyPipeToolStripMenuItem.CheckOnClick = true;
            warnOnEmptyPipeToolStripMenuItem.ForeColor = SystemColors.MenuText;
            warnOnEmptyPipeToolStripMenuItem.Name = "warnOnEmptyPipeToolStripMenuItem";
            warnOnEmptyPipeToolStripMenuItem.Size = new Size(346, 22);
            warnOnEmptyPipeToolStripMenuItem.Text = "Warn on empty pipe";
            warnOnEmptyPipeToolStripMenuItem.ToolTipText = "Warn on en-/decrypting when cipher pipe is empty";
            // 
            // menuItemCreatePipeSettingsFromFileName
            // 
            menuItemCreatePipeSettingsFromFileName.BackColor = SystemColors.Menu;
            menuItemCreatePipeSettingsFromFileName.CheckOnClick = true;
            menuItemCreatePipeSettingsFromFileName.ForeColor = SystemColors.MenuText;
            menuItemCreatePipeSettingsFromFileName.Name = "menuItemCreatePipeSettingsFromFileName";
            menuItemCreatePipeSettingsFromFileName.Size = new Size(346, 22);
            menuItemCreatePipeSettingsFromFileName.Text = "Create Pipe Settings from FileName";
            menuItemCreatePipeSettingsFromFileName.ToolTipText = "Creates Cipher Pipe, hash, encode and zip settings in Form from opened  fileName";
            // 
            // menuFileSettingsItemAutomaticallySaveToTemp
            // 
            menuFileSettingsItemAutomaticallySaveToTemp.BackColor = SystemColors.Menu;
            menuFileSettingsItemAutomaticallySaveToTemp.Checked = true;
            menuFileSettingsItemAutomaticallySaveToTemp.CheckOnClick = true;
            menuFileSettingsItemAutomaticallySaveToTemp.CheckState = CheckState.Checked;
            menuFileSettingsItemAutomaticallySaveToTemp.Name = "menuFileSettingsItemAutomaticallySaveToTemp";
            menuFileSettingsItemAutomaticallySaveToTemp.Size = new Size(346, 22);
            menuFileSettingsItemAutomaticallySaveToTemp.Text = "Automatically Save to Temp";
            menuFileSettingsItemAutomaticallySaveToTemp.ToolTipText = "Don't show a save file dialog, when processimg files";
            // 
            // menuHelp
            // 
            menuHelp.BackColor = SystemColors.MenuBar;
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout, menuHelpHelp, menuHelpCharHexDecOctBin });
            menuHelp.Font = new Font("Lucida Sans Typewriter", 10F);
            menuHelp.ForeColor = SystemColors.MenuText;
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(27, 20);
            menuHelp.Text = "?";
            // 
            // menuAbout
            // 
            menuAbout.BackColor = SystemColors.Menu;
            menuAbout.ForeColor = SystemColors.MenuText;
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(202, 22);
            menuAbout.Text = "About";
            // 
            // menuHelpHelp
            // 
            menuHelpHelp.BackColor = SystemColors.Menu;
            menuHelpHelp.ForeColor = SystemColors.MenuText;
            menuHelpHelp.Name = "menuHelpHelp";
            menuHelpHelp.ShortcutKeys = Keys.Alt | Keys.F3;
            menuHelpHelp.Size = new Size(202, 22);
            menuHelpHelp.Text = "Help";
            // 
            // menuHelpCharHexDecOctBin
            // 
            menuHelpCharHexDecOctBin.BackColor = SystemColors.Menu;
            menuHelpCharHexDecOctBin.ForeColor = SystemColors.MenuText;
            menuHelpCharHexDecOctBin.Name = "menuHelpCharHexDecOctBin";
            menuHelpCharHexDecOctBin.Size = new Size(202, 22);
            menuHelpCharHexDecOctBin.Text = "CharHexDecOctBin";
            // 
            // menuHelpUrlFetch
            // 
            menuHelpUrlFetch.Name = "menuHelpUrlFetch";
            menuHelpUrlFetch.Size = new Size(32, 19);
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
            // pictureBoxKey
            // 
            pictureBoxKey.BackColor = SystemColors.Control;
            pictureBoxKey.Image = Properties.Resources.key_ring;
            pictureBoxKey.Location = new Point(8, 27);
            pictureBoxKey.Margin = new Padding(1);
            pictureBoxKey.Name = "pictureBoxKey";
            pictureBoxKey.Size = new Size(30, 30);
            pictureBoxKey.TabIndex = 3;
            pictureBoxKey.TabStop = false;
            pictureBoxKey.Click += pictureBoxKey_Click;
            // 
            // textBoxKey
            // 
            textBoxKey.BackColor = SystemColors.ControlLightLight;
            textBoxKey.AutoSize = false;
            textBoxKey.Font = new Font("Lucida Sans Typewriter", 10F);
            textBoxKey.Location = new Point(48, 27);
            textBoxKey.Margin = new Padding(1);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new Size(813, 27);
            textBoxKey.TabIndex = 4;
            textBoxKey.Text = "ftp@ftp.cdrom.com";
            textBoxKey.TextChanged += textBoxKey_TextChanged;
            // 
            // buttonSetPipeline
            // 
            buttonSetPipeline.BackColor = SystemColors.Control;
            buttonSetPipeline.Font = new Font("Lucida Sans Typewriter", 10F);
            buttonSetPipeline.Location = new Point(876, 27);
            buttonSetPipeline.Margin = new Padding(1);
            buttonSetPipeline.Name = "buttonSetPipeline";
            buttonSetPipeline.Size = new Size(120, 27);
            buttonSetPipeline.TabIndex = 5;
            buttonSetPipeline.Text = "Set Pipeline";
            buttonSetPipeline.UseVisualStyleBackColor = false;
            buttonSetPipeline.Click += SetPipeline_Click;            
            // 
            // labelGZip
            // 
            labelGZip.AutoSize = false;
            labelGZip.Location = new Point(4, 64);
            labelGZip.Name = "labelGZip";
            labelGZip.Size = new Size(36, 27);
            labelGZip.TabIndex = 13;
            labelGZip.Text = "GZip";
            // 
            // textBoxPipe
            // 
            textBoxPipe.BackColor = SystemColors.ControlLightLight;
            textBoxPipe.AutoSize = false;
            textBoxPipe.Font = new Font("Lucida Sans Typewriter", 10F);
            textBoxPipe.Location = new Point(43, 64);
            textBoxPipe.Margin = new Padding(1);
            textBoxPipe.MaxLength = 8192;
            textBoxPipe.Name = "textBoxPipe";
            textBoxPipe.ReadOnly = true;
            textBoxPipe.Size = new Size(720, 27);
            textBoxPipe.TabIndex = 14;
            // 
            // pictureBoxDelete
            // 
            pictureBoxDelete.BackColor = SystemColors.Control;
            pictureBoxDelete.Image = Properties.Resources.image_delete;
            pictureBoxDelete.Location = new Point(774, 64);
            pictureBoxDelete.Margin = new Padding(1);
            pictureBoxDelete.Name = "pictureBoxDelete";
            pictureBoxDelete.Size = new Size(27, 27);
            pictureBoxDelete.TabIndex = 15;
            pictureBoxDelete.TabStop = false;
            pictureBoxDelete.Click += pictureBoxDelete_Click;
            // 
            // labelBase64
            // 
            labelBase64.AutoSize = false;
            labelBase64.Location = new Point(812, 64);
            labelBase64.Name = "labelBase64";
            labelBase64.Size = new Size(54, 27);
            labelBase64.TabIndex = 16;
            labelBase64.Text = "Base64";
            // 
            // buttonHashPipe
            // 
            buttonHashPipe.BackColor = SystemColors.Control;
            buttonHashPipe.Font = new Font("Lucida Sans Typewriter", 10F);
            buttonHashPipe.Location = new Point(876, 64);
            buttonHashPipe.Margin = new Padding(1);
            buttonHashPipe.Name = "buttonHashPipe";
            buttonHashPipe.Size = new Size(120, 27);
            buttonHashPipe.TabIndex = 18;
            buttonHashPipe.Text = "Hash Pipe";
            buttonHashPipe.UseVisualStyleBackColor = false;
            buttonHashPipe.Click += Hash_Pipe_Click;
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
            // statusStrip
            // 
            statusStrip.Font = new Font("Lucida Sans Typewriter", 9F);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabelSource, statusLabelMsg, statusLabelDestination });
            statusStrip.Location = new Point(0, 703);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1004, 22);
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
            groupBoxFiles.Location = new Point(8, 97);
            groupBoxFiles.Margin = new Padding(1);
            groupBoxFiles.Name = "groupBoxFiles";
            groupBoxFiles.Padding = new Padding(1);
            groupBoxFiles.Size = new Size(996, 156);
            groupBoxFiles.TabIndex = 18;
            groupBoxFiles.TabStop = false;
            groupBoxFiles.Text = "groupBoxFiles";
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
            panel1.Location = new Point(0, 257);
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
            tabControlWithHexSrc.Location = new Point(0, 300);
            tabControlWithHexSrc.Margin = new Padding(1);
            tabControlWithHexSrc.Name = "tabControlWithHexSrc";
            tabControlWithHexSrc.Padding = new Point(1, 1);
            tabControlWithHexSrc.SelectedIndex = 0;
            tabControlWithHexSrc.Size = new Size(504, 400);
            tabControlWithHexSrc.TabIndex = 40;
            // 
            // tabControlWithHexDest
            // 
            tabControlWithHexDest.Font = new Font("Lucida Console", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControlWithHexDest.ItemSize = new Size(72, 19);
            tabControlWithHexDest.Location = new Point(506, 300);
            tabControlWithHexDest.Margin = new Padding(1);
            tabControlWithHexDest.Name = "tabControlWithHexDest";
            tabControlWithHexDest.Padding = new Point(1, 1);
            tabControlWithHexDest.SelectedIndex = 0;
            tabControlWithHexDest.Size = new Size(502, 400);
            tabControlWithHexDest.TabIndex = 46;            
            // 
            // EncryptFormSimple
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1004, 725);
            Controls.Add(labelBase64);
            Controls.Add(labelGZip);
            Controls.Add(tabControlWithHexDest);
            Controls.Add(tabControlWithHexSrc);
            Controls.Add(pictureBoxDelete);
            Controls.Add(textBoxPipe);
            Controls.Add(panel1);
            Controls.Add(groupBoxFiles);
            Controls.Add(statusStrip);
            Controls.Add(buttonHashPipe);
            Controls.Add(buttonSetPipeline);
            Controls.Add(pictureBoxKey);
            Controls.Add(textBoxKey);
            Controls.Add(menuStripEncrypt);
            Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStripEncrypt;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MaximumSize = new Size(1024, 768);
            MinimizeBox = false;
            Name = "EncryptFormSimple";
            Opacity = 0.96D;
            Text = "EncryptFormSimple";
            FormClosed += menuFileExit_Close;
            menuStripEncrypt.ResumeLayout(false);
            menuStripEncrypt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDelete).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private BindingSource enumOptionsBindingSource;
        protected internal PictureBox pictureBoxKey;
        protected internal Button buttonSetPipeline;
        protected internal Button buttonHashPipe;
        protected internal MenuStrip menuStripEncrypt;
        protected internal ToolStripMenuItem toolMenuMain;
        protected internal ToolStripMenuItem menuFileOpen;
        protected internal ToolStripMenuItem menuMainDecrypt;
        protected internal ToolStripSeparator toolStripSeparator1;
        protected internal ToolStripMenuItem menuFileExit;
        protected internal Button buttonReset;
        protected internal TextBox textBoxPipe;
        protected internal ToolStripMenuItem menuMainSave;
        protected internal ToolStripSeparator toolStripSeparator2;
        protected internal ToolStripMenuItem menuMainEncrypt;
        protected internal ToolStripMenuItem menuMainReset;
        protected internal ToolStripMenuItem menuMainRandomText;
        protected internal ToolStripMenuItem menuMainSetPipe;
        protected internal ToolStripSeparator toolStripSeparator3;
        protected internal Button buttonEncrypt;
        protected internal Button buttonDecrypt;
        protected internal PictureBox pictureBoxDelete;
        protected internal Button buttonRandomText;
        internal  ToolStripMenuItem menuMainHashPipe;
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
        private ToolStripMenuItem menuOptionsMenuFileSettings;
        private ToolStripMenuItem menuItemCreatePipeSettingsFromFileName;
        private ToolStripMenuItem menuFileSettingsItemAutomaticallySaveToTemp;
        private Controls.GroupBoxFiles groupBoxFiles;
        private Panel panel1;
        internal ToolStripMenuItem menuHelpCharHexDecOctBin;
        private Controls.TabControlWithHex tabControlWithHexSrc;
        private Controls.TabControlWithHex tabControlWithHexDest;
        protected internal ToolStripMenuItem menuMainComplex;
        internal ToolStripMenuItem menuOptionsMenuWindowsCharHexDecOctBin;
        internal ToolStripMenuItem menuOptionsMenuWindowsitemAbout;
        protected internal ToolStripMenuItem menuMainDownloadImage;
        internal ToolStripMenuItem menuHelpUrlFetch;
        protected internal ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem cipherModeToolStripMenuItem;
        private ToolStripMenuItem menuCipherModeItemCBC;
        private ToolStripMenuItem menuCipherModeItemCCM;
        private ToolStripMenuItem menuCipherModeItemCFB;
        private ToolStripMenuItem menuCipherModeItemCTS;
        private ToolStripMenuItem menuCipherModeItemEAX;
        private ToolStripMenuItem menuCipherModeItemECB;
        private ToolStripMenuItem menuCipherModeItemGOFB;
        private ToolStripMenuItem warnOnEmptyPipeToolStripMenuItem;
        private Label labelGZip;
        private Label labelBase64;
    }


}