using Web.ViewModels.SharedVIewModel;

namespace Web.ViewModels.MemberViewModel
{
#nullable disable

    public class MyEventViewModel
    {
        public int MemberId { get; set; }       //會員ID
        public string AvatarUrl { get; set; }   //頭像圖片網址
        public string DisplayName { get; set; } //顯示名稱
        public List<Card> ParticipateCard { get; set; } //參加活動-卡片清單
        public List<Card> LikeCard { get; set; }        //喜歡活動-卡片清單
        public List<MemberMyEventTrackGridViewModel> TrackGrid { get; set; } //追蹤單位
    }

    //public class MemberEventCardViewModel
    //{
    //    public int EventId { get; set; }
    //    public string EventImage { get; set; }
    //    public string EventName { get; set; }
    //    public string EventStartDate { get; set; }
    //    public string EventEndDate { get; set; }
    //    public string EventIntroduction { get; set; }
    //    public string IconLoveClass { get; set; }
    //    public string EventStatusClass { get; set; }
    //    public string EventStatusLabel { get; set; }
    //}

    public class MemberMyEventTrackGridViewModel
    {
        public int OrganizerId { get; set; }            //主辦ID
        public string OrganizerAvatarUrl { get; set; }  //主辦頭像
        public string OrganizerName { get; set; }       //主辦名稱
        public int TotalEvent { get; set; }             //活動數量
        public int TrackNumber { get; set; }            //追蹤人數
    }
}
