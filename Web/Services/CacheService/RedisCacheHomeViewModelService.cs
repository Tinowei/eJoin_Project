using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Web.Services.HomeService;
using Web.ViewModels.HomeViewModel;
using Web.ViewModels.SharedVIewModel;

namespace Web.Services.CacheService
{
    public class RedisCacheHomeViewModelService : IHomeService
    {
        private readonly IDistributedCache _cache;
        private static readonly string _indexViewModelKey = "HomeIndexViewModel";
        private readonly HomeService.HomeService _homeService;
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromSeconds(60);

        public RedisCacheHomeViewModelService(IDistributedCache cache, HomeService.HomeService homeService)
        {
            _cache = cache;
            _homeService = homeService;
        }

        public async Task<IndexViewModel> GetIndexViewModel()
        {
            try
            {
                var cacheItems = ByteArrayToObj<IndexViewModel>(_cache.Get(_indexViewModelKey));
                if (cacheItems == null)
                {
                    var realItems = await GetItemsFromDatabase();
                    return realItems;
                }

                return cacheItems;
            }
            catch (Exception ex)
            {
                if (ex is StackExchange.Redis.RedisConnectionException || ex is TimeoutException)
                {
                    var realItems = await GetItemsFromDatabase();
                    return realItems;
                }

                throw;
            }
        }

        private async Task<IndexViewModel> GetItemsFromDatabase()
        {
            var realItems = await _homeService.GetIndexViewModel();
            try
            {
                var byteArrResult = ObjectToByteArray(realItems);
                _cache.Set(_indexViewModelKey, byteArrResult, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = _defaultCacheDuration,
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return realItems;
        }

        public async Task<IndexViewModel> GetIndexViewModel(int userId)
        {
            var indexViewModel = await GetIndexViewModel();

            foreach (var card in indexViewModel.FeaturedCards)
            {
                card.IsLike = await _homeService.CheckUserIsPickLike(card, userId);
            }

            foreach (var card in indexViewModel.HotCards)
            {
                card.IsLike = await _homeService.CheckUserIsPickLike(card, userId);
            }

            foreach (var card in indexViewModel.HolidayCards)
            {
                card.IsLike = await _homeService.CheckUserIsPickLike(card, userId);
            }

            return indexViewModel;
        }

        public async Task<EventViewModel> GetEventViewModel(int eventId)
        {
            return await _homeService.GetEventViewModel(eventId);
        }

        public async Task<EventViewModel> GetEventViewModel(int eventId, int userId)
        {
            return await _homeService.GetEventViewModel(eventId);
        }

        public async Task<SearchViewModel> GetSearchViewModel()
        {
            return await _homeService.GetSearchViewModel();
        }

        /// <summary>
        /// 將物件轉換為 Byte Array (分散式快取只支援此格式)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private byte[] ObjectToByteArray(object obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        /// <summary>
        /// 將Byte Array 轉成物件 （從分散式記憶體取得的ByteArray轉回物件） 
        /// </summary>
        /// <param name="byteArr"></param>
        /// <typeparam name="T">參考型別</typeparam>
        /// <returns></returns>
        private T? ByteArrayToObj<T>(byte[]? byteArr) where T : class
        {
            return byteArr is null ? null : JsonSerializer.Deserialize<T>(byteArr);
        }
    }
}