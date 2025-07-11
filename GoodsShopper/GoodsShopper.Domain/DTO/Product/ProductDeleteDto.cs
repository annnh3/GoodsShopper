using Newtonsoft.Json;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 刪除商品DTO
    /// </summary>
    public class ProductDeleteDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
