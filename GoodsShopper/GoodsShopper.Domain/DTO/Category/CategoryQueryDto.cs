using System.Collections.Generic;
using System.IO;
using GoodsShopper.Domain.Model;
using ProtoBuf;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 查詢Dto
    /// </summary>
    public class CategoryQueryDto
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 查詢回應結構Dto
    /// </summary>
    [ProtoContract]
    public class CategoryQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Category> Categories { get; set; }

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
        public static CategoryQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<CategoryQueryResponseDto>(new MemoryStream(bytes));
    }
}
