﻿using Live.Libs.KeepAliveConn;

namespace GoodsShopper.Domain.Action
{
    public class ProductQueryAction: ActionBase
    {
        public string Name { get; set; }

        /// <summary>
        ///     指令字串
        /// </summary>
        public override string Action()
        {
            return "productQuery";
        }

        /// <summary>
        ///     指令目標
        /// </summary>
        /// <returns></returns>
        public override DirectType Direct()
        {
            return DirectType.ToMicroService;
        }

        /// <summary>
        ///     所需流水號
        /// </summary>
        /// <returns></returns>
        public override long SerialNumberQty()
        {
            return 1;
        }
    }
}
