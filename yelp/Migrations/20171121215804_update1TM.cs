using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace yelp.Migrations
{
    public partial class update1TM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_CategoryTypes_CategoryTypeId",
                table: "Businesses");

            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "Reviews",
                type: "int4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Businesses",
                type: "text",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Businesses",
                type: "text",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CategoryTypeId",
                table: "Businesses",
                type: "int4",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BusinessId",
                table: "Reviews",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_CategoryTypes_CategoryTypeId",
                table: "Businesses",
                column: "CategoryTypeId",
                principalTable: "CategoryTypes",
                principalColumn: "BusCategoryTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Businesses_BusinessId",
                table: "Reviews",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "BusinessId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_CategoryTypes_CategoryTypeId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Businesses_BusinessId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BusinessId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Website",
                table: "Businesses",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                table: "Businesses",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryTypeId",
                table: "Businesses",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_CategoryTypes_CategoryTypeId",
                table: "Businesses",
                column: "CategoryTypeId",
                principalTable: "CategoryTypes",
                principalColumn: "BusCategoryTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
