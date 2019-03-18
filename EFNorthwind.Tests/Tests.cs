using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFNorthwind.Tests
{
    [TestClass]
    public class Tests
    {
        EntityDAL db;
        [TestInitialize]
        public void Test_Initialize()
        {
             db = new EntityDAL();

          
        }

        [TestMethod]
        public void Test_IsNotNull_GetOrderDetail()
        {
            Assert.IsNotNull(db.GetOrderDetailByCategory(3));
        }

        


    }
}
