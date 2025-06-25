using System.IO;
using ProtoBuf;

namespace GoodsShopper.Domain.Model
{
    [ProtoContract]
    public class Product
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int BrandId { get; set; }

        [ProtoMember(4)]
        public int CategoryId {  get; set; }

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
        public static Product ToConstruct(byte[] bytes)
            => bytes.Length == 0 ? null : Serializer.Deserialize<Product>(new MemoryStream(bytes));
    }
}
