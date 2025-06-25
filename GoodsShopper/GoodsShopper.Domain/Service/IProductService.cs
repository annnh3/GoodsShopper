using System;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;

namespace GoodsShopper.Domain.Service
{
    public interface IProductService
    {
        /// <summary>
        /// 新增產品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        (Exception exception, Product product) Insert(ProductInsertDto product);

        /// <summary>
        /// 商品查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, ProductQueryResponseDto response) Query(ProductQueryDto request);

        /// <summary>
        /// 商品查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        (Exception exception, ProductQueryResponseDto response) Query();
    }
}
