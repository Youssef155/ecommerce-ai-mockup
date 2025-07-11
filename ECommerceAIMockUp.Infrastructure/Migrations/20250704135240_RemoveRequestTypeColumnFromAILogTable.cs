using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAIMockUp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRequestTypeColumnFromAILogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "AILogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestType",
                table: "AILogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
