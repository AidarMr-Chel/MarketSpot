using System.Threading.Tasks;
using MarketSpot.Application.DTOs;
using MarketSpot.Application.Interfaces;
using MarketSpot.Core.Entities;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        return result;
    }

    public async Task<IdentityResult> RegisterAuthorAsync(AuthorRegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            PortfolioUrl = request.PortfolioUrl
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Author");
        }

        return result;
    }


    public async Task SignInAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, lockoutOnFailure: false);
        }
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public Task<bool> IsSignedInAsync()
    {
        return Task.FromResult(_signInManager.IsSignedIn(_signInManager.Context.User));
    }
}
