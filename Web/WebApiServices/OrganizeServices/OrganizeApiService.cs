using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Microsoft.Extensions.Logging;
using Web.ViewModels.OrganizeViewModel;
using Web.WebApi.Organize;

namespace Web.WebApiServices.OrganizeServices
{
    public class OrganizeApiService
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<EventTheme> _themeRepo;
        private readonly IRepository<TicketType> _ticketTypeRepo;
        public OrganizeApiService(IRepository<Event> eventRepo, IRepository<EventTheme> themeRepo, IRepository<TicketType> ticketTypeRepo)
        {
            _eventRepo = eventRepo;
            _themeRepo = themeRepo;
            _ticketTypeRepo = ticketTypeRepo;
        }

        public async Task EditEventTheme(int userId, EventThemeDTO eventQuery)
        {

            var eventData = await _eventRepo.GetByIdAsync(eventQuery.EventId) ?? throw new Exception("找不到活動");
            if (eventData.MemberId != userId)
            {
                throw new UnauthorizedAccessException("用戶沒有權限編輯此活動");
            }
            
            var currentThemes = await _themeRepo.ListAsync(e => e.EventId == eventQuery.EventId);
            var currentThemeIds = currentThemes.Select(ct => ct.ThemeId).ToList();

            // 檢查新的主題ID是否和現存的一致
            if (!currentThemeIds.SequenceEqual(eventQuery.SelectedThemes.OrderBy(id => id)))
            {
                // 如果不一致，刪除所有現存主題
                foreach (var theme in currentThemes)
                {
                    await _themeRepo.DeleteAsync(theme);
                }

                // 添加新的主題
                foreach (var themeIdToAdd in eventQuery.SelectedThemes)
                {
                    await _themeRepo.AddAsync(new EventTheme
                    {
                        EventId = eventQuery.EventId,
                        ThemeId = themeIdToAdd,
                    });
                }

                eventData.LastEditTime = CustomUtcNow.CurrentTime;
                await _eventRepo.UpdateAsync(eventData);
            }
        }

        public async Task EditBasicInfo(int userId, EditBasicInfoDTO eventQuery)
        {
            try
            {
                var eventData = await _eventRepo.GetByIdAsync(eventQuery.EventId) ?? throw new Exception("找不到活動");
                if (eventData.MemberId != userId)
                {
                    throw new UnauthorizedAccessException("用戶沒有權限編輯此活動");
                }

                eventData.CoverUrl = eventQuery.PictureUrl;
                eventData.Title = eventQuery.Name;
                eventData.StartTime = eventQuery.StartTime;
                eventData.EndTime = eventQuery.EndTime;
                eventData.City = eventQuery.City;
                eventData.Address = eventQuery.Address;
                eventData.AddressDetail = eventQuery.AddressDetail;
                eventData.Latitude = eventQuery.Latitude;
                eventData.Longitude = eventQuery.Longitude;
                eventData.LastEditTime = CustomUtcNow.CurrentTime;
                await _eventRepo.UpdateAsync(eventData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task EditDescInfo(int userId, EditDescInfoDTO eventQuery)
        {
            try
            {
                var eventData = await _eventRepo.GetByIdAsync(eventQuery.EventId) ?? throw new Exception("找不到活動");
                if (eventData.MemberId != userId)
                {
                    throw new UnauthorizedAccessException("用戶沒有權限編輯此活動");
                }

                eventData.Summary = eventQuery.Summary;
                eventData.Introduction = eventQuery.Intro;
                eventData.LastEditTime = CustomUtcNow.CurrentTime;
                await _eventRepo.UpdateAsync(eventData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task EditTicket(int userId, EditTicketDTO eventQuery)
        {
            try
            {
                var eventData = await _eventRepo.GetByIdAsync(eventQuery.EventId) ?? throw new Exception("找不到活動");

                if (eventData.MemberId != userId)
                {
                    throw new UnauthorizedAccessException("用戶沒有權限編輯此活動");
                }

                foreach(var ticket in eventQuery.TicketsData)
                {
                    var ticketData = await _ticketTypeRepo.GetByIdAsync(ticket.Id);
                    if (ticketData == null && ticket.IsDeleted == 0) 
                    {
                        await _ticketTypeRepo.AddAsync(new TicketType
                        {
                            EventId = eventQuery.EventId,
                            CreateTime = DateTime.Now,
                            ReleaseAmount = ticket.Amount,
                            Name = ticket.Name,
                            UnitPrice = ticket.Price,
                            StartSellTime = ticket.StartSellDate,
                            EndSellTime = ticket.EndSellDate,
                            MaxPurchase = ticket.MaxPurchase,
                            Stock = ticket.Amount
                        });
                    }
                    else if (ticketData != null && ticket.IsDeleted == 1)
                    {
                        await _ticketTypeRepo.DeleteAsync(ticketData);
                    }
                    else if (ticketData != null && ticket.IsDeleted == 0)
                    {
                        ticketData.Stock += ticket.Amount - ticketData.ReleaseAmount;
                        ticketData.ReleaseAmount = ticket.Amount;
                        ticketData.Name = ticket.Name;
                        ticketData.UnitPrice = ticket.Price;
                        ticketData.StartSellTime = ticket.StartSellDate;
                        ticketData.EndSellTime = ticket.EndSellDate;
                        ticketData.MaxPurchase = ticket.MaxPurchase;
                        
                        await _ticketTypeRepo.UpdateAsync(ticketData);
                    }
                }

                eventData.LastEditTime = CustomUtcNow.CurrentTime;
                await _eventRepo.UpdateAsync(eventData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task EditStatus(int userId, EditStatusDTO eventQuery)
        {
            try
            {
                var eventData = await _eventRepo.GetByIdAsync(eventQuery.EventId) ?? throw new Exception("找不到活動");

                if (eventData.MemberId != userId)
                {
                    throw new UnauthorizedAccessException("用戶沒有權限編輯此活動");
                }

                if (eventData.Status == 1)
                {
                    eventData.Status = 2;
                    eventData.LastEditTime = CustomUtcNow.CurrentTime;
                }
                else if (eventData.Status == 2)
                {
                    eventData.Status = 4;
                    eventData.LastEditTime = CustomUtcNow.CurrentTime;
                }
                
                await _eventRepo.UpdateAsync(eventData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
