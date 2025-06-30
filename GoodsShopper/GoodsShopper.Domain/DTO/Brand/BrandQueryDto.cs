using System.Collections.Generic;
using System.IO;
using GoodsShopper.Domain.Model;
using ProtoBuf;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 查詢Dto
    /// </summary>
    public class BrandQueryDto
    {
        /// <summary>
        /// 索引ID
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 查詢回應結構Dto
    /// </summary>
    [ProtoContract]
    public class BrandQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Brand> Brands { get; set; }

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
        public static BrandQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<BrandQueryResponseDto>(new MemoryStream(bytes));
    }
}
