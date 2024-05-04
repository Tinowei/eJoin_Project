using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Event
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 活動標題
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 活動封面圖(單張)
    /// </summary>
    public string CoverUrl { get; set; } = null!;

    /// <summary>
    /// 活動建立者，關聯Members
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// 活動開始時間
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 活動結束時間
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 活動地點的城市
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    /// 活動地點的詳細地址
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// 活動地點的補充說明，可空值
    /// </summary>
    public string? AddressDetail { get; set; }

    /// <summary>
    /// 活動摘要
    /// </summary>
    public string Summary { get; set; } = null!;

    /// <summary>
    /// 活動簡介
    /// </summary>
    public string Introduction { get; set; } = null!;

    /// <summary>
    /// 地址對應的緯度
    /// </summary>
    public string Latitude { get; set; } = null!;

    /// <summary>
    /// 地址對應的經度
    /// </summary>
    public string Longitude { get; set; } = null!;

    /// <summary>
    /// 活動狀態：1.草稿，2.上架，3.結束，4.下架
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 活動建立時間，不可變更
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 最後編輯時間，可空值
    /// </summary>
    public DateTime? LastEditTime { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<EventTheme> EventThemes { get; set; } = new List<EventTheme>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ReleaseTicket> ReleaseTickets { get; set; } = new List<ReleaseTicket>();

    public virtual ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();
}
