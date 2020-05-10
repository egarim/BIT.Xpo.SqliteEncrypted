using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using Microsoft.Data.Sqlite;
//using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace BIT.Xpo.SqliteEncrypted
{
    public class EncriptedSQLiteConnectionProvider : SQLiteConnectionProvider
    {

        public new static void Register()
        {
            //HACK review this link https://supportcenter.devexpress.com/ticket/details/BC4261/data-access-library-connection-provider-registration-has-been-changed
            DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
            DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromConnection);

        }

        public new const string XpoProviderTypeString = nameof(EncriptedSQLiteConnectionProvider);
        string EncryptionKey = string.Empty;
        public EncriptedSQLiteConnectionProvider(IDbConnection connection, AutoCreateOption autoCreateOption, string EncryptionKey) : base(connection, autoCreateOption)
        {
            this.EncryptionKey = EncryptionKey;
        }
        public static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, string EncryptionKey, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            
            ConnectionStringParser parser = new ConnectionStringParser(connectionString);
            parser.RemovePartByName("XpoProvider");
            string connectionString1 = parser.GetConnectionString();
            IDbConnection connection = new SqliteConnection(connectionString1);
            objectsToDisposeOnDisconnect = new IDisposable[] { connection };
            return new EncriptedSQLiteConnectionProvider(connection, autoCreateOption, EncryptionKey);
        }
        public static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption, string EncryptionKey)
        {

            return new EncriptedSQLiteConnectionProvider(AddPragmaKey(connection, EncryptionKey), autoCreateOption, EncryptionKey);

        }
        protected override IDbConnection CreateConnection()
        {
            var connection = base.CreateConnection();
            AddPragmaKey(connection,EncryptionKey);
            return connection;
        }

        static private IDbConnection AddPragmaKey(IDbConnection connection, string EncryptionKey)
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT quote($password);";
            command.AddParameter("$password", EncryptionKey);
            var quotedPassword = (string)command.ExecuteScalar();

            command.CommandText = "PRAGMA key = " + quotedPassword;
            command.Parameters.Clear();
            command.ExecuteNonQuery();
            return connection;
        }
    }
}
