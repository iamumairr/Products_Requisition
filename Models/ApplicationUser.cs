#nullable disable
using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ICollection<Request> Requests { get; set; }
    }
}
