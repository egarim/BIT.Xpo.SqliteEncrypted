using BIT.Xpo.SqliteEncrypted;
using DevExpress.Xpo;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using System.IO;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            SQLitePCL.Batteries_V2.Init();
            EncriptedSQLiteConnectionProvider.Register();


            if(File.Exists("mydb.db"))
            {
                File.Delete("mydb.db");
            }

            var connection =new SqliteConnection("Data Source=mydb.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT quote($password);";
            command.AddParameter("$password", "abc123");
            var quotedPassword = (string)command.ExecuteScalar();

            command.CommandText = "PRAGMA key = " + quotedPassword;
            command.Parameters.Clear();
            command.ExecuteNonQuery();



            System.IDisposable[] _discard;
            //var DataStore = BitSQLiteConnectionProvider.CreateProviderFromString(@"XpoProvider=BitSQLiteConnectionProvider;Data Source=mydb.db", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, "abc123", out _discard);
            var DataStore = EncriptedSQLiteConnectionProvider.CreateProviderFromConnection(connection, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, "abc123");
            IDataLayer dl = new SimpleDataLayer(DataStore);
            using (Session session = new Session(dl))
                   {
                       System.Reflection.Assembly[] assemblies = new System.Reflection.Assembly[] {
                       typeof(Customer).Assembly,
                     
                   };
                session.UpdateSchema(assemblies);
                session.CreateObjectTypeRecords(assemblies);
            }

            UnitOfWork UoW = new UnitOfWork(dl);

            Customer Customer = new Customer(UoW);
            Customer.Name = "Jose Manuel Ojeda Melgar";
            Customer.Address = "Saint Petersburg Russia";
            Customer.Inactive = false;
            Customer.CreatedOn = new System.DateTime(2020, 5, 16);

            UoW.CommitChanges();


            UnitOfWork unitOfWork = new UnitOfWork(dl);
            var Criteria = new DevExpress.Data.Filtering.BinaryOperator("Name", "Jose Manuel Ojeda Melgar");
            var CustomerFromDatabase=unitOfWork.FindObject<Customer>(Criteria);

            Assert.AreEqual(CustomerFromDatabase.Name, Customer.Name);
            Assert.AreEqual(CustomerFromDatabase.Address, Customer.Address);
            Assert.AreEqual(CustomerFromDatabase.Inactive, Customer.Inactive);
        }
        [Test]
        public void Test2()
        {
            SQLitePCL.Batteries_V2.Init();
            EncriptedSQLiteConnectionProvider.Register();


            if (File.Exists("mydb.db"))
            {
                File.Delete("mydb.db");
            }



            System.IDisposable[] _discard;
            var DataStore = EncriptedSQLiteConnectionProvider.CreateProviderFromString(@"XpoProvider=BitSQLiteConnectionProvider;Data Source=mydb.db", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, "abc123", out _discard);
        
            IDataLayer dl = new SimpleDataLayer(DataStore);
            using (Session session = new Session(dl))
            {
                System.Reflection.Assembly[] assemblies = new System.Reflection.Assembly[] {
                       typeof(Customer).Assembly,

                   };
                session.UpdateSchema(assemblies);
                session.CreateObjectTypeRecords(assemblies);
            }

            UnitOfWork UoW = new UnitOfWork(dl);

            Customer Customer = new Customer(UoW);
            Customer.Name = "Jose Manuel Ojeda Melgar";
            Customer.Address = "Saint Petersburg Russia";
            Customer.Inactive = false;
            Customer.CreatedOn = new System.DateTime(2020, 5, 16);

            UoW.CommitChanges();


            UnitOfWork unitOfWork = new UnitOfWork(dl);
            var Criteria = new DevExpress.Data.Filtering.BinaryOperator("Name", "Jose Manuel Ojeda Melgar");
            var CustomerFromDatabase = unitOfWork.FindObject<Customer>(Criteria);

            Assert.AreEqual(CustomerFromDatabase.Name, Customer.Name);
            Assert.AreEqual(CustomerFromDatabase.Address, Customer.Address);
            Assert.AreEqual(CustomerFromDatabase.Inactive, Customer.Inactive);
        }
    }
}