using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriceToVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SqFt",
                table: "Villas",
                newName: "Sqft");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Villas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Villas");

            migrationBuilder.RenameColumn(
                name: "Sqft",
                table: "Villas",
                newName: "SqFt");
        }
    }
}
