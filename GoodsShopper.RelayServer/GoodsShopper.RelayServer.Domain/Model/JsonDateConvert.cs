namespace GoodsShopper.RelayServer.Domain.Model
{
    using Newtonsoft.Json.Converters;

    public class JsonDateConvert : IsoDateTimeConverter
    {
        public JsonDateConvert()
        {
            DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        }
    }
}
