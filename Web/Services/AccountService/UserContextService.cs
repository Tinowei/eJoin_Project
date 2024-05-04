namespace Web.Services.AccountService
{
    public class UserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userIdClaim = user.Claims.FirstOrDefault(claim => claim.Type == "UserId");
            if (userIdClaim == null)
            {
                return 0;
            }
            var userId = int.Parse(userIdClaim.Value);

            return userId;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
