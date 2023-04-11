using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopOnline.API.Migrations
{
    public partial class changeDcriptionToContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Reviews",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Reviews",
                newName: "Description");
        }
    }
}
