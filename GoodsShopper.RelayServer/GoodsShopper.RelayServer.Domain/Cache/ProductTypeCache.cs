using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.Signalr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace GoodsShopper.RelayServer.Domain.Cache
{
    public class ProductTypeCache : ICache
    {
        /// <summary>
        /// 物件鎖
        /// </summary>
        private readonly object _lock = new object();

        public ProductTypeCache(MemoryCache cache) : base(cache)
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
            return typeof(T) != typeof(ProductType)
                ? new List<T>().FirstOrDefault()
                : (GetData<IEnumerable<ProductType>>().Where(expression as Func<ProductType, bool>) as IEnumerable<T>).FirstOrDefault();
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override IEnumerable<T> Get<T>(Func<T, bool> expression)
        {
            return typeof(T) != typeof(ProductType)
                ? new List<T>()
                : GetData<IEnumerable<ProductType>>().Where(expression as Func<ProductType, bool>) as IEnumerable<T>;
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
                UpdateData(new List<ProductType>());
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
            if (typeof(T) != typeof(ProductType))
            {
                return;
            }

            // 清空緩存
            lock (_lock)
            {
                UpdateData(new List<ProductType>());
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
            if (typeof(T) != typeof(ProductType))
            {
                return;
            }

            var updateData = records as IEnumerable<ProductType>;

            lock (_lock)
            {
                var legacyData = GetData<IEnumerable<ProductType>>();
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
            if (typeof(T) != typeof(ProductType))
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
