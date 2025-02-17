using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockScraper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MarketCapValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MarketCapCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    YearFounded = table.Column<int>(type: "int", nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "int", nullable: false),
                    HeadquartersCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HeadquartersState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PreviousClosePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OpenPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateRetrieved = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateScraped = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stocks");
        }
    }
}
