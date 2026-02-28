using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace EU.CqrXs.Util
{

    /// <summary>
    /// simple static logger via NLog
    /// </summary>
    public class Area23Log
    {

        #region static fields and properties

        private static readonly object _lock = new object(), _outerLock = new object();
        private static readonly Lazy<Area23Log> instance = new Lazy<Area23Log>(() => new Area23Log());

        private static int daysave = -1;
        private static int checkedToday = DateTime.UtcNow.Date.Day;

        private static string systemDirPath = "";
        private static string logDirPath = "";
        private static string logFilePath = "";
        private static string tempDirPath = "";

        public static readonly char _sepCh = Path.DirectorySeparatorChar;

        /// <summary>
        /// SystemDirPath return system directory path, 
        /// if defined in App.Config, 
        /// otherwise applcation directory of base exe.
        /// </summary>
        public static string SystemDirPath
        {
            get
            {
                if (string.IsNullOrEmpty(systemDirPath))
                {
                    for (int sysDirTry = 0; sysDirTry < 8; sysDirTry++)
                    {
                        try
                        {
                            switch (sysDirTry)
                            {
                                case 0:
                                    if (_sepCh == '/' && Path.DirectorySeparatorChar == '/' && _sepCh == Path.DirectorySeparatorChar &&
                                            ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_UNIX] != null &&
                                            ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_UNIX] != "")
                                        systemDirPath = ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_UNIX];
                                    break;
                                case 1:
                                    if (ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_WIN] != null)
                                        systemDirPath = ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_WIN];
                                    break;
                                case 2: systemDirPath = Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); break;
                                case 3: systemDirPath = Path.GetFullPath(Environment.ProcessPath); break;
                                case 4: systemDirPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location); break;
                                case 5: systemDirPath = AppContext.BaseDirectory; break;
                                case 6: if (AppDomain.CurrentDomain != null) systemDirPath = AppDomain.CurrentDomain.BaseDirectory; break;
                                case 7: systemDirPath = Path.GetFullPath(Assembly.GetExecutingAssembly().CodeBase); break;
                                case 8: systemDirPath = Path.GetFullPath(Environment.GetCommandLineArgs()[0]); break;
                                default:
                                    systemDirPath = Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); break;
                            }
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(systemDirPath) && Directory.Exists(systemDirPath))
                            break;
                    }

                    if (!systemDirPath.EndsWith(_sepCh))
                        systemDirPath += _sepCh;

                    string sysDir = systemDirPath;
                    if (sysDir.EndsWith($"{_sepCh}{Constants.WIN_X86}{_sepCh}") || sysDir.EndsWith($"{_sepCh}{Constants.WIN_X64}{_sepCh}"))
                        sysDir = sysDir.Replace($"{_sepCh}{Constants.WIN_X86}{_sepCh}", _sepCh.ToString()).Replace($"{_sepCh}{Constants.WIN_X64}{_sepCh}", _sepCh.ToString());
                    if (sysDir.EndsWith($"{_sepCh}{Constants.NET9_WINDOWS7}{_sepCh}") || sysDir.EndsWith($"{_sepCh}{Constants.NET9_WINDOWS8}{_sepCh}"))
                        sysDir = sysDir.Replace($"{_sepCh}{Constants.NET9_WINDOWS7}{_sepCh}", _sepCh.ToString()).Replace($"{_sepCh}{Constants.NET9_WINDOWS8}{_sepCh}", _sepCh.ToString());
                    if (sysDir.EndsWith($"{_sepCh}{Constants.RELEASE_DIR}{_sepCh}") || sysDir.EndsWith($"{_sepCh}{Constants.DEBUG_DIR}{_sepCh}"))
                        sysDir = sysDir.Replace($"{_sepCh}{Constants.RELEASE_DIR}{_sepCh}", _sepCh.ToString()).Replace($"{_sepCh}{Constants.DEBUG_DIR}{_sepCh}", _sepCh.ToString());
                    if (sysDir.EndsWith($"{_sepCh}{Constants.BIN_DIR}{_sepCh}") || sysDir.EndsWith($"{_sepCh}{Constants.OBJ_DIR}{_sepCh}"))
                        sysDir = sysDir.Replace($"{_sepCh}{Constants.BIN_DIR}{_sepCh}", _sepCh.ToString()).Replace($"{_sepCh}{Constants.OBJ_DIR}{_sepCh}", _sepCh.ToString());

                    if (Directory.Exists(sysDir))
                        systemDirPath = sysDir;

                }

                return systemDirPath;
            }
        }

        /// <summary>
        /// Path to temp directory
        /// </summary>
        public static string TempDir
        {
            get
            {
                if (string.IsNullOrEmpty(tempDirPath))
                {
                    tempDirPath = Environment.GetEnvironmentVariable("LOCALAPPDATA") ?? "";
                    if (!string.IsNullOrEmpty(tempDirPath) && Directory.Exists(tempDirPath))
                        tempDirPath = Path.Combine(tempDirPath, "Temp");
                    else
                        tempDirPath = Path.Combine(
                            Environment.GetEnvironmentVariable("windir") ?? Environment.GetEnvironmentVariable("SystemRoot") ?? "C:\\Windows",
                            "Temp");

                    if (!Directory.Exists(tempDirPath))
                        Directory.CreateDirectory(tempDirPath);
                }
                return tempDirPath;
            }
        }

        /// <summary>
        /// SystemDirLogPath gets the default full path to logfile in file system
        /// </summary>
        public static string SystemDirLogPath
        {
            get
            {
                if (string.IsNullOrEmpty(logDirPath))
                {
                    logDirPath = (SystemDirPath.EndsWith(Path.DirectorySeparatorChar)) ? SystemDirPath : SystemDirPath + _sepCh;

                    if (!Directory.Exists(logDirPath))
                    {
                        try
                        {
                            if (Constants.DirCreate && !Constants.NOLog)
                                Directory.CreateDirectory(logDirPath);
                        }
                        catch { }
                    }
                }
                return logDirPath;
            }
        }

        public static string LogFileSystemPath { get => SystemDirLogPath + Constants.AppLogFile; }


        /// <summary>
        /// Get the Logger
        /// </summary>
        public static Area23Log Logger { get => instance.Value; }

        /// <summary>
        /// Checked today if logfiles and other needed resources exist
        /// </summary>
        public static bool CheckedToday
        {
            get
            {
                if (DateTime.UtcNow.Day == checkedToday)
                    return true;

                checkedToday = DateTime.UtcNow.Day;
                return false;
            }
        }

        public static string AppName { get; private set; } = string.Empty;

        /// <summary>
        /// LogFile
        /// </summary>
        public static string LogFile { get; private set; }

        #endregion static fields and properties

        #region ctor

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        static Area23Log() 
        {
            LogFile = LogFileSystemPath;
            InitLog("");
        }

        #endregion ctor

        #region static members

        /// <summary>
        /// InitLog init Log configuration
        /// </summary>
        /// <param name="appName">application name</param>
        protected internal static void InitLog(string appName = "")
        {
            if (!string.IsNullOrEmpty(appName))
                AppName = appName;

            if (!string.IsNullOrEmpty(AppName))
                LogFile = GetLogFilePath(AppName);
            else
                LogFile = LogFileSystemPath;
        }

        public static void SetLogFile(string logFilePath, bool createDirectory = false)
        {
            if (string.IsNullOrEmpty(logFilePath))
                return;

            string dirName = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(dirName) && createDirectory)
                Directory.CreateDirectory(dirName);

            if (!Directory.Exists(dirName))
                return;

            if (!File.Exists(logFilePath))
                File.Create(logFilePath);

            LogFile = logFilePath;
        }

        public static void SetLogFileByAppName(string appName = "")
        {
            LogFile = (!string.IsNullOrEmpty(appName)) ? GetLogFilePath(appName) : LogFileSystemPath;
        }

        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="msg">message to log</param>
        /// <param name="appName">application name</param>
        public static void Log(string msg, string appName = "")
        {
            string logMsg = string.Empty, errMsg = string.Empty, allLogMsg = string.Empty;

            lock (_outerLock)
            {
                if (string.IsNullOrEmpty(LogFile) || !CheckedToday || !File.Exists(LogFile))
                {
                    LogFile = (!string.IsNullOrEmpty(appName)) ? GetLogFilePath(appName) : LogFileSystemPath;

                    if (!File.Exists(LogFile))
                    {
                        lock (_lock)
                        {
                            try
                            {
                                File.Create(LogFile);
                            }
                            catch (Exception exLogFiteCreate)
                            {
                                ; // throw
                                Console.Error.WriteLine("Exception creating logfile: " + exLogFiteCreate.ToString());
                            }
                        }
                    }
                }

                try
                {
                    if ((AppDomain.CurrentDomain.GetData(Constants.ALL_KEYS) != null) &&
                        ((allLogMsg = AppDomain.CurrentDomain.GetData(Constants.ALL_KEYS).ToString()) != null && allLogMsg != ""))
                    {
                        lock (_lock)
                        {
                            File.AppendAllText(LogFile, allLogMsg, System.Text.Encoding.UTF8);
                            allLogMsg = "";
                            AppDomain.CurrentDomain.SetData(Constants.ALL_KEYS, allLogMsg);
                        }
                    }
                }
                catch (Exception exLog)
                {
                    errMsg = String.Format("{0} \tWriting to file {1} Exception {2} {3} \n{4}\n",
                        DateTime.Now.Area23DateTimeWithSeconds(), LogFile, exLog.GetType(), exLog.Message, exLog.ToString());
                    AppDomain.CurrentDomain.SetData(Constants.LOG_EXCEPTION_STATIC, errMsg);
                    Console.Error.WriteLine(errMsg);
                }

                logMsg = DateTime.Now.Area23DateTimeWithSeconds() + "\t " + (string.IsNullOrEmpty(msg) ? string.Empty : (msg.EndsWith("\n") ? msg : msg + "\n"));
                try
                {
                    lock (_lock)
                    {
                        File.AppendAllText(LogFile, logMsg, System.Text.Encoding.UTF8);
                    }
                }
                catch (Exception exLogWrite)
                {
                    errMsg = String.Format("{0} \tWriting to file {1} Exception {2} {3} \n{4}\n",
                            DateTime.Now.Area23DateTimeWithSeconds(), LogFile, exLogWrite.GetType(), exLogWrite.Message, exLogWrite.ToString());
                    AppDomain.CurrentDomain.SetData(Constants.LOG_EXCEPTION_STATIC, errMsg);
                    if (AppDomain.CurrentDomain.GetData(Constants.ALL_KEYS) != null)
                        allLogMsg = (string)AppDomain.CurrentDomain.GetData(Constants.ALL_KEYS);
                    allLogMsg += "\n" + logMsg + "\n" + errMsg;
                    AppDomain.CurrentDomain.SetData(Constants.ALL_KEYS, allLogMsg);
                    Console.Error.WriteLine(errMsg);
                }

            }

        }

        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        /// <param name="appName">application name</param>
        public static void Log(Exception exLog, string appName = "")
        {
            string methodBase = "unknown";
            try
            {
                MethodBase mBase = (new StackFrame(1))?.GetMethod();
                methodBase = mBase.ToString();
            }
            catch
            {
                methodBase = "unknown";
            }

            string excMsg = String.Format("{0} throwed {1} ⇒ {2}\t{3}\nStacktrace: \t{4}\n",
                methodBase,
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace.Replace("\r", "").Replace("\n", " "));

            Log(excMsg, appName);
        }

        public static void LogStatic(string msg, string appName = "") => Area23Log.Log(msg, appName);

        public static void LogStatic(string prefix, Exception xZpd, string appName) => Area23Log.LogOriginMsgEx(appName, prefix, xZpd);

        public static void LogStatic(Exception ex, string appName = "") => Area23Log.Log(ex, appName);

        /// <summary>
        /// Log origin with message to NLog
        /// </summary>
        /// <param name="origin">origin of message</param>
        /// <param name="message">enabler message to log</param>
        /// <param name="level">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public static void LogOriginMsg(string origin, string message, int level = 2)
        {
            string logMsg = (string.IsNullOrEmpty(origin) ? "  \t" : origin + " \t") + message;
            LogStatic(logMsg);
        }

        public static void LogOriginEx(string origin, Exception ex, int level = 2)
        {
            string logPrefix = string.IsNullOrEmpty(origin) ? "   " : origin;
            LogStatic($"{logPrefix} \tException {ex.GetType()}: \t{ex.Message}");
            LogStatic($"{logPrefix} \tException {ex.GetType()}: \t{ex.ToString()}");
            if (level < 2)
                LogStatic($"{logPrefix} \t{ex.GetType()} StackTrace: \t{ex.StackTrace}");
        }

        /// <summary>
        /// Log origin with message and thrown exception to NLog
        /// </summary>
        /// <param name="origin">origin of message</param>
        /// <param name="message">logging <see cref="string">string message</see></param>
        /// <param name="ex">logging <see cref="Exception">Exception ex</see></param>
        /// <param name="level"><see cref="int">int log level</see>: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public static void LogOriginMsgEx(string origin, string message, Exception ex, int level = 2)
        {
            string logPrefix = string.IsNullOrEmpty(origin) ? "   " : origin;
            LogStatic($"{logPrefix} \t{message} {ex.GetType()}: \t{ex.Message}");
            LogStatic($"{logPrefix} \tException {ex.GetType()}: \t{ex.ToString()}");
            if (level < 2)
                LogStatic($"{logPrefix} \t{ex.GetType()} StackTrace: \t{ex.StackTrace}");
        }


        /// <summary>
        /// GetLogFilePath - gets individual named logfile with substring appName
        /// </summary>
        /// <param name="appName">application name to customize logfile name</param>
        /// <returns>Full file path to log file in file system</returns>
        public static string GetLogFilePath(string appName)
        {
            int day = DateTime.UtcNow.DayOfYear;
            if (daysave != day)
            {
                daysave = day;
                logFilePath = "";
            }
            if (string.IsNullOrEmpty(logFilePath))
            {
                logFilePath = SystemDirLogPath + DateTime.UtcNow.Area23Date() + Constants.UNDER_SCORE + appName + Constants.LOG_EXT;
                if (!File.Exists(logFilePath))
                {
                    try
                    {
                        if (!Constants.NOLog)
                            File.Create(logFilePath);
                    }
                    catch { }
                }
            }
            return logFilePath;
        }


        #endregion static members

    }

}