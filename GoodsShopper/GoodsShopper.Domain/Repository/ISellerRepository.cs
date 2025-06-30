using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface ISellerRepository
    {
        (Exception exception, Seller seller) Insert(Seller seller);

        (Exception ex, Seller seller) Query(int sellerTypeId);

        (Exception ex, IEnumerable<Seller> sellers) GetAll();
    }
}
