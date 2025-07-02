using System;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;

namespace GoodsShopper.Domain.Service
{
    public interface ICategoryService
    {
        /// <summary>
        /// 新增分類
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        (Exception exception, Category category) Insert(CategoryInsertDto category);

        /// <summary>
        /// 分類查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, CategoryQueryResponseDto response) Query(CategoryQueryDto request);

        /// <summary>
        /// 分類查詢
        /// </summary>
        /// <returns></returns>
        (Exception exception, CategoryQueryResponseDto response) Query();
    }
}
