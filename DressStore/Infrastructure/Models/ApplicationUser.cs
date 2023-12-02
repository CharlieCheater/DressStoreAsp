using Microsoft.AspNetCore.Identity;

namespace DressStore.Infrastructure.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
