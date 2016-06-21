using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpcFramework.DataAccess
{
    public interface IDatabaseAccess : IDisposable
    {
        int ExecuteNonQuery(IDbCommand command);

        DataTable FillTable(IDbCommand command);

        IDbCommand CreateCommand(string query);

        void Open(string connectionString);

        IDbTransaction BeginTransaction();

        void Rollback(IDbTransaction transaction);

        void Commit(IDbTransaction transaction);

        IDbDataParameter CreateParameter(IDbCommand command);

        IDbDataParameter AddParameter(IDbCommand command, string name, object value);

        T GetValueFromDB<T>(IDbCommand command);

        void Close();

        ConnectionState State();
    }
}
