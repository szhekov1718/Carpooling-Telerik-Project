using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carpooling.Data.Enums;
using Carpooling.Data.Models;
using Carpooling.Services.DTO.IncomeDTOs;
public interface IUserService
{
    UserDTO GetBy(string value, string prop);
    Task<User> GetUserByUserNameAsync(string username);
    Task<ICollection<UserDTO>> GetAllUsersAsync();
    Task<UserDTO> CreateUserAsync(UserDTO userDTO);
    Task<User> GetUserByIdAsync(Guid id);

    string HashPassword(string password);
    Task<UserDTO> UpdateUserAsync(string currentUsername, UserDTO userDTO);
    public bool ValidateEmail(string email);
    Task<bool> DeleteUserAsync(string username);
    Task<bool> IsUserBlocked(string username);
    Task<User> BlockUnblockUserAsync(string username, string action);
    Task<bool> MakeUserAdmin(string username);
    Task<TravelStatus> DriverMarkTripCompleted(Guid driverId, Guid tripId);
}
