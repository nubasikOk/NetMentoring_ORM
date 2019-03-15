using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DBNorthwind.Entities
{
    [Table("[dbo].[EmployeeTerritories]")]
    public class EmployeeTerritory
    {
        [Column("EmployeeID")]
        [NotNull]
        public int EmployeeId { get; set; }

        [Column("TerritoryID")]
        [NotNull]
        public int TerritoryId { get; set; }

        [Association(ThisKey = nameof(EmployeeId), OtherKey = nameof(Entities.Employee.EmployeeId))]
        public Employee Employee { get; set; }

        [Association(ThisKey = nameof(TerritoryId), OtherKey = nameof(Entities.Territory.TerritoryId))]
        public Territory Territory { get; set; }
    }
}
