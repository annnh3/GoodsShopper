using System;
using System.Collections.Generic;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;
using Microsoft.AspNet.SignalR;

namespace GoodsShopper.Ap.Model.Service
{
    public class ProductInfoService : IProductInfoService
    {
        private readonly IProductRepository productRepository;

        public ProductInfoService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            this.InitialTestDate();
        }

        private void InitialTestDate()
        {
            var result = this.productRepository.Insert(new Product
            {
                BrandId = 1,
                CategoryId = 1,
                Name = "testProduct"
            });
        }

        public (Exception exception, Product product) Insert(ProductInsertDto request)
        {
            try
            {
                var result = this.productRepository.Insert(new Product
                {
                    BrandId = request.BrandId,
                    CategoryId = request.CategoryId,
                    Name = request.Name
                });

                return (null, result.product);
            }
            catch (Exception ex) 
            {
                return (ex, null);
            }
        }

        public (Exception exception, ProductQueryResponseDto response) Query(ProductQueryDto request)
        {
            try 
            {
                var result = this.productRepository.Query(request.Name);

                return (null, new ProductQueryResponseDto
                {
                    Products = new List<Product> { result.product }
                });
            }
            catch (Exception ex) 
            { 
                return (ex, null);
            }
        }

        public (Exception exception, ProductQueryResponseDto response) Query()
        {
            try
            {
                var result = this.productRepository.GetAll();

                return (null, new ProductQueryResponseDto
                {
                    Products = result.products
                });
            }
            catch (Exception ex) 
            {
                return (ex, null);
            }
        }
    }
}
