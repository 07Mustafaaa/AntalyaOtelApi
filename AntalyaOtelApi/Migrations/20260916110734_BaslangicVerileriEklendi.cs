using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AntalyaOtelApi.Migrations
{
    /// <inheritdoc />
    public partial class BaslangicVerileriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OdaTipleri",
                columns: new[] { "OdaTipId", "Aciklama", "GunlukFiyat", "TipAdi" },
                values: new object[,]
                {
                    { 1, "1 Çift Kişilik Yatak, Şehir Manzaralı", 1000m, "Standart" },
                    { 2, "Geniş Balkon, Deniz Manzaralı, Jakuzili", 2500m, "Deluxe" },
                    { 3, "Özel Teras, Özel Havuz, Ultra Lüks", 7500m, "Kral Dairesi" }
                });

            migrationBuilder.InsertData(
                table: "Odalar",
                columns: new[] { "OdaId", "Kat", "Musaitmi", "OdaNo", "OdaTipId" },
                values: new object[,]
                {
                    { 1, 1, false, 101, 1 },
                    { 2, 1, false, 102, 1 },
                    { 3, 2, false, 201, 2 },
                    { 4, 2, false, 202, 2 },
                    { 5, 3, false, 301, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OdaTipleri",
                keyColumn: "OdaTipId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OdaTipleri",
                keyColumn: "OdaTipId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OdaTipleri",
                keyColumn: "OdaTipId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Odalar",
                keyColumn: "OdaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Odalar",
                keyColumn: "OdaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Odalar",
                keyColumn: "OdaId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Odalar",
                keyColumn: "OdaId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Odalar",
                keyColumn: "OdaId",
                keyValue: 5);
        }
    }
}
