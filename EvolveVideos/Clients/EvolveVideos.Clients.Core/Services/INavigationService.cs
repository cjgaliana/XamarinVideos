namespace EvolveVideos.Clients.Core.Services
{
    public enum PageKey
    {
        SplashScreenPage,
        MainPage,
        SessionDetailsPage,
        VideoCollectionsPage,
        VideoCollectionDetailsPage
    }

    public interface INavigationService
    {
        bool CanGoBack { get; }

        void GoBack();

        void NavigateTo(PageKey page);

        void NavigateTo(PageKey page, object parameters);

        void ClearNavigationStack();
    }
}