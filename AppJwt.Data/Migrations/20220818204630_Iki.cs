using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppJwt.Data.Migrations
{
    public partial class Iki : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "AspNetUsers",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");
        }
    }
}
