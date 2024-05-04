using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.MemberViewModel
{
#nullable disable

    public class MyProfileViewModel
    {
        public int MemberId { get; set; }                  //會員ID
        public string AvatarUrl { get; set; }             //頭像圖片網址
        public string BackgroundUrl { get; set; }         //背景圖片網址
        public string Email { get; set; }                 //Email
        public string DisplayName { get; set; }           //顯示名稱
        public string PersonalIntroduction { get; set; }  //個人簡介
        public string Name { get; set; }                  //真實姓名
        public DateTime? Birthday { get; set; }            //生日
        public Gender Gender { get; set; }                //性別
        public RelationshipStatus EmotionalState { get; set; }        //感情狀態
        public string Phone { get; set; }                 //手機
        public string City { get; set; }                  //縣市      
        public string DetailAddress { get; set; }         //詳細地址

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
}
