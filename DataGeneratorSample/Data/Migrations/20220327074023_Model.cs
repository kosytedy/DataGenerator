using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataGeneratorSample.Data.Migrations
{
    public partial class Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                table: "Property",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Property_ModelId",
                table: "Property",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_ModelId",
                table: "Models",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Models_ModelId",
                table: "Property",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_Models_ModelId",
                table: "Property");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Property_ModelId",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Property");
        }
    }
}
