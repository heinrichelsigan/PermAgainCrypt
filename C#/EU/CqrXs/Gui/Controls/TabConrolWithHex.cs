using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Gui.Helper;
using EU.CqrXs.Gui.Properties;
using EU.CqrXs.Gui.Sound;
using EU.CqrXs.Util;
using System.ComponentModel;
using System.Windows.Forms.VisualStyles;

namespace EU.CqrXs.Gui.Controls
{
    
    /// <summary>
    /// GroupBoxFiles - handles drag and drop events and show cipherpipe image
    /// </summary>
    public partial class TabControlWithHex : TabControl
    {

        private readonly Lock _Lock = new Lock();

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
        internal bool AsciiTextReadonly { get => textBoxAsciiText.ReadOnly; set => textBoxAsciiText.ReadOnly = value; }

        public TabControlWithHex()
        {
            InitializeComponent();            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        protected void SelectedChanged(object sender, EventArgs e)
        {
            if (sender != null &&
                this.SelectedTab.Name.EndsWith("Hex", StringComparison.InvariantCultureIgnoreCase) &&
                !string.IsNullOrEmpty(this.textBoxAsciiText.Text))
            {
                lock (_Lock)
                {
                    this.textBoxViewHex.Clear();
                    string hexString = Hex16.ToHex16(System.Text.Encoding.UTF8.GetBytes(this.textBoxAsciiText.Text));
                    for (int hi = 0; hi < hexString.Length; hi++)
                    {
                        if (hi > 0)
                        {
                            if (hi % 40 == 0)
                                this.textBoxViewHex.Text += Environment.NewLine;
                            else if (hi % 2 == 0)
                                this.textBoxViewHex.Text += " ";
                        }
                        this.textBoxViewHex.Text += hexString[hi].ToString();
                    }
                }
                // this.textBoxSrcHex.Focus();
            }
        }

    }
}
