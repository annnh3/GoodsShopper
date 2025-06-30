using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace GoodsShopper.Domain.DTO.SellerType
{
    /// <summary>
    /// 賣家型態查詢DTO
    /// </summary>
    public class SellerTypeQueryDto
    {
        /// <summary>
        /// 索引ID
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 賣家型態查詢回應結構DTO
    /// </summary>
    [ProtoContract]
    public class SellerTypeQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Model.SellerType> SellerTypes { get; set; }

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
        public static SellerTypeQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<SellerTypeQueryResponseDto>(new MemoryStream(bytes));
    }

}
