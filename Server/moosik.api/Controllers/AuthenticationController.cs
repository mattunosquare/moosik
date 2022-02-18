using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Authentication.Interfaces;
using moosik.api.Authorization;
using moosik.api.Authorization.Interfaces;
using moosik.api.Authorization.RoleAuthorization;
using moosik.api.Controllers.Base;
using moosik.api.ViewModels.Authentication;
using moosik.services.Dtos.Authentication;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAuthorization]
    
    public class AuthenticationController : MoosikBaseController
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IAuthorizedUserProvider _authorizedUserProvider;

        public AuthenticationController(IAuthenticationService authService, ITokenService tokenService, IMapper mapper,
            IAuthorizedUserProvider authorizedUserProvider)
        {
            (_authService, _tokenService, _mapper, _authorizedUserProvider) =
                (authService, tokenService, mapper, authorizedUserProvider);
        }

        /// <summary>
        /// Returns the authenticated user with a generated JWT token.
        /// </summary>
        /// <param name="authenticationRequestViewModel"></param>
        /// <returns>The authenticated user with a generated JWT token</returns>
        /// <response code="200">Success - AuthenticationResponseViewModel successfully returned</response>
        /// <response code="401">Unauthorized</response>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<AuthenticationResponseViewModel> Authenticate([FromBody] AuthenticationRequestViewModel authenticationRequestViewModel)
        {
            var userDto = _authService.Authenticate(_mapper.Map<AuthenticationRequestDto>(authenticationRequestViewModel));

            if (userDto == null) return Unauthorized();
            
            var authenticationResponseDto = _tokenService.AppendTokens(userDto);
            
            return Ok(_mapper.Map<AuthenticationResponseViewModel>(authenticationResponseDto));
        }
        
        /// <summary>
        /// Returns the authenticated user with a generated JWT token.
        /// </summary>
        /// <returns>The authenticated user with a generated JWT token</returns>
        /// <response code="200">Success - AuthenticationResponseViewModel successfully returned</response>
        /// <response code="401">Unauthorized</response>
        [TokenAuthorization(TokenTypes.ValidRefreshToken)]
        [HttpGet("refresh")]
        public ActionResult<AuthenticationResponseViewModel> Refresh()
        {
            var userDto = _authorizedUserProvider.GetLoggedInUser();
            if (userDto == null) return Unauthorized();
            
            var authenticationResponseDto = _tokenService.AppendTokens(userDto);
            
            return Ok(_mapper.Map<AuthenticationResponseViewModel>(authenticationResponseDto));
        }
    }
    
}