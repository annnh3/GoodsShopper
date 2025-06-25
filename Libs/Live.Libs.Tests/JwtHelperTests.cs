
namespace Live.Libs.Tests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JwtHelperTests
    {
        [TestMethod]
        public void JWT加密測試()
        {
            var token = JwtHelper.Encode(new Dictionary<string, object>()
            {
                { "Account", "TEST001-16"},
                { "Site", 16},
                { "Member", "Test001"}
            }, "hk3345678");

            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void JWT解密測試()
        {
            var token = JwtHelper.Encode(new Dictionary<string, object>()
            {
                { "Account", "TEST001-16"},
                { "Site", 16},
                { "Member", "Test001"}
            }, "hk3345678");

            Assert.IsNotNull(token);

            var dic = JwtHelper.Decode(token, "hk3345678");
            Assert.IsNotNull(dic);
            Assert.AreEqual(dic["Account"], "TEST001-16");
            Assert.AreEqual(dic["Site"], (long)16);
            Assert.AreEqual(dic["Member"], "Test001");
        }
    }
}
