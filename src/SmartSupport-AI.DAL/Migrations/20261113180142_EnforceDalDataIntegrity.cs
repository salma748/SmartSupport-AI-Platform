using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSupport_AI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnforceDalDataIntegrity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFeedbacks_Customers_CustomerId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_RefundRequests_Orders_OrderId",
                table: "RefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RefundStatusHistories_RefundRequests_RefundId",
                table: "RefundStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Customers_CustomerId",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignments_SupportTickets_TicketId",
                table: "TicketAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketStatusHistories_SupportTickets_TicketId",
                table: "TicketStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolExecutions_AIResponses_ResponseId",
                table: "ToolExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_CustomerId",
                table: "SupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFeedbacks_CustomerId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerFeedbacks");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments",
                column: "TicketId",
                unique: true,
                filter: "[IsCurrent] = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundRequests_Orders_OrderId",
                table: "RefundRequests",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundStatusHistories_RefundRequests_RefundId",
                table: "RefundStatusHistories",
                column: "RefundId",
                principalTable: "RefundRequests",
                principalColumn: "RefundId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignments_SupportTickets_TicketId",
                table: "TicketAssignments",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketStatusHistories_SupportTickets_TicketId",
                table: "TicketStatusHistories",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToolExecutions_AIResponses_ResponseId",
                table: "ToolExecutions",
                column: "ResponseId",
                principalTable: "AIResponses",
                principalColumn: "ResponseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundRequests_Orders_OrderId",
                table: "RefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RefundStatusHistories_RefundRequests_RefundId",
                table: "RefundStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAssignments_SupportTickets_TicketId",
                table: "TicketAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketStatusHistories_SupportTickets_TicketId",
                table: "TicketStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ToolExecutions_AIResponses_ResponseId",
                table: "ToolExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "SupportTickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "CustomerFeedbacks",
                type: "int",
                nullable: true);

            // Recover the original required customer links from their owning conversations.
            migrationBuilder.Sql("""
                EXEC(N'
                    UPDATE [ticket]
                    SET [CustomerId] = [conversation].[CustomerId]
                    FROM [SupportTickets] AS [ticket]
                    INNER JOIN [Conversations] AS [conversation]
                        ON [conversation].[ConversationId] = [ticket].[ConversationId];

                    UPDATE [feedback]
                    SET [CustomerId] = [conversation].[CustomerId]
                    FROM [CustomerFeedbacks] AS [feedback]
                    INNER JOIN [Conversations] AS [conversation]
                        ON [conversation].[ConversationId] = [feedback].[ConversationId];
                ');
                """);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "SupportTickets",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "CustomerFeedbacks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketAssignments_TicketId",
                table: "TicketAssignments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_CustomerId",
                table: "SupportTickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedbacks_CustomerId",
                table: "CustomerFeedbacks",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedbacks_Customers_CustomerId",
                table: "CustomerFeedbacks",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundRequests_Orders_OrderId",
                table: "RefundRequests",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefundStatusHistories_RefundRequests_RefundId",
                table: "RefundStatusHistories",
                column: "RefundId",
                principalTable: "RefundRequests",
                principalColumn: "RefundId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Customers_CustomerId",
                table: "SupportTickets",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAssignments_SupportTickets_TicketId",
                table: "TicketAssignments",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketStatusHistories_SupportTickets_TicketId",
                table: "TicketStatusHistories",
                column: "TicketId",
                principalTable: "SupportTickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToolExecutions_AIResponses_ResponseId",
                table: "ToolExecutions",
                column: "ResponseId",
                principalTable: "AIResponses",
                principalColumn: "ResponseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
