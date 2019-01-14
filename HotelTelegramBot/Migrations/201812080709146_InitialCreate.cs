namespace HotelTelegramBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HotelRoomReservedDates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HotelRoomId = c.Long(nullable: false),
                        ReservedDate = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelRooms", t => t.HotelRoomId, cascadeDelete: true)
                .Index(t => t.HotelRoomId);
            
            CreateTable(
                "dbo.HotelRooms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HotelRoomTypeId = c.Long(nullable: false),
                        Name = c.String(),
                        Floor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelRoomTypes", t => t.HotelRoomTypeId, cascadeDelete: true)
                .Index(t => t.HotelRoomTypeId);
            
            CreateTable(
                "dbo.HotelRoomTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        MaxNumberOfAdults = c.Int(nullable: false),
                        MaxNumberOfChildren = c.Int(nullable: false),
                        Services = c.String(),
                        Area = c.Double(nullable: false),
                        Description = c.String(),
                        TotalCountRoom = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HotelRoomTypeImages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HotelRoomTypeId = c.Long(nullable: false),
                        ImageURL = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelRoomTypes", t => t.HotelRoomTypeId, cascadeDelete: true)
                .Index(t => t.HotelRoomTypeId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HotelRoomId = c.Long(nullable: false),
                        IdUserChat = c.Long(nullable: false),
                        SecondName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        Number = c.String(),
                        Email = c.String(),
                        DateOfArrival = c.String(),
                        DateOfDeparture = c.String(),
                        NumberOfAdults = c.Int(nullable: false),
                        NumberOfChildren = c.Int(nullable: false),
                        UserChat_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HotelRooms", t => t.HotelRoomId, cascadeDelete: true)
                .ForeignKey("dbo.UserChats", t => t.UserChat_Id)
                .Index(t => t.HotelRoomId)
                .Index(t => t.UserChat_Id);
            
            CreateTable(
                "dbo.UserChats",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdChat = c.Long(nullable: false),
                        ChatPosition = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TempInformations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdUserChat = c.Long(nullable: false),
                        Property = c.String(),
                        Value = c.String(),
                        UserChat_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserChats", t => t.UserChat_Id)
                .Index(t => t.UserChat_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempInformations", "UserChat_Id", "dbo.UserChats");
            DropForeignKey("dbo.Reservations", "UserChat_Id", "dbo.UserChats");
            DropForeignKey("dbo.Reservations", "HotelRoomId", "dbo.HotelRooms");
            DropForeignKey("dbo.HotelRoomTypeImages", "HotelRoomTypeId", "dbo.HotelRoomTypes");
            DropForeignKey("dbo.HotelRooms", "HotelRoomTypeId", "dbo.HotelRoomTypes");
            DropForeignKey("dbo.HotelRoomReservedDates", "HotelRoomId", "dbo.HotelRooms");
            DropIndex("dbo.TempInformations", new[] { "UserChat_Id" });
            DropIndex("dbo.Reservations", new[] { "UserChat_Id" });
            DropIndex("dbo.Reservations", new[] { "HotelRoomId" });
            DropIndex("dbo.HotelRoomTypeImages", new[] { "HotelRoomTypeId" });
            DropIndex("dbo.HotelRooms", new[] { "HotelRoomTypeId" });
            DropIndex("dbo.HotelRoomReservedDates", new[] { "HotelRoomId" });
            DropTable("dbo.TempInformations");
            DropTable("dbo.UserChats");
            DropTable("dbo.Reservations");
            DropTable("dbo.HotelRoomTypeImages");
            DropTable("dbo.HotelRoomTypes");
            DropTable("dbo.HotelRooms");
            DropTable("dbo.HotelRoomReservedDates");
        }
    }
}
