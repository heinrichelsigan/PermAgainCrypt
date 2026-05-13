using EU.CqrXs.Crypt.EnDeCoding;
using System.ComponentModel;

namespace EU.CqrXs.Gui.Controls
{
    
    /// <summary>
    /// GroupBoxFiles - handles drag and drop events and show cipherpipe image
    /// </summary>
    public class TabControlWithHex : Control
    {

        #region fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private readonly Lock _Lock = new Lock();
        protected TextBox textBoxAsciiText;
        

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
                    this.textBoxAsciiText.Clear();
                }
                else
                {
                    this.textBoxAsciiText.Text = value;
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
            textBoxAsciiText = new TextBox();
            SuspendLayout();            
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
            // 
            // 
            this.Controls.Add(textBoxAsciiText);
            this.Location = new Point(1, 1);
            this.Margin = new Padding(1);
            this.Name = "controlAsciiText";
            this.Padding = new Padding(1);
            this.Size = new Size(492, 304);
            this.TabIndex = 40;
            this.BackColor = SystemColors.Control;
            this.Font = new Font("Lucida Console", 8F);
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
