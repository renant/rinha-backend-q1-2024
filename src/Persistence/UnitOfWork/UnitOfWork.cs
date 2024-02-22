using RinhaBackEnd2024.Persistence.Interfaces;

namespace RinhaBackEnd2024.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DbSession session)
        {
            _session = session;
        }

        private readonly DbSession _session;

        public void BeginTransaction()
        {
            _session.Transaction = _session.Connection.BeginTransaction();
        }

        public void Commit()
        {
            _session.Transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _session.Transaction.Rollback();
            Dispose();
        }

        public void Dispose() => _session.Transaction?.Dispose();
    }
}
