using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Follow
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 跟隨者，關聯Members
    /// </summary>
    public int FollowerId { get; set; }

    /// <summary>
    /// 被跟隨者，關聯Members
    /// </summary>
    public int BeingFollowedId { get; set; }

    public virtual Member BeingFollowed { get; set; } = null!;

    public virtual Member Follower { get; set; } = null!;
}
