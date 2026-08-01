using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addBorrowTransactionmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowDate",
                table: "BorrowTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "BorrowTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Fine",
                table: "BorrowTransactions",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinePaid",
                table: "BorrowTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LibraryItemId",
                table: "BorrowTransactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MembershipId",
                table: "BorrowTransactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "BorrowTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BorrowTransactions_LibraryItemId",
                table: "BorrowTransactions",
                column: "LibraryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowTransactions_MembershipId",
                table: "BorrowTransactions",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowTransactions_LibraryItems_LibraryItemId",
                table: "BorrowTransactions",
                column: "LibraryItemId",
                principalTable: "LibraryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowTransactions_Members_MembershipId",
                table: "BorrowTransactions",
                column: "MembershipId",
                principalTable: "Members",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowTransactions_LibraryItems_LibraryItemId",
                table: "BorrowTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowTransactions_Members_MembershipId",
                table: "BorrowTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BorrowTransactions_LibraryItemId",
                table: "BorrowTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BorrowTransactions_MembershipId",
                table: "BorrowTransactions");

            migrationBuilder.DropColumn(
                name: "BorrowDate",
                table: "BorrowTransactions");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "BorrowTransactions");

            migrationBuilder.DropColumn(
                name: "Fine",
                table: "BorrowTransactions");

            migrationBuilder.DropColumn(
                name: "IsFinePaid",
                table: "BorrowTransactions");

            migrationBuilder.DropColumn(
                name: "LibraryItemId",
                table: "BorrowTransactions");

            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "BorrowTransactions");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "BorrowTransactions");
        }
    }
}
