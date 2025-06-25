using ProtoBuf;
using System.IO;

namespace GoodsShopper.Domain.Model
{
    [ProtoContract]
    public class Channel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

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
        public static Channel ToConstruct(byte[] bytes)
            => bytes.Length == 0 ? null : Serializer.Deserialize<Channel>(new MemoryStream(bytes));
    }
}
