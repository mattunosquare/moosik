using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using moosik.api.Authentication.Interfaces;
using moosik.api.Authorization.Interfaces;
using moosik.api.ViewModels.Authentication;
using moosik.services.Dtos;
using moosik.services.Dtos.Authentication;

namespace moosik.api.Authentication.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public TokenService(IConfiguration configuration, IMapper mapper)
    {
        (_configuration) = (configuration);
        _mapper = mapper;
    }

    public string GenerateToken(AuthenticationResponseDto user, int expirationTimeInMinutes)
    {
        var secretKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
         var securityKey = new SymmetricSecurityKey(secretKey);
         var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
         var expiryTime = DateTime.UtcNow.AddMinutes(expirationTimeInMinutes);

         var tokenDescriptor = new SecurityTokenDescriptor()
         {
             
             Subject = new ClaimsIdentity(new Claim[]
             {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.Username),
                 new Claim(ClaimTypes.Role, "User")
             }),
             Expires = expiryTime,
             SigningCredentials = credentials
         };

         switch (user.Role)
         {
             case "SuperAdmin":
                 tokenDescriptor.Subject.AddClaims(new Claim[]
                 {
                     new Claim(ClaimTypes.Role, "SuperAdmin"),
                     new Claim(ClaimTypes.Role, "Admin"),
                 });
                 break;
             case "Admin":
                 tokenDescriptor.Subject.AddClaims(new Claim[]
                 {
                     new Claim(ClaimTypes.Role, "Admin"),
                 });
                 break;
         }

         var tokenHandler = new JwtSecurityTokenHandler();
         var jwtToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
         
         var tokenString = tokenHandler.WriteToken(jwtToken);
         return tokenString;
    }
    
    public AuthenticationResponseDto AppendTokens(UserDto userDto)
    {
        var authenticationResponseDto = _mapper.Map<AuthenticationResponseDto>(userDto);
        
        var accessToken = GenerateToken(authenticationResponseDto, 30);
        var refreshToken = GenerateToken(authenticationResponseDto, 20160);

        authenticationResponseDto.AccessToken = accessToken;
        authenticationResponseDto.RefreshToken = refreshToken;

        return authenticationResponseDto;
    }
}