using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

// https://github.com/DataDink/Bumpkit/blob/master/BumpKit/BumpKit/GifEncoder.cs

namespace Area23.At.Framework.Core.Util
{

    /// <summary>   
    /// This encoder is taken from <see href="https://github.com/DataDink/Bumpkit" />
    /// <seealso href="https://github.com/DataDink/Bumpkit/blob/master/BumpKit/BumpKit/GifEncoder.cs">GifEncoder.cs</seealso>
    /// <seealso href="https://github.com/DataDink/Bumpkit?tab=Unlicense-1-ov-file">Unlicense license of DataDink/Bumpkit</seealso>
    /// Encodes multiple images as an animated gif to a stream. <br />
    /// ALWAYS ALWAYS ALWAYS wire this up   in a using block <br />
    /// Disposing the encoder will complete the file. <br />
    /// Uses default .net GIF encoding and adds animation headers.
    /// </summary>
    public class GifEncoder : IDisposable
    {
        #region Header Constants
        private const string FileType = "GIF";
        private const string FileVersion = "89a";
        private const byte FileTrailer = 0x3b;

        private const int ApplicationExtensionBlockIdentifier = 0xff21;
        private const byte FeByte = 0xfe;
        private const byte ApplicationBlockSize = 0x0b;
        private const string ApplicationIdentification = "NETSCAPE2.0";

        private const int GraphicControlExtensionBlockIdentifier = 0xf921;
        private const byte GraphicControlExtensionBlockSize = 0x04;

        private const long SourceGlobalColorInfoPosition = 10;
        private const long SourceGraphicControlExtensionPosition = 781;
        private const long SourceGraphicControlExtensionLength = 8;
        private const long SourceImageBlockPosition = 789;
        private const long SourceImageBlockHeaderLength = 11;
        private const long SourceColorBlockPosition = 13;
        private const long SourceColorBlockLength = 768;
        #endregion

        private bool _isFirstImage = true;
        private bool _isFinished = false;
        private int? _repeatCount;
        TimeSpan _frameDelay;
        private List<byte> _byteList;
        public byte[] GifBytes { get; protected internal set; }
        internal MemoryStream _memoryStream;
        
        // Public Accessors
        public TimeSpan FrameDelay { get => _frameDelay; }

        public byte[] GifData
        {
            get
            {
                if (!_isFinished) Finish(ref _memoryStream);  
                return GifBytes;
            }
        }



        /// <summary>
        /// GifEncoder
        /// </summary>
        /// <param name="img">img</param>
        /// <param name="repeatCount">repeat n times</param>
        /// <param name="frameDelay">deley betweeen frames</param>
        public GifEncoder(Image img, int? repeatCount = null, TimeSpan? frameDelay = null)
        {
            _byteList = new List<byte>();
            GifBytes = (new List<byte>()).ToArray();
            _memoryStream = new MemoryStream();
            _repeatCount = repeatCount;
            _frameDelay = frameDelay.GetValueOrDefault();
            _isFinished = false;

            using (MemoryStream srcGif = new MemoryStream())
            {
                img.Save(srcGif, ImageFormat.Gif);
                WriteHeader(ref _memoryStream, srcGif, img.Width, img.Height);
                WriteGraphicControlBlock(ref _memoryStream, srcGif, FrameDelay);
                WriteImageBlock(ref _memoryStream, srcGif, !_isFirstImage, 0, 0, img.Width, img.Height);
            }

            _isFirstImage = false;

        }


        public GifEncoder(Bitmap bmp, int? repeatCount = null, TimeSpan? frameDelay = null, params Bitmap[] gifFrames) 
        {
            _byteList = new List<byte>();
            GifBytes = (new List<byte>()).ToArray();
            _memoryStream = new MemoryStream();
            _repeatCount = repeatCount;
            _frameDelay = frameDelay.GetValueOrDefault();
            _isFinished = false;

            using (MemoryStream srcGif = new MemoryStream())
            {
                bmp.Save(srcGif, ImageFormat.Gif);
                WriteHeader(ref _memoryStream, srcGif, bmp.Width, bmp.Height);
                WriteGraphicControlBlock(ref _memoryStream, srcGif, FrameDelay);
                WriteImageBlock(ref _memoryStream, srcGif, !_isFirstImage, 0, 0, bmp.Width, bmp.Height);
            }

            _isFirstImage = false;

            foreach (Bitmap? frame in gifFrames)
            {
                if (frame != null)
                    AddFrame(ref _memoryStream, (Image)frame, FrameDelay);
            }

            if (!_isFinished)
                Finish(ref _memoryStream);
        }


