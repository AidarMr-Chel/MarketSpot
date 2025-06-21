using Microsoft.AspNetCore.Identity;

namespace MarketSpot.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? PortfolioUrl { get; set; }
    }
}
