using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmoConnect_BE.Migrations
{
    /// <inheritdoc />
    public partial class MakeNavigationPropertiesOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image_url",
                table: "BiensImmobiliers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BiensImmobiliers",
                keyColumn: "Image_url",
                keyValue: null,
                column: "Image_url",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Image_url",
                table: "BiensImmobiliers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
