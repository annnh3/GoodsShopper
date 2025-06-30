using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface IProductTypeRepository
    {
        (Exception exception, ProductType productType) Insert(ProductType productType);

        (Exception ex, ProductType productType) Query(int id);

        (Exception ex, IEnumerable<ProductType> productTypes) GetAll();
    }
}
