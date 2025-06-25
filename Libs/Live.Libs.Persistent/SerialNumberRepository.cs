
namespace Live.Libs.Persistent
{
    using System;
    using StackExchange.Redis;

    /// <summary>
    /// 取流水號實例
    /// </summary>
    public class SerialNumberRepository : ISerialNumberRepository
    {
        /// <summary>
        /// Redis 連線物件
        /// </summary>
        private ConnectionMultiplexer conn;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="conn"></param>
        public SerialNumberRepository(ConnectionMultiplexer conn)
        {
            this.conn = conn;
        }

        /// <summary>
        /// 取唯一流水號
        /// </summary>
        /// <param name="qty"></param>
        /// <returns></returns>
        public (Exception exception, long serialNumber) GetSerialNumber(long qty = 1)
        {
            try
            {
                var sn = this.UseConnection(redis =>
                {
                    var keys = new RedisKey[] { $"Live:SerialNumber" };
                    var values = new RedisValue[] { qty };
                    string script =
                    @"
                        return redis.call('INCRBY', KEYS[1], ARGV[1])
                    ";

                    return (long)redis.ScriptEvaluate(script, keys, values);
                });

                return (null, sn);
            }
            catch (Exception ex)
            {
                return (ex, -1);
            }
        }

        private T UseConnection<T>(Func<IDatabase, T> func)
        {
            var redis = this.conn.GetDatabase(15);
            return func(redis);
        }
    }
}
