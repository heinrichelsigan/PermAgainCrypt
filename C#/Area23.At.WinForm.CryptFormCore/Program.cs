#define CLR2COMPATIBILITY
using Area23.At.Framework.Core.Util;
using Area23.At.WinForm.CryptFormCore.Gui.Forms;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;


namespace Area23.At.WinForm.CryptFormCore
{

    #region InnerClasses_Structs

    /// <summary>
    /// Helper class containing kernel32 functions
    /// </summary>
    public class Kernel32
    {
        public const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// AttachConsole to Windows Form App
        /// </summary>
        /// <param name="dwProcessId"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(int dwProcessId);
    }

    /// <summary>
    /// Helper class containing Gdi32 API functions
    /// </summary>
    public class GDI32
    {

        internal const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter            

        [DllImport("gdi32.dll")]
        internal static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, int dwRop);
        [DllImport("GDI32.dll")]
        internal static extern bool BitBlt(int hdcDest, int nXDest, int nYDest,
            int nWidth, int nHeight, int hdcSrc,
            int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("GDI32.dll")]
        internal static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("GDI32.dll")]
        internal static extern int CreateCompatibleDC(int hdc);

        [DllImport("gdi32.dll")]
        internal static extern int CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);


        [DllImport("gdi32.dll")]
        internal static extern bool DeleteDC(IntPtr hDC);
        [DllImport("GDI32.dll")]
        internal static extern bool DeleteDC(int hdc);


        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);
        [DllImport("GDI32.dll")]
        internal static extern bool DeleteObject(int hObject);

        [DllImport("GDI32.dll")]
        internal static extern int GetDeviceCaps(int hdc, int nIndex);

        [DllImport("GDI32.dll")]
        internal static extern int SelectObject(int hdc, int hgdiobj);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

    }

    /// <summary>
    /// User class containing simplified User32 API functions with int instead of IntPtr
    /// </summary>
    public class User
    {

        [DllImport("user32.dll")]
        public static extern int GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);

    }


    /// <summary>
    /// Helper class containing User32 API functions
    /// </summary>
    public class User32
    {

        public const int HT_CAPTION = 0x2;

        public const uint GW_HWNDFIRST = 0x000;
        public const uint GW_HWNDLAST = 0x001;
        public const uint GW_HWNDNEXT = 0x002;
        public const uint GW_HWNDPREV = 0x003;
        public const uint GW_OWNER = 0x004;
        public const uint GW_CHILD = 0x005;
        public const uint GW_ENABLEDPOPUP = 0x006;

        public const uint WM_PRINT = 0x317;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_APPCOMMAND = 0x319;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;
        }

        [Flags]
        public enum PRF_FLAGS : uint
        {
            CHECKVISIBLE = 0x01,
            CHILDREN = 0x02,
            CLIENT = 0x04,
            ERASEBKGND = 0x08,
            NONCLIENT = 0x10,
            OWNED = 0x20
        }


        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();


        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        [DllImport("User32.dll")]
        public static extern int GetWindowDC(int hWnd);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("User32.dll")]
        public static extern int ReleaseDC(int hWnd, int hDC);


        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr hdc, PRF_FLAGS drawingOptions);

    }



    /// <summary>
    /// Wrap the intptr returned by OpenProcess in a safe handle.
    /// </summary>
    public class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
    {

        // Create a SafeHandle, informing the base class
        // that this SafeHandle instance "owns" the handle,
        // and therefore SafeHandle should call
        // our ReleaseHandle method when the SafeHandle
        // is no longer in use
        private SafeProcessHandle() : base(true)
        {
        }
        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Class name is NativeMethodsShared for increased clarity")]
        [DllImport("KERNEL32.DLL")]
        private static extern bool CloseHandle(IntPtr hObject);

    }

    /// <summary>
    /// Contains the security descriptor for an object and specifies whether
    /// the handle retrieved by specifying this structure is inheritable.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class SecurityAttributes
    {

        internal SecurityAttributes()
        {
#if (CLR2COMPATIBILITY)
                _nLength = (uint)Marshal.SizeOf(typeof(SecurityAttributes));
#else
            _nLength = (uint)Marshal.SizeOf<NativeMethods.SecurityAttributes>();
#endif
        }

        private uint _nLength;

        internal IntPtr lpSecurityDescriptor;

        internal bool bInheritHandle;

    }

    

    #region Structs

    /// <summary>
    /// Structure that contain information about the system on which we are running
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_INFO
    {

        // This is a union of a DWORD and a struct containing 2 WORDs.
        internal ushort wProcessorArchitecture;
        internal ushort wReserved;

        internal uint dwPageSize;
        internal IntPtr lpMinimumApplicationAddress;
        internal IntPtr lpMaximumApplicationAddress;
        internal IntPtr dwActiveProcessorMask;
        internal uint dwNumberOfProcessors;
        internal uint dwProcessorType;
        internal uint dwAllocationGranularity;
        internal ushort wProcessorLevel;
        internal ushort wProcessorRevision;

    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_BASIC_INFORMATION
    {

        internal IntPtr ExitStatus;
        internal IntPtr PebBaseAddress;
        internal IntPtr AffinityMask;
        internal IntPtr BasePriority;
        internal IntPtr UniqueProcessId;
        internal IntPtr InheritedFromUniqueProcessId;

        internal int Size
        {
            get { return (6 * IntPtr.Size); }
        }

    };

    /// <summary>
    /// Contains information about a file or directory; used by GetFileAttributesEx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct WIN32_FILE_ATTRIBUTE_DATA
    {

        internal int fileAttributes;
        internal uint ftCreationTimeLow;
        internal uint ftCreationTimeHigh;
        internal uint ftLastAccessTimeLow;
        internal uint ftLastAccessTimeHigh;
        internal uint ftLastWriteTimeLow;
        internal uint ftLastWriteTimeHigh;
        internal uint fileSizeHigh;
        internal uint fileSizeLow;

    }

    #endregion

    #endregion InnerClasses_Structs

    internal static class Program
    {
        internal static List<System.Windows.Forms.Form> tFormsNew = new List<System.Windows.Forms.Form>();
        internal static string progName = string.Empty;
        public static Mutex? mutex;


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// <param name="args">arguments</param>
        [STAThread]
        static void Main(string[] args)
        {
            bool oldFlag = false;
            progName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            mutex = new Mutex(false, progName);

            foreach (string arg in args)
            {
                if (arg.ToLower().Contains("old") || arg.ToLower().Contains("classic"))
                    oldFlag = true;
            }

            if (!mutex.WaitOne(1000, false))
            {
                Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
                // Area23.At.Framework.Library.Area23Log.Logger.LogOriginMsg(roachName, $"Another instance of {roachName} is already running!");
                Console.Out.WriteLine($"Another instance of {progName} is already running!");
                MessageBox.Show($"Another instance of {progName} is already running!", $"{progName}: multiple startup!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Area23Log.SetLogFile(AppContext.BaseDirectory.ToString() + Path.DirectorySeparatorChar + Constants.AppLogFile);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // MessageBox.Show("ScreenCapture", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);            
            System.Windows.Forms.Form encryptForm = new EncryptFormSimple(); 
            if (oldFlag)
                encryptForm = new EncryptForm();

            Application.Run(encryptForm);

            ReleaseCloseDisposeMutex();
        }


        internal static void ReleaseCloseDisposeMutex()
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
                        Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
                        Console.Out.WriteLine($"{progName} exception when releasing mutex: {exRelease.Message}\r\n{exRelease.ToString()}\r\n{exRelease.StackTrace}\r\n");
                    }
                    try
                    {
                        Program.mutex.Close();
                    }
                    catch (Exception exClose)
                    {
                        if (ex == null)
                            Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
                        Console.Out.WriteLine($"{progName} exception when closing mutex: {exClose.Message}\r\n{exClose.ToString()}\r\n{exClose.StackTrace}\r\n");
                        ex = exClose;
                    }
                    try
                    {
                        Program.mutex.Dispose();
                    }
                    catch (Exception exDispose) 
                    {
                        if (ex == null)
                            Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
                        Console.Out.WriteLine($"{progName} exception when disposing mutex: {exDispose.Message}\r\n{exDispose.ToString()}\r\n{exDispose.StackTrace}\r\n");
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
                if (ex == null)
                    Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);
                Console.Out.WriteLine($"{progName} exception when setting mutex to NULL: {exNull.Message}\r\n{exNull.ToString()}\r\n{exNull.StackTrace}\r\n");
                ex = exNull;
            }
            finally
            {
                if (ex != null)
                    throw ex;
            }
        }
    
    }
}