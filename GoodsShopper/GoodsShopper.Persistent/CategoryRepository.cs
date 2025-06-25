using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;

namespace GoodsShopper.Persistent
{
    public class CategoryRepository : ICategoryRepository
    {
        private static ConcurrentBag<Category> categories = new ConcurrentBag<Category>();

        public (Exception ex, IEnumerable<Category> categories) GetAll()
        {
            try
            {
                return (null, categories.ToList());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception exception, Category category) Insert(Category category)
        {
            try
            {
                int id = categories.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;

                category.Id = id;

                categories.Add(category);

                return (null, category);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }

        public (Exception ex, Category category) Query(int id)
        {
            try
            {
                return (null, categories.Where(x => x.Id == id).SingleOrDefault());
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
