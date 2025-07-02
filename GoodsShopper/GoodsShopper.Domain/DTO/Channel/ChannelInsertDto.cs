using Newtonsoft.Json;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 購買通路新增DTO
    /// </summary>
    public class ChannelInsertDto
    {
        /// <summary>
        /// 購買通路名稱
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
