using moosik.dal.Models;
using moosik.services.Dtos.Authentication;

namespace moosik.api.Authentication.Interfaces;

public interface IAuthenticationService
{
    public AuthenticationResponseDto? Authenticate(AuthenticationRequestDto authenticationRequestDto);
}