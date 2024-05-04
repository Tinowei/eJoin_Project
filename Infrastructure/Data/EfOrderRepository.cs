using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfOrderRepository : EfRepository<Order>, IOrderRepository
    {
        public EfOrderRepository(EJoinContext dbContext) : base(dbContext)
        {
        }

        public async Task<Order> CreateOrder(Order order, IEnumerable<TicketType> ticketTypes)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    await DbContext.Orders.AddAsync(order);
                    DbContext.TicketTypes.UpdateRange(ticketTypes);
                    DbContext.SaveChanges();
                    transaction.Commit();
                    return order;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
