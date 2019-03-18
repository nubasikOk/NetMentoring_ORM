using System;
using System.Collections.Generic;
using Linq2DBNorthwind.Entities;
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

        [TestMethod]
        public void Test_IsCorrect_AddEmployeeWithTerritory()
        {
            var newEmployee = new Employee() { FirstName = "Viachaslau", LastName = "Kitsun" };
           Assert.IsNotNull(db.AddNewEmployeeWithTerritories(newEmployee));
        }

        [TestMethod]
        public void Test_IsCorrect_UpdateProductsCategory()
        {
            Assert.IsTrue(db.MoveProductsToAnotherCategory(3,2)>0);
        }

        

        [TestMethod]
        public void Test_IsCorrect_AddListProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    ProductName = "Tesla Model X",
                    Category = new Category {CategoryName = "Vehicles"},
                    Supplier = new Supplier {CompanyName = "Tesla"}
                },
                new Product
                {
                    ProductName = "Tesla Model S",
                    Category = new Category {CategoryName = "Vehicles"},
                    Supplier = new Supplier {CompanyName = "Tesla"}
                }
            };

            db.InsertListProducts(products);

        }


        [TestMethod]
        public void Test_IsCorrect_UpdateNonShippedOrders()
        {
            Assert.IsTrue(db.UpdateNonShippedOrders()>0);

        }
    }
}
