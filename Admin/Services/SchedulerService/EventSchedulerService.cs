using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Coravel.Invocable;

namespace Admin.Services.SchedulerService
{
    public class EventSchedulerService : IInvocable
    {
        private readonly IRepository<Event> _eventsRepo;

        public EventSchedulerService(IRepository<Event> eventsRepo)
        {
            _eventsRepo = eventsRepo;
        }

        public Task Invoke()
        {
            ChangeEventStatus();
            return Task.CompletedTask;
        }

        public void ChangeEventStatus()
        {
            var activeEvents = _eventsRepo.List(e => e.Status == 2 && e.EndTime <= CustomUtcNow.CurrentTime);

            if (!activeEvents.Any())
            {
                return;
            }

            try
            {
                foreach (var eventItem in activeEvents)
                {
                    eventItem.Status = 3;
                }
                _eventsRepo.UpdateRange(activeEvents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
