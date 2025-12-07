using Area23.At.WinForm.CryptFormCore.Gui.Forms;

namespace Area23.At.WinForm.CryptFormCore.Gui.Controls
{
    partial class GroupBoxFiles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupBoxFiles));
            pictureBoxFileIn = new PictureBox();
            pictureBoxFileOut = new PictureBox();
            pictureBoxRunningPipe = new PictureBox();
            labelFileIn = new Label();
            labelOutputFile = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileIn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileOut).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRunningPipe).BeginInit();
            SuspendLayout();
            // 
            // labelFileIn
            // 
            labelFileIn.Font = new Font("Lucida Sans Typewriter", 8.75F);
            labelFileIn.Location = new Point(12, 112);
            labelFileIn.Margin = new Padding(1, 0, 1, 0);
            labelFileIn.Name = "labelFileIn";
            labelFileIn.Size = new Size(432, 23);
            labelFileIn.TabIndex = 21;
            labelFileIn.Text = "[Input File]";
            // 
            // labelOutputFile
            // 
            labelOutputFile.Font = new Font("Lucida Sans Typewriter", 8.75F);
            labelOutputFile.Location = new Point(545, 112);
            labelOutputFile.Margin = new Padding(1, 0, 1, 0);
            labelOutputFile.Name = "labelOutputFile";
            labelOutputFile.RightToLeft = RightToLeft.Yes;
            labelOutputFile.Size = new Size(444, 23);
            labelOutputFile.TabIndex = 24;
            labelOutputFile.Text = "[Output File]";
            labelOutputFile.Visible = false;
            // 
            // pictureBoxFileIn
            // 
            pictureBoxFileIn.Image = Properties.Resources.image_file;
            pictureBoxFileIn.InitialImage = Properties.Resources.img_success;
            pictureBoxFileIn.Location = new Point(12, 32);
            pictureBoxFileIn.Margin = new Padding(2);
            pictureBoxFileIn.Name = "pictureBoxFileIn";
            pictureBoxFileIn.Size = new Size(64, 64);
            pictureBoxFileIn.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBoxFileIn.TabIndex = 22;
            pictureBoxFileIn.TabStop = false;
            // 
            // pictureBoxFileOut
            // 
            pictureBoxFileOut.Image = Properties.Resources.image_file_encrypted;
            pictureBoxFileOut.Location = new Point(918, 32);
            pictureBoxFileOut.Margin = new Padding(1);
            pictureBoxFileOut.Name = "pictureBoxFileOut";
            pictureBoxFileOut.Size = new Size(68, 68);
            pictureBoxFileOut.TabIndex = 25;
            pictureBoxFileOut.TabStop = false;
            pictureBoxFileOut.Visible = false;
            pictureBoxFileOut.Click += pictureBoxFileOut_Click;
            pictureBoxFileOut.DoubleClick += pictureBoxFileOut_Click;
            // 
            // pictureBoxRunningPipe
            // 
            pictureBoxRunningPipe.Image = Properties.Resources.CryptPipe1;
            pictureBoxRunningPipe.Location = new Point(180, 1);
            pictureBoxRunningPipe.Margin = new Padding(1);
            pictureBoxRunningPipe.Name = "pictureBoxRunningPipe";
            pictureBoxRunningPipe.Size = new Size(640, 108);
            pictureBoxRunningPipe.TabIndex = 23;
            pictureBoxRunningPipe.TabStop = false;
            // 
            // groupBoxFiles
            // 
            this.AllowDrop = true;
            this.BackColor = SystemColors.Control;
            this.Controls.Add(pictureBoxRunningPipe);
            this.Controls.Add(pictureBoxFileIn);
            this.Controls.Add(labelFileIn);
            this.Controls.Add(pictureBoxFileOut);
            this.Controls.Add(labelOutputFile);
            this.Font = new Font("Lucida Sans Typewriter", 8F);
            this.Location = new Point(0, 0);
            this.Margin = new Padding(2);
            this.Name = "GroupBoxFiles";
            this.Padding = new Padding(2);
            this.Size = new Size(988, 145);
            this.TabIndex = 20;
            this.TabStop = false;
            this.Text = "Files (drag files into)";
            this.DragDrop += Drag_Drop;
            this.DragEnter += Drag_Enter;
            this.DragOver += Drag_Over;
            this.DragLeave += Drag_Leave;
            this.GiveFeedback += Give_FeedBack;
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileIn).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileOut).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRunningPipe).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        internal PictureBox pictureBoxRunningPipe;
        internal PictureBox pictureBoxFileIn;
        internal PictureBox pictureBoxFileOut;        
        internal Label labelFileIn;
        internal Label labelOutputFile;
    }
}
