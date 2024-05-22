using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetalcutWeb.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addliketable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    LikeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LikedCommentCommentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LikeUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.LikeId);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_LikeUserId",
                        column: x => x.LikeUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Comments_LikedCommentCommentId",
                        column: x => x.LikedCommentCommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_LikedCommentCommentId",
                table: "Likes",
                column: "LikedCommentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_LikeUserId",
                table: "Likes",
                column: "LikeUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Comments");
        }
    }
}
