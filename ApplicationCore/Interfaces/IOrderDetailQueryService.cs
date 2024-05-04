using ApplicationCore.Entities;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IOrderDetailQueryService
{
    //取得自定義的購買記錄
    List<OrderDetailSummaryDTO> GetOrderSummaries(int userId,int page,int pageSize);
    
    Task<IPagedList<OrderDetailSummaryDTO>> GetOrderListByPageAsync(OrderDetailParameters orderDetailParameters);
}