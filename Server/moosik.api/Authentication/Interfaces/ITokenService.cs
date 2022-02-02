using moosik.api.ViewModels.Authentication;
using moosik.services.Dtos.Authentication;

namespace moosik.api.Authentication.Interfaces;

public interface ITokenService
{
    string GenerateToken(AuthenticationResponseDto authenticationResponseDto, int timeInMinutes);
    AuthenticationResponseDto? GetClaimDetailsFromToken(string token);
    AuthenticationResponseDto AppendTokens(AuthenticationResponseDto authenticationResponseDto);
}