using Web.ViewModels.MemberViewModel;
using Web.ViewModels.SharedVIewModel;
using Web.ViewModels.MemberViewModel.MyTicketDTO;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;
using Infrastructure.Data;
using System.Reflection;

namespace Web.Services.MemberService
{
    public class MemberService
    {
        private readonly IRepository<Member> _memberRepo;
        private readonly IRepository<Follow> _followRepo;
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<EventTheme> _eventThemeRepo;
        private readonly IRepository<Theme> _themeRepo;
        private readonly IRepository<Like> _likeRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IMemberRepository _memberRepository;

        public MemberService(
            IRepository<Member> memberRepo,
            IRepository<Follow> followRepo,
            IRepository<Event> eventRepo,
            IRepository<EventTheme> eventThemeRepo,
            IRepository<Theme> themeRepo,
            IRepository<Like> likeRepo,
            IRepository<Order> orderRepo,
            IMemberRepository memberRepository
            )
        {
            _memberRepo = memberRepo;
            _followRepo = followRepo;
            _eventRepo = eventRepo;
            _eventThemeRepo = eventThemeRepo;
            _themeRepo = themeRepo;
            _likeRepo = likeRepo;
            _memberRepository = memberRepository;
            _orderRepo = orderRepo;
        }

        public IndexViewModel GetIndexViewModel(int memberId)
        {
            //TODO 要取得登入者有沒有跟隨這個Member
            var data = new IndexViewModel()
            {
                MemberName = "欸等",
                FansCount = 88,
                EventCount = 12,
                MemberDescription = "好物 為 美好生活 發聲 好物的孵育者，富生達國際行銷 我們總是懷抱熱情與許多夥伴合作 過程中我們一起用心的 追求「好」、分享「好」\r\n 就是希望客戶及顧客們收到 「好」的堅持！ 為了能為夥伴們創造亮點實踐，我們創立了好物平台 一個美好生活分享平台 分享好品質、好品牌、好品味的商品及體驗\r\n幫助人們打造優質的美好生活！ 好物平台，是個共創好事的家， 歡迎不同領域的朋友回家合作， 讓好物為您發聲，一起讓好事發生！",
                MemberBuildDate = DateTime.Now,
                MemberEventThemes = new List<string> {
                    "戶外體驗",
                    "美食"
                },
                PastEventByYear = new List<PastEventYear> {
                    new PastEventYear
                    {
                        EventYear = 2024,
                        Events = new List<PastEvent> {
                            new PastEvent {
                                EventBeginDate = DateTime.Now,
                                PastEventImage = "",
                                EventName = "列車餔餔",
                                EventLocate = "新北市"
                            }
                        }
                    },
                    new PastEventYear
                    {
                        EventYear = 2023,
                        Events = new List<PastEvent>
                        {
                            new PastEvent {
                                EventBeginDate = DateTime.Now,
                                PastEventImage = "",
                                EventName = "2024潮旅行",
                                EventLocate = "屏東縣"
                            },
                            new PastEvent {
                                EventBeginDate = DateTime.Now,
                                PastEventImage = "",
                                EventName = "2033潮旅行",
                                EventLocate = "台南市"
                            },
                        }
                    },
                },

                PersonalCard = new List<Card>
                {
                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2023潮旅行",
                        EventCity = "澎湖縣",
                        EventThemes = new List<string>(){"戶外活動"},
                        HeartCount = 66
                    },

                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2024潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行潮旅行",
                        EventCity = "屏東縣",
                        EventThemes = new List<string>(){"我不想體驗"},
                        HeartCount = 77
                    },

                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2025潮旅行",
                        EventCity = "新北市",
                        EventThemes = new List<string>(){"極限體驗" },
                        HeartCount = 88
                    },

                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2026潮旅行",
                        EventCity = "屏東縣",
                        EventThemes = new List<string>(){"我很厭世" },
                        HeartCount = 99
                    },

