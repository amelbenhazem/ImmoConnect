using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImmoConnect_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddHeureDemandeToDemandeVisite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Heure_demande",
                table: "DemandesVisite",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "DemandesVisite",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Heure_demande",
                table: "DemandesVisite");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "DemandesVisite");
        }
    }
}
