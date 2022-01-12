using Microsoft.EntityFrameworkCore.Migrations;

namespace WordOfTheDay.Repository.Migrations
{
    public partial class CreateIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Words",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "DateEmail_Index",
                table: "Words",
                columns: new[] { "AddTime", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "DateText_Index",
                table: "Words",
                columns: new[] { "AddTime", "Text" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
