using Newtonsoft.Json;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 分類新增DTO
    /// </summary>
    public class CategoryInsertDto
    {
        /// <summary>
        /// 商品種類ID
        /// </summary>
        public int ProductTypeId { get; set; }
        
        /// <summary>
        /// 分類名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
