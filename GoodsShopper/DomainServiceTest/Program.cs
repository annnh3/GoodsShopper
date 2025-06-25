using GoodsShopper.Domain.Service;

namespace DomainServiceTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.ProductTest();
        }

        void ProductTest()
        {
            var service = new ProductService("http://localhost:8999");

            var result = service.Insert(new GoodsShopper.Domain.DTO.ProductInsertDto
            {
                Name = "Test",
                BrandId = 1,
                CategoryId = 2,
            });

            var getallResult = service.Query();

            var queryResult = service.Query(new GoodsShopper.Domain.DTO.ProductQueryDto
            {
                Name = "Test"
            });

            var aa = "";
        }
    }
}
