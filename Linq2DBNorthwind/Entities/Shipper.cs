using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DBNorthwind.Entities
{
    [Table("[dbo].[Shippers]")]
    public class Shipper
    {
        [Column("ShipperID")]
        [Identity]
        [PrimaryKey]
        public int ShipperId { get; set; }

        [Column("CompanyName")]
        [NotNull]
        public string CompanyName { get; set; }

        [Column("Phone")]
        public string Phone { get; set; }
    }
}
