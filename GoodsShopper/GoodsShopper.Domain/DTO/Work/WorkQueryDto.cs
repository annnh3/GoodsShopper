using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace GoodsShopper.Domain.DTO.Work
{
    /// <summary>
    /// 作品查詢DTO
    /// </summary>
    public class WorkQueryDto
    {
        /// <summary>
        /// 索引ID
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 作品查詢回應結構Dto
    /// </summary>
    [ProtoContract]
    public class WorkQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Model.Work> Works { get; set; }

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
        public static WorkQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<WorkQueryResponseDto>(new MemoryStream(bytes));
    }
}
