using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class TicketType
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 所屬活動Id，關聯Events
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// 建立時間，不可變更
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 釋出總數
    /// </summary>
    public int ReleaseAmount { get; set; }

    /// <summary>
    /// 票種名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 單價
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 開始販售時間
    /// </summary>
    public DateTime StartSellTime { get; set; }

    /// <summary>
    /// 結束販售時間
    /// </summary>
    public DateTime EndSellTime { get; set; }

    /// <summary>
    /// 單次最大購買數量，可空值，代表無限制
    /// </summary>
    public int? MaxPurchase { get; set; }

    /// <summary>
    /// 庫存
    /// </summary>
    public int Stock { get; set; }

    public virtual ICollection<CartTicket> CartTickets { get; set; } = new List<CartTicket>();

    public virtual Event Event { get; set; } = null!;
}
