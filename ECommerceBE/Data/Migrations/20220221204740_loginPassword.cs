using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceBE.Migrations
{
    public partial class loginPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Admins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Admins");
        }
    }
}
