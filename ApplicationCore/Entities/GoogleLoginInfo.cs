using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class GoogleLoginInfo
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Google回傳Name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Google回傳Gmail
    /// </summary>
    public string Gamil { get; set; } = null!;

    /// <summary>
    /// Google回傳NameIdentifier欄位
    /// </summary>
    public string NameIdentifier { get; set; } = null!;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    public virtual ICollection<GoogleMemberRelation> GoogleMemberRelations { get; set; } = new List<GoogleMemberRelation>();
}
