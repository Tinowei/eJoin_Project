using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Theme
{
    /// <summary>
    /// 自動生成Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 主題名稱
    /// </summary>
    public string ThemeName { get; set; } = null!;

    /// <summary>
    /// 主題圖片位址，應在本地端
    /// </summary>
    public string IconUrl { get; set; } = null!;

    public virtual ICollection<EventTheme> EventThemes { get; set; } = new List<EventTheme>();
}
