using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntalyaOtelApi.Migrations
{
    /// <inheritdoc />
    public partial class RezervasyonIptalAlaniEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "kat",
                table: "Odalar",
                newName: "Kat");

            migrationBuilder.AddColumn<bool>(
                name: "IptalEdildi",
                table: "Rezervasyonlar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IptalEdildi",
                table: "Rezervasyonlar");

            migrationBuilder.RenameColumn(
                name: "Kat",
                table: "Odalar",
                newName: "kat");
        }
    }
}