        /// <summary>
        /// Adds a frame to this animation.
        /// </summary>
        /// <param name="img">The image to add</param>
        /// <param name="frameDelay">TimeSpan for delay</param>
        public void AddFrame(ref MemoryStream ms, Image img, TimeSpan? frameDelay = null)
        {
            if (_isFinished)
                return;

            ms = ms ?? new MemoryStream(GifBytes);

            _frameDelay = frameDelay.GetValueOrDefault();
            using (var gifStream = new MemoryStream())
            {
                img.Save(gifStream, ImageFormat.Gif);
                if (_isFirstImage) // Init gif header with global color table info of 1st frame
                {
                    WriteHeader(ref ms, gifStream, img.Width, img.Height);
                }
                WriteGraphicControlBlock(ref ms, gifStream, FrameDelay);
                WriteImageBlock(ref ms, gifStream, !_isFirstImage, 0, 0, img.Width, img.Height);
            }
            _isFirstImage = false;
        }

        private void WriteHeader(ref MemoryStream ms, Stream sourceGif, int w, int h)
        {
            if (_isFinished)
                return;

            ms = ms ?? new MemoryStream(GifBytes);

            // File Header
            WriteString(ref ms, FileType);
            WriteString(ref ms, FileVersion);
            WriteShort(ref ms, w); // Initial Logical Width
            WriteShort(ref ms, h); // Initial Logical Height
            
            sourceGif.Position = SourceGlobalColorInfoPosition;
            WriteByte(ref ms, sourceGif.ReadByte()); // Global Color Table Info
            WriteByte(ref ms, 0); // Background Color Index
            WriteByte(ref ms, 0); // Pixel aspect ratio
            WriteColorTable(ref ms, sourceGif);

            // App Extension Header
            WriteShort(ref ms, ApplicationExtensionBlockIdentifier); // 0xff21            
            WriteByte(ref ms, ApplicationBlockSize);
            WriteString(ref ms, ApplicationIdentification);            
            WriteByte(ref ms, 3); // Application block length
            WriteByte(ref ms, 1);            
            WriteShort(ref ms, _repeatCount.GetValueOrDefault(0)); // Repeat count for images.
            WriteByte(ref ms, 0); // terminator
        }

        private void WriteColorTable(ref MemoryStream ms, Stream sourceGif)
        {
            if (_isFinished)
                return;

            ms = ms ?? new MemoryStream(GifBytes);

            sourceGif.Position = SourceColorBlockPosition; // Locating the image color table
            byte[] colorTable = new byte[SourceColorBlockLength];
            sourceGif.Read(colorTable, 0, colorTable.Length);
            
            WriteBytes(ref ms, colorTable, colorTable.Length);
        }

        private void WriteGraphicControlBlock(ref MemoryStream ms, Stream sourceGif, TimeSpan frameDelay)
        {
            if (_isFinished)
                return;

            ms = ms ?? new MemoryStream(GifBytes);

            sourceGif.Position = SourceGraphicControlExtensionPosition; // Locating the source GCE
            var blockhead = new byte[SourceGraphicControlExtensionLength];
            sourceGif.Read(blockhead, 0, blockhead.Length); // Reading source GCE

            WriteShort(ref ms, GraphicControlExtensionBlockIdentifier); // Identifier
            WriteByte(ref ms, GraphicControlExtensionBlockSize); // Block Size
            WriteByte(ref ms, blockhead[3] & 0xf7 | 0x08); // Setting disposal flag
            WriteShort(ref ms, Convert.ToInt32(frameDelay.TotalMilliseconds / 10)); // Setting frame delay
            WriteByte(ref ms, blockhead[6]); // Transparent color index
            WriteByte(ref ms, 0); // Terminator
        }

        private void WriteImageBlock(ref MemoryStream ms, Stream sourceGif, bool includeColorTable, int x, int y, int h, int w)
        {
            if (_isFinished)
                return;

            ms = ms ?? new MemoryStream(GifBytes);

            sourceGif.Position = SourceImageBlockPosition; // Locating the image block
            byte[] header = new byte[SourceImageBlockHeaderLength];
            sourceGif.Read(header, 0, header.Length);
            WriteByte(ref ms, header[0]); // Separator
            WriteShort(ref ms, x); // Position X
            WriteShort(ref ms, y); // Position Y
            WriteShort(ref ms, h); // Height
            WriteShort(ref ms, w); // Width

            if (includeColorTable) // If first frame, use global color table - else use local
            {
                sourceGif.Position = SourceGlobalColorInfoPosition;
                WriteByte(ref ms, sourceGif.ReadByte() & 0x3f | 0x80); // Enabling local color table
                WriteColorTable(ref ms, sourceGif);
            }
            else
            {
                WriteByte(ref ms, header[9] & 0x07 | 0x07); // Disabling local color table
            }

            WriteByte(ref ms, header[10]); // LZW Min Code Size

            // Read/Write image data
            sourceGif.Position = SourceImageBlockPosition + SourceImageBlockHeaderLength;

            var dataLength = sourceGif.ReadByte();
            while (dataLength > 0)
            {
                byte[] imgData = new byte[dataLength];
                sourceGif.Read(imgData, 0, dataLength);

                WriteByte(ref ms, Convert.ToByte(dataLength));
                WriteBytes(ref ms, imgData, dataLength);
                dataLength = sourceGif.ReadByte();
            }

            WriteByte(ref ms, 0); // Terminator

        }

