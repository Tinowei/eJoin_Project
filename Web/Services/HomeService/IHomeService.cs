using Web.ViewModels.HomeViewModel;

namespace Web.Services.HomeService
{
    public interface IHomeService //不用建立interface
    {
        Task<IndexViewModel> GetIndexViewModel();
        Task<IndexViewModel> GetIndexViewModel(int userId);
        Task<SearchViewModel> GetSearchViewModel();
        Task<EventViewModel> GetEventViewModel(int eventId);
        Task<EventViewModel> GetEventViewModel(int eventId, int userId);
    }
}