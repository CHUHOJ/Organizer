namespace Organizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRowVersionToPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "RowVersion");
        }
    }
}
