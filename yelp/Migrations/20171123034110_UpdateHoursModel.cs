using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace yelp.Migrations
{
    public partial class UpdateHoursModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Businesses_BusinessId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Hours_BusinessId",
                table: "Hours");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "Reviews",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WedOpenTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "WedCloseTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "TueOpenTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "TueCloseTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThuOpenTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThuCloseTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SunOpenTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SunCloseTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SatOpenTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SatCloseTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "MonOpenTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "MonCloseTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FriOpenTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FriCloseTime",
                table: "Hours",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<bool>(
                name: "FriClosed",
                table: "Hours",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MonClosed",
                table: "Hours",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SatClosed",
                table: "Hours",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SunClosed",
                table: "Hours",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ThuClosed",
                table: "Hours",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TueClosed",
                table: "Hours",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WedClosed",
                table: "Hours",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Hours_BusinessId",
                table: "Hours",
                column: "BusinessId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Businesses_BusinessId",
                table: "Reviews",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Businesses_BusinessId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Hours_BusinessId",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "FriClosed",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "MonClosed",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "SatClosed",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "SunClosed",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "ThuClosed",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "TueClosed",
                table: "Hours");

            migrationBuilder.DropColumn(
                name: "WedClosed",
                table: "Hours");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "Reviews",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WedOpenTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WedCloseTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TueOpenTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TueCloseTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThuOpenTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThuCloseTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SunOpenTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SunCloseTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SatOpenTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SatCloseTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MonOpenTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MonCloseTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FriOpenTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FriCloseTime",
                table: "Hours",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hours_BusinessId",
                table: "Hours",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Businesses_BusinessId",
                table: "Reviews",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
