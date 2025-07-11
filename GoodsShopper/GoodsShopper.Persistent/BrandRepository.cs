using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;

namespace GoodsShopper.Persistent
{
    public class BrandRepository : IBrandRepository
    {
        private static ConcurrentDictionary<int, Brand> brands = new ConcurrentDictionary<int, Brand>();

        public (Exception ex, IEnumerable<Brand> brands) GetAll()
        {
            try
            {
                return (null, brands.Values);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception exception, Brand brand) Insert(Brand brand)
        {
            try
            {
                int newId = (brands.Values.OrderByDescending(p => p.Id).FirstOrDefault()?.Id ?? 0) + 1;

                brand.Id = newId;

                brands.TryAdd(brand.Id, brand);

                return (null, brand);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception exception, Brand brand) Update(Brand brand)
        {
            try
            {
                if (brands.ContainsKey(brand.Id))
                {
                    brands[brand.Id] = brand;
                    return (null, brand);
                }
                else
                {
                    return (new Exception("查無品牌"), null);
                }
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, Brand brand) Query(int id)
        {
            try
            {
                return (null, brands.Values.SingleOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
