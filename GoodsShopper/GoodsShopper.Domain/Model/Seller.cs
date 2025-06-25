using ProtoBuf;
using System.IO;

namespace GoodsShopper.Domain.Model
{
    [ProtoContract]
    public class Seller
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int SellerTypeId { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }
        
        [ProtoMember(4)]
        public string Info { get; set; }

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
        public static Seller ToConstruct(byte[] bytes)
            => bytes.Length == 0 ? null : Serializer.Deserialize<Seller>(new MemoryStream(bytes));
    }
}
