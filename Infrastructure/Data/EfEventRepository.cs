using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfEventRepository : EfRepository<Event>, IEventRepository
    {
        public EfEventRepository(EJoinContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Event> FindAll()
        {
            return DbContext.Set<Event>();
        }

        public IQueryable<Theme> GetAllThemes()
        {
            return DbContext.Set<Theme>();
        }

        public async Task<IPagedList<Event>> GetEventListByPageAsync(EventParameters eventParameters)
        {
            var pagedList = new PagedList<Event>();
            await pagedList.ReadAsync(
                FindAll().Include(e => e.TicketTypes).Where(e => e.MemberId == eventParameters.MemberId).OrderByDescending(e => e.CreateTime).AsNoTracking(),
                eventParameters.PageNumber,
                eventParameters.PageSize
            );

            return pagedList;
        }

        public IEnumerable<Event> GetSearchResult(SearchOptions options)
        {
            var e = FindAll().Include(e => e.TicketTypes).Include(e => e.EventThemes).Include(e => e.Likes)
                .Where(e => options.Keyword == null || e.Title.Contains(options.Keyword))
                .Where(e => e.Status == 2)
                .Where(e => options.SelectedPrice == null ||
                    (options.SelectedPrice == "free" && e.TicketTypes.Any(tt => tt.UnitPrice == 0)) ||
                    (options.SelectedPrice != "free" && !e.TicketTypes.Any(tt => tt.UnitPrice == 0)))
                .Where(e => options.SelectedPlaces == null || options.SelectedPlaces.Count == 0 || options.SelectedPlaces.Contains(e.City))
                .Where(e => options.SelectedThemes == null || options.SelectedThemes.Count == 0 || e.EventThemes.Where(et => et.EventId == e.Id).Any(et => options.SelectedThemes.Contains(et.ThemeId)))
                .ToList();

            return OrderBy(e, options.SelectedOrderBy).Where(e =>
            {
                switch (options.SelectedTime)
                {
                    case null:
                        return true;
                    case "today":
                        return e.StartTime < DateTime.Now.AddDays(1);
                    case "nextday":
                        return e.StartTime < DateTime.Now.AddDays(2);
                    case "thisweek":
                        return e.StartTime < DateTime.Now.AddDays(8);
                    case "thisweekend":
                        return e.StartTime < DateTime.Now.AddDays(8) && (e.StartTime.DayOfWeek == DayOfWeek.Saturday || e.StartTime.DayOfWeek == DayOfWeek.Sunday);
                    case "nextweek":
                        return e.StartTime < DateTime.Now.AddDays(15);
                    case "nextweekend":
                        return e.StartTime < DateTime.Now.AddDays(15) && (e.StartTime.DayOfWeek == DayOfWeek.Saturday || e.StartTime.DayOfWeek == DayOfWeek.Sunday);
                    default:
                         var t = options.SelectedTime.Split("-");
                        var startTime = DateTime.Parse(t[0]);
                        var endTime = DateTime.Parse(t[1]);
                        return e.StartTime > startTime && e.StartTime < endTime.AddDays(1);
                }
            }).Skip(12 * (options.CurrentPage - 1)).Take(12);

                
            
        }

        private IEnumerable<Event> OrderBy(IEnumerable<Event> events, string orderBy)
        {
            if (orderBy == "popular")
            {
                return events.OrderByDescending(e => e.Likes.Count);
            }
            else if(orderBy == "comingSoon")
            {
                return events.OrderBy(e => e.StartTime);
            }
            else
            {
                return events.OrderByDescending(e => e.CreateTime);
            }
        }
    }
    
}
