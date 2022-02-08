using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Authentication;

namespace moosik.api.Authentication.Interfaces;

public interface IAuthenticationService
{
    public UserDto? Authenticate(AuthenticationRequestDto authenticationRequestDto);
}