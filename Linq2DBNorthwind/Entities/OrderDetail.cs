using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DBNorthwind.Entities
{
    [Table("[dbo].[Order Details]")]
    public class OrderDetail
    {
        [Column("OrderID")]
        [PrimaryKey]
        public int OrderId { get; set; }

        [Column("ProductID")]
        [PrimaryKey]
        public int ProductId { get; set; }

        [Association(ThisKey = nameof(ProductId), OtherKey = nameof(Entities.Product.ProductId))]
        public Product Product { get; set; }

        [Association(ThisKey = nameof(OrderId), OtherKey = nameof(Entities.Order.OrderId))]
        public Order Order { get; set; }
    }
}
