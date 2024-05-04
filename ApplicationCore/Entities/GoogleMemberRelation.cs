using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class GoogleMemberRelation
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 會員Id，關連Members
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// Google紀錄表Id，關聯GoogleLoginInfo
    /// </summary>
    public int GoogleLoginInfoId { get; set; }

    public virtual GoogleLoginInfo GoogleLoginInfo { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
