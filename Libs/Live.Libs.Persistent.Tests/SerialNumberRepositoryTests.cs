
namespace Live.Libs.Persistent.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using StackExchange.Redis;

    [TestClass]
    public class SerialNumberRepositoryTests
    {
        const string RedisConn = @"localhost:6379";

        private SerialNumberRepository repo;

        [TestInitialize]
        public void Init()
        {
            var conn = ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(RedisConn));
            var redis = conn.GetDatabase(15);
            this.repo = new SerialNumberRepository(conn);

            var keys = conn.GetServer(RedisConn).Keys(15, $"Live:SerialNumber*", 10, CommandFlags.None).ToList();
            keys.ForEach(key => redis.KeyDelete(key));
        }

        [TestMethod]
        public void 取流水號測試()
        {
            var result = this.repo.GetSerialNumber();
            Assert.IsNull(result.Item1);
            Assert.AreEqual(result.Item2, 1);

            result = this.repo.GetSerialNumber();
            Assert.IsNull(result.Item1);
            Assert.AreEqual(result.Item2, 2);

            result = this.repo.GetSerialNumber(5);
            Assert.IsNull(result.Item1);
            Assert.AreEqual(result.Item2, 7);
        }
    }
}
