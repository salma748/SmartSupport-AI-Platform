using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSupport_AI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureConversationAgentDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_SupportAgents_AgentId",
                table: "Conversations");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_SupportAgents_AgentId",
                table: "Conversations",
                column: "AgentId",
                principalTable: "SupportAgents",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_SupportAgents_AgentId",
                table: "Conversations");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_SupportAgents_AgentId",
                table: "Conversations",
                column: "AgentId",
                principalTable: "SupportAgents",
                principalColumn: "AgentId");
        }
    }
}
