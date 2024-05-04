using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Cart
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 購物車所屬會員，關聯Members
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// 購物車建立時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 購物車到期時間
    /// </summary>
    public DateTime ExpiredTime { get; set; }

    /// <summary>
    /// 購物車對應活動，關聯Events
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// 參加人姓名
    /// </summary>
    public string? ParticipantName { get; set; }

    /// <summary>
    /// 參加人Email
    /// </summary>
    public string? ParticipantEmail { get; set; }

    /// <summary>
    /// 參加人電話
    /// </summary>
    public string? ParticipantPhone { get; set; }

    /// <summary>
    /// 活動資訊來源
    /// </summary>
    public string? LearnedFrom { get; set; }

    public virtual ICollection<CartTicket> CartTickets { get; set; } = new List<CartTicket>();

    public virtual Event Event { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
