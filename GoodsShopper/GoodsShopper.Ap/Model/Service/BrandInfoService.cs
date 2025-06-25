using System;
using System.Collections.Generic;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;
using Microsoft.AspNet.SignalR;

namespace GoodsShopper.Ap.Model.Service
{
    public class BrandInfoService : IBrandInfoService
    {
        private readonly IBrandRepository brandRepository;

        public BrandInfoService(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
            this.InitialTestDate();
        }

        private void InitialTestDate()
        {
            var result = this.brandRepository.Insert(new Brand
            {
                Name = "TestBrand"
            });
        }

        public (Exception exception, Brand brand) Insert(BrandInsertDto request)
        {
            try
            {
                var result = this.brandRepository.Insert(new Brand
                {
                    Name = request.Name
                });

                return (null, result.brand);
            }
            catch (Exception ex) 
            {
                return (ex, null);
            }
        }

        public (Exception exception, BrandQueryResponseDto response) Query(BrandQueryDto request)
        {
            try 
            {
                var result = this.brandRepository.Query(request.Id);

                return (null, new BrandQueryResponseDto
                {
                    Brands = new List<Brand> { result.brand }
                });
            }
            catch (Exception ex) 
            { 
                return (ex, null);
            }
        }

        public (Exception exception, BrandQueryResponseDto response) Query()
        {
            try
            {
                var result = this.brandRepository.GetAll();

                return (null, new BrandQueryResponseDto
                {
                    Brands = result.brands
                });
            }
            catch (Exception ex) 
            {
                return (ex, null);
            }
        }
    }
}
