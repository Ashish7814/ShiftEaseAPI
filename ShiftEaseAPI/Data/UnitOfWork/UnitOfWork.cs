using Microsoft.AspNetCore.Identity;
using ShiftEaseAPI.Models;
using ShiftEaseAPI.Repositories;

namespace ShiftEaseAPI.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IUserRepository userRepository { get; private set; }

        public UnitOfWork(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            userRepository = new UserRepository(context, userManager);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose(); 
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
