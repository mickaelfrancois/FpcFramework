using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpcFramework.DataAccess
{
    public class SQLiteDataAccess : IDatabaseAccess, IDisposable
    {
        private SQLiteConnection _cnx = null;

        public int Timeout { get { return _cnx.DefaultTimeout; } set { _cnx.DefaultTimeout = value; } }


        public SQLiteDataAccess()
        {
           
        }


        public void SetPassword(string password)
        {
            _cnx.SetPassword(password);
        }


        public void CreateInMemoryDatabase()
        {
            _cnx = new SQLiteConnection("Data Source=:memory:");
            _cnx.Open();
        }
        

        public void CreateDatabase(string fullPath)
        {            
            string cnxString = string.Format("Data Source={0};Version=3;New=True", fullPath);
            this.Open(cnxString);                        
        }




        #region IDatabaseAccess


        public ConnectionState State()
        {
            return _cnx.State;
        }

        public void Open(string connectionString)
        {
            if( _cnx == null)
                _cnx = new SQLiteConnection();

            _cnx.ConnectionString = connectionString;
            _cnx.Open();
        }


        public void Close()
        {
            if (_cnx != null)
                _cnx.Close();
        }


        public IDbTransaction BeginTransaction()
        {
            return _cnx.BeginTransaction();
        }


        public void Commit(IDbTransaction transaction)
        {
            transaction.Commit();
        }


        public void Rollback(IDbTransaction transaction)
        {
            transaction.Rollback();
        }


        public IDbCommand CreateCommand(string query)
        {
            var command = _cnx.CreateCommand();
            command.CommandText = query;
            return command;
        }


        public DataTable FillTable(IDbCommand command)
        {
            DataTable table = new DataTable();
            table.Load(command.ExecuteReader());
            return table;
        }


        public IDbDataParameter CreateParameter(IDbCommand command)
        {
            return command.CreateParameter();
        }


        public IDbDataParameter AddParameter(IDbCommand command, string name, object value)
        {
            var parameter = ((SQLiteCommand)command).CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            ((SQLiteCommand)command).Parameters.Add(parameter);
            return parameter;            
        }


        public T GetValueFromDB<T>(IDbCommand command)
        {         
            var scalarObject = command.ExecuteScalar();
            if (scalarObject == null || scalarObject is DBNull)
            {
                return default(T); ;
            }

            return (T)scalarObject;
        }


        public int ExecuteNonQuery(IDbCommand command)
        {
            return command.ExecuteNonQuery();
        }


        #endregion


        #region IDisposable

        public void Dispose()
        {
            this.Close();
        }

        #endregion
    }
}
