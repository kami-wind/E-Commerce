using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class FourthCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "vnpayPaymentRequests");

            migrationBuilder.RenameColumn(
                name: "OrderInfo",
                table: "vnpayPaymentRequests",
                newName: "Status");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "vnpayPaymentRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "vnpayPaymentRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "36558197-ba07-4bfc-aa3a-3e3e1d9cbc3f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2b4ffe34-508f-4390-a3e6-e61d7da31ea6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "vnpayPaymentRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "vnpayPaymentRequests");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "vnpayPaymentRequests",
                newName: "OrderInfo");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "vnpayPaymentRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ae688460-5cdb-46b6-874d-8ce5b09bbba9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3fa79749-f76e-433e-af8b-d47f0480ede9");
        }
    }
}
