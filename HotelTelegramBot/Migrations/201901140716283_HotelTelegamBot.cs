namespace HotelTelegramBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HotelTelegamBot : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HotelRoomReservedDates", "HotelRoomId", "dbo.HotelRooms");
            DropIndex("dbo.HotelRoomReservedDates", new[] { "HotelRoomId" });
            RenameColumn(table: "dbo.HotelRoomReservedDates", name: "HotelRoomId", newName: "HotelRoom_Id");
            AddColumn("dbo.HotelRoomReservedDates", "ReservationId", c => c.Long(nullable: false));
            AlterColumn("dbo.HotelRoomReservedDates", "HotelRoom_Id", c => c.Long());
            CreateIndex("dbo.HotelRoomReservedDates", "ReservationId");
            CreateIndex("dbo.HotelRoomReservedDates", "HotelRoom_Id");
            AddForeignKey("dbo.HotelRoomReservedDates", "ReservationId", "dbo.Reservations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HotelRoomReservedDates", "HotelRoom_Id", "dbo.HotelRooms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HotelRoomReservedDates", "HotelRoom_Id", "dbo.HotelRooms");
            DropForeignKey("dbo.HotelRoomReservedDates", "ReservationId", "dbo.Reservations");
            DropIndex("dbo.HotelRoomReservedDates", new[] { "HotelRoom_Id" });
            DropIndex("dbo.HotelRoomReservedDates", new[] { "ReservationId" });
            AlterColumn("dbo.HotelRoomReservedDates", "HotelRoom_Id", c => c.Long(nullable: false));
            DropColumn("dbo.HotelRoomReservedDates", "ReservationId");
            RenameColumn(table: "dbo.HotelRoomReservedDates", name: "HotelRoom_Id", newName: "HotelRoomId");
            CreateIndex("dbo.HotelRoomReservedDates", "HotelRoomId");
            AddForeignKey("dbo.HotelRoomReservedDates", "HotelRoomId", "dbo.HotelRooms", "Id", cascadeDelete: true);
        }
    }
}
