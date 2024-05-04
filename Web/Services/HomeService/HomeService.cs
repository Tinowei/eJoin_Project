using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Web.ViewModels.HomeViewModel;
using Web.ViewModels.RegisterViewModel.SharedViewModel;
using Web.ViewModels.SharedVIewModel;
using ApplicationCore;
using System.Linq.Expressions;
using ApplicationCore.Exceptions; // For TestService

namespace Web.Services.HomeService
{
    public class HomeService : IHomeService
    {
        private readonly IRepository<Member> _memberRepo;
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<EventTheme> _eventThemeRepo;
        private readonly IRepository<Theme> _themeRepo;
        private readonly IRepository<Like> _likeRepo;

        public HomeService(//建構子方法 (產生HomeService的時候被觸發)
            IRepository<Member> memberRepo,
            IRepository<Event> eventRepo,
            IRepository<EventTheme> eventThemeRepo,
            IRepository<Theme> themeRepo,
            IRepository<Like> likeRepo)
        {
            _memberRepo = memberRepo;
            _eventRepo = eventRepo;
            _eventThemeRepo = eventThemeRepo;
            _themeRepo = themeRepo;
            _likeRepo = likeRepo;
        }

        public async Task<IndexViewModel> GetIndexViewModel()
        {
            IndexViewModel data = GetFakeIndexViewModel();

            try
            {
                var events = await _eventRepo.ListAsync(x => x.Status == 2);//從資料庫取得狀態為2的活動資料

                #region == 處理幻燈片資料 == 


                // step 1. 根據條件取得資料庫的資料
                var top6Events = events // 取全部的資料
                    .OrderByDescending(x => x.CreateTime) //倒排CreateTime 最新的在最前面
                    .Take(6) //取前六筆
                    .ToList();
                /*
                    select top 6 from Event  >> Take(6)
                    where 1=1                >> List(x => true)
                    order by CreateTime desc >> OrderByDescending(x => x.CreateTime)
                 */


                // step 2. 建立一個空的view model list用來儲存資料
                var carouselCards = new List<CarouselEvent>();

                // step 3. each top6Events 把資料庫的Entity.Event 轉換成 HomeViewModel.Event
                foreach (var _event in top6Events)
                {
                    //step 4. 建立一個空的HomeViewModel.Event物件
                    var viewModelEvent = new CarouselEvent();

                    //step 5. 從_event取得需要的資料欄位
                    var eventId = _event.Id;
                    var eventImageUrl = _event.CoverUrl;

                    //step 6. 把資料設定到viewModelEvent
                    viewModelEvent.EventId = eventId;
                    viewModelEvent.EventImgUrl = eventImageUrl;

                    //step 7. 把viewModelEvent放入carouselCards
                    carouselCards.Add(viewModelEvent);
                }

                //step 8. 把carouselCards放入data.CarouselCards
                data.CarouselCards = carouselCards;

                #endregion

                #region == 處理精選活動資料 == 

                var evnetCount = events.Count;
                var featureCards = new List<Card>();
                var random = new Random();
                var getCount = evnetCount > 6 ? 6 : evnetCount;
                var selectedEventIds = new HashSet<int>();

                for (var i = 0; i < getCount;)//
                {
                    var randomIndex = random.Next(0, evnetCount);
                    var myEvent = events[randomIndex];

                    // 檢查是否已经選擇過這個事件
                    if (!selectedEventIds.Contains(myEvent.Id))
                    {
                        var like = await _likeRepo.ListAsync(x => x.EventId == myEvent.Id);
                        var likeCount = like.Count;

                        var card = new Card
                        {
                            EventId = myEvent.Id,
                            EventCoverUrl = myEvent.CoverUrl,
                            EventTitle = myEvent.Title,
                            EventStartDate = myEvent.StartTime,
                            EventEndDate = myEvent.EndTime,
                            EventCity = myEvent.City,
                            EventThemes = await GetThemesNameByEventId(myEvent.Id),
                            HeartCount = likeCount
                        };

                        featureCards.Add(card);
                        selectedEventIds.Add(myEvent.Id);
                        i++; // 只有在選擇不重複的事件時才增加
                    }
                }

                data.FeaturedCards = featureCards;

                #endregion

                #region == 處理熱門活動資料 ==

                var likeRepo = await _likeRepo.ListAsync(x => true);
                var onlineEventIds = events.Select(x=>x.Id).ToList();

                var top6EventIdByLikeCount = likeRepo
                    .Where(x=> onlineEventIds.Contains(x.EventId))
                    .GroupBy(x => x.EventId)
                    .Select(x => new
                    {
                        EventId = x.Key,
                        LikeCount = x.Count()
                    })
                    .OrderByDescending(x => x.LikeCount)
                    .Take(6)
                    .ToList();

                var hotCards = new List<Card>();

                foreach (var item in top6EventIdByLikeCount)
                {
                    var myEvent = events.SingleOrDefault(x => x.Id == item.EventId);

                    if (myEvent != null)
                    {
                        var card = new Card
                        {
                            EventId = myEvent.Id,
                            EventCoverUrl = myEvent.CoverUrl,
                            EventTitle = myEvent.Title,
                            EventStartDate = myEvent.StartTime,
                            EventEndDate = myEvent.EndTime,
                            EventCity = myEvent.City,
                            EventThemes = await GetThemesNameByEventId(myEvent.Id),
                            HeartCount = item.LikeCount
                        };

                        hotCards.Add(card);
                    }
                    else
                    {
                        Console.WriteLine($"Cannot get event data : event id is {item.EventId}");
                    }
                }

                data.HotCards = hotCards;

                #endregion

                #region == 處理假日的活動資料 ==

                var holidayCards = new List<Card>();
                var maxCount = 6;
                foreach (var myEvent in events)
                {
                    if (holidayCards.Count == maxCount) break;

                    //取得活動的起始時間跟結束時間
                    var eventStartTime = myEvent.StartTime;
                    var eventEndTime = myEvent.EndTime;

                    //for 迴圈抓出時間內的所有日期
                    for (var date = eventStartTime; date <= eventEndTime; date = date.AddDays(1))
                    {
                        // 如果日期是假日就抓出來
                        if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            var like = await _likeRepo.ListAsync(x => x.EventId == myEvent.Id);
                            var likeCount = like.Count;

                            var card = new Card
                            {
                                EventId = myEvent.Id,
                                EventCoverUrl = myEvent.CoverUrl,
                                EventTitle = myEvent.Title,
                                EventStartDate = myEvent.StartTime,
                                EventEndDate = myEvent.EndTime,
                                EventCity = myEvent.City,
                                EventThemes =  await GetThemesNameByEventId(myEvent.Id),
                                HeartCount = likeCount
                            };

                            holidayCards.Add(card);

                            break;//跳出迴圈
                        }
                    }
                }

                data.HolidayCards = holidayCards;

                #endregion

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return data;
        }

