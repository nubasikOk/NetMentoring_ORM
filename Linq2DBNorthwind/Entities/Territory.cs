using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DBNorthwind.Entities
{
    [Table("[dbo].[Territories]")]
    public class Territory
    {
        [Column("TerritoryID")]
        [PrimaryKey]
        [Identity]
        public int TerritoryId { get; set; }

        [Column("TerritoryDescription")]
        [NotNull]
        public string TerritoryDescription { get; set; }

        [Column("RegionID")]
        [NotNull]
        public int RegionId { get; set; }

        [Association(ThisKey = nameof(RegionId), OtherKey = nameof(Entities.Region.RegionId))]
        public Region Region { get; set; }
    }
}
