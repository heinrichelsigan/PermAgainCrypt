using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Crypt.Hash;
using EU.CqrXs.Util;
using EU.CqrXs.Zip;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace EU.CqrXs.Test
{

    /// <summary>
    /// TestEncryptionTwoAlgos tests all en- / decryption with 2 <see cref="CipherEnum"/>  algos in <see cref="CipherPipe"/>
    /// Aes => Aes, Aes => BlowFish, Aes => Camellia, 
    /// BlowFish => Aes, BlowFish => BlowFish, BlowFish => Camellia, 
    /// Camellia => Aes, Camellia => BlowFish, Camellia => Camellia, ...
    /// </summary>
    [TestClass]
    public sealed class TestEncryptionIVAlgos
    {
        internal static string Email = Constants.AUTHOR_EMAIL;

        public byte[] GetImageBytes()
        {
            string simg = "";
            Random rand = new Random();
            if (string.IsNullOrEmpty(simg) || File.Exists(simg))
            {
                simg = rand.GetHexString(8, true);
            }

            Bitmap mergeImage = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif");

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeImage))
            {

                Color color = ColorTranslator.FromHtml("#0000dd");
                string drawString = simg;
                Font drawFont = new Font("Microsoft Sans Serif", 7, FontStyle.Regular);
                SolidBrush drawBrush = new SolidBrush(color);
                float x = 1.5F;
                float y = 2.0F;
                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            }

            string simage = simg + ".png";
            string imgFile = Path.Combine(Area23Log.TempDir, simage);

            mergeImage.Save(imgFile, ImageFormat.Png);

            byte[] bytes = File.ReadAllBytes(imgFile);
            return bytes;
        }

        [TestMethod]
        public void TestEncryptionIVAlgorithmsBytes()
        {           
            string className = "TestEncryptionIVAlgos";
            string methodBase = "TestEncryptionIVAlgosBytes";
            try
            {
                className = MethodBase.GetCurrentMethod().DeclaringType.Name;
                methodBase = MethodBase.GetCurrentMethod().Name;
                Email = RegistryAccessor.GetEmailFromRegistry();
            }            
            catch 
            {
                className = this.GetType().BaseType.Name;
                methodBase = "TestEncryptionIVAlgoBytes";
                Email = Constants.AUTHOR_EMAIL;
            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[started]");

            DateTime startOp = DateTime.Now, midOp = DateTime.Now, endOp = DateTime.Now;
            TimeSpan encOpTime = TimeSpan.Zero, decOpTime = TimeSpan.Zero, allOpTime = TimeSpan.Zero;
            string fileByesTest = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";
            string fileCsvOut = AppContext.BaseDirectory + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd_hh_") + $"{className}_{methodBase}.csv";

            Assert.IsTrue(File.Exists(fileTextTest));
            List<CipherEnum> chipherEList = CipherEnumExtensions.GetCipherTypes().ToList();
            chipherEList.RemoveAt(chipherEList.Count - 1)  ; // remove RSA
            CipherEnum[] cipherEnums = chipherEList.ToArray();
            ZipType[] zTypes = new ZipType[] { ZipType.BZip2, ZipType.GZip, ZipType.Zip, ZipType.None };
            KeyHash[] kHashes = KeyHash_Extensions.GetHashTypes();
            KeyHash kHash = KeyHash.Hex;
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Base16, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;            
            string plainText = File.ReadAllText(fileTextTest);            
            for (int i = 0; i < cipherEnums.Length - 3; i++)
            {

                encType = encodingTypes[i % encodingTypes.Length];
                zType = zTypes[i % zTypes.Length];
                kHash = kHashes[i % kHashes.Length];

                for (int j = 0; j < 4; j++)
                {
                    //if (cipherEnums[j] == CipherEnum.Rsa)
                    //    cipherEnums[j] = CipherEnum.Des3Net;
                    zType = ZipType.None;
                }

                CipherEnum[] cipherQuartupel = new CipherEnum[] { cipherEnums[i], cipherEnums[i + 1], cipherEnums[i + 2], cipherEnums[i + 3] };
                CipherPipe pipe = new CipherPipe(cipherQuartupel, 8, encType, zType, kHash);
                byte[] plainBytes = File.ReadAllBytes(fileByesTest);

                try
                {
                    startOp = DateTime.Now;
                    byte[] cipherBytes = pipe.EncryptEncodeBytes(plainBytes, Email, KeyHash.Hex.Hash(Email), encType, zType, kHash);

                    Assert.IsNotNull(cipherBytes);

                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    byte[] deCodedBytes = pipe.DecodeDecrpytBytes(cipherBytes, Email, KeyHash.Hex.Hash(Email), encType, zType, kHash);

                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 &&
                        (Math.Abs(deCodedBytes.Length - plainBytes.Length) <= 16));

                    Assert.IsTrue(plainBytes != null && deCodedBytes != null && deCodedBytes.Length > 0 && deCodedBytes.Length > 0 &&
                        plainBytes.LongLength == deCodedBytes.LongLength && plainBytes[i] == deCodedBytes[i]);
            
                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    if (deCodedBytes == null || deCodedBytes.Length < 1 || plainBytes.LongLength != deCodedBytes.LongLength || plainBytes[i] != deCodedBytes[i])
                    {
                        Console.WriteLine($"{cipherEnums[i]}=>{cipherEnums[i+1]}=>{cipherEnums[i+2]}=>{cipherEnums[i+3]} for {Email} {zType} {encType} {kHash}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                        Console.WriteLine($"          \tdeCodedBytes.Length ({deCodedBytes.Length}) != plainBytes.Length ({plainBytes.Length})");
                        Assert.Fail();
                    }
                    double size = deCodedBytes.Length / (1024);
                    Console.WriteLine($"{size}KB {cipherEnums[i]}=>{cipherEnums[i+1]}=>{cipherEnums[i+2]}=>{cipherEnums[i+3]} for {Email} {zType} {encType} {kHash}\tencrypt in {encOpTime.ToString("ss'.'ffff")} \tdecrypt in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");

                }
                catch (Exception e)
                {
                    Console.WriteLine($"{cipherEnums[i]}=>{cipherEnums[i+1]}=>{cipherEnums[i+2]}=>{cipherEnums[i+3]} for {Email}\tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }

            }
            Console.WriteLine($"{DateTime.Now.Area23DateTimeWithSeconds()} \t{className}.{methodBase}() \t[finished]");
            return;
        }


    }
}
