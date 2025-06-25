using System;
using System.Collections;
using System.Collections.Generic;
using GoodsShopper.Domain.Model;

namespace GoodsShopper.Domain.Repository
{
    public interface ICategoryRepository
    {
        (Exception exception, Category category) Insert(Category category);

        (Exception ex, Category category) Query(int id);

        (Exception ex, IEnumerable<Category> categories) GetAll();
    }
}
