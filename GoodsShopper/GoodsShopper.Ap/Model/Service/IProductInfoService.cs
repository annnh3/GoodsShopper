namespace GoodsShopper.Ap.Model.Service
{
    using System;
    using GoodsShopper.Domain.DTO;
    using GoodsShopper.Domain.Model;

    public interface IProductInfoService
    {
        (Exception exception, ProductQueryResponseDto response) Query(ProductQueryDto request);

        (Exception exception, ProductQueryResponseDto response) Query();

        (Exception exception, Product product) Insert(ProductInsertDto request);
    }
}
