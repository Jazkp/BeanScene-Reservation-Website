using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanSceneReservationApplication.Data.Migrations
{
    public partial class UpdatedPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sittings_SittingTypes_SittingTypeId",
                table: "Sittings");

            migrationBuilder.AlterColumn<int>(
                name: "SittingTypeId",
                table: "Sittings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "People",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sittings_SittingTypes_SittingTypeId",
                table: "Sittings",
                column: "SittingTypeId",
                principalTable: "SittingTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sittings_SittingTypes_SittingTypeId",
                table: "Sittings");

            migrationBuilder.AlterColumn<int>(
                name: "SittingTypeId",
                table: "Sittings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "People",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sittings_SittingTypes_SittingTypeId",
                table: "Sittings",
                column: "SittingTypeId",
                principalTable: "SittingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
