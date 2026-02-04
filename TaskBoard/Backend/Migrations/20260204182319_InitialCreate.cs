using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskBoard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BoardId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(236), "Main project board", "Project Alpha", new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(236) },
                    { 2, new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(239), "Secondary project", "Project Beta", new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(239) }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "BoardId", "CreatedAt", "Description", "DueDate", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(442), "Initialize database schema", null, 2, 2, "Setup Database", new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(443) },
                    { 2, 1, new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(446), "Build REST API", null, 2, 1, "Create API Endpoints", new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(446) },
                    { 3, 1, new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(527), "Create Angular components", new DateTime(2026, 2, 11, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(448), 1, 0, "Build UI Components", new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(528) },
                    { 4, 2, new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(530), "Unit and integration tests", null, 1, 0, "Write Tests", new DateTime(2026, 2, 4, 18, 23, 16, 312, DateTimeKind.Utc).AddTicks(530) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BoardId",
                table: "Tasks",
                column: "BoardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
