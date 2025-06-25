namespace GoodsShopper.Ap.Applibs
{
    using System.Threading.Tasks;

    public enum ProcedureType
    {
        /// <summary>
        /// KeepAlive程序
        /// </summary>
        KeepAlive
    }

    /// <summary>
    /// 背景程式介面
    /// </summary>
    public interface IProcedure
    {
        Task StartAsync();

        Task StopAsync();
    }
}
