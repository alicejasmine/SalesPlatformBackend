using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class projectAndOrganizationReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEntities_organizationEntities_OrganizationEntityId",
                table: "ProjectEntities");

            migrationBuilder.DropIndex(
                name: "IX_ProjectEntities_OrganizationEntityId",
                table: "ProjectEntities");

            migrationBuilder.DropColumn(
                name: "OrganizationEntityId",
                table: "ProjectEntities");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEntities_OrganizationId",
                table: "ProjectEntities",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEntities_organizationEntities_OrganizationId",
                table: "ProjectEntities",
                column: "OrganizationId",
                principalTable: "organizationEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEntities_organizationEntities_OrganizationId",
                table: "ProjectEntities");

            migrationBuilder.DropIndex(
                name: "IX_ProjectEntities_OrganizationId",
                table: "ProjectEntities");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationEntityId",
                table: "ProjectEntities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEntities_OrganizationEntityId",
                table: "ProjectEntities",
                column: "OrganizationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEntities_organizationEntities_OrganizationEntityId",
                table: "ProjectEntities",
                column: "OrganizationEntityId",
                principalTable: "organizationEntities",
                principalColumn: "Id");
        }
    }
}
