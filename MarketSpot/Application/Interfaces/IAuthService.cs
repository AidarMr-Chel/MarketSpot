using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MarketSpot.Application.DTOs;

namespace MarketSpot.Application.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterRequest request);
        Task<IdentityResult> RegisterAuthorAsync(AuthorRegisterRequest request);
        Task SignInAsync(string email, string password);
        Task SignOutAsync();
        Task<bool> IsSignedInAsync();
    }
}
