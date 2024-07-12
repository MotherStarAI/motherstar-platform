using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotherStar.Platform.Data.Migrations
{
    public partial class AddDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Device",
                table: "PageAudits",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Device",
                table: "PageAudits");
        }
    }
}
