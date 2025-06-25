
namespace Live.Libs.Tests.KFreeCoin.Model
{
    using Live.Libs.KFreeCoin.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WithdrawRequestTests
    {
        [TestMethod]
        public void 扣款請求結構SignCode測試()
        {
            var request = WithdrawRequest.GenerateInstance(
                "9bd19a61ad204502a8a6b65c52ebf118",
                "G016121315000001",
                "User001",
                "20.00",
                "G839895955036700666",
                string.Empty);

            Assert.IsTrue(request.SignCode == "f666ecc6c88cd8bf9731686e99c92a86");
        }
    }
}
