using ApplicationCore.Entities;
using Web.ViewModels.RegisterViewModel;

namespace Web.Services.RegisterService
{
    public interface IRegisterService
    {
        FillFormViewModel GetFillFormViewModel(int EventId);
        PaymentViewModel GetPaymentViewModel(int EventId);
        SelectTicketViewModel GetSelectTicketViewModel(int EventId);
        int GetVaildCart(int memberId);
        int SaveFillFormInput(Cart cart);
        DirectToEcpayViewModel GetDirectToECPayViewModel(int orderId);
        int GetCartIdByOrderId(int orderId);
        void CompleteOrder(string merchantTradeNo);
    }
}