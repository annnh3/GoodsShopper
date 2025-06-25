
namespace Live.Libs.Persistent
{
    using System;

    /// <summary>
    /// 取流水號
    /// </summary>
    public interface ISerialNumberRepository
    {
        /// <summary>
        /// 取唯一流水號
        /// </summary>
        /// <param name="qty">流水號數量</param>
        /// <returns></returns>
        (Exception exception, long serialNumber) GetSerialNumber(long qty = 1);
    }
}
