using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface ITransportationRepository
    {
        (Exception exception, Transportation transportation) Insert(Transportation transportation);

        (Exception ex, Transportation transportation) Query(int id);

        (Exception ex, IEnumerable<Transportation> transportations) GetAll();
    }
}
