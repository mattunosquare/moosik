using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Authentication.Interfaces;
using moosik.api.Authorization;
using moosik.api.ViewModels.Authentication;
using moosik.services.Dtos.Authentication;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorization]
    [TokenAuthorization(TokenTypes.ValidAccessToken)]

    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authService, ITokenService tokenService, IMapper mapper)
        {
            (_authService, _tokenService, _mapper) = (authService, tokenService, mapper);
        }

        /// <summary>
        /// Returns the authenticated user with a generated JWT token.
        /// </summary>
        /// <param name="authenticationRequestViewModel"></param>
        /// <returns>The authenticated user with a generated JWT token</returns>
        /// <response code="200">Success - AuthenticationResponseViewModel successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found</response>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticationRequestViewModel authenticationRequestViewModel)
        {
            var authenticationResponseDto = _authService.Authenticate(_mapper.Map<AuthenticationRequestDto>(authenticationRequestViewModel));

            if (authenticationResponseDto == null)
            {
                return Unauthorized();
            }
            
            authenticationResponseDto = _tokenService.AppendTokens(authenticationResponseDto);
            
            return Ok(_mapper.Map<AuthenticationResponseViewModel>(authenticationResponseDto));
        }
        
        [HttpGet("refresh")]
        public IActionResult Refresh([FromHeader] string authorization)
        {
            var authenticationResponseDto = _tokenService.GetClaimDetailsFromToken(authorization);
            
            if (authenticationResponseDto == null)
            {
                return Unauthorized();
            }
            
            authenticationResponseDto = _tokenService.AppendTokens(authenticationResponseDto);
            
            return Ok(_mapper.Map<AuthenticationResponseViewModel>(authenticationResponseDto));
        }
    }
    
}