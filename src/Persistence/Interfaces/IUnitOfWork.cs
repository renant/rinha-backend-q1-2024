namespace RinhaBackEnd2024.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
