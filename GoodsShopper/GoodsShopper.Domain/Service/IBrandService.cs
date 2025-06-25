using System;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;

namespace GoodsShopper.Domain.Service
{
    public interface IBrandService
    {
        /// <summary>
        /// 新增品牌
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        (Exception exception, Brand brand) Insert(BrandInsertDto brand);

        /// <summary>
        /// 商品查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, BrandQueryResponseDto response) Query(BrandQueryDto request);

        /// <summary>
        /// 商品查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, BrandQueryResponseDto response) Query();
    }
}
