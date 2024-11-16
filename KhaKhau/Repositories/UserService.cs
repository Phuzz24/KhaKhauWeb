using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using KhaKhau.Areas.Identity.Data;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        var user = _httpContextAccessor.HttpContext.User;
        return _userManager.GetUserId(user);
    }
}
