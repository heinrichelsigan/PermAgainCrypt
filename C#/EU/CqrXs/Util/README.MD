# namespace EU.CqrXs.Util

## Area23EventArgs<T>
```
    /// <summary>Area23EventArgs generic event args</summary>
    /// <typeparam name="T" />
    public class Area23EventArgs<T> : EventArgs
    {
        
        /// <summary><typeparamref name="T">geneneric T type or class</typeparamref></summary>
        public T GenericTData { get; protected set; }


        /// <summary>ctor Area23EventArgs</summary>
        /// <param name="genericTData">generic T data</param>
        public Area23EventArgs(T genericTData)
        {
            GenericTData = genericTData;
        }

        public override string? ToString() => (GenericTData != null) ? GenericTData.ToString() : null;
    }
```

## Area23Log
```
public class Area23Log {
    public static void SetLogFile(string logFilePath, bool createDirectory = false)...
    public static void SetLogFileByAppName(string appName = "")...
    public static void Log(string msg, string appName = "")...
    public static void Log(Exception exLog, string appName = "")...
    public static void LogStatic(string msg, string appName = "") => Area23Log.Log(msg, appName)
    public static void LogStatic(string prefix, Exception xZpd, string appName) => Area23Log.LogOriginMsgEx(appName, prefix, xZpd);
    public static void LogStatic(Exception ex, string appName = "") => Area23Log.Log(ex, appName);
    public static void LogOriginMsg(string origin, string message, int level = 2)...
    public static void LogOriginEx(string origin, Exception ex, int level = 2)...
    public static void LogOriginMsgEx(string origin, string message, Exception ex, int level = 2)..    
```
## GifEncoder 
thanx to https://github.com/DataDink/Bumpkit/blob/master/BumpKit/BumpKit/GifEncoder.cs
```
public class GifEncoder : IDisposable
{
    internal MemoryStream _memoryStream;
    // public properties
    public byte[] GifBytes { get; protected internal set; }    
    public Bitmap AnimBitmap { get => new Bitmap(_memoryStream, false); }
    public Image AnimImage { get => ((Image)AnimBitmap); }    
    public TimeSpan FrameDelay { get => _frameDelay; }
    public byte[] GifData {
        get {
            if (!_isFinished) Finish(ref _memoryStream);  
                return GifBytes;
        }
    }
    // constructor for plain basically not animated gif
    public GifEncoder(Image img, int? repeatCount = null, TimeSpan? frameDelay = null)...
    // constructor for animated gif
    public GifEncoder(Bitmap[] gifFrames, int? repeatCount, TimeSpan? frameDelay)
```

## ProcessCmd
```
public static class ProcessCmd
{
    public static string Execute(string filepath = "SystemInfo", string arguments = "", bool useShellExecute = false)...
    public static string Execute(string cmdPath = "SystemInfo", string[]? arguments = null, bool quoteArgs = false, bool useShellExecute = false)...
}
```