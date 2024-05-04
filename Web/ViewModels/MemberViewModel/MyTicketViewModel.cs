using System.Numerics;
using System.Security.Policy;

namespace Web.ViewModels.MemberViewModel
{
#nullable disable

    public class MyTicketViewModel
    {
        public int MemberId { get; set; }
        public string AvatarUrl { get; set; }  //頭像圖片網址
        public string DisplayName { get; set; }//顯示名稱
    }
}
