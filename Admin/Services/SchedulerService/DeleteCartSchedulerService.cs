using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Coravel.Invocable;

namespace Admin.Services.SchedulerService
{
    public class DeleteCartSchedulerService : IInvocable
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCartSchedulerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Invoke()
        {
            RemoveExpireCarts();
            return Task.CompletedTask;
        }

        public void RemoveExpireCarts()
        {
            var expiredCarts = _unitOfWork.GetRepository<Cart>().List(c => c.ExpiredTime <= CustomUtcNow.CurrentTime);

            if (!expiredCarts.Any())
            {
                return;
            }

            try
            {
                _unitOfWork.Begin();
                var expiredCartIds = expiredCarts.Select(c => c.Id).ToList();

                var expiredCartTickets = _unitOfWork.GetRepository<CartTicket>().List(ct => expiredCartIds.Contains(ct.CartId));

                _unitOfWork.GetRepository<CartTicket>().DeleteRange(expiredCartTickets);
                _unitOfWork.GetRepository<Cart>().DeleteRange(expiredCarts);

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
