namespace GoodsShopper.RelayServer.Domain.Signalr
{
    using System;
    using Live.Libs.KeepAliveConn;

    public abstract class IActionHandler : IDisposable
    {
        public abstract bool Execute(ActionModule actionModule);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
