using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class EventTheme
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 活動Id，關聯Events
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// 主題Id，關聯Themes
    /// </summary>
    public int ThemeId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Theme Theme { get; set; } = null!;
}
