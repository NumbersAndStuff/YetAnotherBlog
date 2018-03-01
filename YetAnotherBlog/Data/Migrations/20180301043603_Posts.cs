using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace YetAnotherBlog.Data.Migrations
{
    public partial class Posts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AllowResponses = table.Column<bool>(nullable: false),
                    Edited = table.Column<bool>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    Post = table.Column<string>(nullable: true),
                    ResponseCount = table.Column<int>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    TimePosted = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostModel");
        }
    }
}
