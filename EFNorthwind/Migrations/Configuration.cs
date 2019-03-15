namespace EFNorthwind.Migrations
{
    using EFNorthwind.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NorthwindDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NorthwindDB context)
        {
            context.Categories.AddOrUpdate(c => c.CategoryName,
               new Category { CategoryName = "Vehicles" },
               new Category { CategoryName = "Drinks" });

            context.Regions.AddOrUpdate(r => r.RegionID,
                new Region { RegionDescription = "Grodno", RegionID = 5 });

            context.Territories.AddOrUpdate(t => t.TerritoryID,
                new Territory { TerritoryID = "199766", TerritoryDescription = "Railway station", RegionID = 5 },
                new Territory { TerritoryID = "343241232", TerritoryDescription = "Sovetskaya square", RegionID = 5 });
        }
    }
}
