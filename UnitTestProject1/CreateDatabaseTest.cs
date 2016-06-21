using System;
using System.Data;
using FpcFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class CreateDatabaseTest
    {

        [TestMethod]
        public void CreateMemory()
        {
            using (SQLiteDataAccess cnx = new SQLiteDataAccess())
            {
                cnx.CreateInMemoryDatabase();
                Assert.IsTrue(cnx.State() == ConnectionState.Open);
            }
        }


        [TestMethod]
        public void CreateTableFromModel()
        {
            using (SQLiteDataAccess cnx = new SQLiteDataAccess())
            {
                cnx.CreateInMemoryDatabase();
                Assert.IsTrue(cnx.State() == ConnectionState.Open);

                TestModel model = new TestModel();
                int res = model.CreatTable(cnx);                
            }
        }
    }
}
