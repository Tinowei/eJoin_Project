using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Order
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 訂單編號
    /// </summary>
    public string OrderNo { get; set; } = null!;

    /// <summary>
    /// 買家Id，關聯Members
    /// </summary>
    public int BuyerId { get; set; }

    /// <summary>
    /// 活動Id，關聯Events
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// 訂單狀態，1為待付款，2為已付款
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 待付款的到期時間
    /// </summary>
    public DateTime ExpiredTime { get; set; }

    public virtual Member Buyer { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual OrderDetail? OrderDetail { get; set; }

    public virtual ICollection<ReleaseTicket> ReleaseTickets { get; set; } = new List<ReleaseTicket>();
}
