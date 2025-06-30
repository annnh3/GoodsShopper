using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface ICollectionRepository
    {
        (Exception exception, Collection collection) Insert(Collection collection);

        (Exception ex, Collection collection) Query(int workId);

        (Exception ex, IEnumerable<Collection> collections) GetAll();
    }
}
