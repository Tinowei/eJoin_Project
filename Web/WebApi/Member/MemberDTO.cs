using Web.ViewModels.MemberViewModel;

namespace Web.WebApi.Member
{
    public class MemberDTO
    {
             
    }
    
    public class MemberPassword
    {
        public string OriginalPassword { get; set; } // 原本密碼
        public string NewPassword { get; set; } // 新密碼
        public string CheckPassword { get; set; } // 確認密碼
        public int Code { get; set; } //回傳狀態的代碼
    }

    public class MemberProfile 
    {
        public string AvatarUrl { get; set; }             //頭像圖片網址
        public string BackgroundUrl { get; set; }         //背景圖片網址
        public string DisplayName { get; set; }           //顯示名稱
        public string PersonalIntroduction { get; set; }  //個人簡介
        public string Name { get; set; }                  //真實姓名
        public DateTime? Birthday { get; set; }            //生日
        public Gender Gender { get; set; }                //性別
        public RelationshipStatus EmotionalState { get; set; }        //感情狀態
        public string Phone { get; set; }                 //手機
        public string City { get; set; }                  //縣市      
        public string DetailAddress { get; set; }         //詳細地址
        public int Code { get; set; } //回傳狀態的代碼
    }
    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }

    public enum RelationshipStatus
    {
        Single = 1,
        StableRelationship = 2,
        Married = 3
    }

    public class MemberProfileImage
    {
        public string AvatarPeviewUrl { get; set; }             //預覽頭像圖片網址
        public string BackgroundPeviewUrl { get; set; }         //預覽背景圖片網址
    }

}
