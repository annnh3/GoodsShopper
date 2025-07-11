namespace GoodsShopper.Ap.Model.Service
{
    using GoodsShopper.Domain.DTO;
    using GoodsShopper.Domain.Model;
    using System;

    public interface IProductInfoService
    {
        (Exception exception, ProductQueryResponseDto response) Query(ProductQueryDto request);

        (Exception exception, ProductQueryResponseDto response) Query();

        (Exception exception, Product product) Insert(ProductInsertDto request);

        (Exception exception, Product product) Delete(ProductDeleteDto request);
    }
}
