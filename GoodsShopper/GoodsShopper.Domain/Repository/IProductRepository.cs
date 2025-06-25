using System;
using System.Collections.Generic;
using GoodsShopper.Domain.Model;

namespace GoodsShopper.Domain.Repository
{
    public interface IProductRepository
    {
        (Exception exception, Product product) Insert(Product product);

        (Exception ex, Product product) Query(string name);

        (Exception ex, IEnumerable<Product> products) GetAll();
    }
}
