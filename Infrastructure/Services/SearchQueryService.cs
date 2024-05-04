using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SearchQueryService : ISearchQueryService
    {
        public SearchQueryService() 
        {
            
        }

        public List<Event> FillterEvent(int currentPage, bool isFree, DateTime endDate, List<int> themes, List<string> cities)
        {
            var sql = """
                      SELECT 
                          
                      FROM 
                      INNER JOIN OrderDetails od ON o.Id = od.Id
                      INNER JOIN OrderTickets ot ON od.Id = ot.OrderDetailId
                      ORDER BY o.OrderNo
                      """;
            return new List<Event>();
        }
    }
}
