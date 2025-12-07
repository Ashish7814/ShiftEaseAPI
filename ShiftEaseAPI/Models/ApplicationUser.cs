using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftEaseAPI.Models.BaseModel;

namespace ShiftEaseAPI.Models
{
    [Index(nameof(Email))]
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
