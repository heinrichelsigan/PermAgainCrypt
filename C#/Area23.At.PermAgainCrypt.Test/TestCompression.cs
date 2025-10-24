using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;

namespace Area23.At.PermAgainCrypt.Test
{
    /// <summary>
    /// TestCompression test zipping <see cref="ZipType">enum ZipType</see>
    /// ZipType.GZip, ZipType.BZip2, ZipType.Zip 
    /// </summary>
    [TestClass]
    public sealed class TestCompression
    {

        [TestMethod]
        public void TestAllCompression()
        {
            Console.WriteLine("Cipher,ZipType,OpTime,FullName,MB/s,Size KB");
            DateTime startOp = DateTime.Now;
            TimeSpan opTime = TimeSpan.Zero;
            string fileByesTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";

            Assert.IsTrue(File.Exists(fileTextTest));
            CipherEnum[] cipherEnums = new CipherEnum[] { CipherEnum.Des3 };
            ZipType[] zTypes = new ZipType[] { ZipType.GZip, ZipType.BZip2, ZipType.Zip };
            KeyHash kHash = KeyHash.Hex;
            // ZipType zType = ZipType.None;            
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.None, EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;
            CipherPipe pipe = new CipherPipe(cipherEnums); //  new CipherPipe(Encoding.UTF8.GetBytes(Constants.AUTHOR_EMAIL), 0);
            string plainText = File.ReadAllText(fileTextTest);
            foreach (ZipType zType in zTypes)
            {
                try
                {
                    startOp = DateTime.Now;
                    string cipherText = pipe.EncrpytTextGoRounds(plainText, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                                                encType, zType, kHash);
                    Assert.IsNotNull(cipherText);
                    opTime = DateTime.Now.Subtract(startOp);
                    string deCodedText = pipe.DecryptTextRoundsGo(cipherText, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                                            encType, zType, kHash);

                    Assert.AreEqual<string>(deCodedText, plainText);

                    if (string.IsNullOrEmpty(deCodedText) || !deCodedText.Equals(plainText, StringComparison.Ordinal))
                    {
                        Console.WriteLine(zType.ToString() + " test failed.");
                        Assert.Fail();
                    }
                    Console.WriteLine(zType.ToString() + " \ttest \t[passed]");
                }
                catch (Exception e)
                {
                    Console.WriteLine(zType.ToString() + " \tException: " + e.GetType() + " \t" + e.Message);
                }

            }
            return;
        }


    }
}
