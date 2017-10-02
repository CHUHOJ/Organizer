namespace Organizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPersonPhoneNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonPhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonPhoneNumber", "PersonId", "dbo.Person");
            DropIndex("dbo.PersonPhoneNumber", new[] { "PersonId" });
            DropTable("dbo.PersonPhoneNumber");
        }
    }
}
