using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradeCom.Migrations
{
    /// <inheritdoc />
    public partial class NewFieldInFileModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "SeminarFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "PracticeFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "LectureFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "HomeTaskFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "SeminarFiles");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "PracticeFiles");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "LectureFiles");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "HomeTaskFiles");
        }
    }
}
