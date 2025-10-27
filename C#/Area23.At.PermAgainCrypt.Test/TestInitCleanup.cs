using Area23.At.Framework.Core;
using Area23.At.Framework.Core.Crypt;
using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zip;
using Org.BouncyCastle.Crypto;
using System.Configuration;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;

namespace Area23.At.PermAgainCrypt.Test
{
    [TestClass]
    public sealed class TestInitCleanup
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // This method is called once for the test assembly, before any tests are run.
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // This method is called once for the test assembly, after all tests are run.
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            // This method is called once for the test class, before any tests of the class are run.
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // This method is called once for the test class, after all tests of the class are run.
        }

        [TestInitialize]
        public void TestInit()
        {
            string statdir = ConfigurationManager.AppSettings["StatDir"];
            if (ConfigurationManager.AppSettings["SkipTests"] != null &&
                ConfigurationManager.AppSettings["SkipTests"].Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Inconclusive("Tests are skipped as per configuration.");
            }
            if (!string.IsNullOrEmpty(statdir) && Directory.Exists(statdir))
            {
                foreach (string file in Directory.GetFiles(statdir, "*.csv"))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception exi)
                    {
                        Area23Log.LogOriginEx("TestInit", exi);
                    }
                }
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // This method is called after each test method.
        }

        //[TestMethod]
        //public void TestAllEncodings()
        //{
        //    Console.WriteLine("Cipher,ZipType,OpTime,FullName,MB/s,Size KB");
        //    DateTime startOp = DateTime.Now;
        //    TimeSpan opTime = TimeSpan.Zero;
        //    string fileByesTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "2025-09-23_Stats.gif";
        //    string fileTextTest = AppContext.BaseDirectory + Path.DirectorySeparatorChar + "README.MD";

        //    Assert.IsTrue(File.Exists(fileTextTest));
        //    CipherEnum[] cipherEnums = new CipherEnum[] { CipherEnum.Aes };
        //    ZipType[] zTypes = new ZipType[] { ZipType.None };
        //    KeyHash kHash = KeyHash.Hex;
        //    ZipType zType = ZipType.None;
        //    EncodingType[] encodingTypes = new EncodingType[] { EncodingType.Uu, EncodingType.Xx, EncodingType.Base64, EncodingType.Hex32, EncodingType.Hex16 };
        //    CipherPipe pipe = new CipherPipe(cipherEnums); // new CipherPipe(Encoding.UTF8.GetBytes(Constants.AUTHOR_EMAIL), 0);
        //    string plainText = File.ReadAllText(fileTextTest);
        //    foreach (EncodingType encType in encodingTypes)
        //    {
        //        try
        //        {
        //            startOp = DateTime.Now;
        //            string cipherText = pipe.EncrpytTextGoRounds(plainText, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
        //                                        encType, zType, kHash);
        //            Assert.IsNotNull(cipherText);
        //            opTime = DateTime.Now.Subtract(startOp);
        //            string deCodedText = pipe.DecryptTextRoundsGo(cipherText, Constants.AUTHOR_EMAIL, KeyHash.Hex.Hash(Constants.AUTHOR_EMAIL),
        //                                    encType, zType, kHash);

        //            Assert.AreEqual<string>(deCodedText, plainText);

        //            if (string.IsNullOrEmpty(deCodedText) || !deCodedText.Equals(plainText, StringComparison.Ordinal))
        //            {
        //                Console.WriteLine(encType.ToString() + " test failed.");
        //                Assert.Fail();
        //            }
        //            Console.WriteLine(encType.ToString() + " \ttest \t[passed]");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(encType.ToString() + " \tException: " + e.GetType() + " \t" + e.Message);
        //        }
                
        //    }
        //    return ;
        //}


    }
}
