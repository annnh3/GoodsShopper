using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface IWorkRepository
    {
        (Exception exception, Work work) Insert(Work work);

        (Exception ex, Work work) Query(int id);

        (Exception ex, IEnumerable<Work> works) GetAll();
    }
}
