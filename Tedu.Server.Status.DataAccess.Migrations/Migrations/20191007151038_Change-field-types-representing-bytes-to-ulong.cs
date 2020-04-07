using Microsoft.EntityFrameworkCore.Migrations;

namespace Tedu.Server.Status.DataAccess.Migrations.Migrations
{
    public partial class Changefieldtypesrepresentingbytestoulong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "lastbackupsizebytes",
                table: "backup",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "diskusedbytes",
                table: "backup",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "diskfreebytes",
                table: "backup",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "lastbackupsizebytes",
                table: "backup",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "diskusedbytes",
                table: "backup",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "diskfreebytes",
                table: "backup",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
