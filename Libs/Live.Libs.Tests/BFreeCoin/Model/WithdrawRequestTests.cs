
namespace Live.Libs.Tests.BFreeCoin.Model
{
    using Live.Libs.BFreeCoin.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WithdrawRequestTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var request = WithdrawRequest.GenerateInstance(
                "86sdf79sd6gdss8sd87fd82b085c6d5a",
                "F666",
                21,
                50,
                "1122211425065812345");

            Assert.IsTrue(request.Key == "715a3094d175a87aebddcf4cc57d06a3");
        }
    }
}
