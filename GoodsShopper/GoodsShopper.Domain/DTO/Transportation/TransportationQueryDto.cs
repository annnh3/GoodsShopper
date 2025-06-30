using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace GoodsShopper.Domain.DTO.Transportation
{
    /// <summary>
    /// 運輸方式查詢DTO
    /// </summary>
    public class TransportationQueryDto
    {
        /// <summary>
        /// 索引ID
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 運輸方式查詢回應結構DTO
    /// </summary>
    [ProtoContract]
    public class TransportationQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Model.Transportation> Transportations { get; set; }

        /// <summary>
        /// 強行別轉Protobuf格式
        /// </summary>
        /// <returns></returns>
        public byte[] ToProto()
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Proto轉強行別
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static TransportationQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<TransportationQueryResponseDto>(new MemoryStream(bytes));
    }
}
