using Newtonsoft.Json;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 產品新增DTO
    /// </summary>
    public class ProductInsertDto
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 品牌ID
        /// </summary>
        public int BrandId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
