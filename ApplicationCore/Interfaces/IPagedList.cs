using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IPagedList<T>
    {
        int CurrentPage { get; set; }
        int TotalPages { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
        List<T> Items { get; set; }

        Task ReadAsync(IQueryable<T> source, int pageNumber, int pageSize);
    }
}
