using EFNorthwind.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFNorthwind
{
    public class EntityDAL
    {

        public IEnumerable<string> GetOrderDetailByCategory(int categoryID)
        {
            using (var db = new NorthwindDB())
            {
                var query = db.Orders.Include("Products").Include("Customers")
                .Where(o => o.Order_Details.Any(od => od.Product.CategoryID == categoryID))
                .Select(o => new
                {
                    o.Customer.ContactName,
                    Order_Details = o.Order_Details.Select(od => new
                    {
                        od.Product.ProductName,
                        od.OrderID,
                        od.Discount,
                        od.Quantity,
                        od.UnitPrice,
                        od.ProductID
                    })
                });
                
                foreach (var row in query)
                {
                    yield return($"Customer: {row.ContactName} Products: {string.Join(", ", row.Order_Details.Select(od => od.ProductName))}");
                }



            }
        }
    }
}
