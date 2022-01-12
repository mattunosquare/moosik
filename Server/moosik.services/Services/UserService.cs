using System.Collections.Generic;
using AutoMapper;
using moosik.dal.Contexts;
using moosik.services.Dtos;
using moosik.services.Interfaces;

namespace moosik.services.Services;

public class UserService : IUserService
{
    private readonly MoosikContext _database;
    private readonly IMapper _mapper;

    public UserService(MoosikContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public ICollection<UserDto> GetAllUsers(int? userId)
    {
        throw new System.NotImplementedException();
    }

    public UserDto GetUserById(int userId)
    {
        throw new System.NotImplementedException();
    }

    public UserDto GetUserByUsernameAndEmail(string username, string email)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateUser(UserDto user)
    {
        throw new System.NotImplementedException();
    }

    public void CreateUser(UserDto user)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteUser(int userId)
    {
        throw new System.NotImplementedException();
    }
}