using System.Drawing.Imaging;

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

        private readonly static Lock _gifLock = new Lock();
        private bool _isFirstImage = true;
        private bool _isFinished = false;
        private int? _repeatCount = 0;
        TimeSpan _frameDelay;
        
        public readonly MemoryStream _memoryStream;
        public Bitmap AnimBitmap { get => new Bitmap(_memoryStream, false); }
        public Image AnimImage { get => ((Image)AnimBitmap); }


        // Public Accessors
        public TimeSpan FrameDelay { get => _frameDelay; }

        

        public byte[] GifData
        {
            get
            {
                if (!_isFinished) Finish();
                return _memoryStream.ToByteArray();
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
            _memoryStream = new MemoryStream();
            _repeatCount = repeatCount ?? 0;
            _frameDelay = frameDelay.GetValueOrDefault();
            _isFinished = false;

            using (MemoryStream srcGif = new MemoryStream())
            {
                img.Save(srcGif, ImageFormat.Gif);
                WriteHeader(srcGif, img.Width, img.Height);
                WriteGraphicControlBlock(srcGif, FrameDelay);
                WriteImageBlock(srcGif, !_isFirstImage, 0, 0, img.Width, img.Height);
            }

            _isFirstImage = false;

        }

        /// <summary>
        /// 2025-12-04 4th December Release  v2.25.1204
        /// Added new ctor in GifEncoder  for creating animated gifs
        /// <code>
        /// GifEncoder gifAnimEncoder = new GifEncoder(gifStartImage, 1, ts, bitmaps.ToArray());
        /// Bitmap animGif = gifAnimEncoder.AnimBitmap;
        /// animGif.Save("h:\\tmp\\"+DateTime.Now.ToString("mmss") + ".gif");</code>
        /// </summary>
        /// <param name="bmp">start basic image</param>
        /// <param name="repeatCount">repeat count</param>
        /// <param name="frameDelay">delay between gif frames</param>
        /// <param name="gifFrames"><see cref="T:Image[]">Image[]</see></param>
        public GifEncoder(Bitmap bmp, int? repeatCount, TimeSpan? frameDelay, params Bitmap[] gifFrames)
        {
            _memoryStream = new MemoryStream();
            _repeatCount = repeatCount ?? 0;
            _frameDelay = frameDelay.GetValueOrDefault();
            _isFinished = false;

            using (MemoryStream srcGif = new MemoryStream())
            {
                bmp.Save(srcGif, ImageFormat.Gif);
                WriteHeader(srcGif, bmp.Width, bmp.Height);
                WriteGraphicControlBlock(srcGif, FrameDelay);
                WriteImageBlock(srcGif, !_isFirstImage, 0, 0, bmp.Width, bmp.Height);
            }

            _isFirstImage = false;

            foreach (Bitmap? frame in gifFrames)
            {
                if (frame != null)
                    AddFrame((Image)frame, FrameDelay);
            }

            if (!_isFinished)
                Finish();
        }


        /// <summary>
        /// Initializes a new instance of the GifEncoder class with the specified frames, optional repeat count, and
        /// optional frame delay.
        /// </summary>
        /// <remarks>The first image in the gifFrames array is used as the initial frame of the GIF. All
        /// provided frames are added in the order specified. The resulting GIF is finalized upon construction and
        /// cannot be modified after initialization.</remarks>
        /// <param name="repeatCount">The number of times the GIF animation should repeat. If null, the animation will loop indefinitely.</param>
        /// <param name="frameDelay">The delay between frames, specified as a TimeSpan. If null, a default delay is used.</param>
        /// <param name="gifFrames">An array of Bitmap images to include as frames in the GIF. The order of images determines the frame
        /// sequence. Cannot be null or empty.</param>
        public GifEncoder(Bitmap[] gifFrames, int? repeatCount, TimeSpan? frameDelay)
        {
            _memoryStream = new MemoryStream();
            _repeatCount = repeatCount ?? 0;
            _frameDelay = frameDelay ?? frameDelay.GetValueOrDefault();
            _isFinished = false;

            for (int i = 0; i < gifFrames.Length; i++)
            {
                if (i < 1 && gifFrames[0] != null)
                {
                    using (MemoryStream srcGif = new MemoryStream())
                    {
                        gifFrames[0].Save(srcGif, ImageFormat.Gif);
                        WriteHeader(srcGif, gifFrames[0].Width, gifFrames[0].Height);
                        WriteGraphicControlBlock(srcGif, FrameDelay);
                        WriteImageBlock(srcGif, !_isFirstImage, 0, 0, gifFrames[0].Width, gifFrames[0].Height);
                    }
                    _isFirstImage = false;
                }
                else if (gifFrames[i] != null)
                    AddFrame((Image)gifFrames[i], FrameDelay);
            }

            if (!_isFinished)
                Finish();
        }



        /// <summary>
        /// Adds a frame to this animation.
        /// </summary>
        /// <param name="img">The image to add</param>
        /// <param name="frameDelay">TimeSpan for delay</param>
        public void AddFrame(Image img, TimeSpan? frameDelay = null)
        {
            if (_isFinished)
                return;

            _frameDelay = frameDelay.GetValueOrDefault();
            using (var gifStream = new MemoryStream())
            {
                img.Save(gifStream, ImageFormat.Gif);
                if (_isFirstImage) // Init gif header with global color table info of 1st frame
                {
                    WriteHeader(gifStream, img.Width, img.Height);
                }
                WriteGraphicControlBlock(gifStream, FrameDelay);
                WriteImageBlock(gifStream, !_isFirstImage, 0, 0, img.Width, img.Height);
            }
            _isFirstImage = false;
        }

        private void WriteHeader(Stream sourceGif, int w, int h)
        {
            if (_isFinished)
                return;

            // File Header
            WriteString(FileType);
            WriteString(FileVersion);
            WriteShort(w); // Initial Logical Width
            WriteShort(h); // Initial Logical Height
            
            sourceGif.Position = SourceGlobalColorInfoPosition;
            WriteByte(sourceGif.ReadByte()); // Global Color Table Info
            WriteByte(0); // Background Color Index
            WriteByte(0); // Pixel aspect ratio
            WriteColorTable(sourceGif);

            // App Extension Header
            WriteShort(ApplicationExtensionBlockIdentifier); // 0xff21            
            WriteByte(ApplicationBlockSize);
            WriteString(ApplicationIdentification);            
            WriteByte(3); // Application block length
            WriteByte(1);            
            WriteShort(_repeatCount.GetValueOrDefault(0)); // Repeat count for images.
            WriteByte(0); // terminator
        }

        private void WriteColorTable(Stream sourceGif)
        {
            if (_isFinished)
                return;
            
            sourceGif.Position = SourceColorBlockPosition; // Locating the image color table
            byte[] colorTable = new byte[SourceColorBlockLength];
            sourceGif.Read(colorTable, 0, colorTable.Length);
            
            WriteBytes(colorTable, colorTable.Length);
        }

        private void WriteGraphicControlBlock(Stream sourceGif, TimeSpan frameDelay)
        {
            if (_isFinished)
                return;

            sourceGif.Position = SourceGraphicControlExtensionPosition; // Locating the source GCE
            var blockhead = new byte[SourceGraphicControlExtensionLength];
            sourceGif.Read(blockhead, 0, blockhead.Length); // Reading source GCE

            WriteShort(GraphicControlExtensionBlockIdentifier); // Identifier
            WriteByte(GraphicControlExtensionBlockSize); // Block Size
            WriteByte(blockhead[3] & 0xf7 | 0x08); // Setting disposal flag
            WriteShort(Convert.ToInt32(frameDelay.TotalMilliseconds / 10)); // Setting frame delay
            WriteByte(blockhead[6]); // Transparent color index
            WriteByte(0); // Terminator
        }

        private void WriteImageBlock(Stream sourceGif, bool includeColorTable, int x, int y, int h, int w)
        {
            if (_isFinished)
                return;


            sourceGif.Position = SourceImageBlockPosition; // Locating the image block
            byte[] header = new byte[SourceImageBlockHeaderLength];
            sourceGif.Read(header, 0, header.Length);
            WriteByte(header[0]); // Separator
            WriteShort(x); // Position X
            WriteShort(y); // Position Y
            WriteShort(h); // Height
            WriteShort(w); // Width

            if (includeColorTable) // If first frame, use global color table - else use local
            {
                sourceGif.Position = SourceGlobalColorInfoPosition;
                WriteByte(sourceGif.ReadByte() & 0x3f | 0x80); // Enabling local color table
                WriteColorTable(sourceGif);
            }
            else
            {
                WriteByte(header[9] & 0x07 | 0x07); // Disabling local color table
            }

            WriteByte(header[10]); // LZW Min Code Size

            // Read/Write image data
            sourceGif.Position = SourceImageBlockPosition + SourceImageBlockHeaderLength;

            var dataLength = sourceGif.ReadByte();
            while (dataLength > 0)
            {
                byte[] imgData = new byte[dataLength];
                sourceGif.Read(imgData, 0, dataLength);

                WriteByte(Convert.ToByte(dataLength));
                WriteBytes(imgData, dataLength);
                dataLength = sourceGif.ReadByte();
            }

            WriteByte(0); // Terminator

        }

        private void WriteByte(int value)
        {
            if (!_isFinished)
            {
                byte b = Convert.ToByte(value);
                _memoryStream.Write(new byte[] { b });
            }
        }

        private void WriteShort(int value)
        {
            if (!_isFinished)
            {
                Byte b0 = Convert.ToByte(value & 0xff);
                Byte b1 = Convert.ToByte((value >> 8) & 0xff);

                _memoryStream.Write(new byte[] { b0, b1 });
            }
        }

        private void WriteString(string value)
        {
            if (!_isFinished)
            {
                byte[] stringBytes = value.ToArray().Select(c => (byte)c).ToArray();
                _memoryStream.Write(stringBytes);
            }
        }

        private void WriteBytes(byte[]? bytes, int length)
        {
            if (!_isFinished)
            {
                if (bytes != null && bytes.Length > 0)
                {
                    length = (int)Math.Abs(length);
                    int bytesLen = Math.Min((int)length, ((int)bytes?.Length));
                    byte[] bytesToWrite = new byte[bytesLen];
                    Array.Copy(bytes, bytesToWrite, bytesLen);

                    _memoryStream.Write(bytesToWrite);
                }
            }
        }

        private void WriteBytes(ref MemoryStream ms, byte[] bytes)
        {
            if (!_isFinished)
            {
                if (bytes != null && bytes.Length > 0)
                {
                    _memoryStream.Write(bytes);
                }
            }
        }
  
        public void Finish()
        {
            lock (_gifLock)
            {
                if (!_isFinished)
                {
                    // Complete File
                    WriteByte(FileTrailer);
                    _memoryStream.Flush();
                    _isFinished = true;
                }
            }
        }

        public void Dispose()
        {
            Finish();
            try
            {
                _memoryStream.Close();
            }
            catch
            {
                // ignore
            }
            try
            {
                _memoryStream.Dispose();
            }
            catch
            {
                // ignore
            }            
        }
    }
    
}