        private void WriteByte(ref MemoryStream ms, int value)
        {
            GifBytes = GifBytes.WriteByte(ref ms, value, _isFinished);
        }

        private void WriteShort(ref MemoryStream ms, int value)
        {
            GifBytes = GifBytes.WriteShort(ref ms, value, _isFinished);
        }

        private void WriteString(ref MemoryStream ms, string value)
        {
            GifBytes = GifBytes.WriteString(ref ms, value, _isFinished);            
        }

        private void WriteBytes(ref MemoryStream ms, byte[]? bytes, int length)
        {
            GifBytes = GifBytes.WriteBytes(ref ms, bytes, length, _isFinished);
        }

        private void WriteBytes(ref MemoryStream ms, byte[] bytes)
        {
            GifBytes = GifBytes.WriteBytes(ref ms, bytes, _isFinished);
        }
  
        public void Finish(ref MemoryStream ms)
        {            
            // Complete File
            WriteByte(ref ms, FileTrailer);
            ms.Flush();
            _isFinished = true;
        }

        public void Dispose()
        {
            
        }
    }

    public static class GIfEncoderExtensions
    {

        public static byte[] WriteByte(this byte[] gifBytes, ref MemoryStream ms, int value, bool _isFinished)
        {
            List<byte> byteList = new List<byte>(gifBytes);
            ms = ms ?? new MemoryStream(gifBytes);
            if (!_isFinished)
            {                
                byte b = Convert.ToByte(value);
                byteList.Add(b);
                ms.Write(new byte[] { b });
            }

            return byteList.ToArray();
        }

        public static byte[] WriteShort(this byte[] gifBytes, ref MemoryStream ms, int value, bool _isFinished)
        {
            List<byte> byteList = new List<byte>(gifBytes);
            ms = ms ?? new MemoryStream(gifBytes);
            if (!_isFinished)
            {                
                Byte b0 = Convert.ToByte(value & 0xff);
                Byte b1 = Convert.ToByte((value >> 8) & 0xff);

                byteList.AddRange(new byte[] { b0, b1 });
                ms.Write(new byte[] { b0, b1 });
                
            }

            return byteList.ToArray();
        }

        public static byte[] WriteString(this byte[] gifBytes, ref MemoryStream ms, string value, bool _isFinished = false)
        {
            List<byte> byteList = new List<byte>(gifBytes);
            ms = ms ?? new MemoryStream(gifBytes);
            if (!_isFinished)
            {
                byte[] stringBytes = value.ToArray().Select(c => (byte)c).ToArray();

                byteList.AddRange(stringBytes);
                ms.Write(stringBytes);

                return byteList.ToArray();
            }

            return byteList.ToArray();
        }

        public static byte[] WriteBytes(this byte[] gifBytes, ref MemoryStream ms, byte[] bytes, bool _isFinished = false)
        {
            List<byte> byteList = new List<byte>(gifBytes);
            ms = ms ?? new MemoryStream(gifBytes);

            if (!_isFinished)
            {                
                if (bytes != null && bytes.Length > 0)
                {
                    byteList.AddRange(bytes);
                    ms.Write(bytes);
                    return byteList.ToArray();
                }
            }

            return byteList.ToArray();
        }

        public static byte[] WriteBytes(this byte[] gifBytes, ref MemoryStream ms, byte[]? bytes, int length, bool _isFinished = false)
        {
            List<byte> byteList = new List<byte>(gifBytes);
            ms = ms ?? new MemoryStream(gifBytes);
            
            if (!_isFinished)
            {                
                if (bytes != null && bytes.Length > 0)
                {
                    length = (int)Math.Abs(length);
                    int bytesLen = Math.Min((int)length, ((int)bytes?.Length));
                    byte[] bytesToWrite = new byte[bytesLen];
                    Array.Copy(bytes, bytesToWrite, bytesLen);

                    byteList.AddRange(bytesToWrite);
                    ms.Write(bytesToWrite);
                    return byteList.ToArray();
                }
            }

            return byteList.ToArray();
        }

    }
}
