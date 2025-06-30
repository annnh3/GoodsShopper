using GoodsShopper.Domain.Model;
using System;
using System.Collections.Generic;

namespace GoodsShopper.Domain.Repository
{
    public interface IStyleRepository
    {
        (Exception exception, Style style) Insert(Style style);

        (Exception ex, Style style) Query(int id);

        (Exception ex, IEnumerable<Style> styles) GetAll();
    }
}
