using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetalcutWeb.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingStorageProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StorageProducts",
                columns: table => new
                {
                    StorageProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StorageProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageProdCount = table.Column<int>(type: "int", nullable: false),
                    StorageProductTypes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageProducts", x => x.StorageProductId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorageProducts");
        }
    }
}
