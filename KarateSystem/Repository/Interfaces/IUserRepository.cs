using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task AddUserAsync(UserDto user);
        Task UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int userId);
        bool AuthenticateUser(NetworkCredential credential);
        Task<UserDto> GetUserDtoByName(string username);
    }
}
