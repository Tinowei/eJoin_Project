using Web.ViewModels.MemberViewModel;

namespace Web.ViewModels.TicketViewModel
{
#nullable disable

    public class IndexViewModel
    {
        public int MemberId { get; set; }           //會員ID
        public int TicketId { get; set; }           //票劵ID
        public string EventName { get; set; }       //活動名稱
        public string EventEndDate { get; set; }    //活動到期日
        public string TicketNumber { get; set; }    //票號
        public string TicketType { get; set; }      //票種(票劵名稱)
        public string ParticipantName { get; set; }     //參加人顯示姓名
        public string ParticipantEmail { get; set; }    //參加人電子郵件
        public string ParticipantPhone { get; set; }    //參加人手機
        public string QRcodeUrl { get; set; }       // QRcode圖片網址
        public string OrganizerName { get; set; }   // 主辦名稱
        public string OrganizerEmail { get; set; }  // 主辦電子郵件
        public string OrganizerPhone { get; set; }  // 主辦手機

    }

}
