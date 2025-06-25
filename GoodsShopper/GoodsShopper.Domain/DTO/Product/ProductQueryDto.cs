using System.Collections.Generic;
using System.IO;
using GoodsShopper.Domain.Model;
using Newtonsoft.Json;
using ProtoBuf;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 產品查詢Dto
    /// </summary>
    public class ProductQueryDto
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 產品查詢回應結構Dto
    /// </summary>
    [ProtoContract]
    public class ProductQueryResponseDto
    {
        [ProtoMember(1)]
        public IEnumerable<Product> Products { get; set; }

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
        public static ProductQueryResponseDto ToConstruct(byte[] bytes)
            => Serializer.Deserialize<ProductQueryResponseDto>(new MemoryStream(bytes));
    }
}
