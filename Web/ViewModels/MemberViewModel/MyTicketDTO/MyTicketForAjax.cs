namespace Web.ViewModels.MemberViewModel.MyTicketDTO
{
    public class MyTicketForAjax
    {
        public int EventId { get; set; } // 活動的ID跳轉用
        public string EventName { get; set; } // 活動的活動名稱
        public string EventEndTime { get; set; } // 活動的結束時間，採ISO8601 yyyy-MM-ddTHH:mm
        public int TicketId { get; set; } // 發行票券的ID
        public string TicketTypeName { get; set;} // 該票券的票種的名稱
        public int TicketRemainingAmount { get; set; } // 該發行票券的剩餘張數
        public decimal TicketUnitPrice { get; set; } // 該發行票券的票種的單價
        public string ParticipantName { get; set;} // 該發行票券的明細的參加人名稱
        public string ParticipantEmail { get; set; } // 該發行票券的明細的參加人Email
        public string ParticipantPhone { get; set; } // 該發行票券的明細的參加人電話
    }
}
