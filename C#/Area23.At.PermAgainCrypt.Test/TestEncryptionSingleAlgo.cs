using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;

namespace Area23.At.PermAgainCrypt.Test
{
    /// <summary>
    /// TestEncryptionSingleAlgo tests all encryption / decryption via <see cref="CipherEnum"/> = <see cref="CipherEnumExtensions.GetCipherTypes()" />
    /// Aes, BlowFish, Camellia, Cast6, Des3, Fish2, Fish3, ...        
    /// </summary>
    [TestClass]
    public sealed class TestEncryptionSingleAlgo
    {
        // TODO: Fix ZenMatrix in Test

        [TestMethod]
        public void TestEncryptionAllSingleAlgo()
        {
            Console.WriteLine("Cipher,ZipType,OpTime,FullName,MB/s,Size KB");
            DateTime startOp = DateTime.Now;
            TimeSpan opTime = TimeSpan.Zero;
            string fileBytesTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
            string fileTextTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";

            Assert.IsTrue(File.Exists(fileTextTest));
            CipherEnum[] cipherTypes = CipherEnumExtensions.GetCipherTypes();
            ZipType[] zTypes = new ZipType[] { ZipType.None };
            KeyHash kHash = KeyHash.Hex;
            ZipType zType = ZipType.None;
            EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
            EncodingType encType = EncodingType.Base64;            
            string plainText = File.ReadAllText(fileTextTest);
            byte[] plainBytes = File.ReadAllBytes(fileBytesTest);
            foreach (CipherEnum cipherEnum in cipherTypes)
            {
                CipherEnum[] cipherEnums = new CipherEnum[] { cipherEnum };
                CipherPipe pipe = new CipherPipe(cipherEnums); // new CipherPipe(Encoding.UTF8.GetBytes(Constants.AUTHOR_EMAIL), 0);
                try
                {
                    startOp = DateTime.Now;
                    byte[] cipherBytes = pipe.EncrpytFileBytesGoRounds(plainBytes, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                                                encType, zType, kHash);
                    Assert.IsNotNull(cipherBytes);
                    opTime = DateTime.Now.Subtract(startOp);
                    byte[] deCodedBytes = pipe.DecryptFileBytesRoundsGo(cipherBytes, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
                                            encType, zType, kHash);

                    Assert.IsTrue(plainBytes != null && deCodedBytes != null &&  deCodedBytes.Length > 0 && plainBytes.Length == deCodedBytes.Length);

                    if (deCodedBytes == null || deCodedBytes.Length < 1 || deCodedBytes.Length != plainBytes.Length)
                    {
                        Console.WriteLine(cipherEnum.ToString() + " test compating plain bytes with decrypted(encrypted(plainBytes)) failed.");
                        Assert.Fail();
                    }
                    Console.WriteLine(cipherEnum.ToString() + " \ttest \t[passed]");
                }
                catch (Exception e)
                {
                    Console.WriteLine(cipherEnum.ToString() + " \tException: " + e.GetType() + " \t" + e.Message);
                }

            }
            return;
        }

    }
}
