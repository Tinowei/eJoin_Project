using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class ReleaseTicket
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 票券所屬種類，關聯TicketTypes
    /// </summary>
    public int TicketTypeId { get; set; }

    /// <summary>
    /// 票券所屬活動，關聯Events
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// 票券持有者，關聯Members
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// 票券編號
    /// </summary>
    public string ReleaseTicketNumber { get; set; } = null!;

    /// <summary>
    /// 票券所屬訂單，關聯Orders
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// 參加人姓名
    /// </summary>
    public string ParticipantName { get; set; } = null!;

    /// <summary>
    /// 參加人Email
    /// </summary>
    public string ParticipantEmail { get; set; } = null!;

    /// <summary>
    /// 參加人電話
    /// </summary>
    public string ParticipanPhone { get; set; } = null!;

    /// <summary>
    /// 票券異動狀態，
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 到期時間
    /// </summary>
    public DateTime ExpireTime { get; set; }

    /// <summary>
    /// 異動時間，可空值
    /// </summary>
    public DateTime? ChangedTime { get; set; }

    /// <summary>
    /// 核銷人員
    /// </summary>
    public string? Staff { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
