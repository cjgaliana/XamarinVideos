namespace EvolveVideos.Clients.Core.Services
{
    public enum PageKey
    {
        MainPage,
        SessionDetailsPage
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