using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Live.Libs.Tests
{
    [TestClass]
    public class LanguageHelperTests
    {
        [TestMethod]
        public void 站別轉語系測試()
        {
            var tw = LanguageHelper.FromSite(21);
            var cn = LanguageHelper.FromSite(16);
            var vi = LanguageHelper.FromSite(18);
            var th = LanguageHelper.FromSite(19);

            Assert.AreEqual(LanguageType.Tw, tw);
            Assert.AreEqual(LanguageType.Cn, cn);
            Assert.AreEqual(LanguageType.Vi, vi);
            Assert.AreEqual(LanguageType.Th, th);
        }

        [TestMethod]
        public void 錯誤站別轉語系測試()
        {
            var none = LanguageHelper.FromSite(99);

            Assert.AreEqual(LanguageType.None, none);
        }
    }
}