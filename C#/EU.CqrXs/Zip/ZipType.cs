using System.ComponentModel;

namespace EU.CqrXs.Zip
{

    [DefaultValue(None)]
    public enum ZipType
    {
        None = 0x00,
        Zip = 0x10,
        GZip = 0x20,
        BZip2 = 0x30,
        Z7 = 0x40
    }


    public static class ZipTypeExtensions
    {
        public static ZipType[] ZipTypes { get => new ZipType[] { ZipType.None, ZipType.GZip, ZipType.BZip2, ZipType.Zip }; }

        public static ZipType[] GetZipTypes()
        {
            List<ZipType> list = new List<ZipType>();
            foreach (string encName in Enum.GetNames(typeof(ZipType)))
            {
                list.Add((ZipType)Enum.Parse(typeof(ZipType), encName));
            }

            return list.ToArray();
        }

        public static ZipType GetZipType(string zipTypeStr)
        {
            if (!string.IsNullOrEmpty(zipTypeStr)) 
            {
                string detectzip = zipTypeStr.ToLower().Replace("zmenu", "");
                detectzip = detectzip.ToLower().Replace("menu", "");
                switch (detectzip)
                {
                    case "zip": return ZipType.Zip;
                    case "gzip": return ZipType.GZip;
                    case "bzip2": return ZipType.BZip2;
                    case "7z": return ZipType.Z7;
                    case "none":
                    default: break;
                }
            }
            return ZipType.None;
        }

        public static string GetZipTypeExtension(this ZipType zipType) {   
        {
                switch (zipType)
                {
                    case ZipType.Zip: return ".zip";
                    case ZipType.GZip: return ".gz";
                    case ZipType.BZip2: return ".bz2";
                    case ZipType.Z7: // return ".7z";
                    case ZipType.None:
                    default: break;
                }
            }
            return string.Empty;
        }


        public static ZipType GetZipTypeFromValue(short zValue)
        {
            zValue = (short)((zValue % 0x100) - (zValue % 0x10));
            foreach (ZipType zType in GetZipTypes())
            {
                if ((short)zType == zValue)
                    return zType;
            }
            return ZipType.None;
        }

        public static string GetUnzipString(this ZipType zType)
        {
            string zipString = zType.ToString();
            if (zipString.Contains("Zip"))
                zipString = zipString.Replace("Zip", "UnZip");
            else if (zipString.Contains("7"))
                zipString = "7unzip";
            else
                zipString = "";

            return zipString;
        }

        /// <summary>
        /// Generic zip extension method for <see cref="ZipType"/>
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns>zipped bytes</returns>
        public static byte[] Zip(this ZipType zipType, byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length == 0)
                throw new InvalidOperationException("byte[] Zip(this ZipType zipType, byte[] inBytes = NULL)");

            switch (zipType)
            {
                case ZipType.BZip2:
                    return BZip2.BZip(inBytes);
                case ZipType.GZip:
                    return GZ.GZipBytes(inBytes);
                case ZipType.Zip:
                    return WinZip.Zip(inBytes);
                case ZipType.Z7: // TODO
                case ZipType.None:
                    break;
                default: // Asset(0)
                    break;
            }

            return inBytes;
        }

        /// <summary>
        /// Generic unzip extension method for <see cref="ZipType"/>
        /// </summary>
        /// <param name="zipType">this the <see cref="ZipType"/></param>
        /// <param name="compressedBytes"></param>
        /// <returns>decompressed bytes</returns>
        public static byte[] Unzip(this ZipType zipType, byte[] compressedBytes)
        {
            if (compressedBytes == null || compressedBytes.Length == 0)
                throw new InvalidOperationException("byte[] Unzip(this ZipType zipType, byte[] compressedBytes = NULL)");

            switch (zipType)
            {
                case ZipType.BZip2:
                    byte[] dbz2bytes = new List<byte>().ToArray();
                    try
                    {
                        dbz2bytes = BZip2.BUnZip(compressedBytes);
                    }
                    catch (Exception exBZ2)
                    {
                        dbz2bytes = BZip2.BUnZip2Bytes(compressedBytes);
                    }
                    if (dbz2bytes.Length < (compressedBytes.Length - 16))
                        dbz2bytes = compressedBytes;
                    return dbz2bytes;
                case ZipType.GZip:
                    byte[] gzBytes = new List<byte>().ToArray();
                    // try
                    // {
                    gzBytes = GZ.GUnZipBytes(compressedBytes);
                    // }
                    //                     catch (Exception exGZ)
                    // {
                    // gzBytes = GZ.GUnZip(compressedBytes);
                    // }
                    // if (gzBytes.Length < (compressedBytes.Length - 16))
                    //      gzBytes = compressedBytes;
                    return gzBytes;
                case ZipType.Zip:
                    return WinZip.UnZip(compressedBytes);
                case ZipType.Z7: // TODO
                case ZipType.None:
                    break;
                default: // Asset(0)
                    break;
            }

            return compressedBytes;
        }
    }
    
}
