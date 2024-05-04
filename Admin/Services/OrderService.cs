using Admin.Helpers;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Admin.Services;

public class OrderService
{
    private readonly ConnectDBHelper _dbHelper;
    public OrderService(IConfiguration configuration)
    {
        _dbHelper = ConnectDBHelper.GetInstance(configuration);
    }

    public List<CustomOrder> GetOrders()
    {
        List<CustomOrder> orders = null;
        using (var connection = _dbHelper.OpenConnection())
        {
            var sql = """
                      SELECT 
                          o.Id,
                          o.OrderNo AS OrderNo,
                          od.Id AS OrderDetailId,
                          od.EventTitle AS EventName,
                          od.CreateTime AS CreateTime,
                          o.OrderNo AS OrderNo,
                          od.Id AS OrderDetailId,
                          od.EventTitle AS EventName,
                          od.CreateTime AS CreateTime,
                          od.ParticipantName AS ParticipantName,
                          od.ParticipantEmail AS ParticipantEmail,
                          od.ParticipantPhone AS ParticipantPhone,
                          od.TotalMoney AS TotalMoney,
                          ot.TicketTypeId AS TicketTypeId,
                          ot.TicketTypeName AS TicketTypeName,
                          ot.OrderDetailId AS OrderDetailId,
                          ot.UnitPrice AS UnitPrice,
                          ot.PurchaseQuantity AS PurchaseQuantity
                      FROM Orders o
                      INNER JOIN OrderDetails od ON o.Id = od.Id
                      INNER JOIN OrderTickets ot ON od.Id = ot.OrderDetailId
                      Where o.Status = 2
                      ORDER BY od.CreateTime DESC 
                      """;
            
            var orderDictionary = new Dictionary<int, CustomOrder>();

            connection.Query<CustomOrder, Ticket, CustomOrder>(sql, (order, ticket) =>
            {
                CustomOrder orderEntry;
                if (!orderDictionary.TryGetValue(order.OrderDetailId, out orderEntry)) //確保訂單不會重複只會有一筆
                {
                    orderEntry = order;
                    orderEntry.Tickets = new List<Ticket>(); //create empty List<Ticket> on each order
                    orderDictionary.Add(orderEntry.OrderDetailId, orderEntry);//key: orderDetailId , value : orderEntry
                }

                orderEntry.Tickets.Add(ticket); //將屬於同一筆訂單編號的票券明細加入List<Ticket> (階層式資料結構)
                return orderEntry;
            }, splitOn: "TicketTypeId").Distinct().ToList();

            orders = orderDictionary.Values.ToList();
        }
        //foreach (var order in orders)
        //{
        //    Console.WriteLine($"OrderNo: {order.OrderNo}");
        //    Console.WriteLine($"OrderDetailId: {order.OrderDetailId}");
        //    Console.WriteLine($"EventName: {order.EventName}");
        //    Console.WriteLine($"ParticipantName: {order.ParticipantName}"); 
        //    Console.WriteLine($"ParticipantPhone: {order.ParticipantPhone}"); 
        //    Console.WriteLine($"ParticipantEmail: {order.ParticipantEmail}"); 
        //    Console.WriteLine($"CreateTime: {order.CreateTime}");
        //    Console.WriteLine($"TotalMoney: {order.TotalMoney}");
        //    Console.WriteLine("Tickets:");
        //    foreach (var ticket in order.Tickets)
        //    {
        //        Console.WriteLine($"\tTicketId: {ticket.OrderTicketId}");
        //        Console.WriteLine($"\tTicketTypeId: {ticket.TicketTypeId}");
        //        Console.WriteLine($"\tTicketTypeName: {ticket.TicketTypeName}");
        //        Console.WriteLine($"\tOrderDetailId: {ticket.OrderDetailId}");
        //        Console.WriteLine($"\tUnitPrice: {ticket.UnitPrice}");
        //        Console.WriteLine($"\tPurchaseQuantity: {ticket.PurchaseQuantity}");
        //    }
        //    Console.WriteLine("--------------------------------------------------");
        //}
    
        return orders;
    }
}

public class CustomOrder
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; }
    public int OrderDetailId { get; set; }
    public string EventName { get; set; }
    
    public string ParticipantName { get; set; }
    
    public string ParticipantEmail { get; set; }
    
    public string ParticipantPhone { get; set; }
    public DateTime CreateTime { get; set; }
    public decimal TotalMoney { get; set; }
    public List<Ticket> Tickets { get; set; }
}

public class Ticket
{
    public int OrderTicketId { get; set; }
    public int TicketTypeId { get; set; }
    public string TicketTypeName { get; set; }
    public int OrderDetailId { get; set; }
    public decimal UnitPrice { get; set; }
    public int PurchaseQuantity { get; set; }
}