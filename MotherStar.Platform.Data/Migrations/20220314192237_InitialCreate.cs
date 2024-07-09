using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotherStar.Platform.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LighthouseCustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedByEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LighthouseCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageAuditRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LighthouseCustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageAuditRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageAuditRequests_LighthouseCustomers",
                        column: x => x.LighthouseCustomerId,
                        principalTable: "LighthouseCustomers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StatusTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statuses_StatusTypes",
                        column: x => x.StatusTypeId,
                        principalTable: "StatusTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageAudits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: true),
                    PageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PageAuditRequestId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageAudits_PageAuditRequests",
                        column: x => x.PageAuditRequestId,
                        principalTable: "PageAuditRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PageAudits_Statuses",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageAuditItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PageAuditId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ReferenceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AuditCategoryId = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageAuditItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageAuditItems_PageAudits",
                        column: x => x.PageAuditId,
                        principalTable: "PageAudits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PageAuditItems_Statuses",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "StatusTypes",
                columns: new[] { "Id", "StatusTypeName" },
                values: new object[,]
                {
                    { new Guid("202b50ff-4e0b-4a4f-8542-6c7641114cb6"), "Page Audit Items" },
                    { new Guid("31bfc614-8575-4cc1-89e5-0b2090d62358"), "Page Audit Requests" },
                    { new Guid("c7c12605-f0c5-457f-9b76-3fb03d0527df"), "Page Audits" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "StatusName", "StatusTypeId" },
                values: new object[,]
                {
                    { new Guid("48a62047-2f22-453c-ab16-590dafa383b1"), "Failed", new Guid("31bfc614-8575-4cc1-89e5-0b2090d62358") },
                    { new Guid("67f8bdc0-04ca-448f-a5b1-46aea7c569c3"), "Completed", new Guid("c7c12605-f0c5-457f-9b76-3fb03d0527df") },
                    { new Guid("c16ca33c-2949-4f88-82f2-36eb24bbf874"), "Started", new Guid("31bfc614-8575-4cc1-89e5-0b2090d62358") },
                    { new Guid("c7625069-2055-4669-a0f2-5169b20ebc6e"), "Created", new Guid("31bfc614-8575-4cc1-89e5-0b2090d62358") },
                    { new Guid("c8cc655b-0de4-484a-abf2-4e8c659059f4"), "Completed", new Guid("202b50ff-4e0b-4a4f-8542-6c7641114cb6") },
                    { new Guid("cccae8e9-350c-43c5-bf0c-5d0d7fe1409c"), "Completed", new Guid("31bfc614-8575-4cc1-89e5-0b2090d62358") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageAuditItems_PageAuditId",
                table: "PageAuditItems",
                column: "PageAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_PageAuditItems_StatusId",
                table: "PageAuditItems",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PageAuditRequests_LighthouseCustomerId",
                table: "PageAuditRequests",
                column: "LighthouseCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PageAudits_PageAuditRequestId",
                table: "PageAudits",
                column: "PageAuditRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PageAudits_StatusId",
                table: "PageAudits",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_StatusTypeId",
                table: "Statuses",
                column: "StatusTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageAuditItems");

            migrationBuilder.DropTable(
                name: "PageAudits");

            migrationBuilder.DropTable(
                name: "PageAuditRequests");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "LighthouseCustomers");

            migrationBuilder.DropTable(
                name: "StatusTypes");
        }
    }
}
