using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Coravel.Invocable;

namespace Admin.Services.SchedulerService
{
    public class OrderScheldulerService : IInvocable
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderScheldulerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Invoke()
        {
            RemoveExpiredOrders();
            return Task.CompletedTask;
        }

        public void RemoveExpiredOrders()
        {
            var orderRepository = _unitOfWork.GetRepository<Order>();
            var orderDetailRepository = _unitOfWork.GetRepository<OrderDetail>();
            var orderTicketRepository = _unitOfWork.GetRepository<OrderTicket>();
            var ticketTypeRepository = _unitOfWork.GetRepository<TicketType>();

            var expiredOrders = orderRepository.List(c => c.Status == 1 && c.ExpiredTime <= CustomUtcNow.CurrentTime);

            if (!expiredOrders.Any())
            {
                return;
            }

            try
            {
                _unitOfWork.Begin();

                var expiredOrderIds = expiredOrders.Select(c => c.Id).ToList();
                var expiredOrderDetails = orderDetailRepository.List(od => expiredOrderIds.Contains(od.Id));

                var expiredOrderDetailIds = expiredOrderDetails.Select(od => od.Id).ToList();
                var expiredOrderTickets = orderTicketRepository.List(od => expiredOrderDetailIds.Contains(od.OrderDetailId));

                var ticketTypeIds = expiredOrderTickets.Select(t => t.TicketTypeId).Distinct();

                var ticketTypes = ticketTypeRepository.List(tt => ticketTypeIds.Contains(tt.Id));

                foreach (var ticketType in ticketTypes)
                {
                    var increment = expiredOrderTickets.Count(t => t.TicketTypeId == ticketType.Id);
                    ticketType.Stock += increment;
                }
                ticketTypeRepository.UpdateRange(ticketTypes);

                orderTicketRepository.DeleteRange(expiredOrderTickets);
                orderDetailRepository.DeleteRange(expiredOrderDetails);
                orderRepository.DeleteRange(expiredOrders);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                Console.WriteLine(ex);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
