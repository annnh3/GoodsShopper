namespace GoodsShopper.RelayServer.Domain.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;
    using RelayServer.Domain.Signalr;

    public abstract class ICache
    {
        /// <summary>
        /// 健康度
        /// </summary>
        protected bool health;

        private readonly ObjectCache cache;

        public ICache(MemoryCache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// 為服務HUB名稱
        /// </summary>
        /// <returns></returns>
        public HubType MicroServiceHub { get; protected set; }

        /// <summary>
        /// 清除過期資料
        /// </summary>
        public abstract void ClearExpireData();

        /// <summary>
        /// 移除資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        public abstract void Delete<T>(Func<T, bool> expression)
            where T : class;

        /// <summary>
        /// 查找緩存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public abstract T Find<T>(Func<T, bool> expression)
            where T : class;

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> Get<T>(Func<T, bool> expression)
            where T : class;

        /// <summary>
        /// 健康度
        /// </summary>
        /// <param name="health"></param>
        /// <returns></returns>
        public abstract bool Health(bool? health = null);

        /// <summary>
        /// 初始化所有資料
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 初始化所有資料
        /// </summary>
        public virtual void Initialize<T>(IEnumerable<T> records)
            where T : class
        {
            // 需要自定義的再覆寫
        }

        /// <summary>
        /// 初始化資料更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        public abstract void InitializeUpsert<T>(IEnumerable<T> records)
            where T : class;

        /// <summary>
        /// 更新/寫入資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        public abstract void Upsert<T>(IEnumerable<T> records)
            where T : class;

        /// <summary>
        /// 純更新資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateFunc"></param>
        public abstract void Update<T>(Func<IEnumerable<T>, IEnumerable<T>> updateFunc)
            where T : class;

        /// <summary>
        /// 取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetData<T>()
            where T : class
        {
            var type = typeof(T).GenericTypeArguments.Length > 0 ? typeof(T).GenericTypeArguments[0] : typeof(T);

            return cache[type.Name] == null
                ? typeof(T).GenericTypeArguments.Length > 0
                    ? (T)Activator.CreateInstance(typeof(List<>).MakeGenericType(type))
                    : (T)Activator.CreateInstance(typeof(T))
                : cache[type.Name] as T;
        }

        /// <summary>
        /// 更新緩存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        protected void UpdateData<T>(T data)
            where T : class
        {
            var type = typeof(T).GenericTypeArguments.Length > 0 ? typeof(T).GenericTypeArguments[0] : typeof(T);

            cache.Set(type.Name, data, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
        }
    }
}
