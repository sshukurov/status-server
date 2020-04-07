using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Tedu.Server.Status.DataAccess.Migrations.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "server",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    host = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "probe",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    serverid = table.Column<int>(nullable: false),
                    checkeddatetimeutc = table.Column<DateTime>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    result = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_probe", x => x.id);
                    table.ForeignKey(
                        name: "fk_probe_server_serverid",
                        column: x => x.serverid,
                        principalTable: "server",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_probe_serverid",
                table: "probe",
                column: "serverid");

            migrationBuilder.CreateIndex(
                name: "ix_server_host",
                table: "server",
                column: "host",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "probe");

            migrationBuilder.DropTable(
                name: "server");
        }
    }
}
