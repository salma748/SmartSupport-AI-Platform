using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSupport_AI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueActiveAIConfigurationIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AIConfigurations_IsActive",
                table: "AIConfigurations",
                column: "IsActive",
                unique: true,
                filter: "[IsActive] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AIConfigurations_IsActive",
                table: "AIConfigurations");
        }
    }
}
