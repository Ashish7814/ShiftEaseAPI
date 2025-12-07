using Microsoft.AspNetCore.Identity;
using ShiftEaseAPI.Data;
using ShiftEaseAPI.Models;
using ShiftEaseAPI.Repositories.GenericRepository;

namespace ShiftEaseAPI.Repositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
    }
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(AppDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }
    }
}