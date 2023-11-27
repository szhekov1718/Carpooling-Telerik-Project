using System.Threading.Tasks;
using Carpooling.Data.Models;
using Carpooling.Services.DTO.IncomeDTOs;

namespace Carpooling.Services.Contracts
{
    public interface ILoginService
    {
        Task<User> CheckUsersPasswordKeyAsync(string key);
        Task<string> CheckUserCredentialsAsync(LoginDTO loginDTO);
    }
}