                    new Card
                    {
                        EventCoverUrl ="",
                        EventStartDate = DateTime.Now,
                        EventEndDate = DateTime.Now,
                        EventTitle = "2027潮旅行",
                        EventCity = "屏東縣",
                        EventThemes = new List<string>(){"專題快結束"},
                        HeartCount = 55
                    }
                }
            };

            try
            {
                // 從資料庫取得Member
                var memberInfo = _memberRepository.GetMemberInfoByMemberId(memberId);
                if (memberInfo != null)
                {
                    data.MemberId = memberInfo.MemberId;
                    data.MemberImage = memberInfo.MemberAvatarUrl;
                    data.CoverImage = memberInfo.CoverImage;
                    data.MemberName = memberInfo.MemberName;
                    data.MemberDescription = memberInfo.MemberDescription;
                    data.MemberBuildDate = memberInfo.MemberBuildDate;
                    data.FansCount = memberInfo.FansCount;
                    data.EventCount = memberInfo.EventCount;
                    data.MemberEventThemes = memberInfo.MemberEventThemes;

                    var pastEventByYear = new List<PastEventYear>();
                    if (memberInfo.PastEvents.Count > 0)
                    {
                        //過往活動時間倒排 以年做分類
                        foreach (var item in memberInfo.PastEvents.OrderByDescending(x=>x.EventBeginDate))
                        {
                            var isShowDate = true;
                            var eventYear = item.EventBeginDate.Year;
                            var pastEventYear = pastEventByYear.FirstOrDefault(x => x.EventYear == eventYear);
                            var pastEvent = new PastEvent
                            {
                                EventBeginDate = item.EventBeginDate,
                                PastEventImage = item.PastEventImage,
                                EventId = item.EventId,
                                EventName = item.EventName,
                                EventLocate = item.EventLocate,
                                IsShowDate = isShowDate
                            };
                            if (pastEventYear == null)
                            {
                                pastEventYear = new PastEventYear
                                {
                                    EventYear = eventYear,
                                    Events = new List<PastEvent>
                                    {
                                        pastEvent
                                    }
                                };
                                pastEventByYear.Add(pastEventYear);
                            }
                            else
                            {
                                //檢查是否已經有出現相同日期的活動 如果有的話則不顯示該活動的日期
                                if(pastEventYear.Events.Any(x=> x.EventBeginMonth == pastEvent.EventBeginMonth && x.EventBeginDate.Day == pastEvent.EventBeginDate.Day))
                                {
                                    pastEvent.IsShowDate = false;
                                }
                                pastEventYear.Events.Add(pastEvent);
                            }
                        }
                    }
                    data.PastEventByYear = pastEventByYear.OrderByDescending(x=>x.EventYear).ToList();

                    var personalCard = new List<Card>();
                    if(memberInfo.FutureEvents.Count > 0)
                    {
                        foreach(var item in memberInfo.FutureEvents)
                        {
                            personalCard.Add(new Card
                            {
                                EventId= item.EventId,
                                EventCoverUrl = item.EventConverUrl,
                                EventStartDate = item.EventStartDate,
                                EventEndDate = item.EventEndDate,
                                EventTitle = item.EventTitle,
                                EventCity = item.EventCity,
                                EventThemes = item.EventThemes,
                                HeartCount = item.HeartCount
                            });
                        }
                    }
                    data.PersonalCard = personalCard;


                    //取得member最常舉辦的活動主題2筆
                    var allEvents = _eventRepo.List(x => x.MemberId == memberId);
                    if (allEvents.Count > 0)
                    {
                        var themeIds = new List<int>();
                        foreach (var item in allEvents) //取ThemeId
                        {
                            var eventThemes = _eventThemeRepo.List(x => x.EventId == item.Id);
                            if (eventThemes.Count > 0) themeIds.AddRange(eventThemes.Select(x => x.ThemeId));
                        }
                        var top2ThemeIds = themeIds.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(2).Select(x => x.Key).ToList();

                        var themeNames = new List<string>();
                        foreach (var themeId in top2ThemeIds) //取themeName
                        {
                            var theme = _themeRepo.SingleOrDefault(x => x.Id == themeId);
                            if (theme != null)
                            {
                                themeNames.Add(theme.ThemeName); //儲存主題名稱
                            }
                        }

                        data.MemberEventThemes = themeNames;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return data;

        }


        public MyEventViewModel GetMyEventViewModel(int memberId)
        {
            var data = new MyEventViewModel();

            try
            {
                var member = _memberRepo.SingleOrDefault(x => x.Id == memberId && !x.IsDelete);

                if (member != null)
                {
                    //取得member所有的追蹤者
                    var allFollows = _followRepo.List(x => x.FollowerId == memberId).Select(x => x.BeingFollowedId);

                    //取得member所有的活動
                    var allEvents = _eventRepo.List(x => x.MemberId == memberId);
                    data.AvatarUrl = member.AvatarUrl;
                    data.DisplayName = member.DisplayName;

                    var myLikes = _likeRepo.List(x => x.MemberId == memberId);
                    var myLikeEventIds = myLikes.Select(x => x.EventId).ToList();
                    var myLikeEvent = _eventRepo.List(x => myLikeEventIds.Contains(x.Id));

                    //====取得訂單中的活動===
                    //orders-BuyerId等於Members-id，然後挑出orders-EventId
                    var orderEvents = _orderRepo.List(x => x.BuyerId == memberId).Select(x => x.EventId);
                    //orders-EventId等於Events-id
                    var myEvents = _eventRepo.List(x => orderEvents.Contains(x.Id));

                    data.ParticipateCard = myEvents.Select(e => new Card
                    {
                        EventId = e.Id,
                        EventCoverUrl = e.CoverUrl,
                        EventStartDate = e.StartTime,
                        EventEndDate = e.EndTime,
                        EventTitle = e.Title,
                        EventCity = e.City,
                        EventThemes = GetThemesNameByEventId(e.Id),
                        HeartCount = _likeRepo.List(x => x.EventId == e.Id).Count(),
                        IsLike = myLikeEventIds.Any(eventId => eventId == e.Id)
                    }).ToList();

                    data.LikeCard = myLikeEvent.Select(e => new Card
                    {
                        EventId = e.Id,
                        EventCoverUrl = e.CoverUrl,
                        EventStartDate = e.StartTime,
                        EventEndDate = e.EndTime,
                        EventTitle = e.Title,
                        EventCity = e.City,
                        EventThemes = GetThemesNameByEventId(e.Id),
                        HeartCount = _likeRepo.List(x => x.EventId == e.Id).Count(),
                        IsLike = true
                    }).ToList();

                    data.TrackGrid = allFollows.Select(memberId =>
                    {
                        var follower = _memberRepo.SingleOrDefault(m => m.Id == memberId);
                        return new MemberMyEventTrackGridViewModel
                        {
                            OrganizerId = memberId,
                            OrganizerAvatarUrl = follower?.AvatarUrl ?? "",
                            OrganizerName = follower?.DisplayName ?? "",
                            TotalEvent = _eventRepo.List(x => x.MemberId == memberId).Count(), // 取得追蹤者參與的活動數
                            TrackNumber = _followRepo.List(x => x.BeingFollowedId == memberId).Count() // 取得追蹤者的追蹤數
                        };
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return data;
        }

        public bool IsFollowThisMember(int userId, int memberId)
        {
            if (userId == 0) return false;
            return _followRepo.Any(x => x.FollowerId == userId && x.BeingFollowedId == memberId);
        }

        public bool CheckUserIsPickLike(Card card, int userId)
        {
            if (card == null) return false;

            return _likeRepo.Any(x => x.EventId == card.EventId && x.MemberId == userId);
        }

        private List<string> GetThemesNameByEventId(int eventId)
        {
            var eventThemeIds = _eventThemeRepo
                .List(x => x.EventId == eventId)
                .Select(x => x.ThemeId)
                .ToList();

            var list = _themeRepo
                .List(x => eventThemeIds.Contains(x.Id))
                .Select(x => x.ThemeName)
                .ToList();

            return list ?? new List<string>();
        }


        public MyTicketViewModel GetMyTicketViewModel(int memberId)
        {
            if (memberId == null)
            {
                return new MyTicketViewModel();
            }

            try
            {
                var member = _memberRepo.FirstOrDefault(x => x.Id == memberId && !x.IsDelete);

                if (member == null)
                {
                    return new MyTicketViewModel();
                }

                return new MyTicketViewModel
                {
                    AvatarUrl = member.AvatarUrl,
                    DisplayName = member.DisplayName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MyTicketViewModel();
            }
        }

        //public MyProfileViewModel GetMyProfileViewModel()
        //{
        //    return new MyProfileViewModel
        //    {
        //        AvatarUrl = "/images/profilePicture.png",
        //        BackgroundUrl = "/images/backgroundUploadImg.png",
        //        Email = "1234567@gmail.com",
        //        DisplayName = "舒舒",
        //        PersonalIntroduction = "我有一隻貓，他叫做馬布丁!",
        //        Name = "譚舒舒",
        //        Birthday = new DateTime(1222, 2, 22),
        //        Gender = (Gender)2,
        //        EmotionalState = (RelationshipStatus)2,
        //        Phone = "0909123123",
        //        City = "Taipei City",
        //        DetailAddress = "貓貓路二段22巷2樓"

        //    };
        //}

        //0318-關於自己-測試

        public MyProfileViewModel GetMyProfileViewModel(int memberId)
        {
            if (memberId == null)
            {
                return new MyProfileViewModel();
            }

            try
            {
                var member = _memberRepo.FirstOrDefault(x => x.Id == memberId && !x.IsDelete);

                Gender gender = 0;
                RelationshipStatus relationshipStatus = 0;
                if (member == null)
                {
                    return new MyProfileViewModel();
                }

                if (member.Gender != null)
                {
                    gender = (Gender)member.Gender;
                }

                if(member.Relationship != null) 
                {
                    relationshipStatus = (RelationshipStatus)member.Relationship;
                }


                var viewModel = new MyProfileViewModel
                {
                    MemberId = member.Id,
                    AvatarUrl = member.AvatarUrl,
                    BackgroundUrl = member.CoverUrl,
                    //AvatarUrl = "/images/profilePicture.png",
                    //BackgroundUrl = "/images/cardUploadImg.png",
                    Email = member.Email,
                    DisplayName = member.DisplayName,
                    PersonalIntroduction = member.Description,
                    Name = member.Name,
                    Birthday = member.Birthday,
                    Gender = gender,
                    EmotionalState = relationshipStatus,
                    Phone = member.Phone,
                    City = member.City,
                    DetailAddress = member.Address
                };

                return viewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MyProfileViewModel();
            }
        }

        public MyAccountViewModel GetMyAccountViewModel(int memberId)
        {
            if (memberId == null)
            {
                return new MyAccountViewModel();
            }

            try
            {
                var member = _memberRepo.FirstOrDefault(x => x.Id == memberId && !x.IsDelete);


                if (member == null)
                {
                    return new MyAccountViewModel();
                }

                var viewModel = new MyAccountViewModel
                {
                    MemberId = member.Id,

                };

                return viewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MyAccountViewModel();

            }
        }


        /// <summary>
        /// 測試用假資料，後續以Ajax使用
        /// </summary>
        public List<MyTicketForAjax> GetMyTicketForAjax()
        {
            return new List<MyTicketForAjax>
            {
                new MyTicketForAjax
                {
                    EventId = 1,
                    EventName = "第一波 免費住宿抽獎活動",
                    EventEndTime = "2024-03-10T19:00",
                    TicketId = 1,
                    TicketTypeName = "一般票",
                    TicketRemainingAmount = 2,
                    TicketUnitPrice = 350,
                    ParticipantName = "譚舒舒",
                    ParticipantEmail = "123456@gmail.com",
                    ParticipantPhone = "0909090090"
                },
                new MyTicketForAjax
                {
                    EventId = 1,
                    EventName = "第一波 免費住宿抽獎活動",
                    EventEndTime = "2024-03-10T19:00",
                    TicketId = 1,
                    TicketTypeName = "一般票",
                    TicketRemainingAmount = 2,
                    TicketUnitPrice = 350,
                    ParticipantName = "譚舒舒",
                    ParticipantEmail = "123456@gmail.com",
                    ParticipantPhone = "0909090090"
                }
            };
        }

        public List<UsageRecordForAjax> GetUsageRecordForAjax()
        {
            return new List<UsageRecordForAjax>
            {

            };
        }

        public List<PurchaseRecordForAjax> GetPurchaseRecordForAjax()
        {
            return new List<PurchaseRecordForAjax>
            {

            };
        }
    }
}

