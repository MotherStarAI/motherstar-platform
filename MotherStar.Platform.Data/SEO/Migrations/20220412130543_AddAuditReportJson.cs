using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotherStar.Platform.Data.Migrations
{
    public partial class AddAuditReportJson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditReport",
                table: "PageAudits",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditReport",
                table: "PageAudits");
        }
    }
}
