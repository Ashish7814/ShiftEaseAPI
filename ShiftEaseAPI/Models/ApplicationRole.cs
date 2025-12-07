using Microsoft.AspNetCore.Identity;

namespace ShiftEaseAPI.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
