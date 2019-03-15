using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFNorthwind.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            EntityDAL db = new EntityDAL();
            foreach(var s in db.GetOrderDetailByCategory(3))
            {
                Console.WriteLine(s);
            }
            Assert.IsNotNull(db.GetOrderDetailByCategory(3));
        }
    }
}
