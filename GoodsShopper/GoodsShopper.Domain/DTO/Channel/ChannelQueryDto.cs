using GoodsShopper.Domain.Model;
using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 購買通路查詢DTO
    /// </summary>
    public class ChannelQueryDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 查詢回應結構Dto
    /// </summary>
    [ProtoContract]
    public class ChannelQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Channel> Channels { get; set; }

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
        public static ChannelQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<ChannelQueryResponseDto>(new MemoryStream(bytes));
    }
}
