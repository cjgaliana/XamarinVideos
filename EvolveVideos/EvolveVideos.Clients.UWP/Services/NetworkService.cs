using System;
using Windows.Networking.Connectivity;
using EvolveVideos.Clients.Services;

namespace EvolveVideos.Clients.UWP.Services
{
    public class NetworkService : INetworkService
    {
        public NetworkService()
        {
            NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;
        }

        public event EventHandler<NetworkStatusChangedEvents> NetworkStatusChanged;

        public bool IsOnline
        {
            get
            {
                var internetProfile = NetworkInformation.GetInternetConnectionProfile();
                return internetProfile != null
                       && internetProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
        }

        public NetworkType NetworkType
        {
            get
            {
                var internetProfile = NetworkInformation.GetInternetConnectionProfile();
                if (internetProfile != null)
                {
                    if (internetProfile.IsWlanConnectionProfile)
                    {
                        return NetworkType.Wifi;
                    }
                    if (internetProfile.IsWwanConnectionProfile)
                    {
                        var type = internetProfile.WwanConnectionProfileDetails.GetCurrentDataClass();
                        switch (type)
                        {
                            case WwanDataClass.None:
                                return NetworkType.None;

                            default:
                                return NetworkType.Cellular;
                        }
                    }
                }

                return NetworkType.Unknow;
            }
        }

        private void OnNetworkStatusChanged(object sender)
        {
            var handler = NetworkStatusChanged;
            if (handler != null)
            {
                var args = new NetworkStatusChangedEvents
                {
                    IsOnline = IsOnline,
                    NetworkType = NetworkType
                };
                handler.Invoke(this, args);
            }
        }
    }
}