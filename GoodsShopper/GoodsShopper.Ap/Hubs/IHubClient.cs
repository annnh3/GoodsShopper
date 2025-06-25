namespace GoodsShopper.Ap.Hubs
{
    using Live.Libs.KeepAliveConn;

    public interface IHubClient
    {
        /// <summary>
        /// 廣撥用
        /// </summary>
        void BroadCastAction<A>(A act)
            where A : ActionBase;
    }
}
