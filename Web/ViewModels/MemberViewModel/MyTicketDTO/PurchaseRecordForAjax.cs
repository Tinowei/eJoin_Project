namespace Web.ViewModels.MemberViewModel.MyTicketDTO
{
    public class PurchaseRecordForAjax
    {
        public int PurchaseDetailId { get; set; }
        public string PurchaseTime { get; set; } // 購入時間=明細的成立時間，採ISO8601 yyyy-MM-ddTHH:mm
        public int EventId { get; set; } // 活動的ID跳轉用
        public string EventName { get; set; } // 活動的活動名稱
        public string ParticipantName { get; set; } // 明細的參加人名稱
        public string ParticipantEmail { get; set; } // 明細的參加人Email
        public string ParticipantPhone { get; set; } // 該明細的參加人電話
        public List<TicketInPurchaseRecord> Tickets { get; set; }

    }
    public class TicketInPurchaseRecord
    {
        public string TicketId { get; set; }
        public string TicketTypeName { get; set; } // 該發行票券的票種的票種名稱
        public int TicketPurchaseAmount { get; set; } // 購入數量
        public decimal TicketUnitPrice { get; set; } // 該發行票券的票種的單價
    }
}
