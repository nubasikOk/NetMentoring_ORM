using System;
using System.Collections.Generic;
using System.Linq;
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
                foreach (var product in connection.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier))
                {
                    yield return ($"Product name: {product.ProductName}; Category: {product.Category?.CategoryName}; Supplier: {product.Supplier?.ContactName}");
                }
            }

        }

        public IEnumerable<string> GetAllEmployees()
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                var query = from e in connection.Employees
                            join et in connection.EmployeeTerritories on e.EmployeeId equals et.EmployeeId into el
                            from w in el.DefaultIfEmpty()
                            join t in connection.Territories on w.TerritoryId equals t.TerritoryId into zl
                            from z in zl.DefaultIfEmpty()
                            join r in connection.Regions on z.RegionId equals r.RegionId into kl
                            from k in kl.DefaultIfEmpty()
                            select new { e.FirstName, e.LastName, Region = k };
                query = query.Distinct();

                foreach (var record in query)
                {
                    yield return ($"Employee: {record.FirstName} {record.LastName}; Region: {record.Region?.RegionDescription}");
                }
            }
        }

        public IEnumerable<string> EmployeesCountForRegion()
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                var query = from r in connection.Regions
                            join t in connection.Territories on r.RegionId equals t.RegionId into kl
                            from k in kl.DefaultIfEmpty()
                            join et in connection.EmployeeTerritories on k.TerritoryId equals et.TerritoryId into zl
                            from z in zl.DefaultIfEmpty()
                            join e in connection.Employees on z.EmployeeId equals e.EmployeeId into dl
                            from d in dl.DefaultIfEmpty()
                            select new { Region = r, d.EmployeeId };
                var result = from row in query.Distinct()
                             group row by row.Region into ger
                             select new { RegionDescription = ger.Key.RegionDescription, EmployeesCount = ger.Count(e => e.EmployeeId != 0) };

                foreach (var record in result.ToList())
                {
                    yield return ($"Region: {record.RegionDescription}; Employees count: {record.EmployeesCount}");
                }
            }
        }

        public int AddNewEmployeeWithTerritories(Employee newEmployee)
        {
            using (var connection = new NorthwindConnection("Northwind"))
            {
                try
                {
                    connection.BeginTransaction();
                    newEmployee.EmployeeId = Convert.ToInt32(connection.InsertWithIdentity(newEmployee));
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
            if (categoryIDFirst == categoryIDSecond) throw new Exception("categories are same!");
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
                        product.CategoryId = category?.CategoryId ?? Convert.ToInt32(connection.InsertWithIdentity(
                                                 new Category
                                                 {
                                                     CategoryName = product.Category.CategoryName
                                                 }));
                        var supplier = connection.Suppliers.FirstOrDefault(s => s.CompanyName == product.Supplier.CompanyName);
                        product.SupplierId = supplier?.SupplierId ?? Convert.ToInt32(connection.InsertWithIdentity(
                                                 new Supplier
                                                 {
                                                     CompanyName = product.Supplier.CompanyName
                                                 }));
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
                int updatedRowsCount = 
                    connection.OrderDetails.LoadWith(od => od.Order).LoadWith(od => od.Product)
                    .Where(od => od.Order.ShippedDate == null)
                    .Count();
                var updatedRows = connection.OrderDetails.LoadWith(od => od.Order).LoadWith(od => od.Product)
                .Where(od => od.Order.ShippedDate == null).Update(
                    od => new OrderDetail
                    {
                        ProductId = connection.Products.First(p => p.CategoryId == od.Product.CategoryId && p.ProductId > od.ProductId) != null
                            ? connection.Products.First(p => p.CategoryId == od.Product.CategoryId && p.ProductId > od.ProductId).ProductId
                            : connection.Products.First(p => p.CategoryId == od.Product.CategoryId).ProductId
                    });
                return updatedRowsCount;
            }
        }
    }
}
