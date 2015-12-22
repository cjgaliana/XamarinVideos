using Windows.Networking.Connectivity;
using EvolveVideos.Clients.Core.Services;

namespace EvolveVideos.Clients.UWP.Services
{
    public class NetworkService : INetworkService
    {
        public bool IsOnline
        {
            get
            {
                var internetProfile = NetworkInformation.GetInternetConnectionProfile();
                return internetProfile != null
                       && internetProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
        }
    }
}