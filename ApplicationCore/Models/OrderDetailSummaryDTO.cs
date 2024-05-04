using ApplicationCore.Entities;

namespace ApplicationCore.Models;

public class OrderDetailSummaryDTO
{
    //訂單的id
    public int OrderDetailId { get; set; }
    //訂單編號
    public string OrderNo { get; set; }
    
    public int EventId { get; set; }
    //活動名稱
    public string EventName { get; set; }
    /// <summary>
    /// 訂單建立時間
    /// </summary>
    public DateTime CreateTime { get; set; }
    
    /// <summary>
    /// 總價格
    /// </summary>
    public decimal TotalMoney { get; set; }
    
    /// <summary>
    /// 活動標題
    /// </summary>
    public string EventTitle { get; set; } = null!;
    /// <summary>
    /// 該購買明細的所有票券種類
    /// </summary>
    public List<CustomOrderTicketDTO> Tickets { get; set; }
    
}

public class CustomOrderTicketDTO
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 票種名稱
    /// </summary>
    public string TicketTypeName { get; set; } = null!;

    /// <summary>
    /// 單價
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 購入數量
    /// </summary>
    public int PurchaseQuantity { get; set; }

    /// <summary>
    /// 所屬的明細Id，關聯OrderDetials
    /// </summary>
    public int OrderDetailId { get; set; }

    /// <summary>
    /// 原票種的Id
    /// </summary>
    public int? TicketTypeId { get; set; }
}