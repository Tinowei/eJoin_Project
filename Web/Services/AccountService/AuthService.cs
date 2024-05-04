using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Web.ViewModels.RegisterViewModel.SharedViewModel;

namespace Web.Services.AccountService
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<Member> _memberRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IRepository<Member> memberRepo, IHttpContextAccessor httpContextAccessor)
        {
            _memberRepo = memberRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Member> AuthenticateUser(string email, string password)
        {
            var hashedPassword = password.HashPassword();
            var user = _memberRepo.SingleOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);
            return await user;
        }

        public async Task Login(Member user)
        {
            List<Claim> claims = BuildClaims(user);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    //IsPersistent = false
                });
        }

        private List<Claim> BuildClaims(Member user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim("UserId", user.Id.ToString())
            };
            return claims;
        }

        //註冊

        public async Task<Member> RegisterLocalLoginAsync(string email, string fullname, string phone, string password)
        {
            // 檢查輸入的有效性，例如檢查電子郵件是否已經被註冊
            if (await IsEmailRegistered(email))
            {
                return new Member()
                {
                    Id =0,
                };
                //throw new Exception("這個電子郵件已經被註冊。");
            }

            // 創建一個新的Member實例
            var member = new Member()
            {
                Name = fullname,
                Email = email,
                DisplayName = fullname,
                CoverUrl = "/images/cardUploadImg.png",
                AvatarUrl = "/images/profilePicture.png",
                Phone = phone,
                Description = string.Empty,
                Birthday = DateTime.Now,
                Gender = 0,
                Relationship = 0,
                City = string.Empty,
                Address = string.Empty,
                Password = password.HashPassword(), // 假設你有一個方法來將密碼進行雜湊
                RegisterTime = DateTime.Now,
                IsDelete = false,
            };

            // 將新的Member實例添加到資料庫中
            var savedMember = _memberRepo.Add(member);

            // 返回創建的Member實例
            return savedMember;
        }

        // 檢查電子郵件是否已經被註冊的方法
        private async Task<bool> IsEmailRegistered(string email)
        {
            // 假設你有一個方法來從資料庫中查詢是否有使用這個電子郵件的用戶
            var existingMember = await _memberRepo.SingleOrDefaultAsync(m => m.Email == email);
            return existingMember != null;
        }
    }
}
