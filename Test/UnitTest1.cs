using BIT.Xpo.SqliteEncrypted;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using System;
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
        //Scenario Result Expected
        public void CreateDataStoreFromIDbConnection_DataIsConsistent_Pass()
        {
            SQLitePCL.Batteries_V2.Init();
            EncriptedSQLiteConnectionProvider.Register();


            if(File.Exists("CreateDataStoreFromIDbConnection.db"))
            {
                File.Delete("CreateDataStoreFromIDbConnection.db");
            }

            var connection =new SqliteConnection("Data Source=CreateDataStoreFromIDbConnection.db");
          



            System.IDisposable[] _discard;
          
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
        //Scenario Result Expected
        public void CreateDataStoreFromConnectionString_DataIsConsistent_Pass()
        {
            SQLitePCL.Batteries_V2.Init();
            EncriptedSQLiteConnectionProvider.Register();


            if (File.Exists("CreateDataStoreFromConnectionString.db"))
            {
                File.Delete("CreateDataStoreFromConnectionString.db");
            }



            System.IDisposable[] _discard;
            var DataStore = EncriptedSQLiteConnectionProvider.CreateProviderFromString(@"XpoProvider=EncriptedSQLiteConnectionProvider;Data Source=CreateDataStoreFromConnectionString.db", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, "abc123", out _discard);
        
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
        [Test]
        //Scenario Result Expected
        public void CreateDataWithEncryptedConnectionReadDataWithNormalConnection_FileCantBeReaded_Fail()
        {
            SQLitePCL.Batteries_V2.Init();
            EncriptedSQLiteConnectionProvider.Register();


            if (File.Exists("CreateDataWithEncryptedConnectionReadDataWithNormalConnection.db"))
            {
                File.Delete("CreateDataWithEncryptedConnectionReadDataWithNormalConnection.db");
            }



            System.IDisposable[] _discard;
            var CipherDataStore = EncriptedSQLiteConnectionProvider.CreateProviderFromString(@"XpoProvider=EncriptedSQLiteConnectionProvider;Data Source=CreateDataWithEncryptedConnectionReadDataWithNormalConnection.db", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, "abc123", out _discard);

            IDataLayer dl = new SimpleDataLayer(CipherDataStore);
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


            var NormalDataStore = SQLiteConnectionProvider.CreateProviderFromString(@"Data Source=CreateDataWithEncryptedConnectionReadDataWithNormalConnection.db", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, out _discard);



            UnitOfWork unitOfWork = new UnitOfWork(new SimpleDataLayer(NormalDataStore));
            var Criteria = new DevExpress.Data.Filtering.BinaryOperator("Name", "Jose Manuel Ojeda Melgar");

            Assert.Throws<DevExpress.Xpo.DB.Exceptions.SqlExecutionErrorException>(() =>
            {
                var CustomerFromDatabase = unitOfWork.FindObject<Customer>(Criteria);

                Assert.AreEqual(CustomerFromDatabase.Name, Customer.Name);
                Assert.AreEqual(CustomerFromDatabase.Address, Customer.Address);
                Assert.AreEqual(CustomerFromDatabase.Inactive, Customer.Inactive);
            });


        }
        [Test]
        //Scenario Result Expected
        public void CreateDataStoreFromConnectionStringWithoutEncryptionKey_DataIsConsistent_Pass()
        {
            SQLitePCL.Batteries_V2.Init();
            EncriptedSQLiteConnectionProvider.Register();


            if (File.Exists("CreateDataStoreFromConnectionStringWithoutEncryptionKey.db"))
            {
                File.Delete("CreateDataStoreFromConnectionStringWithoutEncryptionKey.db");
            }

            EncriptedSQLiteConnectionProvider.RequestPassword = new System.Func<string>(() => "abc123");

            //
            System.IDisposable[] _discard;
            var DataStore = EncriptedSQLiteConnectionProvider.CreateProviderFromString(@"XpoProvider=EncriptedSQLiteConnectionProvider;Data Source=CreateDataStoreFromConnectionStringWithoutEncryptionKey.db", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, string.Empty, out _discard);

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
        [Test]
        //Scenario Result Expected
        public void CreateDataStoreFromConnectionStringWithoutEncryptionKeyAndNullFunction_ExceptionIsThrown_Pass()
        {
          

            Assert.Throws<ArgumentNullException>(() =>
            {
                SQLitePCL.Batteries_V2.Init();
                EncriptedSQLiteConnectionProvider.Register();


                if (File.Exists("CreateDataStoreFromConnectionStringWithoutEncryptionKeyAndNullFunction.db"))
                {
                    File.Delete("CreateDataStoreFromConnectionStringWithoutEncryptionKeyAndNullFunction.db");
                }

                //EncriptedSQLiteConnectionProvider.RequestPassword = new System.Func<string>(() => "abc123");


                System.IDisposable[] _discard;
                var DataStore = EncriptedSQLiteConnectionProvider.CreateProviderFromString(@"XpoProvider=EncriptedSQLiteConnectionProvider;Data Source=CreateDataStoreFromConnectionStringWithoutEncryptionKeyAndNullFunction.db", DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema, string.Empty, out _discard);

                //ArgumentNullException


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
            });


          
        }
    }
}