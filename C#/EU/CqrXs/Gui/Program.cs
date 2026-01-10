#define CLR2COMPATIBILITY
using EU.CqrXs.Util;
using EU.CqrXs.Gui.Forms;


namespace EU.CqrXs.Gui
{

    #region enum FormMode
    public enum FormMode
    {
        Simple = 0,
        MultiComponent = 1,
        Complex = 2
    }
    #endregion enum FormMode

    #region program

    /// <summary>
    /// Main Program
    /// </summary>
    public static class Program
    {

        #region static fields
        public static string ProgName { get => Constants.APP_NAME_WINFORM; }

        internal static Mutex? mutex;

        internal static SystemColorMode colorMode = SystemColorMode.System;
        internal static FormMode formMode = FormMode.MultiComponent;
        // internal static CipherPipe? ciperPipe;
        #endregion static fields

        #region Main

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// <param name="args">arguments</param>
        [STAThread]
        internal static void Main(string[] args)
        {
            mutex = (mutex == null) ? new Mutex(false, Constants.APP_NAME_WINFORM) : mutex;

            if (!mutex.WaitOne(1000, false))
            {
                // show MsgBox and exit
                MessageBox.Show($"Another instance of {ProgName} is already running!", "Attention");
                return;
            }

            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (arg.Contains("dark", StringComparison.CurrentCultureIgnoreCase))
                        colorMode = SystemColorMode.Dark;
                    if (arg.Contains("classic", StringComparison.CurrentCultureIgnoreCase))
                        colorMode = SystemColorMode.Classic;
                    if (arg.Contains("simple", StringComparison.CurrentCultureIgnoreCase))
                        formMode = FormMode.Simple;
                }
            }
                

            // set Application basic settings
            Application.EnableVisualStyles();
            Application.SetColorMode(colorMode);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // instanciate a new EncryptForm
            EncryptFormBase formBase = (formMode == FormMode.Simple) ? new EncryptForm() : new EncryptFormMultiControls();

            // Run application
            Application.Run(formBase);

            // Release, Close, Dispose Mutal Exclusion
            ReleaseCloseDisposeMutex();
        }

        #endregion Main

        #region ReleaseCloseDisposeMutex

        public static void ReleaseCloseDisposeMutex()
        {
            Exception? ex = null;
            if (Program.mutex != null)
            {
                var safeWaitHandle = Program.mutex.GetSafeWaitHandle();
                if (safeWaitHandle != null && !safeWaitHandle.IsInvalid && !safeWaitHandle.IsClosed)
                {
                    try
                    {
                        Program.mutex.ReleaseMutex();
                    }
                    catch (Exception exRelease)
                    {
                        ex = exRelease;
                    }
                    try
                    {
                        Program.mutex.Close();
                    }
                    catch (Exception exClose)
                    {
                        if (ex == null)
                            ex = exClose;
                    }
                    try
                    {
                        Program.mutex.Dispose();
                    }
                    catch (Exception exDispose)
                    {
                        ex = exDispose;
                    }

                }
            }
            try
            {
                Program.mutex = null;
            }
            catch (Exception exNull)
            {
                ex = exNull;
            }
            finally
            {
                if (ex != null)
                    throw ex;
            }
        }

        #endregion ReleaseCloseDisposeMutex
    }

    #endregion program

}