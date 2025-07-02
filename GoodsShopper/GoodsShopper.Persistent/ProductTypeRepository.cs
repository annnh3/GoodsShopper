using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GoodsShopper.Persistent
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private static ConcurrentBag<ProductType> productTypes = new ConcurrentBag<ProductType>();

        public (Exception ex, IEnumerable<ProductType> productTypes) GetAll()
        {
            try
            {
                return (null, productTypes.ToList());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception exception, ProductType productType) Insert(ProductType productType)
        {
            try
            {
                int id = productTypes.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;

                productType.Id = id;

                productTypes.Add(productType);

                return (null, productType);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, ProductType productType) Query(int id)
        {
            try
            {
                return (null, productTypes.Where(x => x.Id == id).SingleOrDefault());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
