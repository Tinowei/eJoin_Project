namespace Web.ViewModels.OrganizeViewModel
{
    public class OverviewViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 活動總覽
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 活動建立時間
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 活動圖
        /// </summary>
        public string PictureUrl { get; set; }
        /// <summary>
        /// 活動開始
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 喜歡人數
        /// </summary>
        public int FavoriteNumber { get; set; }
        /// <summary>
        /// 活動狀態
        /// </summary>
        public string Status { get; set; }
        public int StatusInt { get; set; }
        /// <summary>
        /// 報名人數
        /// </summary>
        public int ApplyNumber { get; set; }
        /// <summary>
        /// 活動總人數
        /// </summary>
        public int ActivityTotalNumber { get; set; }
        /// <summary>
        /// 剩餘名額
        /// </summary>
        public int RemainingNumber { get; set; }
        public List<Ticket>? Tickets { get; set; }
        public TicketCheck? CheckInStatus { get; set; }
    }

    public class TicketCheck
    {
        public int NotCheckedInTickets { get; set; }
        public int CheckedInTickets { get; set; }
    }
}
