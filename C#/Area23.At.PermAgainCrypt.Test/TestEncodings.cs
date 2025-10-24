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
            Console.WriteLine("Cipher,ZipType,OpTime,FullName,MB/s,Size KB");
            DateTime startOp = DateTime.Now;
            TimeSpan opTime = TimeSpan.Zero;
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
                    opTime = DateTime.Now.Subtract(startOp);
                    string deCodedText = pipe.DecryptTextRoundsGo(cipherText, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                                            encType, zType, kHash);

                    Assert.AreEqual<string>(deCodedText, plainText);

                    if (string.IsNullOrEmpty(deCodedText) || !deCodedText.Equals(plainText, StringComparison.Ordinal))
                    {
                        Console.WriteLine(encType.ToString() + " test failed.");
                        Assert.Fail();
                    }
                    Console.WriteLine(encType.ToString() + " \ttest \t[passed]");
                }
                catch (Exception e)
                {
                    Console.WriteLine(encType.ToString() + " \tException: " + e.GetType() + " \t" + e.Message);
                }

            }
            return;
        }

    }
}
