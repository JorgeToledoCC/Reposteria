using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiReposteria.Migrations
{
    /// <inheritdoc />
    public partial class constock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: new Guid("a123bc45-1234-4321-8765-abcdef123456"),
                column: "Stock",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Productos");
        }
    }
}
