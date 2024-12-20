using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCreditHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditHistoryEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    PartnershipCredits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditsSpend = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentCredits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditHistoryEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditHistoryEntities_organizationEntities_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "organizationEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditHistoryEntities_OrganizationId",
                table: "CreditHistoryEntities",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditHistoryEntities");
        }
    }
}
