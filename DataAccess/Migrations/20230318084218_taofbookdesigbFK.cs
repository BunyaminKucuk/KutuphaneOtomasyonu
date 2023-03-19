using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class taofbookdesigbFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakeOfBooks_Books_Id",
                table: "TakeOfBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_TakeOfBooks_Users_Id",
                table: "TakeOfBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TakeOfBooks",
                table: "TakeOfBooks");

            migrationBuilder.DropIndex(
                name: "IX_TakeOfBooks_Id",
                table: "TakeOfBooks");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TakeOfBooks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TakeOfBooks",
                table: "TakeOfBooks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TakeOfBooks_BookId",
                table: "TakeOfBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_TakeOfBooks_UserId",
                table: "TakeOfBooks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakeOfBooks_Books_BookId",
                table: "TakeOfBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TakeOfBooks_Users_UserId",
                table: "TakeOfBooks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakeOfBooks_Books_BookId",
                table: "TakeOfBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_TakeOfBooks_Users_UserId",
                table: "TakeOfBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TakeOfBooks",
                table: "TakeOfBooks");

            migrationBuilder.DropIndex(
                name: "IX_TakeOfBooks_BookId",
                table: "TakeOfBooks");

            migrationBuilder.DropIndex(
                name: "IX_TakeOfBooks_UserId",
                table: "TakeOfBooks");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TakeOfBooks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TakeOfBooks",
                table: "TakeOfBooks",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.CreateIndex(
                name: "IX_TakeOfBooks_Id",
                table: "TakeOfBooks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TakeOfBooks_Books_Id",
                table: "TakeOfBooks",
                column: "Id",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TakeOfBooks_Users_Id",
                table: "TakeOfBooks",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
