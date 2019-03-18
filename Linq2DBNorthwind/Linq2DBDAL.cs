using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Linq2DBNorthwind.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Linq2DBNorthwind
{
    public class Linq2DBDAL
    {
        public IEnumerable<string> GetAllProducts()
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                 var allProducts=connection.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier)
                    .Select(product=>$"Product name: {product.ProductName}; Category: {product.Category.CategoryName}; Supplier: {product.Supplier.ContactName}");
                 return allProducts;
            }

        }

        public IEnumerable<string> GetAllEmployees()
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                var employeeWithTerritories = from employee in connection.Employees
                                              join employeeTerritory in connection.EmployeeTerritories on employee.EmployeeId equals employeeTerritory.EmployeeId 
                                              join territory in connection.Territories on employeeTerritory.TerritoryId equals territory.TerritoryId
                                              join regions in connection.Regions on territory.RegionId equals regions.RegionId
                                              select new { employee.FirstName, employee.LastName, regions };

                return employeeWithTerritories
                        .Distinct()
                        .Select(item=> $"Employee: {item.FirstName} {item.LastName}; Region: {item.regions.RegionDescription}");

                
            }
        }

        public IEnumerable<string> EmployeesCountForRegion()
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                var allEmployeesWithRegions = from reg in connection.Regions
                                              join territories in connection.Territories on reg.RegionId equals territories.RegionId 
                                              join empTerritories in connection.EmployeeTerritories on territories.TerritoryId equals empTerritories.TerritoryId
                                              join employee in connection.Employees on empTerritories.EmployeeId equals employee.EmployeeId
                                              select new { region=reg.RegionDescription, employee.EmployeeId };

                var employeesGroupedByRegion = from row in allEmployeesWithRegions.Distinct()
                                               group row by row.region into groupRegion
                                               select new {
                                                              groupRegion.Key,
                                                              EmployeesCount = groupRegion.Count(e => e.EmployeeId != 0)
                                                          };

                return employeesGroupedByRegion.Select(item => ($"Region: {item.Key}; Employees count: {item.EmployeesCount}")); ;
            }
        }

        public int AddNewEmployeeWithTerritories(Employee newEmployee)
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                try
                {
                    var a= connection.BeginTransaction();
                    newEmployee.EmployeeId = connection.InsertWithInt32Identity(newEmployee);
                    connection.Territories.Where(t => t.TerritoryDescription.Length <= 5)
                        .Insert(connection.EmployeeTerritories, t => new EmployeeTerritory { EmployeeId = newEmployee.EmployeeId, TerritoryId = t.TerritoryId });
                    connection.CommitTransaction();
                    return newEmployee.EmployeeId;
                }
                catch
                {
                    connection.RollbackTransaction();
                    return 0;
                }
            }
        }

        public int MoveProductsToAnotherCategory(int categoryIDFirst, int categoryIDSecond)
        {
            if (categoryIDFirst == categoryIDSecond)
                throw new Exception("categories are same!");

            using (var connection = new NorthwindConnection("Northwind"))
            {
                int updatedCount = connection.Products.Update(p => p.CategoryId == categoryIDFirst, pr => new Product
                {
                    CategoryId = categoryIDSecond
                });
                return updatedCount;
            }
        }

        public void InsertListProducts(List<Product> products)
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                try
                {
                    connection.BeginTransaction();
                    foreach (var product in products)
                    {
                        var category = connection.Categories.FirstOrDefault(c => c.CategoryName == product.Category.CategoryName);
                        product.CategoryId = category?.CategoryId ?? connection.InsertWithInt32Identity(
                                                 new Category
                                                 {
                                                     CategoryName = product.Category.CategoryName
                                                 });
                        var supplier = connection.Suppliers.FirstOrDefault(s => s.CompanyName == product.Supplier.CompanyName);
                        product.SupplierId = supplier?.SupplierId ?? connection.InsertWithInt32Identity(
                                                 new Supplier
                                                 {
                                                     CompanyName = product.Supplier.CompanyName
                                                 });
                    }

                    connection.BulkCopy(products);
                    connection.CommitTransaction();
                    }
                catch
                {
                    connection.RollbackTransaction();
                }
            }
        }


        public int UpdateNonShippedOrders()
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                var rowsToUpdate = connection.OrderDetails.LoadWith(od => od.Order).LoadWith(od => od.Product)
                    .Where(od => od.Order.ShippedDate == null);
                
                int updatedRows = rowsToUpdate.Update(UpdateOrders(connection.Products));
                return updatedRows;
            }
        }
        
        private Expression<Func<OrderDetail, OrderDetail>> UpdateOrders(ITable<Product> products)
        {
            return od => new OrderDetail
            {
                ProductId = products.First(p => p.CategoryId == od.Product.CategoryId && p.ProductId > od.ProductId) != null
                            ? products.First(p => p.CategoryId == od.Product.CategoryId && p.ProductId > od.ProductId).ProductId
                            : products.First(p => p.CategoryId == od.Product.CategoryId).ProductId
            };
        }
       
    }
}
