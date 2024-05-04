using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.EventService;

public interface IEventService
{
    List<Event> GetEvents();
    Task<int> UpdateEventAsync(int id, string status, string? title);
}