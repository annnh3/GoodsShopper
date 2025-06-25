using System;
using System.Collections.Generic;
using GoodsShopper.Domain.Model;

namespace GoodsShopper.Domain.Repository
{
    public interface IBrandRepository
    {
        (Exception exception, Brand brand) Insert(Brand brand);

        (Exception ex, Brand brand) Query(int id);

        (Exception ex, IEnumerable<Brand> brands) GetAll();
    }
}
