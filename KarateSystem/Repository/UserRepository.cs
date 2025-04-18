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
        public async Task<bool> AddUserAsync(UserDto user)
        {
            if(string.IsNullOrWhiteSpace(user.UserFirstName) ||
               string.IsNullOrWhiteSpace(user.UserLastName) ||
               string.IsNullOrWhiteSpace(user.UserLogin) ||
               string.IsNullOrWhiteSpace(user.UserPass) ||
               string.IsNullOrWhiteSpace(user.UserRole))
            {
                return false;
            }
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserLogin == user.UserLogin);
            if(existingUser != null) return false;
            var newUser = _mapper.Map<User>(user);
            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
        public async Task<bool> UpdateUserAsync(UserDto user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if(existingUser == null) return false;
            if (string.IsNullOrWhiteSpace(user.UserFirstName) ||
               string.IsNullOrWhiteSpace(user.UserLastName) ||
               string.IsNullOrWhiteSpace(user.UserLogin) ||
               string.IsNullOrWhiteSpace(user.UserPass) ||
               string.IsNullOrWhiteSpace(user.UserRole))
            {
                return false;
            }

            existingUser.UserFirstName = user.UserFirstName;
            existingUser.UserLastName = user.UserLastName;
            existingUser.UserLogin = user.UserLogin;
            existingUser.UserPass = user.UserPass.Encrypt();
            existingUser.UserRole = user.UserRole;

            try
            {
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AuthenticateUser(NetworkCredential credential)
        {
            var user = _context.Users
                    .AsEnumerable() 
                    .FirstOrDefault(u => u.UserLogin == credential.UserName && 
                                    u.UserPass.Decrypt() == credential.Password &&
                                    u.UserRole == "Admin");
            return user == null ? false : true;
        }
        public UserDto GetUserDtoByName(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserLogin == username);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }
    }
}
