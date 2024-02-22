using System.Data;

namespace RinhaBackEnd2024.Persistence.UnitOfWork
{
    public class DbSession : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession(IDbConnection connection)
        {
            Connection = connection;
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
