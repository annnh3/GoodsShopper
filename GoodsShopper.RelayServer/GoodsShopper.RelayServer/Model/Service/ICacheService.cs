namespace GoodsShopper.RelayServer.Model.Service
{
    using System;
    using System.Collections.Generic;
    using RelayServer.Domain.Cache;

    /// <summary>
    /// 快取服務
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 緩存集合
        /// </summary>
        IEnumerable<ICache> Caches { get; }

        /// <summary>
        /// 緩存初始化
        /// </summary>
        /// <param name="cache"></param>
        void CacheInitialize(ICache cache);

        /// <summary>
        /// 檢查緩存健康
        /// </summary>
        void CheckCacheHealth();

        /// <summary>
        /// 移除過期資料
        /// </summary>
        void ClearExpireData();

        /// <summary>
        /// 移除資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        void Delete<T>(Func<T, bool> expression)
            where T : class;

        /// <summary>
        /// 查找資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        T Find<T>(Func<T, bool> expression)
            where T : class;

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<T> Get<T>(Func<T, bool> expression)
            where T : class;

        /// <summary>
        /// 初始化更新/寫入資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        void InitializeUpsert<T>(IEnumerable<T> records)
            where T : class;

        /// <summary>
        /// 更新/寫入資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        void Upsert<T>(IEnumerable<T> records)
            where T : class;

        /// <summary>
        /// 純更新資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateFunc"></param>
        void Update<T>(Func<IEnumerable<T>, IEnumerable<T>> updateFunc)
            where T : class;
    }
}
