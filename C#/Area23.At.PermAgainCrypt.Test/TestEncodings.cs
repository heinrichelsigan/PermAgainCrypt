using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;

namespace Area23.At.PermAgainCrypt.Test
{
    /// <summary>
    /// TestEncodings tests several  Encodings from enum <see cref="EncodingType"/>
    /// EncodingType.Base16, EncodingType.Hex16, EncodingType.Base32, EncodingType.Hex32, EncodingType.Base64, EncodingType.Uu, EncodingType.Xx,  
    /// EncodingType.Base16, EncodingType.Hex16, EncodingType.Base32, EncodingType.Hex32, EncodingType.Base64, EncodingType.Uu, EncodingType.Xx,  
    /// </summary>
    [TestClass]
    public sealed class TestEncodings
    {
        [TestMethod]
        public void TestAllEncodings()
        {
            Console.WriteLine("TestEncodings.TestAllEncodings() \t[started]");  
            DateTime startOp = DateTime.Now, midOp = DateTime.Now, endOp = DateTime.Now;
            TimeSpan encOpTime = TimeSpan.Zero, decOpTime = TimeSpan.Zero, allOpTime = TimeSpan.Zero;
            string fileByesTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";

            Assert.IsTrue(File.Exists(fileTextTest));
            CipherEnum[] cipherEnums = new CipherEnum[] { CipherEnum.Aes };
            ZipType[] zTypes = new ZipType[] { ZipType.None };
            KeyHash kHash = KeyHash.Hex;
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Base16, EncodingType.Hex16, EncodingType.Base32, EncodingType.Hex32, EncodingType.Base64, EncodingType.Uu, EncodingType.Xx, };
            CipherPipe pipe = new CipherPipe(cipherEnums); // new CipherPipe(Encoding.UTF8.GetBytes(Constants.AUTHOR_EMAIL), 0);
            string plainText = File.ReadAllText(fileTextTest);
            foreach (EncodingType encType in encodingTypes)
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

                    if (string.IsNullOrEmpty(deCodedText) || plainText.Length != deCodedText.Length || !deCodedText.Equals(plainText, StringComparison.Ordinal))
                    {
                        Console.WriteLine($"{encType} \tencoded {encOpTime.ToString("ss'.'ffff")} \tdecoded in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [failed]");
                        Console.WriteLine($"          \tdeCodedText Length=[{deCodedText.Length}] != plainText Length[{plainText.Length}]");
                        Assert.Fail();
                    }
                    Console.WriteLine($"{encType} \tencoded in {encOpTime.ToString("ss'.'ffff")} \tdecoded in {decOpTime.ToString("ss'.'ffff")} \ttotal {allOpTime.ToString("ss'.'ffff")} [passed]");                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{encType} \tException: {e.GetType()} \t{e.Message}\r\n      \t{e.StackTrace}");
                }

            }
            return;
        }

    }
}
