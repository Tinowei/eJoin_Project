using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Like
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 會員，關聯Members
    /// </summary>
    public int MemberId { get; set; }

    /// <summary>
    /// 被喜歡的活動，關聯Events
    /// </summary>
    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
