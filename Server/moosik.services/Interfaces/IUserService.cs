using System.Collections.Generic;
using System.Linq;
using moosik.dal.Models;
using moosik.services.Dtos;

namespace moosik.services.Interfaces;

public interface IUserService
{
    UserDto[] GetAllUsers(int? userId = null);
    UserDetailDto GetDetailedUserById(int userId);
    UserDto GetUserByUsernameAndEmail(string username, string email);
    void UpdateUser(UpdateUserDto user);
    void CreateUser(CreateUserDto user);
    void DeleteUser(int userId);
    public IQueryable<User> RetrieveUserForId(int userId );
}