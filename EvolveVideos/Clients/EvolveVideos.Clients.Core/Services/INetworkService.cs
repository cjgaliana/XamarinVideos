using System;

namespace EvolveVideos.Clients.Core.Services
{
    public class NetworkStatusChangedEvents : EventArgs
    {
        public bool IsOnline { get; set; }
        public NetworkType NetworkType { get; set; }
    }

    public enum NetworkType
    {
        Unknow,
        None,
        Wifi,
        Lan,
        Cellular
    }

    public interface INetworkService
    {
        bool IsOnline { get; }

        NetworkType NetworkType { get; }

        event EventHandler<NetworkStatusChangedEvents> NetworkStatusChanged;
    }
}