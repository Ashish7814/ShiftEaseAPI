using ShiftEaseAPI.Repositories;

namespace ShiftEaseAPI.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        Task CompleteAsync();
        void Dispose();
    }
}
