using Newtonsoft.Json;

namespace GoodsShopper.Domain.DTO.Transportation
{
    /// <summary>
    /// 運輸方式新增DTO
    /// </summary>
    public class TransportationInsertDto
    {
        /// <summary>
        /// 運輸方式
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
