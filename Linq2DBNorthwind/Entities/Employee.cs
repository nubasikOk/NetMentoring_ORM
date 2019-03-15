using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DBNorthwind.Entities
{
    [Table("[dbo].[Employees]")]
    public class Employee
    {
        [Column("EmployeeID")]
        [PrimaryKey]
        [Identity]
        public int EmployeeId { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }
    }
}
