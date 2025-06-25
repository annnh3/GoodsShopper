namespace GoodsShopper.RelayServer.Domain.Signalr
{
    using Microsoft.AspNet.SignalR.Client;

    public interface IHubClientNotify
    {
        /// <summary>
        /// hubclient狀態異動回呼
        /// </summary>
        /// <param name="hubClient"></param>
        /// <param name="stateChange"></param>
        /// <param name="forceInitialize"></param>
        void StateChangeCallback(IHubClient hubClient, StateChange stateChange, bool forceInitialize = false);
    }
}
