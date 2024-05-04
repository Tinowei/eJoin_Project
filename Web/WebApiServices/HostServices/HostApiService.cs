using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Web.WebApi.Host;

namespace Web.WebApiServices.HostServices
{
    public class HostApiService
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<EventTheme> _themeRepo;
        private readonly IRepository<TicketType> _ticketTypeRepo;

        public HostApiService(IRepository<Event> eventRepo, 
                              IRepository<EventTheme> themeRepo, 
                              IRepository<TicketType> ticketTypeRepo)
        {
            _eventRepo = eventRepo;
            _themeRepo = themeRepo;
            _ticketTypeRepo = ticketTypeRepo;
        }

        public async Task CreateEvent(int userId, CreateEventDTO eventQuery)
        {
            try
            {
                var newEvent = new Event
                {
                    Title = eventQuery.Name,
                    CoverUrl = eventQuery.PictureUrl,
                    StartTime = eventQuery.StartTime,
                    EndTime = eventQuery.EndTime,
                    City = eventQuery.City,
                    Address = eventQuery.Address,
                    AddressDetail = eventQuery.AddressDetail,
                    Summary = eventQuery.Summary,
                    Introduction = eventQuery.Intro,
                    Latitude = eventQuery.Latitude,
                    Longitude = eventQuery.Longitude,
                    MemberId = userId,
                    Status = 1,
                    CreateTime = DateTime.Now
                };

                await _eventRepo.AddAsync(newEvent);

                var eventThemes = eventQuery.SelectedThemes.Select(themeId => new EventTheme
                {
                    EventId = newEvent.Id,
                    ThemeId = themeId,
                }).ToList();

                await _themeRepo.AddRangeAsync(eventThemes);

                if (eventQuery.TicketsData.Count != 0)
                {
                    var ticketTypes = eventQuery.TicketsData
                        .Where(ticket => ticket.IsDeleted == 0)
                        .Select(ticket => new TicketType
                        {
                            EventId = newEvent.Id,
                            CreateTime = DateTime.Now,
                            ReleaseAmount = ticket.Amount,
                            Name = ticket.Name,
                            UnitPrice = ticket.Price,
                            StartSellTime = ticket.StartSellDate,
                            EndSellTime = ticket.EndSellDate,
                            MaxPurchase = ticket.MaxPurchase,
                            Stock = ticket.Amount
                        }).ToList();

                    await _ticketTypeRepo.AddRangeAsync(ticketTypes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}
