using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace GoodsShopper.Domain.DTO.Style
{
    /// <summary>
    /// 外觀查詢DTO
    /// </summary>
    public class StyleQueryDto
    {
        /// <summary>
        /// 索引ID
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 外觀查詢回應結構DTO
    /// </summary>
    [ProtoContract]
    public class StyleQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Model.Style> Styles { get; set; }

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
        public static StyleQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<StyleQueryResponseDto>(new MemoryStream(bytes));
    }
}
