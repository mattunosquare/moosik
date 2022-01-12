using System.Collections.Generic;
using moosik.services.Dtos;

namespace moosik.services.Interfaces;

public interface IUserService
{
    ICollection<UserDto> GetAllUsers(int? userId);
    UserDto GetUserById(int userId);
    UserDto GetUserByUsernameAndEmail(string username, string email);
    void UpdateUser(UserDto user);
    void CreateUser(UserDto user);
    void DeleteUser(int userId);
}