using System;
using System.Data;
using FpcFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class DataAccessTest
    {
        IDatabaseAccess cnx;



        [TestInitialize]
        public void TestInitialize()
        {
            cnx = new SQLiteDataAccess();
            
           ((SQLiteDataAccess)cnx).CreateInMemoryDatabase();
            string query = "CREATE TABLE testModel ( ID INTEGER , creatDate DATETIME NULL,  name varchar(255) NULL, enabled TINYINT(1) NULL,  PRIMARY KEY ( ID AUTOINCREMENT ) );";
            var command = cnx.CreateCommand(query);
            cnx.ExecuteNonQuery(command);

            TestModel test = new TestModel();
            test.ID = 1;
            test.Name = "Test";
            test.CreationDate = DateTime.Now;
            test.Insert(cnx);

            test = new TestModel();
            test.ID = 2;
            test.Name = "Test2";
            test.CreationDate = DateTime.Now;
            test.Insert(cnx);

            test = new TestModel();
            test.ID = 3;
            test.Name = "Test3";
            test.CreationDate = DateTime.Now;
            test.Insert(cnx);
        }


        [TestCleanup]
        public void ClassCleanup()
        {
            cnx.Close();
        }


        [TestMethod]
        public void InsertTest()
        {
            TestModel test = new TestModel();
            test.ID = 999;
            test.Name = "Test";
            test.Enabled = true;
            test.CreationDate = DateTime.Now;
            test.Insert(cnx);
        }


        [TestMethod]
        public void UpdateTest()
        {
            TestModel test = new TestModel();
            test.ID = 1;
            test.Name = "Test";
            test.Enabled = true;
            test.CreationDate = DateTime.Now;
            test.Update(cnx);
        }


        [TestMethod]
        public void DeleteTest()
        {
            TestModel test = new TestModel();
            test.ID = 1;
            test.Name = "Test";
            test.CreationDate = DateTime.Now;
            test.Enabled = true;
            test.Delete(cnx);
        }


        [TestMethod]
        public void SelectTest()
        {           
            var listResult = cnx.FillTable(cnx.CreateCommand("SELECT * FROM testModel")).To<TestModel>();
            Assert.IsTrue(listResult.Count > 0);

        }
    }
}
