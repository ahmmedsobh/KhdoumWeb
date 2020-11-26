using Microsoft.EntityFrameworkCore.Migrations;

namespace KhdoumWeb.Data.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberRole_Members_MemberId",
                table: "MemberRole");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRole_Roless_RoleId",
                table: "MemberRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberRole",
                table: "MemberRole");

            migrationBuilder.RenameTable(
                name: "MemberRole",
                newName: "MemberRoles");

            migrationBuilder.RenameIndex(
                name: "IX_MemberRole_MemberId",
                table: "MemberRoles",
                newName: "IX_MemberRoles_MemberId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roless",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Privacy",
                table: "Items",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "LocationMap",
                table: "Items",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberRoles",
                table: "MemberRoles",
                columns: new[] { "RoleId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRoles_Members_MemberId",
                table: "MemberRoles",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRoles_Roless_RoleId",
                table: "MemberRoles",
                column: "RoleId",
                principalTable: "Roless",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberRoles_Members_MemberId",
                table: "MemberRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRoles_Roless_RoleId",
                table: "MemberRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberRoles",
                table: "MemberRoles");

            migrationBuilder.DropColumn(
                name: "LocationMap",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "MemberRoles",
                newName: "MemberRole");

            migrationBuilder.RenameIndex(
                name: "IX_MemberRoles_MemberId",
                table: "MemberRole",
                newName: "IX_MemberRole_MemberId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roless",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Privacy",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberRole",
                table: "MemberRole",
                columns: new[] { "RoleId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRole_Members_MemberId",
                table: "MemberRole",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRole_Roless_RoleId",
                table: "MemberRole",
                column: "RoleId",
                principalTable: "Roless",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
