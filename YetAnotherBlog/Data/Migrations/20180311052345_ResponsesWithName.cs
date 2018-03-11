using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace YetAnotherBlog.Data.Migrations
{
    public partial class ResponsesWithName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostedByName",
                table: "Responses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostedByName",
                table: "Responses");
        }
    }
}
