using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> CreateOrder(Order order, IEnumerable<TicketType> ticketTypes);
    }
}
