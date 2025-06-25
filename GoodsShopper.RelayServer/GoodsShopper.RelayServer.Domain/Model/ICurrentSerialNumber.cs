namespace GoodsShopper.RelayServer.Domain.Model
{
    /// <summary>
    /// 當前流水號
    /// </summary>
    public interface ICurrentSerialNumber
    {
        /// <summary>
        /// 系統流水號
        /// </summary>
        long SerialNumber { get; set; }
    }
}
