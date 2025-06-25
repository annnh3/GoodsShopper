
namespace Live.Libs.KeepAliveConn
{
    using System.IO;
    using Newtonsoft.Json;
    using ProtoBuf;

    /// <summary>
    /// 長連接模組
    /// </summary>
    [ProtoContract]
    public class ActionModule
    {
        /// <summary>
        /// 指令
        /// </summary>
        [ProtoMember(1)]
        public string Action { get; set; }

        /// <summary>
        /// 流水號
        /// </summary>
        [ProtoMember(2)]
        public long SerialNumber { get; set; }

        /// <summary>
        /// 指令內容
        /// </summary>
        [ProtoMember(3)]
        public string Message { get; set; }

        /// <summary>
        /// 轉Protobuf
        /// </summary>
        /// <returns></returns>
        public byte[] ToProto()
        {
            using(var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 物件轉Json結構字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);

        /// <summary>
        /// 轉強型別
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ActionModule ToConstruct(byte[] bytes)
            => Serializer.Deserialize<ActionModule>(new MemoryStream(bytes));

        /// <summary>
        /// 轉強型別
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ActionModule FromString(string message)
            => JsonConvert.DeserializeObject<ActionModule>(message);
    }
}
