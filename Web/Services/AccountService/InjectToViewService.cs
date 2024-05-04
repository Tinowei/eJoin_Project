using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Web.Services.AccountService;

public class InjectToViewService
{
    private readonly UserContextService _userContextService;
    private readonly IRepository<Member> _memberRepo;
    
    public InjectToViewService(UserContextService userContextService,IRepository<Member> memberRepo)
    {
        _userContextService = userContextService;
        _memberRepo = memberRepo;
    }
    
    //todo:查出當前會員資訊的頭像及背景圖片、暱稱
    public AccountInfoViewModel GetAccountInfoForLayout()
    {
        var userId = _userContextService.GetUserId();
        
        if (userId == null)
        {
            return ExampleAccount();
        }
        
        try
        {
            var user = _memberRepo.FirstOrDefault(m => m.Id == userId && !m.IsDelete);
            if (user == null)
            {
                return ExampleAccount();
            }

            return new AccountInfoViewModel()
            {
                NickName = user.DisplayName,
                Account = user.Email,
                AvatarUrl = user.AvatarUrl ?? "/images/profilePicture.png",
                BackgroundUrl = user.CoverUrl ?? "/images/backgroundUploadImg.png"
            };
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ExampleAccount();
        }
    }
    
    private AccountInfoViewModel ExampleAccount()
    {
        return new AccountInfoViewModel()
        {
            NickName = "範例帳號",
            Account = "example@gmail.com",
            AvatarUrl = "/images/profilePicture.png",
            BackgroundUrl = "/images/backgroundUploading.png"
        };
    }
}

public class AccountInfoViewModel
{
    public string NickName { get; set; }
    /// <summary>
    /// 帳號即信箱
    /// </summary>
    public string Account { get; set; }

    public string AvatarUrl { get; set; }
    
    public string BackgroundUrl { get; set; }
}