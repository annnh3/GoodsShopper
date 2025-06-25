namespace GoodsShopper.Ap.Model.Service
{
    using System;
    using GoodsShopper.Domain.DTO;
    using GoodsShopper.Domain.Model;

    public interface ICategoryInfoService
    {
        (Exception exception, CategoryQueryResponseDto response) Query(CategoryQueryDto request);

        (Exception exception, CategoryQueryResponseDto response) Query();

        (Exception exception, Category category) Insert(CategoryInsertDto request);
    }
}
