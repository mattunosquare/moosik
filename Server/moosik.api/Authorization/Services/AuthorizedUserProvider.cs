using System.Security.Claims;
using moosik.api.Authorization.Interfaces;
using moosik.services.Dtos;
using moosik.services.Interfaces;

namespace moosik.api.Authorization.Services;

public class AuthorizedUserProvider : IAuthorizedUserProvider
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private UserDto? _user;

    public AuthorizedUserProvider(IUserService userService, IHttpContextAccessor contextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = contextAccessor;
    }

    public UserDto? GetLoggedInUser()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId)) return null;
        if (_user != null) return _user;

        _user = _userService.GetAllUsers(int.Parse(userId)).FirstOrDefault();

        return _user;
    }
}