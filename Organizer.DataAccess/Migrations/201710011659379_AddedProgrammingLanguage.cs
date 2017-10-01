namespace Organizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProgrammingLanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgrammingLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Person", "FavouriteLanguageId", c => c.Int());
            CreateIndex("dbo.Person", "FavouriteLanguageId");
            AddForeignKey("dbo.Person", "FavouriteLanguageId", "dbo.ProgrammingLanguage", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Person", "FavouriteLanguageId", "dbo.ProgrammingLanguage");
            DropIndex("dbo.Person", new[] { "FavouriteLanguageId" });
            DropColumn("dbo.Person", "FavouriteLanguageId");
            DropTable("dbo.ProgrammingLanguage");
        }
    }
}
