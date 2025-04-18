using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Misc;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Enum = KarateSystem.Misc.Enum;

namespace KarateSystem.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddUserAsync(UserDto user)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.UserLogin == user.UserLogin && u.UserId != user.UserId);

            if (existingUser)
            {
                throw new Exception("Użytkownik o takim loginie już istnieje.");
            }

            var newUser = _mapper.Map<User>(user);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task UpdateUserAsync(UserDto user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (existingUser == null)
                throw new Exception("Nie znaleziono użytkownika do edycji.");

            var loginTaken = await _context.Users.AnyAsync(u => u.UserLogin == user.UserLogin && u.UserId != user.UserId);

            if (loginTaken)
                throw new Exception("Użytkownik o takim loginie już istnieje.");

            existingUser.UserFirstName = user.UserFirstName;
            existingUser.UserLastName = user.UserLastName;
            existingUser.UserLogin = user.UserLogin;
            existingUser.UserPass = user.UserPass.Encrypt();
            existingUser.UserRole = user.UserRole;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("Nie znaleziono użytkownika do usunięcia.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            var user = _context.Users
                .AsEnumerable()
                .FirstOrDefault(u =>
                    u.UserLogin == credential.UserName &&
                    u.UserPass.Decrypt() == credential.Password &&
                    u.UserRole == Enum.UserRole.Admin.ToString());

            return user != null;
        }

        public async Task<UserDto> GetUserDtoByName(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserLogin == username);
            if (user == null)
                throw new Exception("Nie znaleziono użytkownika.");

            return _mapper.Map<UserDto>(user);
        }
    }
}
