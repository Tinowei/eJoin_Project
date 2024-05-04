using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.ViewModels.OrganizeViewModel
{
    #nullable disable
    public class IndexViewModel
    {
        public List<EventItem> EventList { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
    public class EventItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        //活動建立日期
        public DateTime CreatedDate { get; set; }
        //活動開始/結束時間
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //開放報名人數
        public int TotalTicketsCount { get; set; }
        //目前報名人數
        public int SoldTicketsCount { get; set; }
        //活動狀態
        public string Status { get; set; }
    }
}
