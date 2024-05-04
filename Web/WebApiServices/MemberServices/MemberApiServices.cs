using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.RegularExpressions;
using Web.WebApi.Member;
using Web.WebApi.Register;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static QRCoder.PayloadGenerator;
using System.Diagnostics.Metrics;
using Web.Services.AccountService;
using ApplicationCore.Extensions;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Web.WebApiServices.MemberServices
{
    public class MemberApiServices
    {
        private readonly IRepository<Member> _memberRepo; //宣告一個唯讀的欄位
        private readonly UserContextService _userContextService;
        private readonly int _memberId;

        public MemberApiServices(IRepository<Member> memberRepo, UserContextService userContextService ) //初始化建構式(透過注入的方式，像這邊是注入資料庫)
        {
            _memberRepo = memberRepo; //指派給欄位
            _userContextService = userContextService;
            _memberId = _userContextService.GetUserId();
        }

        //變更密碼-後端驗證
        public MemberPassword GetMemberApiServices(MemberPassword password)
        {
            string truePassword = _memberRepo.SingleOrDefault(m => m.Id == _memberId).Password;
            MemberPassword result = password;

            // 將使用者輸入的原始密碼進行雜湊
            var hashedOriginalPassword = password.OriginalPassword.HashPassword();
            if (hashedOriginalPassword != truePassword) 
            {
                result.Code = 1; //儲存失敗!輸入的「原有密碼」不正確。               
                return result;
            }
            if (password.NewPassword != password.CheckPassword)
            {
                result.Code = 2;//儲存失敗!輸入的「新密碼」與「確認密碼」不一致。
                return result;
            }
            if (password.NewPassword.Length < 7 && password.NewPassword != "")
            {
                result.Code = 3;//儲存失敗!「新密碼」的長度，不能少於7個字元。
                return result;
            }
            if (password.NewPassword.Length >30 && password.NewPassword != "")
            {
                result.Code = 4;//儲存失敗!「新密碼」的長度，不能多於30個字元。
                return result;
            }
            if (!Regex.IsMatch(password.NewPassword, "^[a-zA-Z0-9]+$"))
            {
                result.Code = 5;//儲存失敗!「新密碼」的格式，僅能輸入大小寫英文和數字。
                return result;
            }
            if (string.IsNullOrEmpty(hashedOriginalPassword))
            {
                result.Code = 6;//儲存失敗!請輸入「原本密碼」。
                return result;
            }
            if (string.IsNullOrEmpty(password.NewPassword))
            {
                result.Code = 7;//儲存失敗!請輸入「新密碼」。
                return result;
            }
            if (string.IsNullOrEmpty(password.CheckPassword))
            {
                result.Code = 8;//儲存失敗!請輸入「確認密碼」。
                return result;
            }


            _memberRepo.SetMemberPassword(password.NewPassword, _memberId);
            result.Code = 9; //儲存成功!
            return result;
        }

        //個人檔案-後端驗證
        public MemberProfile GetMemberApiServices(MemberProfile profile) 
        {
            MemberProfile result = profile;

            if (string.IsNullOrEmpty(profile.Name))
            {
                result.Code = 1;//儲存失敗!請輸入「姓名」。
                return result;
            }
            if (string.IsNullOrEmpty(profile.DisplayName))
            {
                result.Code = 2;//儲存失敗!請輸入「顯示名稱」。
                return result;
            }
            if (string.IsNullOrEmpty(profile.Phone))
            {
                result.Code = 3;//儲存失敗!請輸入「手機號碼」。
                return result;
            }
            if (profile.Phone.Length < 10 && profile.Phone != "")
            {
                result.Code = 4;//儲存失敗!「手機」的長度，不能少於10個字元。
                return result;
            }
            if (profile.Phone.Length > 12 && profile.Phone != "")
            {
                result.Code = 5;//儲存失敗!「手機」的長度，不能多於12個字元。
                return result;
            }
            if (profile.Name.Length > 50 && profile.Name != "")
            {
                result.Code = 6;//儲存失敗!「姓名」的長度，不能多於50個字元。
                return result;
            }
            if (profile.DisplayName.Length > 100 && profile.DisplayName != "")
            {
                result.Code = 7;//儲存失敗!「顯示姓名」的長度，不能多於100個字元。
                return result;
            }
            if (profile.PersonalIntroduction.Length > 250 && profile.PersonalIntroduction != "")
            {
                result.Code = 8;//儲存失敗!「個人簡介」的長度，不能多於250個字元。
                return result;
            }
            if (profile.DetailAddress.Length > 200 && profile.DetailAddress != "")
            {
                result.Code = 9;//儲存失敗!「詳細地址」的長度，不能多於200個字元。
                return result;
            }
            if (!Regex.IsMatch(profile.Phone, "^09\\d{8}$|^09\\d{2}-\\d{3}-\\d{3}$|^09\\d{2}-\\d{6}$"))
            {
                result.Code = 10;//儲存失敗!需以09開頭，且僅能輸入數字和減號。<br>(範例：09xxxxxxxx、09xx-xxx-xxx、09xx-xxxxxx)
                return result;
            }
            if (!Regex.IsMatch(profile.Name, "^(?! )(?!.* $)[a-zA-Z\u4e00-\u9fa5 ]+$"))
            {
                result.Code = 11;//儲存失敗!「姓名」的格式，僅能輸入大小寫英文或中文。
                return result;
            }
            if (!Regex.IsMatch(profile.DisplayName, "^(?! )(?!.* $)[a-zA-Z\u4e00-\u9fa5 ]+$"))
            {
                result.Code = 12;//儲存失敗!「顯示名稱」的格式，僅能輸入大小寫英文或中文。
                return result;
            }
            _memberRepo.SetMemberProfile(profile, _memberId);
            result.Code = 13; //儲存成功!
            return result;
        }


        // 個人檔案-圖片
        public MemberProfileImage GetMemberProfileImage(MemberProfileImage Image)
        {
            MemberProfileImage result = Image;

            if (string.IsNullOrEmpty(result.AvatarPeviewUrl))
            {
                result.AvatarPeviewUrl = "~/images/profilePicture.png";
            }
            if (string.IsNullOrEmpty(result.BackgroundPeviewUrl))
            {
                result.BackgroundPeviewUrl = "~/images/cardUploadImg.png";
            }

            return result;
        }


    }
    
    public static class  TestHelper
    {
        //變更密碼-更新資料庫
        public static Member SetMemberPassword(this IRepository<Member> memberRepo, string newPassword, int memberId)
        {
            Member result = new Member();
            try
            {
                //抓出資料庫中的會員ID
                Member memberTarget = memberRepo.GetById<int>(memberId);
                //將新密碼進行雜湊
                var hashedPassword = newPassword.HashPassword();
                
                //會員的密碼，變更為新密碼的雜湊值
                memberTarget.Password = hashedPassword;

                //更新資料庫中的會員資料
                result = memberRepo.Update(memberTarget);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return result;
            }
            return result;
        }

        //個人檔案-更新資料庫
        public static Member SetMemberProfile(this IRepository<Member> memberRepo, MemberProfile profile, int memberId)
        {
            Member result = new Member();
            try
            {
                //抓出資料庫中的會員ID
                Member memberTarget = memberRepo.GetById<int>(memberId);

                //會員的個人檔案，變更為新的內容
                memberTarget.AvatarUrl = profile.AvatarUrl;
                memberTarget.CoverUrl = profile.BackgroundUrl;
                memberTarget.DisplayName = profile.DisplayName;
                memberTarget.Description = profile.PersonalIntroduction;
                memberTarget.Name = profile.Name;
                memberTarget.Birthday = profile.Birthday;
                memberTarget.Gender = (byte?)profile.Gender;
                memberTarget.Relationship = (byte?)profile.EmotionalState;
                memberTarget.Phone = profile.Phone;
                memberTarget.City = profile.City;
                memberTarget.Address = profile.DetailAddress;
                memberTarget.LastEditTime = DateTime.Now;

                //更新資料庫中的會員資料
                result = memberRepo.Update(memberTarget);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return result;
            }
            return result;
        }
    }
}
