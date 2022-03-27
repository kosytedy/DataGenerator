using Microsoft.EntityFrameworkCore.Migrations;

namespace DataGeneratorSample.Data.Migrations
{
    public partial class Endpoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "Models",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "Models");
        }
    }
}
