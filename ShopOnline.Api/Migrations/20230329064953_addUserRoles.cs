using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopOnline.API.Migrations
{
    public partial class addUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userRoleId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_userRoleId",
                table: "Users",
                column: "userRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRole_userRoleId",
                table: "Users",
                column: "userRoleId",
                principalTable: "UserRole",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRole_userRoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_Users_userRoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "userRoleId",
                table: "Users");
        }
    }
}
