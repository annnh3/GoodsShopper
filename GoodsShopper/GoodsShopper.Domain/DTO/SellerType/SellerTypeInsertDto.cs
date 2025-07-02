using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsShopper.Domain.DTO
{
    /// <summary>
    /// 賣家型態新增DTO
    /// </summary>
    public class SellerTypeInsertDto
    {
        /// <summary>
        /// 賣家型態
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
