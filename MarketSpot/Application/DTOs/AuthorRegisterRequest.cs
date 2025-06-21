namespace MarketSpot.Application.DTOs
{
    public class AuthorRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PortfolioUrl { get; set; }
    }
}