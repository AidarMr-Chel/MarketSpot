public interface IAuthService
{
    string GenerateJwtToken(User user);
    bool VerifyPassword(string password, string hash);
}