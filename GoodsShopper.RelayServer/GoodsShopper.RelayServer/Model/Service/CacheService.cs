using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Features.Indexed;
using GoodsShopper.Domain.Service;
using GoodsShopper.RelayServer.Applibs;
using GoodsShopper.RelayServer.Domain.Cache;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using Newtonsoft.Json;
using NLog;

namespace GoodsShopper.RelayServer.Model.Service
{
    public class CacheService : ICacheService
    {
        private readonly ILogger logger = LogManager.GetLogger("RelayServer");

        /// <summary>
        /// 商品服務
        /// </summary>
        private readonly IProductService productSvc;

        /// <summary>
        /// 商品種類服務
        /// </summary>
        private readonly IProductTypeService productTypeSvc;
        
        /// <summary>
        /// 商品分類服務
        /// </summary>
        private readonly ICategoryService categorySvc;

        /// <summary>
        /// 品牌服務
        /// </summary>
        private readonly IBrandService brandSvc;

        /// <summary>
        /// 緩存雜湊表
        /// </summary>
        private readonly Dictionary<string, ICache> cacheDic = new Dictionary<string, ICache>();

        public CacheService(
            IIndex<string, ICache> cacheSet,
            IProductService productSvc,
            IProductTypeService productTypeSvc,
            ICategoryService categorySvc,
            IBrandService brandSvc)
        {
            this.productSvc = productSvc;
            this.productTypeSvc = productTypeSvc;
            this.categorySvc = categorySvc;
            this.brandSvc = brandSvc;

            cacheDic = new Dictionary<string, ICache>();

            Assembly.Load("GoodsShopper.RelayServer.Domain").GetTypes().Where(p => p.Namespace == "GoodsShopper.RelayServer.Domain.Cache" && !p.IsInterface && p.IsPublic && !p.IsAbstract).ToList().ForEach(type =>
            {
                // 有訂閱的再註冊
                if (ConfigHelper.SubscribeHubs.Contains(cacheSet[type.Name].MicroServiceHub) &&
                    ConfigHelper.SubscribeCaches.Contains(type))
                {
                    cacheDic.Add(type.Name.Replace("Cache", string.Empty), cacheSet[type.Name]);
                }
            });
        }

        /// <summary>
        /// 緩存集合
        /// </summary>
        public IEnumerable<ICache> Caches
            => cacheDic.Select(p => p.Value);

        /// <summary>
        /// 緩存初始化
        /// </summary>
        /// <param name="cache"></param>
        public void CacheInitialize(ICache cache)
        {
            if (cache.GetType() == typeof(BrandCache))
            {
                var getResult = this.brandSvc.Query();

                if (getResult.exception != null)
                {
                    this.logger.Error(getResult.exception, $"{this.GetType().Name} CacheInitialize Brand Query");
                    return;
                }

                this.logger.Trace($"{this.GetType().Name} CacheInitialize Brand Query:{JsonConvert.SerializeObject(getResult.response.Brands)}");
                var brands = getResult.response.Brands.Select(x => new Brand
                {
                     Id = x.Id,
                     Name = x.Name
                });
                cache.Initialize(brands);
            }
            else if (cache.GetType() == typeof(CategoryCache))
            {
                var getResult = this.categorySvc.Query();

                if (getResult.exception != null)
                {
                    this.logger.Error(getResult.exception, $"{this.GetType().Name} CacheInitialize Category Query");
                    return;
                }

                this.logger.Trace($"{this.GetType().Name} CacheInitialize Category Query:{JsonConvert.SerializeObject(getResult.response.Categories)}");
                var categories = getResult.response.Categories.Select(x => new Category
                {
                    Id = x.Id,
                    Name = x.Name
                });
                cache.Initialize(categories);
            }
            else if (cache.GetType() == typeof(ProductInfoCache))
            {
                var getResult = this.productSvc.Query();

                if (getResult.exception != null)
                {
                    this.logger.Error(getResult.exception, $"{this.GetType().Name} ProductInfo Query");
                    return;
                }

                this.logger.Trace($"{this.GetType().Name} CacheInitialize ProductInfo Query:{JsonConvert.SerializeObject(getResult.response.Products)}");
                var products = getResult.response.Products.Select(x => new ProductInfo
                {
                    Id = x.Id,
                    Name = x.Name,
                    BrandId = x.BrandId,
                    CategoryId = x.CategoryId
                });
                cache.Initialize(products);
            }
            else if (cache.GetType() == typeof(ProductTypeCache))
            {
                var getResult = this.productTypeSvc.Query();

                if (getResult.exception != null)
                {
                    this.logger.Error(getResult.exception, $"{this.GetType().Name} ProductType Query");
                    return;
                }

                this.logger.Trace($"{this.GetType().Name} CacheInitialize ProductType Query:{JsonConvert.SerializeObject(getResult.response.ProductTypes)}");
                var productTypes = getResult.response.ProductTypes.Select(x => new ProductType
                {
                    Id = x.Id,
                    Name = x.Name,
                });
                cache.Initialize(productTypes);
            }
            else
            {
                cache.Initialize();
            }
        }

        /// <summary>
        /// 檢查緩存健康
        /// </summary>
        public void CheckCacheHealth()
        {
            foreach (var dic in cacheDic)
            {
                if (!dic.Value.Health())
                {
                    CacheInitialize(dic.Value);
                }
            }
        }

        /// <summary>
        /// 移除過期資料
        /// </summary>
        public void ClearExpireData()
        {
            foreach (var dic in cacheDic)
            {
                dic.Value.ClearExpireData();
            }
        }

        /// <summary>
        /// 移除資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        public void Delete<T>(Func<T, bool> expression)
            where T : class
            => this.cacheDic[typeof(T).Name.Replace("Cache", string.Empty)].Delete(expression);

        /// <summary>
        /// 查找資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T Find<T>(Func<T, bool> expression)
            where T : class
            => this.cacheDic[typeof(T).Name.Replace("Cache", string.Empty)].Find(expression);

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<T> Get<T>(Func<T, bool> expression)
            where T : class
            => this.cacheDic[typeof(T).Name.Replace("Cache", string.Empty)].Get(expression);

        /// <summary>
        /// 初始化更新/寫入資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        public void InitializeUpsert<T>(IEnumerable<T> records) where T : class
        {
            var cache = this.cacheDic[typeof(T).Name.Replace("Cache", string.Empty)];
            cache.InitializeUpsert(records);
        }

        /// <summary>
        /// 更新/寫入資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        public void Upsert<T>(IEnumerable<T> records)
            where T : class
        {
            var cache = this.cacheDic[typeof(T).Name.Replace("Cache", string.Empty)];
            cache.Upsert(records);
        }

        /// <summary>
        /// 純更新資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateFunc"></param>
        public void Update<T>(Func<IEnumerable<T>, IEnumerable<T>> updateFunc) where T : class
            => cacheDic[typeof(T).Name.Replace("Cache", string.Empty)].Update(updateFunc);
    }
}
