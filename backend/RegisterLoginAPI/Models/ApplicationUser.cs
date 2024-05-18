using Microsoft.AspNetCore.Identity;

namespace RegisterLoginAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
