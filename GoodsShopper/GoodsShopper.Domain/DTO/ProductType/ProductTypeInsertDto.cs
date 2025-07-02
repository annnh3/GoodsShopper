using Newtonsoft.Json;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 商品種類新增DTO
    /// </summary>
    public class ProductTypeInsertDto
    {
        /// <summary>
        /// 商品種類名稱
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
