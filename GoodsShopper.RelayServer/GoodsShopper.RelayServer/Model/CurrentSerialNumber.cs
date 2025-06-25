namespace GoodsShopper.RelayServer.Model
{
    using RelayServer.Applibs;
    using RelayServer.Domain.Model;

    /// <summary>
    /// 當前流水號
    /// </summary>
    public class CurrentSerialNumber : ICurrentSerialNumber
    {
        private long chatSerialNumber = 1;

        /// <summary>
        /// 系統流水號
        /// </summary>
        public long SerialNumber
        {
            get
                => ConfigHelper.SerialNumber;
            set
            {
                ConfigHelper.SerialNumber = value;
            }
        }
    }
}
