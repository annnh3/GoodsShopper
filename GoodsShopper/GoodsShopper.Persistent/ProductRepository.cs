using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;

namespace GoodsShopper.Persistent
{
    public class ProductRepository : IProductRepository
    {
        private static ConcurrentDictionary<int, Product> products = new ConcurrentDictionary<int, Product>();

        public (Exception ex, IEnumerable<Product> products) GetAll()
        {
            try
            {
                return (null, products.Values);
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
                int newId = (products.Values.OrderByDescending(p => p.Id).FirstOrDefault()?.Id ?? 0) + 1;

                product.Id = newId;

                products.TryAdd(product.Id, product);

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
                return (null, products.Values.SingleOrDefault(x => x.Name == name));
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, Product product) Delete(int id)
        {
            try
            {
                products.TryRemove(id, out var deletedProduct);
                return (null, deletedProduct);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
