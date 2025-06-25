using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;

namespace GoodsShopper.Persistent
{
    public class ProductRepository : IProductRepository
    {
        private static ConcurrentBag<Product> products = new ConcurrentBag<Product>();

        public (Exception ex, IEnumerable<Product> products) GetAll()
        {
            try
            {
                return (null, products.ToList());
            }
            catch (Exception ex) 
            {
                return (ex, null);
            }
        }

        public (Exception exception,Product product) Insert(Product product)
        {
            try
            {
                int id = products.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;

                product.Id = id;

                products.Add(product);

                return (null, product);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, Product product) Query(string name)
        {
            try
            {
                return (null, products.Where(x => x.Name == name).SingleOrDefault());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
