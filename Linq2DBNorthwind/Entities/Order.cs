using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DBNorthwind.Entities
{
    [Table("[dbo].[Orders]")]
    public class Order
    {
        [Column("OrderID")]
        [Identity]
        [PrimaryKey]
        public int OrderId { get; set; }

        [Column("ShipVia")]
        public int? ShipperId { get; set; }

        [Column("ShippedDate")]
        public DateTime? ShippedDate { get; set; }

        [Column("EmployeeID")]
        public int? EmployeeId { get; set; }

        [Association(ThisKey = nameof(EmployeeId), OtherKey = nameof(Entities.Employee.EmployeeId), CanBeNull = true)]
        public Employee Employee { get; set; }

        [Association(ThisKey = nameof(ShipperId), OtherKey = nameof(Entities.Shipper.ShipperId), CanBeNull = true)]
        public Shipper Shipper { get; set; }

    }
}
