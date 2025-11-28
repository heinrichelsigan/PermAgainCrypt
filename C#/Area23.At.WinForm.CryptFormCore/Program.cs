#define CLR2COMPATIBILITY
using Area23.At.Framework.Core.Util;
using Area23.At.WinForm.CryptFormCore.Gui.Forms;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;


namespace Area23.At.WinForm.CryptFormCore
{

    #region program

    public static class Program
    {

        public static string ProgName { get => Constants.APP_NAME_WINFORM; }

        internal static Mutex? mutex;


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

            // set Application basic settings
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // instanciate a new EncryptForm
            System.Windows.Forms.Form encryptForm = new EncryptForm();

            // Run application
            Application.Run(encryptForm);

            // Release, Close, Dispose Mutal Exclusion
            ReleaseCloseDisposeMutex();
        }

        #endregion program

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
}