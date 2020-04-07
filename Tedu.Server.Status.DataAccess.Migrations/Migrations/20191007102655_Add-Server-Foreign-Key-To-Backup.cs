using Microsoft.EntityFrameworkCore.Migrations;

namespace Tedu.Server.Status.DataAccess.Migrations.Migrations
{
    public partial class AddServerForeignKeyToBackup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_backup_serverid",
                table: "backup",
                column: "serverid");

            migrationBuilder.AddForeignKey(
                name: "fk_backup_server_serverid",
                table: "backup",
                column: "serverid",
                principalTable: "server",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_backup_server_serverid",
                table: "backup");

            migrationBuilder.DropIndex(
                name: "ix_backup_serverid",
                table: "backup");
        }
    }
}
