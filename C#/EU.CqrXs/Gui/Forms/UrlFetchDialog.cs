using EU.CqrXs.Util;
using EU.CqrXs.Gui.Controls;
using EU.CqrXs.Net.WebHttp;
using Microsoft.Web.WebView2.WinForms;
using System.Security.Cryptography;
using static EU.CqrXs.Gui.Forms.EncryptFormBase;

namespace EU.CqrXs.Gui.Forms
{
    /// <summary>
    /// About Dialog is a modal running about application dialog
    /// </summary>
    public class UrlFetchDialog : Form
    {

        #region fields 

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        static string simg = "";
        static int instances = 0;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        string url = "";
        Boolean isUpdating = false;
        protected internal delegate void PrintImageCallback(WebView2 webView23);

        #endregion fields 

        /// <summary>
        /// Default ctor CharHexDecOctBinDialog
        /// </summary>
        public UrlFetchDialog(string simage)
        {
            InitializeComponent();
            instances++;
            simg = simage;
            this.Text = String.Format("Urlfetch Dialog {0}", instances);
        }

        protected internal virtual void PrintImage(WebView2 webView23)
        {
            if (webView23 != null)
            {
                if (webView23.InvokeRequired)
                {
                    PrintImageCallback printImageCallback = delegate (WebView2 webView43)
                    {
                        if (webView43 != null)
                        {
                            Bitmap bm = new Bitmap(webView43.Width, webView43.Height);
                            webView43.DrawToBitmap(bm, webView43.Bounds);

                            string imgfile = Path.Combine(LibPaths.TempDir, simg);
                            bm.Save(imgfile, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    };
                    try
                    {
                        Invoke(printImageCallback, new object[] { webView23 });
                    }
                    catch (System.Exception exDelegate)
                    {
                        Area23Log.LogOriginMsgEx(this.Name, $"Exception in delegate SetVisibleCallback form name = \"{this.Name}\".\n", exDelegate);
                    }
                }
                else
                {
                    if (webView23 != null)
                    {
                        Bitmap bm = new Bitmap(webView23.Width, webView23.Height);
                        webView23.DrawToBitmap(bm, webView21.Bounds);

                        string imgfile = Path.Combine(LibPaths.TempDir, simg);
                        bm.Save(imgfile, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            PrintImage(webView21);
            base.OnFormClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            
            Uri uri = new Uri("https://duckduckgo.com/?q=site%3A.at&df=d&ia=images&iax=images");
            this.webView21.Source = uri;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            System.Timers.Timer closeTimer = new System.Timers.Timer { Interval = 45000 };
            closeTimer.Elapsed += (s, en) =>
            {
                Task.Run(new System.Action(() =>
                {
                    PrintImage(webView21);
                }));
                closeTimer.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            closeTimer.Start();           
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(2, 2);
            webView21.Name = "webView21";
            webView21.Size = new Size(780, 557);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // UrlFetchDialog
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(webView21);
            Font = new Font("Microsoft Sans Serif", 11F);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 2, 4, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UrlFetchDialog";
            Opacity = 0.8D;
            Padding = new Padding(2);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "UrlFetch Dialog";
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);

        }

        #endregion

        #region gui control members


        protected void ClearDialog()
        {            
            this.Close();
            this.Dispose();             
        }

        #endregion gui control members
        

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
