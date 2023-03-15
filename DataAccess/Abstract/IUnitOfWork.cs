namespace DataAccess.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Book { get; }
        IUserRepository User { get; }
        ITakeOfBook TakeOfBook { get; }

        int SaveChanges();
    }
}
