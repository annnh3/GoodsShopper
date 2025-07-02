using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Ap.Model.Service
{
    public class ProductTypeInfoService : IProductTypeInfoService
    {
        private readonly IProductTypeRepository productTypeRepository;

        public ProductTypeInfoService(IProductTypeRepository productTypeRepository)
        {
            this.productTypeRepository = productTypeRepository;
            this.InitialTestData();
        }

        public void InitialTestData()
        {
            var result = this.productTypeRepository.Insert(new ProductType
            {
                Name = "周邊商品"
            });
        }

        public (Exception exception, ProductType productType) Insert(ProductTypeInsertDto request)
        {
            try
            {
                var result = this.productTypeRepository.Insert(new ProductType
                {
                    Name = request.Name,
                });

                return (null, result.productType);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception exception, ProductTypeQueryResponseDto response) Query(ProductTypeQueryDto request)
        {
            try
            {
                var result = this.productTypeRepository.Query(request.Id);
                return (null, new ProductTypeQueryResponseDto
                {
                    ProductTypes = new List<ProductType> { result.productType}
                });
            }
            catch(Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception exception, ProductTypeQueryResponseDto response) Query()
        {
            try
            {
                var result = this.productTypeRepository.GetAll();

                return (null, new ProductTypeQueryResponseDto
                {
                    ProductTypes = result.productTypes
                });
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
