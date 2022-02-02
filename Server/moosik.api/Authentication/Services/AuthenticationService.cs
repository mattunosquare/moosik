using AutoMapper;
using Microsoft.EntityFrameworkCore;
using moosik.api.Authentication.Interfaces;
using moosik.dal.Contexts;
using moosik.dal.Models;
using moosik.services.Dtos.Authentication;

namespace moosik.api.Authentication.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly MoosikContext _database;
    private readonly IMapper _mapper;

    public AuthenticationService(MoosikContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public AuthenticationResponseDto? Authenticate(AuthenticationRequestDto authenticationRequestDto)
    {
        var user = _database.Get<User>()
            .Include(u => u.Role)
            .SingleOrDefault(u => u.Username == authenticationRequestDto.Username && u.Password == authenticationRequestDto.Password);

        return user != null ? _mapper.Map<AuthenticationResponseDto>(user) : null;
    }
    
}