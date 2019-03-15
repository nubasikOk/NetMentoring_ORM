namespace EFNorthwind.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditCards",
                c => new
                {
                    CreditCardID = c.Int(nullable: false, identity: true),
                    CardNumber = c.String(maxLength: 16),
                    ExpirationDate = c.DateTime(nullable: false),
                    CardHolderName = c.String(nullable: false, maxLength: 200),
                    EmployeeID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.CreditCardID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditCards", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.CreditCards", new[] { "EmployeeID" });
            DropTable("dbo.CreditCards");
        }
    }
}
