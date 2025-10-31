using Area23.At.Framework.Core.Util;
using System.Configuration;

namespace Area23.At.PermAgainCrypt.Test
{
    [TestClass]
    public sealed class TestInitCleanup
    {

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

    }
}
