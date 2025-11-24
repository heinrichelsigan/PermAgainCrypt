using System.Reflection;

namespace Area23.At.WinForm.CryptFormCore.Gui.Forms
{
    /// <summary>
    /// About Dialog is a modal running about application dialog
    /// </summary>
    partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }

                return System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetAssembly(typeof(AboutDialog)).Location);                
            }
        }

        public string AssemblyVersion { get => Assembly.GetExecutingAssembly().GetName().Version.ToString(); }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return (attributes.Length == 0) ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;               
            }
        }

        public string AssemblyProduct { get => Application.ProductName ?? "No Product Name"; }

        public string AssemblyCopyright 
        {
            get
            {                
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return (attributes.Length == 0) ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;               
            }
        }

        public string AssemblyCompany { get => Application.CompanyName.ToString();  }
        #endregion

        private void logoPictureBox_MouseHover(object sender, EventArgs e)
        {
            this.logoPictureBox.Image = Properties.Resources.ChiffreCryptDisk;
        }

        private void logoPictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.logoPictureBox.Image = Properties.Resources.ChiffrePentacle;
        }
    }
}
