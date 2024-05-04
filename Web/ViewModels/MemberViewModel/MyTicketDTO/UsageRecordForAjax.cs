namespace Web.ViewModels.MemberViewModel.MyTicketDTO
{
    public class UsageRecordForAjax
    {
        public int EventId { get; set; } // 活動的ID跳轉用
        public string EventName { get; set; } // 活動的活動名稱
        public int TicketId { get; set; } // 發行票券的ID
        public string TicketTypeName { get; set; } // 該發行票券的票種的票種名稱
        public int TicketUsageAmount { get; set; } // 該次使用張數
        public string UsageTime { get; set; } // 使用時間，採ISO8601 yyyy-MM-ddTHH:mm
    }
}
