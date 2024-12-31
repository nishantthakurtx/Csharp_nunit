using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.Repository.Migrations
{
    /// <inheritdoc />
    public partial class updateRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserRefreshToken",
                table: "AppUserRefreshToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserRefreshToken",
                table: "AppUserRefreshToken",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserRefreshToken",
                table: "AppUserRefreshToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserRefreshToken",
                table: "AppUserRefreshToken",
                column: "UserId");
        }
    }
}
