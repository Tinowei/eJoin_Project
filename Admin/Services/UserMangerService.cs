using Admin.WebApi;

namespace Admin.Services
{
    public class UserMangerService
    {
        private readonly HttpContext _httpContext;
        public UserMangerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public bool IsUserValid(LoginInDTO request)
        {
            //TODO 轉寫實際的User判斷邏輯，這裡只是簡單的範例
            return (request.UserName.Trim() == "a") && (request.Password.Trim() == "a");
        }

        public bool IsAuthenticated()
        {
            return _httpContext.User.Identity?.IsAuthenticated ?? false;
        }

        public string GetUserName()
        {
            if (!IsAuthenticated()) return string.Empty;

            return _httpContext.User.Identity?.Name ?? string.Empty;
        }
    }
}
