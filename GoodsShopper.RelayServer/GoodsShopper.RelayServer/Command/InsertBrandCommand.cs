using System;
using GoodsShopper.Domain.Model;
using GoodsShopper.Domain.Service;
using GoodsShopper.RelayServer.Domain.Cache.Structure;
using GoodsShopper.RelayServer.Domain.ClientAction.ToClient;
using GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Newtonsoft.Json;

namespace GoodsShopper.RelayServer.Command
{
    public class InsertBrandCommand : ICommand
    {
        /// <summary>
        /// 緩存服務
        /// </summary>
        private readonly ICacheService cacheSvc;

        /// <summary>
        /// 新增品牌服務
        /// </summary>
        private readonly IBrandService brandSvc;

        public InsertBrandCommand(ICacheService cacheSvc, IBrandService brandSvc)
        {
            this.cacheSvc = cacheSvc;
            this.brandSvc = brandSvc;
        }

        public override bool Execute(string content, User user)
        {
            try
            {
                var cmd = JsonConvert.DeserializeObject<Action_Brand_in_insertBrand>(content);

                var result = brandSvc.Insert(new GoodsShopper.Domain.DTO.BrandInsertDto
                {
                    Name = cmd.Name
                });

                var brandData = new Domain.Cache.Structure.Brand
                {
                    Name = result.brand.Name,
                    Id = result.brand.Id
                };

                cacheSvc.Upsert(new[] 
                {
                    brandData
                });

                user.AddMsgQueueWithDc(new Action_Brand_out_insertBrand()
                {
                    Data = brandData
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
