namespace Organizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMeeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonMeeting",
                c => new
                    {
                        Person_Id = c.Int(nullable: false),
                        Meeting_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_Id, t.Meeting_Id })
                .ForeignKey("dbo.Person", t => t.Person_Id, cascadeDelete: true)
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id, cascadeDelete: true)
                .Index(t => t.Person_Id)
                .Index(t => t.Meeting_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonMeeting", "Meeting_Id", "dbo.Meeting");
            DropForeignKey("dbo.PersonMeeting", "Person_Id", "dbo.Person");
            DropIndex("dbo.PersonMeeting", new[] { "Meeting_Id" });
            DropIndex("dbo.PersonMeeting", new[] { "Person_Id" });
            DropTable("dbo.PersonMeeting");
            DropTable("dbo.Meeting");
        }
    }
}
