using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class dbContextRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loan_BookId",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Book");

            migrationBuilder.AddColumn<int>(
                name: "LoanId",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Loan_BookId",
                table: "Loan",
                column: "BookId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loan_BookId",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "Book");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Loan_BookId",
                table: "Loan",
                column: "BookId");
        }
    }
}
