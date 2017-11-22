using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace yelp.Migrations
{
    public partial class OneToOneBizProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_BusinessId",
                table: "Properties");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_BusinessId",
                table: "Properties",
                column: "BusinessId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_BusinessId",
                table: "Properties");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_BusinessId",
                table: "Properties",
                column: "BusinessId");
        }
    }
}
