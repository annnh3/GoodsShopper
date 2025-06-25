using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.Signalr;

namespace GoodsShopper.RelayServer.Domain.Cache
{
    public class BrandCache : ICache
    {
        /// <summary>
        /// 物件鎖
        /// </summary>
        private readonly object _lock = new object();

        public BrandCache(MemoryCache cache) : base(cache)
        {
            MicroServiceHub = HubType.GoodsShopperHub;
            health = false;
        }

        /// <summary>
        /// 清除過期資料
        /// </summary>
        public override void ClearExpireData()
        {
        }

        /// <summary>
        /// 移除資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        public override void Delete<T>(Func<T, bool> expression)
        {
            // 沒有刪除功能
        }

        /// <summary>
        /// 查找緩存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override T Find<T>(Func<T, bool> expression)
        {
            return typeof(T) != typeof(Brand)
                ? new List<T>().FirstOrDefault()
                : (GetData<IEnumerable<Brand>>().Where(expression as Func<Brand, bool>) as IEnumerable<T>).FirstOrDefault();
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override IEnumerable<T> Get<T>(Func<T, bool> expression)
        {
            return typeof(T) != typeof(Brand)
                ? new List<T>()
                : GetData<IEnumerable<Brand>>().Where(expression as Func<Brand, bool>) as IEnumerable<T>;
        }

        /// <summary>
        /// 健康度
        /// </summary>
        /// <param name="health"></param>
        /// <returns></returns>
        public override bool Health(bool? health = null)
        {
            if (health.HasValue)
            {
                this.health = health.Value;
            }

            return this.health;
        }

        /// <summary>
        /// 初始化所有資料
        /// </summary>
        public override void Initialize()
        {
            // 清空緩存
            lock (_lock)
            {
                UpdateData(new List<Brand>());
            }
        }

        /// <summary>
        /// 初始化所有資料
        /// </summary>
        public override void Initialize<T>(IEnumerable<T> records)
        {
            InitializeUpsert(records);
        }

        /// <summary>
        /// 初始化資料更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        public override void InitializeUpsert<T>(IEnumerable<T> records)
        {
            if (typeof(T) != typeof(Brand))
            {
                return;
            }

            // 清空緩存
            lock (_lock)
            {
                UpdateData(new List<Brand>());
            }

            Upsert(records.ToList());
            health = true;
        }

        /// <summary>
        /// 更新/寫入資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        public override void Upsert<T>(IEnumerable<T> records)
        {
            if (typeof(T) != typeof(Brand))
            {
                return;
            }

            var updateData = records as IEnumerable<Brand>;

            lock (_lock)
            {
                var legacyData = GetData<IEnumerable<Brand>>();
                var source = legacyData.Where(p => !updateData.Select(s => s.Id).Contains(p.Id));
                UpdateData(source.Concat(updateData).ToList());
            }
        }

        /// <summary>
        /// 純更新資料
        /// </summary>
        /// <param name="updateFunc"></param>
        /// <typeparam name="T"></typeparam>
        public override void Update<T>(Func<IEnumerable<T>, IEnumerable<T>> updateFunc)
        {
            if (typeof(T) != typeof(Brand))
            {
                return;
            }

            lock (_lock)
            {
                UpdateData(updateFunc(GetData<IEnumerable<T>>().ToList()).ToList());
            }
        }
    }
}
