using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class OrderTicket
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

    public virtual OrderDetail OrderDetail { get; set; } = null!;
}