        private static IndexViewModel GetFakeIndexViewModel()
        {
            return new IndexViewModel
            {
                CarouselCards = new List<CarouselEvent>
                {
                    new CarouselEvent
                    {
                        EventId = 1,
                        EventImgUrl = ""
                    }
                },
                FeaturedCards = new List<Card>
                {
                    new Card
                    {
                        EventId = 1,
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2023 生活音樂節",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "戶外"
                        },
                        HeartCount = 66
                    },
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "星球漫遊",
                        EventCity = "台北市",
                        EventThemes = new List<string> {
                            "遊樂",
                            "交友"
                        },
                        HeartCount = 66
                    },
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "3月親子木工手作課程｜ 木作森活｜木作森友會",
                        EventCity = "新北市",
                        EventThemes = new List<string> {
                            "手作",
                            "親子"
                        },
                        HeartCount = 66
                    },


                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "松山小旅行｜十大財神x推薦美食x公共藝術",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },



                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2024 藝文寫作班 - 企劃入門、策展實戰、標案撰寫 (春季班)",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },


                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "《韓式裱花 x 藝術生活》",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },

                },
                HotCards = new List<Card>
                {
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "113年度ITI國企班招生說明會",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "拾夢者的記憶伏流 － 恆春篇",
                        EventCity = "台北市",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "【優席夫野生藝術教室】自畫像彩繪課",
                        EventCity = "新北市",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },


                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "【Sound Fest】Aroma Cafe音樂之夜 3/9",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },



                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "親子理財齊步走",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },


                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "缺工潮下設計與工時的效率提升：2024 住宅設計論壇",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },

                },
                HolidayCards = new List<Card>
                {
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "兒少表創性藝術治療工作坊",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "大人の樂團-長笛合奏體驗",
                        EventCity = "台北市",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "112學年度慈濟中山大同-親子成長班暨慈少班",
                        EventCity = "新北市",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },


                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "松山小旅行｜十大財神x推薦美食x公共藝術",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },



                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "【新月．藝文講座】博物館裡的穆斯林世界",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },


                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "「辣妹過招」美國校園電影英文團班報名",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },

                }
            };
        }

        public async Task<IndexViewModel> GetIndexViewModel(int userId)
        {
            var indexViewModel = await GetIndexViewModel();

            foreach (var card in indexViewModel.FeaturedCards)
            {
                card.IsLike = await CheckUserIsPickLike(card, userId);
            }

            foreach (var card in indexViewModel.HotCards)
            {
                card.IsLike = await CheckUserIsPickLike(card, userId);
            }

            foreach (var card in indexViewModel.HolidayCards)
            {
                card.IsLike = await CheckUserIsPickLike(card, userId);
            }

            return indexViewModel;
        }

        public async Task<bool> CheckUserIsPickLike(Card card, int userId)
        {
            if (card == null) return false;

            return await _likeRepo.AnyAsync(x => x.EventId == card.EventId && x.MemberId == userId);
        }


        public async Task<SearchViewModel> GetSearchViewModel()
        {
            return new SearchViewModel
            {
                SearchCard = new List<Card>
                {

                    new Card
                    {
                        EventCoverUrl = "",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "【熱銷】欸等的一小時MVC魔鬼訓練營",
                        EventCity = "澎湖縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 66
                    },

                    new Card
                    {
                        EventCoverUrl = "",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2024潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 77
                    },

                    new Card
                    {
                        EventCoverUrl = "",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2025潮旅行",
                        EventCity = "新北市",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 88
                    },

                    new Card
                    {
                        EventCoverUrl = "",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2026潮旅行",
                        EventCity = "屏東縣",
                        EventThemes = new List<string> {
                            "學習",
                            "科技"
                        },
                        HeartCount = 99
                    },

                },
                Themes = await _themeRepo.ListAsync(x => true),
            };
        }

        public async Task<EventViewModel> GetEventViewModel(int eventId, int userId)
        {
            var data = await GetEventViewModel(eventId);

            data.IsLike = await _likeRepo.AnyAsync(x => x.EventId == data.EventId && x.MemberId == userId);

            foreach (var card in data.PreferredLikeEvents)
            {
                card.IsLike = await CheckUserIsPickLike(card, userId);
            }
            return data;
        }

        public async Task<EventViewModel> GetEventViewModel(int eventId)
        {
            Event eventTarget = await _eventRepo.SingleOrDefaultAsync(x => x.Id == eventId) ?? throw new KeyNotFoundException(@$"[GetEventViewModel] not found event , id : {eventId}");
            if (eventTarget.Status == 4) throw new EventIsRemovedException($"[GetEventViewModel] this event is removed , id : {eventId}");

            var memberId = eventTarget?.MemberId ?? default;
            var memberName = string.Empty;
            var memberImageUrl = string.Empty;
            if (memberId > 0)
            {
                var member = await _memberRepo.SingleOrDefaultAsync(x => x.Id == memberId && !x.IsDelete);
                memberName = member?.Name ?? string.Empty;
                memberImageUrl = member?.AvatarUrl ?? string.Empty;
            }

            var events = await _eventRepo.ListAsync(x => x.Status == 2);//從資料庫取得狀態為2的活動資料
            var evnetCount = events.Count;

            var random = new Random();

            var getCount = evnetCount > 6 ? 6 : evnetCount;

            var preferredLikeEvents = new List<Card>();
            var selectedEventIds = new HashSet<int>();

            // 最多選擇6個不重複的事件
            for (var i = 0; i < getCount;)
            {
                var randomIndex = random.Next(0, evnetCount);
                var myEvent = events[randomIndex];

                // 檢查是否已经選擇過這個事件
                if (!selectedEventIds.Contains(myEvent.Id))
                {
                    var like = await _likeRepo.ListAsync(x => x.EventId == myEvent.Id);
                    var likeCount = like.Count;

                    var card = new Card
                    {
                        EventId = myEvent.Id,
                        EventCoverUrl = myEvent.CoverUrl,
                        EventTitle = myEvent.Title,
                        EventStartDate = myEvent.StartTime,
                        EventEndDate = myEvent.EndTime,
                        EventCity = myEvent.City,
                        EventThemes = await GetThemesNameByEventId(myEvent.Id),
                        HeartCount = likeCount
                    };

                    preferredLikeEvents.Add(card);
                    selectedEventIds.Add(myEvent.Id);
                    i++; // 只有在選擇不重複的事件時才增加
                }
            }

            var likeRepo = await _likeRepo.ListAsync(x => x.EventId == eventId);

            var data = new EventViewModel
            {
                EventId = eventId, // 讓eventId接收現在是哪個活動
                HeartCount = likeRepo.Count,
                PreferredLikeEvents = preferredLikeEvents,
                EventCoverUrl = eventTarget?.CoverUrl ?? string.Empty,
                EventThemes = await GetThemesNameByEventId(eventId),
                MemberID = memberId,
                MemberImages = memberImageUrl,
                MemberName = memberName,
                EventTitle = eventTarget?.Title ?? string.Empty,
                EventStartDate = eventTarget?.StartTime ?? default,
                EventEndDate = eventTarget?.EndTime ?? default,
                Address = eventTarget?.Address ?? string.Empty,
                AddressDetail = eventTarget?.AddressDetail ?? string.Empty,
                EventSummary = eventTarget?.Summary ?? string.Empty,
                EventIntroduction = eventTarget?.Introduction ?? string.Empty,
                EventStatus = eventTarget?.Status,
                Longitude = eventTarget.Longitude,
                Latitude = eventTarget.Latitude,
                City = eventTarget.City
            };

            return data;
        }

        private async Task<List<string>> GetThemesNameByEventId(int eventId)
        {
            var eventThemeRepo = await _eventThemeRepo.ListAsync(x => x.EventId == eventId);
            var eventThemeIds = eventThemeRepo
                .Select(x => x.ThemeId)
                .ToList();

            var themeRepo = await _themeRepo.ListAsync(x => eventThemeIds.Contains(x.Id));
            var list = themeRepo
                .Select(x => x.ThemeName)
                .ToList();

            return list ?? new List<string>();
        }
    }
}
