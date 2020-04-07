using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Tedu.Server.Status.DataAccess.Migrations.Migrations
{
    public partial class AddBackuptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "backup",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    serverid = table.Column<int>(nullable: false),
                    isstatusok = table.Column<bool>(nullable: false),
                    backupsamount = table.Column<int>(nullable: false),
                    createddatetimeutc = table.Column<DateTime>(nullable: false),
                    lastbackupstartdatetimeutc = table.Column<DateTime>(nullable: false),
                    lastbackupenddatetimeutc = table.Column<DateTime>(nullable: false),
                    lastbackupsizebytes = table.Column<int>(nullable: false),
                    backupdurationtotalseconds = table.Column<int>(nullable: false),
                    backupdurationcopyseconds = table.Column<int>(nullable: false),
                    oldestbackupenddatetimeutc = table.Column<DateTime>(nullable: false),
                    diskusedbytes = table.Column<int>(nullable: false),
                    diskfreebytes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_backup", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "backup");
        }
    }
}
