using GoodsShopper.Domain.Model;
using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 角色查詢DTO
    /// </summary>
    public class CharacterQueryDto
    {
        /// <summary>
        /// 索引ID
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 角色查詢回應結構DTO
    /// </summary>
    [ProtoContract]
    public class CharacterQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Character> Characters { get; set; }

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
        public static CharacterQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<CharacterQueryResponseDto>(new MemoryStream(bytes));
    }
}
