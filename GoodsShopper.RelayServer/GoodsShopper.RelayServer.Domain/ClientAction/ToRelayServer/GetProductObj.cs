using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsShopper.RelayServer.Domain.ClientAction.ToRelayServer
{
    public class GetProductObj
    {
        public List<ProductObj> Products { get; set; }

        public class ProductObj
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int BrandId { get; set; }

            public int CategoryId { get; set; }
        }
    }
}
