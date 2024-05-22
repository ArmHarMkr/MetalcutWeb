using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetalcutWeb.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMsgChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatOfMessageId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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
                name: "IX_Messages_ChatOfMessageId",
                table: "Messages",
                column: "ChatOfMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserOneId",
                table: "Chats",
                column: "UserOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserTwoId",
                table: "Chats",
                column: "UserTwoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatOfMessageId",
                table: "Messages",
                column: "ChatOfMessageId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatOfMessageId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatOfMessageId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatOfMessageId",
                table: "Messages");
        }
    }
}
