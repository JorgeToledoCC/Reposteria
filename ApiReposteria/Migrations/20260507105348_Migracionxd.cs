using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiReposteria.Migrations
{
    /// <inheritdoc />
    public partial class Migracionxd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CI_Extension",
                table: "Clientes",
                columns: new[] { "CI", "Extension" },
                unique: true,
                filter: "[Extension] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clientes_CI_Extension",
                table: "Clientes");
        }
    }
}
