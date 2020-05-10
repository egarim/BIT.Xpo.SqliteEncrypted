using DevExpress.Xpo.DB;
using Microsoft.Data.Sqlite;
//using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace BIT.Xpo.SqliteEncrypted
{
    public class BitSQLiteConnectionProvider : SQLiteConnectionProvider
    {

        public new static void Register()
        {
            //HACK review this link https://supportcenter.devexpress.com/ticket/details/BC4261/data-access-library-connection-provider-registration-has-been-changed
            DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
            DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromConnection);

        }

        public new const string XpoProviderTypeString = nameof(BitSQLiteConnectionProvider);
        string EncryptionKey = string.Empty;
        public BitSQLiteConnectionProvider(IDbConnection connection, AutoCreateOption autoCreateOption, string EncryptionKey) : base(connection, autoCreateOption)
        {
            this.EncryptionKey = EncryptionKey;
        }
        public static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, string EncryptionKey, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            //TODO edit connection string with connection string helper ASK JAVIER
            IDbConnection connection = new SqliteConnection("Data Source=mydb.db");
            objectsToDisposeOnDisconnect = new IDisposable[] { connection };
            return new BitSQLiteConnectionProvider(connection, autoCreateOption, EncryptionKey);
        }
        public static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption, string EncryptionKey)
        {

            return new BitSQLiteConnectionProvider(connection, autoCreateOption, EncryptionKey);

        }
        protected override IDbConnection CreateConnection()
        {
            var connection = base.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT quote($password);";
            command.AddParameter("$password", EncryptionKey);
            var quotedPassword = (string)command.ExecuteScalar();

            command.CommandText = "PRAGMA key = " + quotedPassword;
            command.Parameters.Clear();
            command.ExecuteNonQuery();

            //var connection = base.CreateConnection();

            //connection.ConnectionString =
            //new SqliteConnectionStringBuilder(connection.ConnectionString)
            //{ Password = EncryptionKey }
            //    .ToString();

            return connection;
        }


    }
}
