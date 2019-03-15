using System;
using System.Collections.Generic;
using System.Linq;
using Linq2DBNorthwind.Entities;
using LinqToDB;


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
    }
}
