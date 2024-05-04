using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class OrderDetailQueryService : EfRepository<OrderDetail>,IOrderDetailQueryService
{
    
    private readonly IRepository<Order> _orderRepo;

    private readonly IRepository<OrderTicket> _orderTicketRepo;
    
    public OrderDetailQueryService(EJoinContext dbContext,IRepository<Order> orderRepo,IRepository<OrderTicket> orderTicketRepo) : base(dbContext)
    {
        _orderRepo = orderRepo;
        _orderTicketRepo = orderTicketRepo;
    }

    /// <summary>
    /// 找到該會員的購買記錄
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public List<OrderDetailSummaryDTO> GetOrderSummaries(int userId, int page, int pageSize)
    {
        //找到該會員的Order
        //var buyerId = _orderRepo.FirstOrDefault(o => o.BuyerId == userId && o.Status == 2);

        // var orderDetailsTarget = DbContext.OrderDetails.Include()
        var orderDetailQuery = DbContext.Orders
            .Where(o => o.BuyerId == userId && o.Status == 2)
            .Include(o => o.OrderDetail)
            .ThenInclude(od => od.OrderTickets)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        //建立要傳回前端的自訂義DTO
        var result = orderDetailQuery.Select(o => new OrderDetailSummaryDTO()
        {
            OrderDetailId = o.OrderDetail.Id,
            OrderNo = o.OrderNo,
            EventName = o.OrderDetail.EventTitle,
            CreateTime = o.OrderDetail.CreateTime,
            TotalMoney = o.OrderDetail.TotalMoney,
            Tickets = o.OrderDetail.OrderTickets.Select(ot=>new CustomOrderTicketDTO()
            {
                Id = ot.Id,
                TicketTypeId = ot.TicketTypeId,
                TicketTypeName = ot.TicketTypeName,
                OrderDetailId = ot.OrderDetailId,
                UnitPrice = ot.UnitPrice,
                PurchaseQuantity = ot.PurchaseQuantity,
            }).ToList()
        }).ToList();
        
        return result;
    }

    /// <summary>
    /// 取得會員購買記錄，後端分頁功能
    /// </summary>
    /// <param name="orderDetailParameters"></param>
    /// <returns></returns>
    public async Task<IPagedList<OrderDetailSummaryDTO>> GetOrderListByPageAsync(OrderDetailParameters orderDetailParameters)
    {

        
        var pagedList = new PagedList<OrderDetailSummaryDTO>();
        var orderDetailQuery = DbContext.Orders
            .Where(o => o.BuyerId == orderDetailParameters.MemberId && o.Status == 2)
            .Include(o=>o.Event)
            .Include(o => o.OrderDetail)
            .ThenInclude(od => od.OrderTickets)
            .Select(o => new OrderDetailSummaryDTO
            {
                OrderDetailId = o.OrderDetail.Id,
                OrderNo = o.OrderNo,
                EventId = o.EventId,
                EventName = o.OrderDetail.EventTitle,
                CreateTime = o.OrderDetail.CreateTime,
                TotalMoney = o.OrderDetail.TotalMoney,
                Tickets = o.OrderDetail.OrderTickets.Select(ot => new CustomOrderTicketDTO
                {
                    Id = ot.Id,
                    TicketTypeId = ot.TicketTypeId,
                    TicketTypeName = ot.TicketTypeName,
                    OrderDetailId = ot.OrderDetailId,
                    UnitPrice = ot.UnitPrice,
                    PurchaseQuantity = ot.PurchaseQuantity,
                }).ToList()
            }).OrderByDescending(dto=>dto.CreateTime);

        await pagedList.ReadAsync(orderDetailQuery, orderDetailParameters.PageNumber, orderDetailParameters.PageSize);
        
        return pagedList;

    }
    
}

// public class OrderParameters
// {
//     public int MemberId { get; set; }
//
//     const int maxPageSize = 50;
//     public int PageNumber { get; set; } = 1;
//     private int _pageSize = 3;
//     public int PageSize
//     {
//         get
//         {
//             return _pageSize;
//         }
//         set
//         {
//             _pageSize = (value > maxPageSize) ? maxPageSize : value;
//         }
//     }
// }