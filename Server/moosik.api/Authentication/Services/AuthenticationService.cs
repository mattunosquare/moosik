using AutoMapper;
using Microsoft.EntityFrameworkCore;
using moosik.api.Authentication.Interfaces;
using moosik.dal.Contexts;
using moosik.dal.Interfaces;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Authentication;
using BC = BCrypt.Net.BCrypt;

namespace moosik.api.Authentication.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMoosikDatabase _database;
    private readonly IMapper _mapper;

    public AuthenticationService(IMoosikDatabase database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public UserDto? Authenticate(AuthenticationRequestDto authenticationRequestDto)
    {
        var user = _database.Get<User>()
            .Include(u => u.Role)
            .SingleOrDefault(u => u.Username == authenticationRequestDto.Username);

        if (user == null || !BC.Verify(authenticationRequestDto.Password, user.Password))
        {
            return null;
        }
        return _mapper.Map<UserDto>(user);
    }
    
}