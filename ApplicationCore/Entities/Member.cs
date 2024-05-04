using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Member
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 帳號用姓名
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 驗證與登入用信箱，不可變更
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// 顯示名稱，可空值
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 封面圖片網址，可空值
    /// </summary>
    public string? CoverUrl { get; set; }

    /// <summary>
    /// 大頭照網址，可空值
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    public string Phone { get; set; } = null!;

    /// <summary>
    /// 個人簡介，可空值
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 生日，可空值
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 性別，可空值
    /// </summary>
    public byte? Gender { get; set; }

    /// <summary>
    /// 感情狀態，可空值
    /// </summary>
    public byte? Relationship { get; set; }

    /// <summary>
    /// 城市，可空值
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 詳細地址，可空值
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 雜湊後的密碼
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 註冊時間，不可變更
    /// </summary>
    public DateTime RegisterTime { get; set; }

    /// <summary>
    /// 最後編輯時間，可空值
    /// </summary>
    public DateTime? LastEditTime { get; set; }

    /// <summary>
    /// 是否刪除
    /// </summary>
    public bool IsDelete { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Follow> FollowBeingFolloweds { get; set; } = new List<Follow>();

    public virtual ICollection<Follow> FollowFollowers { get; set; } = new List<Follow>();

    public virtual ICollection<GoogleMemberRelation> GoogleMemberRelations { get; set; } = new List<GoogleMemberRelation>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ReleaseTicket> ReleaseTickets { get; set; } = new List<ReleaseTicket>();
}
