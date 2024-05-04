using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class CartTicket
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 票券所屬票種，關聯TicketTypes
    /// </summary>
    public int TicketTypeId { get; set; }

    /// <summary>
    /// 票券所屬購物車，關聯Carts
    /// </summary>
    public int CartId { get; set; }

    /// <summary>
    /// 選取的票券數量
    /// </summary>
    public int Quantity { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual TicketType TicketType { get; set; } = null!;
}
