using moosik.services.Dtos;

namespace moosik.api.Authorization.Interfaces;

public interface IAuthorizedUserProvider
{
    UserDto? GetLoggedInUser();
}