using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftEaseAPI.Data;
using ShiftEaseAPI.Models;
using System.Linq.Expressions;

namespace ShiftEaseAPI.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where  T: class
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        protected DbSet<T> dbSet;


        public GenericRepository(
           AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            dbSet = _context.Set<T>();
            _userManager = userManager;
        }
        public async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public async Task<IdentityResult> CreateAsync(T entity, string extra)
        {
            if (entity is ApplicationUser user)
            {
                return await _userManager.CreateAsync(user, extra);
            }
            return IdentityResult.Success;

        }
        public async Task<IdentityResult> ConfirmEmailAsync(T entity, string extra)
        {
            if (entity is ApplicationUser user)
            {
                return await _userManager.ConfirmEmailAsync(user, extra);
            }
            return IdentityResult.Success;
        }
        public async Task<string> GenerateEmailToken(T entity)
        {
            if(entity is ApplicationUser user)
            {
                return await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            return null;
        }

        public async Task<bool> CheckPasswordAsync(T entity, string extra)
        {
            if (entity is ApplicationUser user)
            {
                return await _userManager.CheckPasswordAsync(user, extra);
            }
            return false;
        }

        public async Task<bool> IsEmailConfirmedAsync(T entity)
        {
            if(entity is ApplicationUser user)
            {
                return await _userManager.IsEmailConfirmedAsync(user);
            }
            return false;
        }

        public Task<string> GeneratePasswordResetToken(T entity)
        {
            if (entity is ApplicationUser user)
            {
                return _userManager.GeneratePasswordResetTokenAsync(user);
            }
            return null;
        }
        public async Task<bool> AddRange(IEnumerable<T> entity)
        {
            await dbSet.AddRangeAsync(entity);
            return true;
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            return await dbSet.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllIncludingDeactiveAsync(Expression<Func<T, bool>> expression)
        {
            return await dbSet.Where(expression).ToListAsync();
        }


        public virtual Task<List<T>> GetAllAsync(bool isDistinct = false)
        {
            if (isDistinct)
                return dbSet.Distinct().ToListAsync();
            else
                return dbSet.ToListAsync();
        }

        public virtual List<T> GetAll(bool isDistinct = false)
        {
            if (isDistinct)
                return dbSet.Distinct().ToList();
            else
                return dbSet.ToList();
        }



        public T GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Generic get method on the basis of id for entities asynchronously
        /// </summary>
        /// <param name="id">Should be primary key of table</param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<bool> Remove(int id)
        {
            var t = await dbSet.FindAsync(id);

            if (t != null)
            {
                dbSet.Remove(t);
                return true;
            }
            else
                return false;
        }

        public Task<bool> RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return Task.FromResult(true);
        }


        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual List<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public Task<bool> Update(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(true);
        }

        public Task<bool> UpdateRange(IEnumerable<T> entity)
        {
            dbSet.AttachRange(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(true);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return dbSet.CountAsync(cancellationToken);


        }





        /// <summary>
        /// generic count method , fetches count for the entities on the basis of condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> where)
        {
            return dbSet.Count(where);
        }

        /// <summary>
        /// generic count method , fetches count for the entities on the basis of condition asynchronously
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<int> CountAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> where)
        {
            return dbSet.CountAsync(where, cancellationToken);


        }

        /// <summary>
        /// generic get method , fetches count for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return dbSet.CountAsync(where);
        }


        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return dbSet.FirstOrDefault<T>(predicate);
        }

        /// <summary>
        /// The first record matching the specified criteria asynchronously
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public Task<T> GetFirstOrDefaultAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate)
        {
            return dbSet.FirstOrDefaultAsync<T>(predicate, cancellationToken);
        }


        /// <summary>
        /// The first record matching the specified criteria asynchronously
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return dbSet.FirstOrDefaultAsync<T>(predicate);
        }


        public virtual bool Any(Expression<Func<T, bool>> expression)
        {
            var isExit = dbSet.Any(expression);
            if (isExit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region IDisposable Implementation

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
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

        public async Task<T> AddAndReturnAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        #endregion
    }
}
