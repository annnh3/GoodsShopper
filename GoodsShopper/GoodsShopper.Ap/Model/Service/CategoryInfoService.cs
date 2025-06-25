using System;
using System.Collections.Generic;
using GoodsShopper.Domain.DTO;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Repository;

namespace GoodsShopper.Ap.Model.Service
{
    public class CategoryInfoService : ICategoryInfoService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryInfoService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
            this.InitialTestDate();
        }

        private void InitialTestDate()
        {
            var result = this.categoryRepository.Insert(new Category
            {
                Name = "TestCategory"
            });
        }

        public (Exception exception, Category category) Insert(CategoryInsertDto request)
        {
            try
            {
                var result = this.categoryRepository.Insert(new Category
                {
                    Name = request.Name
                });

                return (null, result.category);
            }
            catch (Exception ex) 
            {
                return (ex, null);
            }
        }

        public (Exception exception, CategoryQueryResponseDto response) Query(CategoryQueryDto request)
        {
            try 
            {
                var result = this.categoryRepository.Query(request.Id);

                return (null, new CategoryQueryResponseDto
                {
                    Categories = new List<Category> { result.category }
                });
            }
            catch (Exception ex) 
            { 
                return (ex, null);
            }
        }

        public (Exception exception, CategoryQueryResponseDto response) Query()
        {
            try
            {
                var result = this.categoryRepository.GetAll();

                return (null, new CategoryQueryResponseDto
                {
                    Categories = result.categories
                });
            }
            catch (Exception ex) 
            {
                return (ex, null);
            }
        }
    }
}
