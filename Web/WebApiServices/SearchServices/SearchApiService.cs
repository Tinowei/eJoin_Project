using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Web.Models;
using Web.Services.AccountService;
using Web.ViewModels.SharedVIewModel;
using Web.WebApi.Search;

namespace Web.WebApiServices.SearchServices
{
    public class SearchApiService
    {
        private readonly IEventRepository _eventRepository;
        private readonly UserContextService _userContextService;
        private readonly IRepository<Theme> _themeRepo;
        public SearchApiService(
            IEventRepository eventRepository, 
            UserContextService userContextService,
            IRepository<Theme> themeRepo) 
        {
            _eventRepository = eventRepository;
            _userContextService = userContextService;
            _themeRepo = themeRepo;
        }

        public OperationResult GetSearchEvents(SearchDTO searchDTO)
        {
            var options = new SearchOptions()
            {
                Keyword = searchDTO.Keyword,
                SelectedOrderBy = searchDTO.SelectedOrderBy,
                SelectedPlaces = searchDTO.SelectedPlaces,
                SelectedPrice = searchDTO.SelectedPrice,
                SelectedThemes = searchDTO.SelectedThemes,
                SelectedTime = searchDTO.SelectedTime,
                CurrentPage = searchDTO.CurrentPage,
            };
            var themes = _themeRepo.List(x => true);
            var result = _eventRepository.GetSearchResult(options).Select(e => new
            {
                EventId = e.Id,
                IsLike = e.Likes.Any(l => l.MemberId == _userContextService.GetUserId()),
                EventCoverUrl = e.CoverUrl,
                EventTitle = e.Title,
                EventStartDate = e.StartTime.ToString("yyyy.MM.dd HH:mm"),
                EventEndDate = e.EndTime.ToString("yyyy.MM.dd HH:mm"),
                EventCity = e.City,
                HeartCount = e.Likes?.Count ?? 0,
                EventThemes = e.EventThemes.Select(et => themes.SingleOrDefault(t => t.Id == et.ThemeId)?.ThemeName ?? string.Empty).ToList(),
            }) ;

            var msg = $"return page {searchDTO.CurrentPage}";

            if (!(result.Count() == 12)) msg = "end";

            return new OperationResult()
            {
                IsSuccess = true,
                Message = msg,
                Result = result,
            };
        }
    }
}
