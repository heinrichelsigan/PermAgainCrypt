using EU.CqrXs.Crypt.EnDeCoding;
using System.ComponentModel;

namespace EU.CqrXs.Gui.Controls
{
    
    /// <summary>
    /// GroupBoxFiles - handles drag and drop events and show cipherpipe image
    /// </summary>
    public class TabControlWithHex : TabControl
    {

        #region fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private readonly Lock _Lock = new Lock();
        protected TabPage tabPageAscii;
        protected TextBox textBoxAsciiText;
        protected TabPage tabPageHex;
        protected TextBox textBoxViewHex;

        #endregion fields

        #region properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal string AsciiText
        {
            get => this.textBoxAsciiText.Text ?? "";
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.textBoxViewHex.Clear();
                    this.textBoxAsciiText.Clear();
                }
                else
                {
                    this.textBoxAsciiText.Text = value;
                    SelectedChanged("AsciiText", new EventArgs());
                }                
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal EncodingType EncoderType
        {
            set
            {
                switch (value)
                {
                    case EncodingType.Uu:
                    case EncodingType.Xx:
                        textBoxAsciiText.Font = new Font("Lucida Console", 8.75F);
                        break;
                    case EncodingType.Hex64:
                    case EncodingType.Base64:
                        textBoxAsciiText.Font = new Font("Lucida Console", 7.4F);
                        break;
                    case EncodingType.None:
                    default:
                        textBoxAsciiText.Font = new Font("Lucida Console", 9F);
                        break;
                }
            }

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool AsciiTextReadonly { get => textBoxAsciiText.ReadOnly; set => textBoxAsciiText.ReadOnly = value; }

        #endregion properties

        /// <summary>
        /// Parameterless default constructor
        /// </summary>
        public TabControlWithHex()
        {
            InitializeComponent();            
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
            tabPageAscii = new TabPage();
            textBoxAsciiText = new TextBox();
            tabPageHex = new TabPage();
            textBoxViewHex = new TextBox();
            tabPageAscii.SuspendLayout();
            tabPageHex.SuspendLayout();
            SuspendLayout();
            // 
            // tabPageAscii
            // 
            tabPageAscii.Controls.Add(textBoxAsciiText);
            tabPageAscii.Cursor = Cursors.Cross;
            tabPageAscii.Font = new Font("Lucida Console", 8.5F);
            tabPageAscii.Location = new Point(4, 23);
            tabPageAscii.Margin = new Padding(2);
            tabPageAscii.Name = "tabPageAscii";
            tabPageAscii.Padding = new Padding(2);
            tabPageAscii.Size = new Size(484, 277);
            tabPageAscii.TabIndex = 0;
            tabPageAscii.Text = " Ascii Text ";
            tabPageAscii.UseVisualStyleBackColor = true;
            // 
            // textBoxAsciiText
            // 
            textBoxAsciiText.BackColor = SystemColors.ControlLight;
            textBoxAsciiText.Dock = DockStyle.Fill;
            textBoxAsciiText.Font = new Font("Lucida Console", 8.4F);
            textBoxAsciiText.Location = new Point(1, 1);
            textBoxAsciiText.Margin = new Padding(1);
            textBoxAsciiText.MaxLength = 1048576;
            textBoxAsciiText.Multiline = true;
            textBoxAsciiText.Name = "textBoxAsciiText";
            textBoxAsciiText.ScrollBars = ScrollBars.Vertical;
            textBoxAsciiText.Size = new Size(480, 273);
            textBoxAsciiText.TabIndex = 43;
            // 
            // tabPageHex
            // 
            tabPageHex.Controls.Add(textBoxViewHex);
            tabPageHex.Font = new Font("Lucida Console", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabPageHex.Location = new Point(4, 23);
            tabPageHex.Margin = new Padding(2);
            tabPageHex.Name = "tabPageHex";
            tabPageHex.Padding = new Padding(2);
            tabPageHex.Size = new Size(484, 277);
            tabPageHex.TabIndex = 1;
            tabPageHex.Text = " Hex View ";
            tabPageHex.UseVisualStyleBackColor = true;
            // 
            // textBoxViewHex
            // 
            textBoxViewHex.BackColor = SystemColors.Control;
            textBoxViewHex.BorderStyle = BorderStyle.FixedSingle;
            textBoxViewHex.Dock = DockStyle.Fill;
            textBoxViewHex.Font = new Font("Lucida Console", 8F);
            textBoxViewHex.Location = new Point(1, 1);
            textBoxViewHex.Margin = new Padding(1);
            textBoxViewHex.MaxLength = 1048576;
            textBoxViewHex.Multiline = true;
            textBoxViewHex.Name = "textBoxViewHex";
            textBoxViewHex.ReadOnly = true;
            textBoxViewHex.ScrollBars = ScrollBars.Vertical;
            textBoxViewHex.Size = new Size(480, 273);
            textBoxViewHex.TabIndex = 33;
            // 
            // 
            // 
            this.Controls.Add(tabPageAscii);
            this.Controls.Add(tabPageHex);
            this.ItemSize = new Size(72, 19);
            this.Location = new Point(1, 1);
            this.Margin = new Padding(1);
            this.Name = "tabControlWithHex";
            this.Padding = new Point(1, 1);
            this.SelectedIndex = 0;
            this.Size = new Size(492, 304);
            this.TabIndex = 40;
            this.SelectedIndexChanged += SelectedChanged;
            this.BackColor = SystemColors.Control;
            this.Font = new Font("Lucida Console", 8F);
            this.tabPageAscii.ResumeLayout(false);
            this.tabPageAscii.PerformLayout();
            this.tabPageHex.ResumeLayout(false);
            this.tabPageHex.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        /// OnPaint is executed on painting event
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <summary>
        /// SelectedChanged is fired, when selected tab is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedChanged(object sender, EventArgs e)
        {
            this.textBoxViewHex.BackColor = SystemColors.Control;
            if (sender != null &&
                this.SelectedTab.Name.EndsWith("Hex", StringComparison.InvariantCultureIgnoreCase) &&
                !string.IsNullOrEmpty(this.textBoxAsciiText.Text))
            {
                lock (_Lock)
                {                    
                    this.textBoxViewHex.Clear();
                    BackColor = SystemColors.Control;
                    String ascii = this.textBoxAsciiText.Text;
                    if (ascii.Length >= 8000)
                    {
                        this.tabPageHex.Text = "Hex: 1st & last 2kb";
                        ascii = ascii.Substring(0, 4000) +
                                Environment.NewLine + "--- file too long, showing first & last 4k lines --" + Environment.NewLine +
                                ascii.Substring(ascii.Length - 4000);
                        this.textBoxViewHex.BackColor = SystemColors.ButtonHighlight;
                    }
                    string hexString = Hex16.ToHex16(System.Text.Encoding.UTF8.GetBytes(ascii));
                    this.textBoxViewHex.Text = hexString;

                }
                // this.textBoxSrcHex.Focus();
            }
        }


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

    }
}
