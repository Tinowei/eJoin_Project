using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IPagedList<Event>> GetEventListByPageAsync(EventParameters eventParameters);

        IQueryable<Event> FindAll();

        IQueryable<Theme> GetAllThemes();
        IEnumerable<Event> GetSearchResult(SearchOptions options);
    }
}
