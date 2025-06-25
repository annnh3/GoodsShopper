namespace GoodsShopper.Ap.Model.Service
{
    using System;
    using GoodsShopper.Domain.DTO;
    using GoodsShopper.Domain.Model;

    public interface IBrandInfoService
    {
        (Exception exception, BrandQueryResponseDto response) Query(BrandQueryDto request);

        (Exception exception, BrandQueryResponseDto response) Query();

        (Exception exception, Brand brand) Insert(BrandInsertDto request);
    }
}
