using System;
using System.Configuration;

namespace GoodsShopper.Ap.Applibs
{
    internal static class ConfigHelper
    {
        /// <summary>
        /// 版本號
        /// </summary>
        public static string Version = "23.10.2";



        /// <summary>
        /// 主服務URL
        /// </summary>
        public static string ServiceUrl
        {
            get
                => $"http://*:9999";
        }

        /// <summary>
        /// Signalr主服務URL
        /// </summary>
        public static string SignalrServiceUrl
        {
            get
                => $"http://*:8999";
        }
     
        /// <summary>
        /// 運作線程數
        /// </summary>
        public static readonly int WorkThreads = Convert.ToInt32(ConfigurationManager.AppSettings["WorkThreads"]);

        /// <summary>
        /// IO線程數
        /// </summary>
        public static readonly int IoThreads = Convert.ToInt32(ConfigurationManager.AppSettings["IoThreads"]);

        /// <summary>
        /// Redis連線字串
        /// </summary>
        public static readonly string RedisConn = ConfigurationManager.ConnectionStrings["Redis"].ConnectionString;

        /// <summary>
        /// Redis前贅詞
        /// </summary>
        public static readonly string RedisAffixKey = ConfigurationManager.AppSettings["RedisAffixKey"];

        /// <summary>
        /// REDIS DB
        /// </summary>
        public static readonly int RedisDataBase = Convert.ToInt32(ConfigurationManager.AppSettings["RedisDataBase"]);
    }
}
