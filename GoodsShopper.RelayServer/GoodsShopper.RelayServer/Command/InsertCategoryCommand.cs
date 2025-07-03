using System;
using GoodsShopper.Domain.Service;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Command
{
    public class InsertCategoryCommand : ICommand
    {
        /// <summary>
        /// 新增分類服務
        /// </summary>
        private readonly ICategoryService categorySvc;

        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        public InsertCategoryCommand(ICacheService cacheSvc, ICategoryService categorySvc)
        {
            this.cacheSvc = cacheSvc;
            this.categorySvc = categorySvc;
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Category_in_insertCategory>(content);

                var result = categorySvc.Insert(new GoodsShopper.Domain.DTO.CategoryInsertDto
                {
                    ProductTypeId = cmd.ProductTypeId,
                    Name = cmd.Name
                });

                var categoryData = new Category
                {
                    Id = result.category.Id,
                    ProductTypeId = result.category.ProductTypeId,
                    Name = result.category.Name
                };

                cacheSvc.Upsert(new[]
                {
                    categoryData
                });

                user.AddMsgQueueWithDc(new Action_Category_out_insertCategory()
                {
                    Data = categoryData
                });

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{GetType().Name} Execute Exception Cmd:{content} User:{user}");
                return false;
            }
        }
    }
}
