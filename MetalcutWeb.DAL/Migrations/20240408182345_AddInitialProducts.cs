using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetalcutWeb.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatEntityId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatEntityId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatEntityId",
                table: "Messages");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Price", "ProdDescription", "ProdName", "StarCount" },
                values: new object[] { "PlasmaId", 200.0, "Cutting thick metal with a plasma cutter. The price could be changed because of thickness", "Metal cutting with plasmacutter", 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: "PlasmaId");

            migrationBuilder.AddColumn<string>(
                name: "ChatEntityId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserOneId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserTwoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChatName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_UserOneId",
                        column: x => x.UserOneId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_UserTwoId",
                        column: x => x.UserTwoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatEntityId",
                table: "Messages",
                column: "ChatEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserOneId",
                table: "Chats",
                column: "UserOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserTwoId",
                table: "Chats",
                column: "UserTwoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatEntityId",
                table: "Messages",
                column: "ChatEntityId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
