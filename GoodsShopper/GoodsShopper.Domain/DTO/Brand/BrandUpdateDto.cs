using Newtonsoft.Json;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 更新品牌DTO
    /// </summary>
    public class BrandUpdateDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 品牌名稱
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
