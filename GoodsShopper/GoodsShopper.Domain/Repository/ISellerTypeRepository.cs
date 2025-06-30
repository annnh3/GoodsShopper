using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface ISellerTypeRepository
    {
        (Exception exception, SellerType sellerType) Insert(SellerType sellerType);

        (Exception ex, SellerType sellerType) Query(int id);

        (Exception ex, IEnumerable<SellerType> sellerTypes) GetAll();
    }
}
