using Project.API.Entities;
using Project.API.Models;
using Projet.API.Models;

namespace Project.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        bool VerifyPassword(string password, string passwordHash);
        string CreatePasswordHash(string password);
    }
}
