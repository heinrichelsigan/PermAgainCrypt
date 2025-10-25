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
            Console.WriteLine("TestCompression.TestAllCompression() \t[started]");
            DateTime startOp = DateTime.Now, midOp = DateTime.Now, endOp = DateTime.Now;
            TimeSpan encOpTime = TimeSpan.Zero, decOpTime = TimeSpan.Zero, allOpTime = TimeSpan.Zero;
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
                    
                    midOp = DateTime.Now;
                    encOpTime = midOp.Subtract(startOp);
                    string deCodedText = pipe.DecryptTextRoundsGo(cipherText, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                                            encType, zType, kHash);
                    Assert.AreEqual<string>(deCodedText, plainText);
                    
                    endOp = DateTime.Now;
                    decOpTime = endOp.Subtract(midOp);
                    allOpTime = endOp.Subtract(startOp);

                    if (string.IsNullOrEmpty(deCodedText) || !deCodedText.Equals(plainText, StringComparison.Ordinal))
                    {
                        Console.WriteLine($"{zType} \tzipped in {encOpTime.ToString("ss'.'ffff")} \tunzipped in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                        Console.WriteLine($"          \tdeCodedText Length=[{deCodedText.Length}] != plainText Length[{plainText.Length}]");
                        Assert.Fail();
                    }
                    Console.WriteLine($"{zType} \tzipped in {encOpTime.ToString("ss'.'ffff")} \tunzipped in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{zType} \tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }

            }
            return;
        }


    }
}
