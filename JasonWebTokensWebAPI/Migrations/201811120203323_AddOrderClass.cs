namespace JasonWebTokensWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        ShipperCity = c.String(),
                        IsShipped = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
        }
    }
}
