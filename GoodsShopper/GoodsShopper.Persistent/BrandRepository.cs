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
        private static ConcurrentBag<Brand> brands = new ConcurrentBag<Brand>();

        public (Exception ex, IEnumerable<Brand> brands) GetAll()
        {
            try
            {
                return (null, brands.OrderBy(x => x.Id).ToList());
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
                int id = brands.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;

                brand.Id = id;

                brands.Add(brand);

                return (null, brand);
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
                return (null, brands.Where(x => x.Id == id).SingleOrDefault());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
