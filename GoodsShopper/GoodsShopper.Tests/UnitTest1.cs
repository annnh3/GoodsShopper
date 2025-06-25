using GoodsShopper.Domain.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoodsShopper.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = new ProductService("http://localhost:8999");
            var res = a.Insert(new Domain.DTO.ProductInsertDto
            {
                BrandId = 1,
                CategoryId = 1,
                Name = "123"
            });

            var aa = "";
        }
    }
}
