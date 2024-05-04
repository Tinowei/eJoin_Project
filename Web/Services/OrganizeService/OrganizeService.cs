using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using Web.Services.AccountService;
using Web.ViewModels.OrganizeViewModel;

namespace Web.Services.OrganizeService
{
    public class OrganizeService
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<EventTheme> _eventThemeRepo;
        private readonly IRepository<TicketType> _ticketRepo;
        private readonly IEventRepository _eventRepoForPage;
        private readonly IRepository<Like> _likeRepo;
        private readonly IRepository<ReleaseTicket> _releaseTicketRepo;

        public OrganizeService(IEventRepository eventRepoForPage,
                               IRepository<Event> eventRepo,
                               IRepository<EventTheme> eventThemeRepo,
                               IRepository<TicketType> ticketRepo,
                               IRepository<Like> likeRepo,
                               IRepository<ReleaseTicket> releaseTicketRepo)
        {
            _eventRepoForPage = eventRepoForPage;
            _eventRepo = eventRepo;
            _eventThemeRepo = eventThemeRepo;
            _ticketRepo = ticketRepo;
            _likeRepo = likeRepo;
            _releaseTicketRepo = releaseTicketRepo;
        }

        public async Task<IndexViewModel> GetIndexViewModel(int userId, int currentPage)
        {
            var eventParameters = new EventParameters
            {
                MemberId = userId,
                PageNumber = currentPage,
                PageSize = 5
            };

            try
            {
                var pagedEvents = await _eventRepoForPage.GetEventListByPageAsync(eventParameters);

                var eventsForCurrentPage = pagedEvents.Items
                                            .Select(item => new EventItem
                                            {
                                                Id = item.Id,
                                                Name = item.Title,
                                                PictureUrl = item.CoverUrl,
                                                CreatedDate = item.CreateTime,
                                                StartTime = item.StartTime,
                                                EndTime = item.EndTime,
                                                TotalTicketsCount = item.TicketTypes.Sum(t => t.ReleaseAmount),
                                                SoldTicketsCount = item.TicketTypes.Sum(t => t.ReleaseAmount - t.Stock),
                                                Status = GetStatus(item.Status),
                                            }).ToList();

                var data = new IndexViewModel
                {
                    EventList = eventsForCurrentPage,
                    TotalPages = pagedEvents.TotalPages,
                    CurrentPage = pagedEvents.CurrentPage
                };


                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return new IndexViewModel { EventList = new List<EventItem> { } };
            }
        }

        public async Task<OverviewViewModel> GetOverView(int eventId, int userId)
        {
            var result = new OverviewViewModel();

            try
            {
                //比對網址evendId，是否等於資料庫Event資料表的Id，並撈出這一筆資料
                var eventData = await _eventRepo.GetByIdAsync(eventId) ?? throw new Exception("找不到活動");
                if (eventData.MemberId != userId)
                {
                    throw new UnauthorizedAccessException("用戶沒有權限瀏覽此活動");
                }

                var ticketData = await _ticketRepo.ListAsync(e => e.EventId == eventId);
                var checkInData = await _releaseTicketRepo.ListAsync(t => t.EventId == eventId);

                //資料庫like資料表的EventId，是否等於資料庫Event資料表的Id，撈出後計算加總筆數
                var likeCountRepo = await _likeRepo.ListAsync(l => l.EventId == eventId);
                var likeCount = likeCountRepo.Count;

                result.Id=eventData.Id;
                result.Title=eventData.Title;
                result.PictureUrl = eventData.CoverUrl;
                result.CreatedDate = eventData.CreateTime;
                result.StartTime = eventData.StartTime;
                result.EndTime = eventData.EndTime;
                result.FavoriteNumber = likeCount;
                result.StatusInt = eventData.Status;
                result.Status = GetStatus(eventData.Status);
                result.ApplyNumber = ticketData.Sum(t => t.ReleaseAmount - t.Stock);
                result.RemainingNumber = ticketData.Sum(t => t.Stock);
                result.Tickets = ticketData.Select(t => new Ticket
                {
                    Id = t.Id,
                    Name = t.Name,
                    Amount = t.ReleaseAmount,
                    Stock = t.Stock,
                    Price = t.UnitPrice,
                    StartSellDate = t.StartSellTime,
                    EndSellDate = t.EndSellTime,
                    MaxPurchase = t.MaxPurchase,
                }).ToList();
                result.CheckInStatus = new TicketCheck
                {
                    CheckedInTickets = checkInData.Where(t => t.Status == 1).Count(),
                    NotCheckedInTickets = checkInData.Count - checkInData.Where(t => t.Status == 1).Count(),
                };

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<EditViewModel> GetEditViewModel(int eventId, int userId)
        {
            try
            {
                var eventData = await _eventRepo.GetByIdAsync(eventId) ?? throw new Exception("找不到活動");
                                     
                if (eventData.MemberId != userId)
                {
                    throw new UnauthorizedAccessException("用戶沒有權限編輯此活動");
                }

                var themeData = await _eventRepoForPage.GetAllThemes().ToListAsync();
                var eventTheme = await _eventThemeRepo.ListAsync(e => e.EventId == eventId);
                var ticketData = await _ticketRepo.ListAsync(e => e.EventId == eventId);

                EditViewModel editModel = new EditViewModel()
                {
                    Id = eventData.Id,
                    PictureUrl = eventData.CoverUrl,
                    Name = eventData.Title,
                    StartTime = eventData.StartTime,
                    EndTime = eventData.EndTime,
                    City = eventData.City,
                    Address = eventData.Address,
                    AddressDetail = eventData.AddressDetail,
                    Summary = eventData.Summary,
                    Intro = eventData.Introduction,
                    SelectedTheme = eventTheme.Select(et => et.ThemeId).ToList(),
                    Status = eventData.Status,
                    Latitude = eventData.Latitude,
                    Longitude = eventData.Longitude,
                    Theme = themeData.Select(t => new Themes 
                    { 
                        Id = t.Id, Image = t.IconUrl, Name = t.ThemeName}).ToList(),
                    Tickets = ticketData.Select(t => new Ticket
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Amount = t.ReleaseAmount,
                        Stock = t.Stock,
                        Price = t.UnitPrice,
                        StartSellDate = t.StartSellTime,
                        EndSellDate = t.EndSellTime,
                        MaxPurchase = t.MaxPurchase,
                    }).ToList(),
                };

                return editModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static string GetStatus(int statusNumber)
        {
            string status = string.Empty;
            switch (statusNumber)
            {
                case 1:
                    status = "草稿";
                    break;
                case 2:
                    status = "上架";
                    break;
                case 3:
                    status = "結束";
                    break;
                case 4:
                    status = "下架";
                    break;
                default:
                    throw new Exception("未知的狀態");
            }
            return status;
        }
    }
}
