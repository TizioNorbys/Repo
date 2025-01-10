using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Symbol_DateTime",
                table: "Stocks",
                columns: new[] { "Symbol", "DateTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stocks_Symbol_DateTime",
                table: "Stocks");
        }
    }
}
