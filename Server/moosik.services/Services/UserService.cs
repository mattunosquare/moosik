using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using moosik.dal.Interfaces;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.User;
using moosik.services.Exceptions;
using moosik.services.Interfaces;

namespace moosik.services.Services;

public class UserService : IUserService
{
    private readonly IMoosikDatabase _database;
    private readonly IMapper _mapper;

    public UserService(IMoosikDatabase database, IMapper mapper) => (_database,_mapper) = (database, mapper);

    public UserDto[] GetAllUsers(int? userId = null)
    {
        Expression<Func<User, bool>> returnAll = u => true;
        Expression<Func<User, bool>> returnSingle = u => u.Id == userId;
        var filterUserById = userId >= 0 ? returnSingle : returnAll;

        return _mapper.ProjectTo<UserDto>(
                _database.Get<User>()
                    .Where(filterUserById))
            .ToArray();
    }

    public UserDetailDto GetDetailedUserById(int userId)
    {
        return _mapper.ProjectTo<UserDetailDto>(
                _database.Get<User>()
                    .Where(u => u.Id == userId))
            .SingleOrDefault();
    }

    public UserDto GetUserByUsernameAndEmail(string username, string email)
    {
        return _mapper.ProjectTo<UserDto>(
                _database.Get<User>()
                    .Where(u => u.Username == username && u.Email == email))
            .SingleOrDefault();
    }

    public void UpdateUser(UpdateUserDto updateUser)
    {
        var existingUser = RetrieveUserForId(updateUser.Id).SingleOrDefault();

        if (existingUser == null)
        {
            throw new NotFoundException($"No user found for id: {updateUser.Id}");
        }

        _mapper.Map(updateUser, existingUser);
        _database.SaveChanges();
    }

    public void CreateUser(CreateUserDto createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);
        
        _database.Add(_mapper.Map<User>(user));
        _database.SaveChanges();
    }

    public void DeleteUser(int userId)
    {
        var user = RetrieveUserForId(userId).SingleOrDefault();

        if (user == null)
        {
            throw new NotFoundException($"No user found with userId: {userId}");
        }

        user.Active = false;
        _database.SaveChanges();
    }

    public IQueryable<User> RetrieveUserForId(int userId)
    {
        return _database
            .Get<User>()
            .Where(u => u.Id == userId);
    }
}