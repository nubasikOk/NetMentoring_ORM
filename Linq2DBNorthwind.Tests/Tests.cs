using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2DBNorthwind.Tests
{
    [TestClass]
    public class Tests
    {
        private Linq2DBDAL db;


        [TestInitialize]
        public void Test_Initialize()
        {
            db = new Linq2DBDAL();
        }
        [TestMethod]
        public void Test_IsNotNull_GetAllEmployees()
        {
            Assert.IsNotNull(db.GetAllEmployees());
        }

        [TestMethod]
        public void Test_IsNotNull_GetAllProducts()
        {
            Assert.IsNotNull(db.GetAllProducts());
        }

        [TestMethod]
        public void Test_IsNotNull_EmployeesCountForRegion()
        {
            Assert.IsNotNull(db.EmployeesCountForRegion());
        }
    }
}
