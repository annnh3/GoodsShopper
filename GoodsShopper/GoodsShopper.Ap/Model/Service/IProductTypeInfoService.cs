using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using System;

namespace GoodsShopper.Ap.Model.Service
{
    public interface IProductTypeInfoService
    {
        (Exception exception, ProductTypeQueryResponseDto response) Query(ProductTypeQueryDto request);

        (Exception exception, ProductTypeQueryResponseDto response) Query();

        (Exception exception, ProductType productType) Insert(ProductTypeInsertDto request);
    }
}
