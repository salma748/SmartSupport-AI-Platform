using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSupport_AI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAgentIdToConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Conversations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_AgentId",
                table: "Conversations",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_SupportAgents_AgentId",
                table: "Conversations",
                column: "AgentId",
                principalTable: "SupportAgents",
                principalColumn: "AgentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_SupportAgents_AgentId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_AgentId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Conversations");
        }
    }
}
