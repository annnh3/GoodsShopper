namespace GoodsShopper.RelayServer.Model
{
    using GoodsShopper.RelayServer.Applibs;
    using Microsoft.AspNet.SignalR.Client;
    using RelayServer.Domain.Signalr;

    public class HubClientNotify : IHubClientNotify
    {
        /// <summary>
        /// hubclient狀態異動回呼
        /// </summary>
        /// <param name="hubClient"></param>
        /// <param name="stateChange"></param>
        /// <param name="forceInitialize"></param>
        public void StateChangeCallback(IHubClient hubClient, StateChange stateChange, bool forceInitialize = false)
        {
            RelayServerProcess.DrawView(hubClient, stateChange, forceInitialize);
        }
    }
}
