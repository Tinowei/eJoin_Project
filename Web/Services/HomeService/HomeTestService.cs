
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Web.ViewModels.HomeViewModel;
using Web.ViewModels.SharedVIewModel;

namespace Web.Services.HomeService
{
    public class HomeTestService
    {
        private readonly IRepository<Member> _memberRepo;
        private readonly IRepository<ApplicationCore.Entities.Event> _eventRepo;
        public HomeTestService(IRepository<Member> memberRepo, IRepository<ApplicationCore.Entities.Event> eventRepo)
        {
            _memberRepo = memberRepo;
            _eventRepo = eventRepo;
        }
        // 參考以下方法建立Get{Action}ViewModel
        public IndexViewModel GetIndexViewModel()
        {
            return new IndexViewModel
            {
                CarouselCards = new List<ViewModels.HomeViewModel.CarouselEvent>
                {
                    new ViewModels.HomeViewModel.CarouselEvent
                    {
                        EventId = 1,
                        EventImgUrl =""
                    } 
                },
                //RecentlyViewed = new List<Card>
                //{
                //    new Card
                //    {
                //        EventId = 1,
                //        EventImgUrl ="",
                //        EventStartDate = DateTime.Now,
                //        EventEndDate = DateTime.Now,
                //        EventTitle = "2023 生活音樂節",
                //        EventCity = "屏東縣",
                //        EventCategories = new List<string> {
                //            "學習",
                //            "戶外"
                //        },
                //        HeartCount = 66
                //    },
                //    new Card
                //    {
                //        EventImgUrl ="",
                //        EventStartDate = DateTime.Now,
                //        EventEndDate = DateTime.Now,
                //        EventTitle = "星球漫遊",
                //        EventCity = "台北市",
                //        EventCategories = new List<string> {
                //            "遊樂",
                //            "交友"
                //        },
                //        HeartCount = 66
                //    },
                //    new Card
                //    {
                //        EventImgUrl ="",
                //        EventStartDate = DateTime.Now,
                //        EventEndDate = DateTime.Now,
                //        EventTitle = "3月親子木工手作課程｜ 木作森活｜木作森友會",
                //        EventCity = "新北市",
                //        EventCategories = new List<string> {
                //            "手作",
                //            "親子"
                //        },
                //        HeartCount = 66
                //    },

                //},
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
        public SearchViewModel GetSearchViewModel()
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

                }
            };
        }
        public EventViewModel GetEventViewModel(int eventId)
        {
            return new EventViewModel
            {

                PreferredLikeEvents = new List<Card>
                {
                    new Card
                    {
                        EventCoverUrl ="",
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
                        EventCoverUrl ="",
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
                        EventCoverUrl ="",
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
                        EventCoverUrl ="",
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
                        EventCoverUrl ="",
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
                },
                EventCoverUrl = "",
                EventThemes = new List<string>
                {
                    "學習",
                    "科技"
                },
                MemberID = _eventRepo.SingleOrDefault(e => e.Id == eventId).MemberId,
                MemberName = "欸等",
                EventTitle = "【熱銷】欸等的一小時MVC魔鬼訓練營",
                EventStartDate = DateTime.Now,
                EventEndDate = DateTime.Now,
                Address = "台北市大安區忠孝東路三段96號",
                AddressDetail = "台灣屏東縣潮州火車站",
                EventSummary = "接續2022潮旅行，今年活動從「圈」的意象出發，圈是繪畫圓形的動詞，在大武山下的環繞下，以能量豐沛的潮州為半徑，旋轉出從山到海、從農田到結市各種生活樣貌，旋開大武山的按鈕，由圓環往外擴散的各鄉鎮，將能量相互傳送，一起打造出共生共創的生活圈。",
                EventIntroduction = "接續2022潮旅行，今年活動從「圈」的意象出發，圈是繪畫圓形的動詞，在大武山下的環繞下，以能量豐沛的潮州為半徑，旋轉出從山到海、從農田到結市各種生活樣貌，旋開大武山的按鈕，由圓環往外擴散的各鄉鎮，將能量相互傳送，一起打造出共生共創的生活圈。\r\n\r\n\r\n接續2022潮旅行，今年活動從「圈」的意象出發，圈是繪畫圓形的動詞，在大武山下的環繞下，以能量豐沛的潮州為半徑，旋轉出從山到海、從農田到結市各種生活樣貌，旋開大武山的按鈕，由圓環往外擴散的各鄉鎮，將能量相互傳送，一起打造出共生共創的生活圈。\r\n\r\n\r\n接續2022潮旅行，今年活動從「圈」的意象出發，圈是繪畫圓形的動詞，在大武山下的環繞下，以能量豐沛的潮州為半徑，旋轉出從山到海、從農田到結市各種生活樣貌，旋開大武山的按鈕，由圓環往外擴散的各鄉鎮，將能量相互傳送，一起打造出共生共創的生活圈。\r\n\r\n\r\n接續2022潮旅行，今年活動從「圈」的意象出發，圈是繪畫圓形的動詞，在大武山下的環繞下，以能量豐沛的潮州為半徑，旋轉出從山到海、從農田到結市各種生活樣貌，旋開大武山的按鈕，由圓環往外擴散的各鄉鎮，將能量相互傳送，一起打造出共生共創的生活圈。\r\n\r\n\r\n接續2022潮旅行，今年活動從「圈」的意象出發，圈是繪畫圓形的動詞，在大武山下的環繞下，以能量豐沛的潮州為半徑，旋轉出從山到海、從農田到結市各種生活樣貌，旋開大武山的按鈕，由圓環往外擴散的各鄉鎮，將能量相互傳送，一起打造出共生共創的生活圈。\r\n\r\n\r\n接續2022潮旅行，今年活動從「圈」的意象出發，圈是繪畫圓形的動詞，在大武山下的環繞下，以能量豐沛的潮州為半徑，旋轉出從山到海、從農田到結市各種生活樣貌，旋開大武山的按鈕，由圓環往外擴散的各鄉鎮，將能量相互傳送，一起打造出共生共創的生活圈。"

            };
        }

        public IndexViewModel GetIndexViewModel(int userId)
        {
            throw new NotImplementedException();
        }

        public EventViewModel GetEventViewModel(int eventId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}







