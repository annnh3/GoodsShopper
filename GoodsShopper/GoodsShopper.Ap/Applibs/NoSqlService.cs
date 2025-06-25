using System;
using NLog;
using StackExchange.Redis;

namespace GoodsShopper.Ap.Applibs
{
    internal static class NoSqlService
    {
        private static ILogger logger = LogManager.GetLogger("PointSystem");

        private static Lazy<ConnectionMultiplexer> lazyRedisConnections;

        public static ConnectionMultiplexer RedisConnections
        {
            get
            {
                if (lazyRedisConnections == null)
                {
                    NoSqlInit();
                }

                return lazyRedisConnections.Value;
            }
        }

        public static string RedisAffixKey
        {
            get
            {
                return ConfigHelper.RedisAffixKey;
            }
        }

        public static int RedisDataBase
        {
            get
            {
                return ConfigHelper.RedisDataBase;
            }
        }

        private static void NoSqlInit()
        {
            lazyRedisConnections = new Lazy<ConnectionMultiplexer>(() =>
            {
                var options = ConfigurationOptions.Parse(ConfigHelper.RedisConn);
                options.AbortOnConnectFail = false;

                var muxer = ConnectionMultiplexer.Connect(options);
                muxer.ConnectionFailed += (sender, e) =>
                {
                    logger.Error("redis failed: " + EndPointCollection.ToString(e.EndPoint) + "/" + e.ConnectionType);
                };
                muxer.ConnectionRestored += (sender, e) =>
                {
                    logger.Error("redis restored: " + EndPointCollection.ToString(e.EndPoint) + "/" + e.ConnectionType);
                };

                return muxer;
            });
        }
    }
}
