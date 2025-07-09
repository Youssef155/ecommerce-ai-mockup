using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAIMockUp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToDesignDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Scale",
                table: "DesignDetails",
                newName: "ScaleY");

            migrationBuilder.AddColumn<float>(
                name: "Opacity",
                table: "DesignDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Rotation",
                table: "DesignDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ScaleX",
                table: "DesignDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opacity",
                table: "DesignDetails");

            migrationBuilder.DropColumn(
                name: "Rotation",
                table: "DesignDetails");

            migrationBuilder.DropColumn(
                name: "ScaleX",
                table: "DesignDetails");

            migrationBuilder.RenameColumn(
                name: "ScaleY",
                table: "DesignDetails",
                newName: "Scale");
        }
    }
}
