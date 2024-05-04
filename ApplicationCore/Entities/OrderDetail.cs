using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class OrderDetail
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 訂單建立時間
    /// </summary>
    public DateTime CreateTime { get; set; }

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
    public string ParticipantPhone { get; set; } = null!;

    /// <summary>
    /// 總價格
    /// </summary>
    public decimal TotalMoney { get; set; }

    /// <summary>
    /// 買家名稱
    /// </summary>
    public string BuyerName { get; set; } = null!;

    /// <summary>
    /// 活動標題
    /// </summary>
    public string EventTitle { get; set; } = null!;

    /// <summary>
    /// 活動資訊來源
    /// </summary>
    public string LearnedFrom { get; set; } = null!;

    public virtual Order IdNavigation { get; set; } = null!;

    public virtual ICollection<OrderTicket> OrderTickets { get; set; } = new List<OrderTicket>();
}
