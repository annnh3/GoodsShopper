using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using System;

namespace GoodsShopper.Domain.Service
{
    public interface IProductTypeService
    {
        /// <summary>
        /// 新增商品種類
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        (Exception exception, ProductType productType) Insert(ProductTypeInsertDto productType);

        /// <summary>
        /// 種類查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, ProductTypeQueryResponseDto response) Query(ProductTypeQueryDto request);

        /// <summary>
        /// 查詢全部商品種類
        /// </summary>
        /// <returns></returns>
        (Exception exception, ProductTypeQueryResponseDto response) Query();
    }
}
